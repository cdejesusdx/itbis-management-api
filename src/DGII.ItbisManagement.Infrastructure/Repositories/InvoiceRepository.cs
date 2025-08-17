using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Infrastructure.Persistence;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Infrastructure.Repositories;

/// <summary>
/// Implementación de repositorio de comprobantes fiscales usando EF Core.
/// Centrado en consultas por TaxId y NCF.
/// </summary>
/// <remarks>Crea una nueva instancia del repositorio.</remarks>
/// <param name="context">Contexto de datos de EF Core.</param>
/// <param name="logger"></param>
public class InvoiceRepository(ItbisDbContext context, ILogger<InvoiceRepository> logger) : IInvoiceRepository
{
    private readonly ItbisDbContext _context = context;
    private readonly ILogger<InvoiceRepository> _logger = logger;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Invoice>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Invoices
                 .AsNoTracking()
                 .Include(i => i.Contributor)
                 .OrderBy(i => i.Ncf)
                 .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar comprobantes");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Invoice>> GetByContributorAsync(string taxId, CancellationToken cancellationToken = default) 
    {
        try
        {
            return await _context.Invoices
                 .AsNoTracking()
                 .Include(i => i.Contributor)
                 .Where(i => i.Contributor != null && i.Contributor.TaxId == taxId)
                 .OrderBy(i => i.Ncf)
                 .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar comprobantes por contribuyente {TaxId}", taxId);
            throw;
        }
    }

    /// <inheritdoc/>
    public Task<Invoice?> GetAsync(string taxId, string ncf, CancellationToken cancellationToken = default) 
    {
        try
        {
            return _context.Invoices
                   .AsNoTracking()
                   .FirstOrDefaultAsync(i => i.Contributor != null &&
                                     i.Contributor.TaxId == taxId &&
                                     i.Ncf == ncf, cancellationToken);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener comprobante {TaxId}-{NCF}", taxId, ncf);
            throw;
        }
    }
       
    /// <inheritdoc/>
    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.Invoices.AddAsync(invoice, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al crear comprobante {TaxId}-{NCF}", invoice.Contributor!.TaxId, invoice.Ncf);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear comprobante {TaxId}-{NCF}", invoice.Contributor!.TaxId, invoice.Ncf);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrencia al actualizar comprobante {TaxId}-{NCF}", invoice.Contributor!.TaxId, invoice.Ncf);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar comprobante {TaxId}-{NCF}", invoice.Contributor!.TaxId, invoice.Ncf);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string taxId, string ncf, CancellationToken cancellationToken = default)
    {
        try
        {
            var invoice = await _context.Invoices
                              .FirstOrDefaultAsync(i => i.Contributor != null &&
                                                        i.Contributor.TaxId == taxId &&
                                                        i.Ncf == ncf, cancellationToken);
            if (invoice is null) return;

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al eliminar comprobante {TaxId}-{NCF}", taxId, ncf);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar comprobante {TaxId}-{NCF}", taxId, ncf);
            throw;
        }
    }
}