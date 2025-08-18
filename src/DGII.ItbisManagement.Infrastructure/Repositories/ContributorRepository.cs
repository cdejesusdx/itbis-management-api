using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Infrastructure.Persistence;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Infrastructure.Repositories;

/// <summary>
/// Implementación de repositorio de contribuyentes usando EF Core.
/// Provee operaciones CRUD y consultas.
/// </summary>
/// <remarks>Crea una nueva instancia del repositorio.</remarks>
/// <param name="context">Contexto de datos de EF Core.</param>
/// <param name="logger"></param>
public class ContributorRepository(ItbisDbContext context, ILogger<ContributorRepository> logger) : IContributorRepository
{
    private readonly ItbisDbContext _context = context;
    private readonly ILogger<ContributorRepository> _logger = logger;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<Contributor>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.Contributors
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener el listado de contribuyentes");
            throw;
        }
    }

    /// <inheritdoc/>
    public Task<Contributor?> GetByIdAsync(string taxId, CancellationToken cancellationToken = default) 
    {
        try
        {
           return _context.Contributors.AsNoTracking().FirstOrDefaultAsync(c => c.TaxId == taxId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener contribuyente {TaxId}", taxId);
            throw;
        }
    } 

    /// <inheritdoc/>
    public async Task AddAsync(Contributor contributor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Contributors.Add(contributor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al crear contribuyente {TaxId}", contributor.TaxId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear contribuyente {TaxId}", contributor.TaxId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task UpdateAsync(Contributor contributor, CancellationToken cancellationToken = default)
    {
        try
        {
            _context.Contributors.Update(contributor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Conflicto de concurrencia al actualizar {TaxId}", contributor.TaxId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar contribuyente {TaxId}", contributor.TaxId);
            throw;
        }  
    }

    /// <inheritdoc/>
    public async Task DeleteAsync(string taxId, CancellationToken cancellationToken = default)
    {
        try
        {
            var contributor = await _context.Contributors.FirstOrDefaultAsync(c => c.TaxId == taxId, cancellationToken);
            if (contributor is null) return;

            _context.Contributors.Remove(contributor);
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error de base de datos al eliminar {TaxId}", taxId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar contribuyente {TaxId}", taxId);
            throw;
        }
    }
}