using AutoMapper;
using DGII.ItbisManagement.Domain.Enums;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>
/// Convierte string en español a enum ContributorType.
/// </summary>
public sealed class StringToContributorTypeConverter : IValueConverter<string, ContributorType>
{
    public ContributorType Convert(string sourceMember, ResolutionContext context)
    {
        var s = (sourceMember ?? string.Empty).Trim().ToUpperInvariant();
        return s switch
        {
            "PERSONA FISICA" => ContributorType.Individual,
            "PERSONA JURIDICA" => ContributorType.LegalEntity,
            _ => throw new AutoMapperMappingException($"Tipo de contribuyente inválido: '{sourceMember}'")
        };
    }
}
