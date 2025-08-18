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
        var s = (sourceMember ?? string.Empty).Trim().ToLowerInvariant();

        return s switch
        {
            "persona fisica" => ContributorType.Individual,
            "persona física" => ContributorType.Individual,
            "persona juridica" => ContributorType.LegalEntity,
            "persona jurídica" => ContributorType.LegalEntity,
            _ => throw new ArgumentException($"Tipo de contribuyente no válido: '{sourceMember}'.")
        };
    }
}
