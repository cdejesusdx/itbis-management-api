using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Mappings.Converters;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Perfil de mapeos para Contributor.</summary>
public class ContributorProfile : Profile
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
        // Ignorar propiedades del tracking/auditoría y colecciones de navegación.
        CreateMap<ContributorCreateDto, Contributor>()
            .ForMember(d => d.Type, m => m.ConvertUsing(strToType, s => s.Type))
            .ForMember(d => d.Status, m => m.ConvertUsing(strToStatus, s => s.Status))
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.Invoices, m => m.Ignore())
            .ForMember(d => d.CreateBy, m => m.Ignore())
            .ForMember(d => d.Created, m => m.Ignore())
            .ForMember(d => d.UpdateBy, m => m.Ignore())
            .ForMember(d => d.Updated, m => m.Ignore());
        // Las propiedades: TaxId, Name, Type, Status se mapean por convención

        // Update DTO -> Entity
        CreateMap<ContributorUpdateDto, Contributor>()
            .ForMember(d => d.Type, m => m.ConvertUsing(strToType, s => s.Type))
            .ForMember(d => d.Status, m => m.ConvertUsing(strToStatus, s => s.Status))
            .ForMember(d => d.TaxId, m => m.Ignore())
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.Invoices, m => m.Ignore())
            .ForMember(d => d.CreateBy, m => m.Ignore())
            .ForMember(d => d.Created, m => m.Ignore())
            .ForMember(d => d.UpdateBy, m => m.Ignore())
            .ForMember(d => d.Updated, m => m.Ignore());
    }
}

///// <summary>Perfil de mapeo para contribuyentes.</summary>
//public sealed class ContributorProfile : Profile
//{
//    public ContributorProfile()
//    {
//        var enumToEs = new EnumToSpanishConverters();
//        var strToType = new StringToContributorTypeConverter();
//        var strToStatus = new StringToStatusConverter();

//        // Entity -> DTO
//        CreateMap<Contributor, ContributorDto>()
//            .ForMember(d => d.Type, m => m.ConvertUsing(enumToEs, s => s.Type))
//            .ForMember(d => d.Status, m => m.ConvertUsing(enumToEs, s => s.Status));

//        // Create DTO -> Entity
//        CreateMap<ContributorCreateDto, Contributor>()
//            .ForMember(d => d.Type, m => m.ConvertUsing(strToType, s => s.Type))
//            .ForMember(d => d.Status, m => m.ConvertUsing(strToStatus, s => s.Status));

//        // Update DTO -> Entity
//        CreateMap<ContributorUpdateDto, Contributor>()
//            .ForMember(d => d.Type, m => m.ConvertUsing(strToType, s => s.Type))
//            .ForMember(d => d.Status, m => m.ConvertUsing(strToStatus, s => s.Status));
//    }
//}