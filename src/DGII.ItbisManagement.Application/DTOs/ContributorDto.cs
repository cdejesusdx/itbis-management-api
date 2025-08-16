using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DGII.ItbisManagement.Application.DTOs;

/// <summary>Representa la información básica de un contribuyente para el API.</summary>
public class ContributorDto
{
    /// <summary>Identificación fiscal (RNC o cédula).</summary>
    [JsonPropertyName("rncCedula")]
    [XmlElement("rncCedula")]
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Nombre del contribuyente.</summary>
    [JsonPropertyName("nombre")]
    [XmlElement("nombre")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Tipo de contribuyente en texto (PERSONA FISICA / PERSONA JURIDICA).</summary>
    [JsonPropertyName("tipo")]
    [XmlElement("tipo")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Estatus en texto (activo / inactivo).</summary>
    [JsonPropertyName("estatus")]
    [XmlElement("estatus")]
    public string Status { get; set; } = string.Empty;
}
