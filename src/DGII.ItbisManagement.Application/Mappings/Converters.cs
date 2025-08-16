using AutoMapper;

using DGII.ItbisManagement.Domain.Enums;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Conversores de enums a texto en español.</summary>
public sealed class EnumToSpanishConverters :
    IValueConverter<ContributorType, string>,
    IValueConverter<Status, string>
{
    public string Convert(ContributorType sourceMember, ResolutionContext context) =>
        sourceMember switch
        {
            ContributorType.Individual => "PERSONA FISICA",
            ContributorType.LegalEntity => "PERSONA JURIDICA",
            _ => string.Empty
        };

    public string Convert(Status sourceMember, ResolutionContext context) =>
        sourceMember switch
        {
            Status.Active => "Activo",
            Status.Inactive => "Inactivo",
            _ => string.Empty
        };
}