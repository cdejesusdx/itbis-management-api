using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Interfaces.Services;

/// <summary>
/// Contrato para los servicios relacionados con contribuyentes y comprobantes fiscales.
/// </summary>
public interface IContributorService
{
    /// <summary>
    /// Obtiene el listado de todos los contribuyentes registrados.
    /// </summary>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>Listado de contribuyentes en formato DTO.</returns>
    Task<IReadOnlyList<ContributorDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene la información de un contribuyente específico junto con sus comprobantes fiscales.
    /// </summary>
    /// <param name="taxId">Identificación fiscal (RNC o cédula).</param>
    /// <param name="cancellationToken">Token para cancelar la operación asíncrona.</param>
    /// <returns>DTO con el contribuyente, comprobantes e ITBIS total.</returns>
    Task<ContributorWithInvoicesDto?> GetWithInvoicesAsync(string taxId, CancellationToken cancellationToken = default);
}
