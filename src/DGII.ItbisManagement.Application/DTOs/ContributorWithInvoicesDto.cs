namespace DGII.ItbisManagement.Application.DTOs;

/// <summary>DTO que combina la información de un contribuyente con sus comprobantes e ITBIS total.</summary>
public class ContributorWithInvoicesDto
{
    /// <summary>Información básica del contribuyente.</summary>
    public ContributorDto Contributor { get; set; } = new ContributorDto();

    /// <summary>Listado de comprobantes fiscales del contribuyente.</summary>
    public List<InvoiceDto> Invoices { get; set; } = new();

    /// <summary>Suma total del ITBIS de todos los comprobantes.</summary>
    public decimal TotalItbis { get; set; }
}
