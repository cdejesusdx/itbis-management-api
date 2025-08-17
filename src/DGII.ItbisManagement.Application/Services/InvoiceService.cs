using AutoMapper;
using Microsoft.Extensions.Logging;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Application.Services;

/// <summary>Servicio de aplicación para gestionar comprobantes fiscales.</summary>
/// <remarks>Crea una nueva instancia del servicio de facturas.</remarks>
public class InvoiceService(IInvoiceRepository invoiceRepository, IContributorRepository contributorRepository,
    ILogger<InvoiceService> logger, IMapper mapper) : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
    private readonly IContributorRepository _contributorRepository = contributorRepository;
    private readonly ILogger<InvoiceService> _logger = logger;
    private readonly IMapper _mapper = mapper;
   
    /// <inheritdoc/>
    public async Task<IReadOnlyList<InvoiceDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var invoice = await _invoiceRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<InvoiceDto>>(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al listar comprobantes");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<InvoiceDto>> GetByContributorAsync(string taxId, CancellationToken cancellationToken = default)
    {
        try
        {
            var invoices = await _invoiceRepository.GetByContributorAsync(taxId, cancellationToken);
            return _mapper.Map<IReadOnlyList<InvoiceDto>>(invoices);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al listar comprobantes de {TaxId}", taxId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto?> GetAsync(string taxId, string ncf, CancellationToken cancellationToken = default)
    {
        try
        {
            var invoice = await _invoiceRepository.GetAsync(taxId, ncf, cancellationToken);
            return invoice is null ? null : _mapper.Map<InvoiceDto>(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al obtener comprobante {TaxId}-{NCF}", taxId, ncf);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validar existencia del contribuyente
            var contributor = await _contributorRepository.GetByIdAsync(dto.TaxId, cancellationToken);
            if (contributor is null)
            {
                _logger.LogWarning("Intento de crear comprobante para contribuyente inexistente {TaxId}", dto.TaxId);
                throw new InvalidOperationException($"No existe el contribuyente {dto.TaxId}.");
            }

            // Validar que no exista el mismo NCF para ese contribuyente
            var invoice = await _invoiceRepository.GetAsync(dto.TaxId, dto.Ncf, cancellationToken);
            if (invoice is not null)
            {
                _logger.LogWarning("Intento de crear comprobante existente {TaxId}-{NCF}", dto.TaxId, dto.Ncf);
                throw new InvalidOperationException("El comprobante ya existe.");
            }

            var newInvoice = new Invoice
            {
                ContributorId = contributor.Id,
                Contributor = contributor,
                Ncf = dto.Ncf,
                Amount = dto.Amount,
                Itbis18 = dto.Itbis18
            };

            await _invoiceRepository.AddAsync(newInvoice, cancellationToken);
            _logger.LogInformation("Comprobante {TaxId}-{NCF} creado", dto.TaxId, dto.Ncf);

            return _mapper.Map<InvoiceDto>(newInvoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al crear comprobante {TaxId}-{NCF}", dto.TaxId, dto.Ncf);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<InvoiceDto?> UpdateAsync(string taxId, string ncf, InvoiceUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var invoice = await _invoiceRepository.GetAsync(taxId, ncf, cancellationToken);
            if (invoice is null) return null;

            invoice.Amount = dto.Amount;
            invoice.Itbis18 = dto.Itbis18;

            await _invoiceRepository.UpdateAsync(invoice, cancellationToken);

            return _mapper.Map<InvoiceDto>(invoice);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al actualizar comprobante {TaxId}-{NCF}", taxId, ncf);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string taxId, string ncf, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await _invoiceRepository.GetAsync(taxId, ncf, cancellationToken);
            if (entity is null) return false;

            await _invoiceRepository.DeleteAsync(taxId, ncf, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al eliminar comprobante {TaxId}-{NCF}", taxId, ncf);
            throw;
        }
    }
}
