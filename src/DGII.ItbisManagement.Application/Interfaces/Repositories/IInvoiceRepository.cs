using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repositorio para operaciones CRUD de comprobantes fiscales (facturas).
    /// </summary>
    public interface IInvoiceRepository
    {
        /// <summary>
        /// Obtiene todos los comprobantes del sistema.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Lista de <see cref="Invoice"/>.</returns>
        Task<IReadOnlyList<Invoice>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene los comprobantes asociados a un contribuyente.
        /// </summary>
        /// <param name="taxId">RNC/Cédula del contribuyente.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>Listado de <see cref="Invoice"/>.</returns>
        Task<IReadOnlyList<Invoice>> GetByContributorAsync(string taxId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene un comprobante por clave(TaxId + NCF).
        /// </summary>
        /// <param name="taxId">RNC/Cédula del contribuyente.</param>
        /// <param name="ncf">Número de comprobante fiscal.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>La factura encontrada o <c>null</c> si no existe.</returns>
        Task<Invoice?> GetAsync(string taxId, string ncf, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserta un nuevo comprobante fiscal.
        /// </summary>
        /// <param name="invoice">Entidad a insertar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <remarks>El (ContributorId, Ncf) debe ser único.</remarks>
        Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza los datos de un comprobante fiscal existente.
        /// </summary>
        /// <param name="invoice">Entidad con cambios.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina un comprobante por su clave(TaxId + NCF).
        /// </summary>
        /// <param name="taxId">RNC/Cédula del contribuyente.</param>
        /// <param name="ncf">Número de comprobante.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task DeleteAsync(string taxId, string ncf, CancellationToken cancellationToken = default);
    }
}
