using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace DGII.ItbisManagement.Application.DTOs;

/// <summary>Representa la información básica de un contribuyente para el API.</summary>
public class ContributorDto
{
    /// <summary>Identificación fiscal (RNC o cédula).</summary>
    [XmlElement("rncCedula")]
    [JsonPropertyName("rncCedula")]
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Nombre del contribuyente.</summary>
    [XmlElement("nombre")]
    [JsonPropertyName("nombre")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Tipo de contribuyente en texto (PERSONA FISICA / PERSONA JURIDICA).</summary>
    [XmlElement("tipo")]
    [JsonPropertyName("tipo")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Estatus en texto (activo / inactivo).</summary>
    [XmlElement("estatus")]
    [JsonPropertyName("estatus")]
    public string Status { get; set; } = string.Empty;
}

/// <summary>Datos requeridos para crear un contribuyente.</summary>
public class ContributorCreateDto
{
    /// <summary>Identificación fiscal (RNC o cédula).</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("rncCedula"), XmlElement("rncCedula")]
    public string TaxId { get; set; } = string.Empty;

    /// <summary>Nombre del contribuyente.</summary>
    [Required, StringLength(200)]
    [JsonPropertyName("nombre"), XmlElement("nombre")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Tipo (PERSONA FISICA / PERSONA JURIDICA).</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("tipo"), XmlElement("tipo")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Estatus (activo / inactivo).</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("estatus"), XmlElement("estatus")]
    public string Status { get; set; } = string.Empty;
}

/// <summary>Datos para actualizar un contribuyente existente.</summary>
public class ContributorUpdateDto
{
    /// <summary>Nombre del contribuyente.</summary>
    [Required, StringLength(200)]
    [JsonPropertyName("nombre"), XmlElement("nombre")]
    public string Name { get; set; } = string.Empty;

    /// <summary>Tipo (PERSONA FISICA / PERSONA JURIDICA).</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("tipo"), XmlElement("tipo")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Estatus (activo / inactivo).</summary>
    [Required, StringLength(20)]
    [JsonPropertyName("estatus"), XmlElement("estatus")]
    public string Status { get; set; } = string.Empty;
}