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

public class InvoiceServiceTests
{
    private readonly Mock<IInvoiceRepository> _invoiceRepository = new();
    private readonly Mock<IContributorRepository> _contributorRepository = new();
    
    private readonly Mock<ILogger<InvoiceService>> _logger = new();
    private readonly IMapper _mapper;

    public InvoiceServiceTests()
    {
        var cfg = new MapperConfiguration(
            cfg => cfg.AddMaps(typeof(AssemblyMarker).Assembly),
            NullLoggerFactory.Instance
        );
        cfg.AssertConfigurationIsValid();
        _mapper = cfg.CreateMapper();
    }

    /// <summary>No debe permitir crear un comprobante si el contribuyente no existe.</summary>
    [Fact]
    public async Task CreateAsync_Should_Throw_When_Owner_NotFound()
    {
        var dto = Fakes.NewInvoiceCreateDto("99999999999", "E310000000010");
        _contributorRepository.Setup(r => r.GetByIdAsync(dto.TaxId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Contributor?)null);

        var sut = new InvoiceService(_invoiceRepository.Object, _contributorRepository.Object, _logger.Object, _mapper);

        var act = async () => await sut.CreateAsync(dto, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage($"No existe el contribuyente {dto.TaxId}.");
    }

    /// <summary>Se debe crear un comprobante cuando el dueño existe y no hay duplicados.</summary>
    [Fact]
    public async Task CreateAsync_Should_Create_When_Ok()
    {
        var dto = Fakes.NewInvoiceCreateDto("98754321012", "E310000000003");
        _contributorRepository.Setup(r => r.GetByIdAsync(dto.TaxId, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(Fakes.NewContributor(dto.TaxId));
        _invoiceRepository.Setup(r => r.GetAsync(dto.TaxId, dto.Ncf, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Invoice?)null);
        _invoiceRepository.Setup(r => r.AddAsync(It.IsAny<Invoice>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        var sut = new InvoiceService(_invoiceRepository.Object, _contributorRepository.Object, _logger.Object, _mapper);

        var created = await sut.CreateAsync(dto, CancellationToken.None);

        created.TaxId.Should().Be(dto.TaxId);
        created.Ncf.Should().Be(dto.Ncf);
        _invoiceRepository.Verify(r => r.AddAsync(It.Is<Invoice>(i => i.Contributor!.TaxId == dto.TaxId && i.Ncf == dto.Ncf), It.IsAny<CancellationToken>()), Times.Once);
    }

    /// <summary>Se debe devolver null cuando el comprobante no existe.</summary>
    [Fact]
    public async Task UpdateAsync_Should_Return_Null_When_NotFound()
    {
        var taxId = "98754321012";
        var ncf = "E310000000004";
        _invoiceRepository.Setup(r => r.GetAsync(taxId, ncf, It.IsAny<CancellationToken>()))
                 .ReturnsAsync((Invoice?)null);

        var sut = new InvoiceService(_invoiceRepository.Object, _contributorRepository.Object, _logger.Object, _mapper);

        var result = await sut.UpdateAsync(taxId, ncf, Fakes.NewInvoiceUpdateDto(), CancellationToken.None);

        result.Should().BeNull();
    }
}