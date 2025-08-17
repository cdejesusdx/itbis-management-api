using AutoMapper;
using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Mappings.Converters;
using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Perfil de mapeo para contribuyentes.</summary>
public sealed class ContributorProfile : Profile
{
    public ContributorProfile()
    {
        var enumToEs = new EnumToSpanishConverters();
        var strToType = new StringToContributorTypeConverter();
        var strToStatus = new StringToStatusConverter();

        // Entity -> DTO
        CreateMap<Contributor, ContributorDto>()
            .ForMember(d => d.Type, m => m.ConvertUsing(enumToEs, s => s.Type))
            .ForMember(d => d.Status, m => m.ConvertUsing(enumToEs, s => s.Status));

        // Create DTO -> Entity
        CreateMap<ContributorCreateDto, Contributor>()
            .ForMember(d => d.Type, m => m.ConvertUsing(strToType, s => s.Type))
            .ForMember(d => d.Status, m => m.ConvertUsing(strToStatus, s => s.Status));

        // Update DTO -> Entity
        CreateMap<ContributorUpdateDto, Contributor>()
            .ForMember(d => d.Type, m => m.ConvertUsing(strToType, s => s.Type))
            .ForMember(d => d.Status, m => m.ConvertUsing(strToStatus, s => s.Status));
    }
}