using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Interfaces.Services;

/// <summary>
/// Contrato para servicios para operaciones de contribuyentes.
/// </summary>
public interface IContributorService
{
    /// <summary>Obtiene todos los contribuyentes.</summary>
    Task<IReadOnlyList<ContributorDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Obtiene un contribuyente por su RNC/Cédula con sus facturas e ITBIS.</summary>
    Task<ContributorWithInvoicesDto?> GetWithInvoicesAsync(string taxId, CancellationToken cancellationToken = default);

    /// <summary>Obtiene un contribuyente por su RNC/Cédula.</summary>
    Task<ContributorDto?> GetAsync(string taxId, CancellationToken cancellationToken = default);

    /// <summary>Crea un nuevo contribuyente.</summary>
    Task<ContributorDto> CreateAsync(ContributorCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Actualiza un contribuyente existente.</summary>
    Task<ContributorDto?> UpdateAsync(string taxId, ContributorUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Elimina un contribuyente por su RNC/Cédula.</summary>
    Task<bool> DeleteAsync(string taxId, CancellationToken cancellationToken = default);
}