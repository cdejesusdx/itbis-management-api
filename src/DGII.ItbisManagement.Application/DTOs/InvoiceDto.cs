using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DGII.ItbisManagement.Application.DTOs;

/// <summary>Representa un comprobante fiscal para el API.</summary>
public class InvoiceDto
{
    /// <summary>Identificación fiscal del contribuyente dueño del comprobante.</summary>
    [XmlElement("rncCedula")]
    [JsonPropertyName("rncCedula")]
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Número de comprobante fiscal.</summary>
    [XmlElement("NCF")]
    [JsonPropertyName("NCF")]
    public string Ncf { get; set; } = string.Empty;

    /// <summary>Monto total de la factura.</summary>
    [XmlElement("monto")]
    [JsonPropertyName("monto")]
    public decimal Amount { get; set; }

    /// <summary>Monto de ITBIS (18%).</summary>
    [XmlElement("itbis18")]
    [JsonPropertyName("itbis18")]
    public decimal Itbis18 { get; set; }
}

/// <summary>Datos requeridos para crear un comprobante fiscal.</summary>
public class InvoiceCreateDto
{
    /// <summary>RNC/Cédula del contribuyente propietario.</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("rncCedula"), XmlElement("rncCedula")]
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Número de comprobante fiscal (NCF).</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("NCF"), XmlElement("NCF")]
    public string Ncf { get; set; } = string.Empty;

    /// <summary>Monto de la factura.</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("monto"), XmlElement("monto")]
    public decimal Amount { get; set; }

    /// <summary>Valor de ITBIS (18%).</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("itbis18"), XmlElement("itbis18")]
    public decimal Itbis18 { get; set; }
}

/// <summary>Datos para actualizar un comprobante fiscal existente.</summary>
public class InvoiceUpdateDto
{
    /// <summary>Monto de la factura.</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("monto"), XmlElement("monto")]
    public decimal Amount { get; set; }

    /// <summary>Valor de ITBIS (18%).</summary>
    [Range(0, double.MaxValue)]
    [JsonPropertyName("itbis18"), XmlElement("itbis18")]
    public decimal Itbis18 { get; set; }
}