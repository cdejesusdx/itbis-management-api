using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Interfaces.Services;

/// <summary>Contrato de servicios para operaciones de comprobantes fiscales (facturas).</summary>
public interface IInvoiceService
{
    /// <summary>Obtiene el listado completo de comprobantes.</summary>
    Task<IReadOnlyList<InvoiceDto>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>Obtiene los comprobantes de un contribuyente por RNC/Cédula.</summary>
    Task<IReadOnlyList<InvoiceDto>> GetByContributorAsync(string taxId, CancellationToken cancellationToken = default);

    /// <summary>Obtiene un comprobante por RNC/Cédula y NCF.</summary>
    Task<InvoiceDto?> GetAsync(string taxId, string ncf, CancellationToken cancellationToken = default);

    /// <summary>Crea un nuevo comprobante.</summary>
    Task<InvoiceDto> CreateAsync(InvoiceCreateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Actualiza un comprobante existente (monto/itbis) por RNC/Cédula y NCF.</summary>
    Task<InvoiceDto?> UpdateAsync(string taxId, string ncf, InvoiceUpdateDto dto, CancellationToken cancellationToken = default);

    /// <summary>Elimina un comprobante por RNC/Cédula y NCF.</summary>
    Task<bool> DeleteAsync(string taxId, string ncf, CancellationToken cancellationToken = default);
}
