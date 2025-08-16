using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Perfil de mapeo para contribuyentes.</summary>
public sealed class ContributorProfile : Profile
{
    public ContributorProfile()
    {
        var conv = new EnumToSpanishConverters();

        CreateMap<Contributor, ContributorDto>()
            .ForMember(d => d.TaxId, m => m.MapFrom(s => s.TaxId))
            .ForMember(d => d.Name, m => m.MapFrom(s => s.Name))
            .ForMember(d => d.Type, m => m.ConvertUsing(conv, s => s.Type))
            .ForMember(d => d.Status, m => m.ConvertUsing(conv, s => s.Status));
    }
}