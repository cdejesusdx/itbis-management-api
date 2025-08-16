using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DGII.ItbisManagement.Application.DTOs;

/// <summary>Representa un comprobante fiscal para el API.</summary>
public class InvoiceDto
{
    /// <summary>Identificación fiscal del contribuyente dueño del comprobante.</summary>
    [JsonPropertyName("rncCedula")]
    [XmlElement("rncCedula")]
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Número de comprobante fiscal.</summary>
    [JsonPropertyName("NCF")]
    [XmlElement("NCF")]
    public string Ncf { get; set; } = string.Empty;

    /// <summary>Monto total de la factura.</summary>
    [JsonPropertyName("monto")]
    [XmlElement("monto")]
    public decimal Amount { get; set; }

    /// <summary>Monto de ITBIS (18%).</summary>
    [JsonPropertyName("itbis18")]
    [XmlElement("itbis18")]
    public decimal Itbis18 { get; set; }
}
