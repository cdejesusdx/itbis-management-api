using Moq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.Mappings;
using DGII.ItbisManagement.Application.Services;
using DGII.ItbisManagement.Application.Tests.TestHelpers;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Application.Tests;

public class ContributorServiceTests
{
    private readonly Mock<IContributorRepository> _contributorRepository = new();
    private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
    
    private readonly Mock<ILogger<ContributorService>> _logger = new();
    private readonly IMapper _mapper;

    public ContributorServiceTests()
    {
        var cfg = new MapperConfiguration(
            cfg => cfg.AddMaps(typeof(AssemblyMarker).Assembly),
            NullLoggerFactory.Instance
        );
        cfg.AssertConfigurationIsValid();
        _mapper = cfg.CreateMapper();
    }

    /// <summary>Se debe crear un nuevo contribuyente cuando no existe previamente.</summary>
    [Fact]
    public async Task CreateAsync_Should_Create_When_NotExists()
    {
        var dto = Fakes.NewContributorCreateDto("40212345678");
        _contributorRepository.Setup(r => r.GetByIdAsync(dto.TaxId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Contributor?)null);
        _contributorRepository.Setup(r => r.AddAsync(It.IsAny<Contributor>(), It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

        var sut = new ContributorService(_contributorRepository.Object, _invoiceRepository.Object,_logger.Object, _mapper);

        var result = await sut.CreateAsync(dto, CancellationToken.None);

        result.TaxId.Should().Be(dto.TaxId);
        _contributorRepository.Verify(r => r.AddAsync(It.Is<Contributor>(c => c.TaxId == dto.TaxId), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>Se debe retornar facturas y total ITBIS=216.00 para el contribuyente de ejemplo.</summary>
    [Fact]
    public async Task GetWithInvoicesAsync_Should_Return_Invoices_And_Total()
    {
        var taxId = "98754321012";
        var contributor = Fakes.NewContributor(taxId);
        var invoices = Fakes.SampleInvoicesForJuan();

        _contributorRepository.Setup(r => r.GetByIdAsync(taxId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(contributor);
        _invoiceRepository.Setup(r => r.GetByContributorAsync(taxId, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(invoices);

        var sut = new ContributorService(_contributorRepository.Object, _invoiceRepository.Object, _logger.Object, _mapper);

        var result = await sut.GetWithInvoicesAsync(taxId, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Invoices.Should().HaveCount(2);
        result.TotalItbis.Should().Be(216m);
    }

    /// <summary>Se debe actualizar el nombre del contribuyente y devolver DTO actualizado.</summary>
    [Fact]
    public async Task UpdateAsync_Should_Update_When_Exists()
    {
        var taxId = "40212345678";
        var entity = Fakes.NewContributor(taxId);
        var dto = Fakes.NewContributorUpdateDto();

        _contributorRepository.Setup(r => r.GetByIdAsync(taxId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(entity);
        _contributorRepository.Setup(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()))
                     .Returns(Task.CompletedTask);

        var sut = new ContributorService(_contributorRepository.Object, _invoiceRepository.Object, _logger.Object, _mapper);

        var updated = await sut.UpdateAsync(taxId, dto, CancellationToken.None);

        updated.Should().NotBeNull();
        updated!.Name.Should().Be(dto.Name);
        _contributorRepository.Verify(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
    }
}