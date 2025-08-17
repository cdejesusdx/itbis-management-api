using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
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

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ContributorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _contributorRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<ContributorDto>>(entities);
    }

    /// <inheritdoc/>
    public async Task<ContributorWithInvoicesDto?> GetWithInvoicesAsync(string taxId, CancellationToken cancellationToken = default)
    {
        var contrib = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
        if (contrib is null) return null;

        var inv = await _invoiceRepository.GetByContributorAsync(taxId, cancellationToken);
        return new ContributorWithInvoicesDto
        {
            Contributor = _mapper.Map<ContributorDto>(contrib),
            Invoices = _mapper.Map<List<InvoiceDto>>(inv),
            TotalItbis = inv.Sum(i => i.Itbis18)
        };
    }

    /// <inheritdoc/>
    public async Task<ContributorDto?> GetAsync(string taxId, CancellationToken cancellationToken = default)
    {
        var entity = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
        return entity is null ? null : _mapper.Map<ContributorDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<ContributorDto> CreateAsync(ContributorCreateDto dto, CancellationToken cancellationToken = default)
    {
        // Validar duplicado por TaxId
        var exists = await _contributorRepository.GetByIdAsync(dto.TaxId, cancellationToken);
        if (exists is not null)
            throw new InvalidOperationException($"Ya existe el contribuyente con RNC/Cédula {dto.TaxId}");

        var entity = _mapper.Map<Contributor>(dto);
        await _contributorRepository.AddAsync(entity, cancellationToken);

        return _mapper.Map<ContributorDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<ContributorDto?> UpdateAsync(string taxId, ContributorUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
        if (entity is null) return null;

        _mapper.Map(dto, entity);
        await _contributorRepository.UpdateAsync(entity, cancellationToken);

        return _mapper.Map<ContributorDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string taxId, CancellationToken cancellationToken = default)
    {
        var entity = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
        if (entity is null) return false;

        await _contributorRepository.DeleteAsync(taxId, cancellationToken);
        return true;
    }
}