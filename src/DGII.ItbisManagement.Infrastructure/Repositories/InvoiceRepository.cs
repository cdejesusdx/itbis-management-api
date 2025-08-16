using Microsoft.EntityFrameworkCore;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Infrastructure.Persistence;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Infrastructure.Repositories;

/// <summary>
/// Implementación de repositorio de comprobantes fiscales usando EF Core.
/// Centrado en consultas por TaxId y NCF.
/// </summary>
public class InvoiceRepository : IInvoiceRepository
{
    private readonly ItbisDbContext _context;

    /// <summary>Crea una nueva instancia del repositorio.</summary>
    /// <param name="db">Contexto de datos de EF Core.</param>
    public InvoiceRepository(ItbisDbContext context) => _context = context;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Invoice>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Invoices
                 .AsNoTracking()
                 .OrderBy(i => i.Ncf)
                 .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Invoice>> GetByContributorAsync(string taxId, CancellationToken cancellationToken = default) =>
        await _context.Invoices
                 .AsNoTracking()
                 .Where(i => i.Contributor != null && i.Contributor.TaxId == taxId)
                 .OrderBy(i => i.Ncf)
                 .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<Invoice?> GetAsync(string taxId, string ncf, CancellationToken cancellationToken = default) =>
        _context.Invoices
           .AsNoTracking()
           .FirstOrDefaultAsync(i => i.Contributor != null &&
                                     i.Contributor.TaxId == taxId &&
                                     i.Ncf == ncf, cancellationToken);

    /// <inheritdoc/>
    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        // Si solo llega TaxId, resolver ContributorId antes de insertar.
        if (invoice.ContributorId == 0 && invoice.Contributor is not null && !string.IsNullOrWhiteSpace(invoice.Contributor.TaxId))
        {
            var id = await _context.Contributors
                              .Where(c => c.TaxId == invoice.Contributor.TaxId)
                              .Select(c => c.Id)
                              .SingleAsync(cancellationToken);

            invoice.ContributorId = id;
            invoice.Contributor = null;
        }

        await _context.Invoices.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string taxId, string ncf, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Invoices
                              .FirstOrDefaultAsync(i => i.Contributor != null &&
                                                        i.Contributor.TaxId == taxId &&
                                                        i.Ncf == ncf, cancellationToken);
        if (entity is null) return;

        _context.Invoices.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}