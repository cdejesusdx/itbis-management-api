using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Infrastructure.Persistence;

/// <summary>
/// Almacén en memoria para desarrollo y pruebas locales.
/// </summary>
public class InMemoryDataStore
{
    /// <summary>Contribuyentes en memoria.</summary>
    public List<Contributor> Contributors { get; } = new();

    /// <summary>Comprobantes en memoria.</summary>
    public List<Invoice> Invoices { get; } = new();

    /// <summary>
    /// Reemplaza las colecciones con los conjuntos provistos.
    /// </summary>
    public void SetData(IEnumerable<Contributor> contributors, IEnumerable<Invoice> invoices)
    {
        Contributors.Clear(); 
        Invoices.Clear();

        Contributors.AddRange(contributors);
        Invoices.AddRange(invoices);
    }
}