using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Perfil de mapeo para comprobantes fiscales.</summary>
public sealed class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        // Entity -> DTO
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(d => d.TaxId, m => m.MapFrom(s => s.Contributor!.TaxId))
            .ForMember(d => d.Ncf, m => m.MapFrom(s => s.Ncf))
            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

        // Create DTO -> Entity
        CreateMap<InvoiceCreateDto, Invoice>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.ContributorId, m => m.Ignore()) 
            .ForMember(d => d.Ncf, m => m.MapFrom(s => s.Ncf))
            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

        // Update DTO -> Entity
        CreateMap<InvoiceUpdateDto, Invoice>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.ContributorId, m => m.Ignore())
            .ForMember(d => d.Ncf, m => m.Ignore()) 
            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

    }
}