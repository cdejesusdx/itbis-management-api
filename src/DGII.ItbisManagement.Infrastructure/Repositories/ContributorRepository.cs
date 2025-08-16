using Microsoft.EntityFrameworkCore;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.Interfaces;
using DGII.ItbisManagement.Infrastructure.Persistence;

namespace DGII.ItbisManagement.Infrastructure.Repositories;

/// <summary>
/// Implementación de repositorio de contribuyentes usando EF Core.
/// Provee operaciones CRUD y consultas.
/// </summary>
/// <remarks>Crea una nueva instancia del repositorio.</remarks>
/// <param name="context">Contexto de datos de EF Core.</param>
public class ContributorRepository(ItbisDbContext context) : IContributorRepository
{
    private readonly ItbisDbContext _context = context;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Contributor>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Contributors
                 .AsNoTracking()
                 .OrderBy(c => c.Name)
                 .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<Contributor?> GetByIdAsync(string taxId, CancellationToken cancellationToken = default) =>
        _context.Contributors
           .AsNoTracking()
           .FirstOrDefaultAsync(c => c.TaxId == taxId, cancellationToken);

    /// <inheritdoc/>
    public async Task AddAsync(Contributor contributor, CancellationToken cancellationToken = default)
    {
        await _context.Contributors.AddAsync(contributor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Contributor contributor, CancellationToken cancellationToken = default)
    {
        _context.Contributors.Update(contributor);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string taxId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Contributors.FirstOrDefaultAsync(c => c.TaxId == taxId, cancellationToken);
        if (entity is null) return;

        _context.Contributors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}