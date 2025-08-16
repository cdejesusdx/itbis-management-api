using AutoMapper;

using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Application.Services;

/// <summary>Servicio para gestionar contribuyentes y sus comprobantes.</summary>
public class ContributorService : IContributorService
{
    private readonly IContributorRepository _contributorRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;

    /// <summary>Constructor con la inyección de dependencias.</summary>
    public ContributorService(IContributorRepository contributorRepository,
                              IInvoiceRepository invoiceRepository, IMapper mapper)
    {
        _contributorRepository = contributorRepository;
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
    }

    /// <summary>Obtiene todos los contribuyentes registrados.</summary>
    public async Task<IReadOnlyList<ContributorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var contributors = await _contributorRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ContributorDto>>(contributors);
    }

    /// <summary>Obtiene un contribuyente y sus comprobantes con el ITBIS total.</summary>
    public async Task<ContributorWithInvoicesDto?> GetWithInvoicesAsync(string taxId, CancellationToken cancellationToken = default)
    {
        var contributor = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
        if (contributor is null) return null;

        var invoices = await _invoiceRepository.GetByContributorAsync(taxId, cancellationToken);

        return new ContributorWithInvoicesDto
        {
            Contributor = _mapper.Map<ContributorDto>(contributor),
            Invoices = _mapper.Map<List<InvoiceDto>>(invoices),
            TotalItbis = invoices.Sum(i => i.Itbis18)
        };
    }
}