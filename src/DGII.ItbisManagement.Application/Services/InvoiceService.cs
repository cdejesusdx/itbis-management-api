using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Application.Services;

/// <summary>Servicio de aplicación para gestionar comprobantes fiscales.</summary>
/// <remarks>Crea una nueva instancia del servicio de facturas.</remarks>
public class InvoiceService(IInvoiceRepository invoiceRepository, IContributorRepository contributorRepository,
    IMapper mapper) : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
    private readonly IContributorRepository _contributorRepository = contributorRepository;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<InvoiceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _invoiceRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<IReadOnlyList<InvoiceDto>>(entities);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<InvoiceDto>> GetByContributorAsync(string taxId, CancellationToken cancellationToken = default)
    {
        var entities = await _invoiceRepository.GetByContributorAsync(taxId, cancellationToken);
        return _mapper.Map<IReadOnlyList<InvoiceDto>>(entities);
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto?> GetAsync(string taxId, string ncf, CancellationToken cancellationToken = default)
    {
        var entity = await _invoiceRepository.GetAsync(taxId, ncf, cancellationToken);
        return entity is null ? null : _mapper.Map<InvoiceDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto, CancellationToken cancellationToken = default)
    {
        // Validar existencia del contribuyente
        var contributor = await _contributorRepository.GetByIdAsync(dto.TaxId, cancellationToken)
                      ?? throw new KeyNotFoundException($"Contribuyente {dto.TaxId} no encontrado");

        // Validar que no exista el mismo NCF para ese contribuyente
        var exists = await _invoiceRepository.GetAsync(dto.TaxId, dto.Ncf, cancellationToken);
        if (exists is not null)
            throw new InvalidOperationException($"Ya existe el comprobante {dto.Ncf} para {dto.TaxId}");

        var entity = new Invoice
        {
            ContributorId = contributor.Id,
            Contributor = contributor, // Esto es opcional
            Ncf = dto.Ncf,
            Amount = dto.Amount,
            Itbis18 = dto.Itbis18
        };

        await _invoiceRepository.AddAsync(entity, cancellationToken);

        // Mapear a DTO de salida
        return _mapper.Map<InvoiceDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto?> UpdateAsync(string taxId, string ncf, InvoiceUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _invoiceRepository.GetAsync(taxId, ncf, cancellationToken);
        if (entity is null) return null;

        entity.Amount = dto.Amount;
        entity.Itbis18 = dto.Itbis18;

        await _invoiceRepository.UpdateAsync(entity, cancellationToken);

        return _mapper.Map<InvoiceDto>(entity);
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string taxId, string ncf, CancellationToken cancellationToken = default)
    {
        var entity = await _invoiceRepository.GetAsync(taxId, ncf, cancellationToken);
        if (entity is null) return false;

        await _invoiceRepository.DeleteAsync(taxId, ncf, cancellationToken);
        return true;
    }
}
