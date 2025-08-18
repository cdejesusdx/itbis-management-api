using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Tests.TestHelpers;

/// <summary>
/// Utilidades para datos de prueba.
/// </summary>
public static class Fakes
{
    public static Contributor NewContributor(string taxId = "98754321012")
        => new Contributor { TaxId = taxId, Name = "JUAN PEREZ", Type = Domain.Enums.ContributorType.Individual, Status = Domain.Enums.Status.Active };

    public static Invoice NewInvoice(string ncf, decimal amount, decimal itbis)
        => new Invoice { Ncf = ncf, Amount = amount, Itbis18 = itbis };

    public static Invoice NewInvoice(string taxId, string ncf, decimal amount, decimal itbis)
    => new Invoice
    {
        Ncf = ncf,
        Amount = amount,
        Itbis18 = itbis,
        Contributor = new Contributor { TaxId = taxId, Name = "Dummy" }
    };


    public static ContributorCreateDto NewContributorCreateDto(string taxId = "40212345678")
        => new ContributorCreateDto { TaxId = taxId, Name = "COMERCIAL ABC", Type = "PERSONA JURIDICA", Status = "activo" };

    public static ContributorUpdateDto NewContributorUpdateDto()
        => new ContributorUpdateDto { Name = "COMERCIAL ABC SRL", Type = "PERSONA JURIDICA", Status = "activo" };

    public static InvoiceCreateDto NewInvoiceCreateDto(string taxId = "98754321012", string ncf = "E310000000003")
        => new InvoiceCreateDto { TaxId = taxId, Ncf = ncf, Amount = 500m, Itbis18 = 90m };

    public static InvoiceUpdateDto NewInvoiceUpdateDto()
        => new InvoiceUpdateDto { Amount = 750m, Itbis18 = 135m };

    public static List<Invoice> SampleInvoicesForJuan()
        => new()
        {
            NewInvoice("98754321012","E310000000001", 200m, 36m),
            NewInvoice("98754321012","E310000000002", 1000m, 180m)
        };
}
