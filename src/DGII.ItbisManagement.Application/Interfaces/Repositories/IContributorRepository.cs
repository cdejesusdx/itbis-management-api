using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repositorio para operaciones CRUD de contribuyentes.
    /// Encapsula el acceso a datos y permite suplantación en pruebas.
    /// </summary>
    public interface IContributorRepository
    {
        /// <summary>
        /// Obtiene el listado completo de contribuyentes ordenado por nombre.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación de la operación.</param>
        /// <returns>Lista de <see cref="Contributor"/>.</returns>
        Task<IReadOnlyList<Contributor>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Busca un contribuyente por su identificador (RNC/Cédula).
        /// </summary>
        /// <param name="taxId">RNC o Cédula del contribuyente.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>El <see cref="Contributor"/> encontrado o <c>null</c> si no existe.</returns>
        Task<Contributor?> GetByIdAsync(string taxId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Crea un nuevo contribuyente.
        /// </summary>
        /// <param name="contributor">Entidad a agregar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <remarks>El campo <c>TaxId</c> debe ser único; la base de datos valida por índice único.</remarks>
        Task AddAsync(Contributor contributor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza un contribuyente existente.
        /// </summary>
        /// <param name="contributor">Entidad con los cambios.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        Task UpdateAsync(Contributor contributor, CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina un contribuyente por su identificador.
        /// </summary>
        /// <param name="taxId">RNC o Cédula del contribuyente.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <remarks>Si no existe, la operación no realiza cambios.</remarks>
        Task DeleteAsync(string taxId, CancellationToken cancellationToken = default);
    }
}