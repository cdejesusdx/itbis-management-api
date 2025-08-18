using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Perfil de mapeos para Invoice.</summary>
public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        // Entity -> DTO
        // TaxId proviene de la navegación Contributor (el diseño no lo tiene directo en Invoice).
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(d => d.TaxId, m => m.MapFrom(s => s.Contributor!.TaxId))
            .ForMember(d => d.Ncf, m => m.MapFrom(s => s.Ncf))
            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

        // Create DTO -> Entity
        // La FK (ContributorId) y la navegación se resuelven en el servicio.
        CreateMap<InvoiceCreateDto, Invoice>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.ContributorId, m => m.Ignore())
            .ForMember(d => d.Contributor, m => m.Ignore())
            .ForMember(d => d.CreateBy, m => m.Ignore())
            .ForMember(d => d.Created, m => m.Ignore())
            .ForMember(d => d.UpdateBy, m => m.Ignore())
            .ForMember(d => d.Updated, m => m.Ignore());
        // Las propiedades: Ncf, Amount, Itbis18 se mapean por convención

        // Update DTO -> Entity
        // No se toca claves ni navegación desde Update.
        CreateMap<InvoiceUpdateDto, Invoice>()
            .ForMember(d => d.Id, m => m.Ignore())
            .ForMember(d => d.Ncf, m => m.Ignore())
            .ForMember(d => d.ContributorId, m => m.Ignore())
            .ForMember(d => d.Contributor, m => m.Ignore())
            .ForMember(d => d.CreateBy, m => m.Ignore())
            .ForMember(d => d.Created, m => m.Ignore())
            .ForMember(d => d.UpdateBy, m => m.Ignore())
            .ForMember(d => d.Updated, m => m.Ignore());
    }
}

///// <summary>Perfil de mapeo para comprobantes fiscales.</summary>
//public sealed class InvoiceProfile : Profile
//{
//    public InvoiceProfile()
//    {
//        // Entity -> DTO
//        CreateMap<Invoice, InvoiceDto>()
//            .ForMember(d => d.TaxId, m => m.MapFrom(s => s.Contributor!.TaxId))
//            .ForMember(d => d.Ncf, m => m.MapFrom(s => s.Ncf))
//            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
//            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

//        // Create DTO -> Entity
//        CreateMap<InvoiceCreateDto, Invoice>()
//            .ForMember(d => d.Id, m => m.Ignore())
//            .ForMember(d => d.ContributorId, m => m.Ignore()) 
//            .ForMember(d => d.Ncf, m => m.MapFrom(s => s.Ncf))
//            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
//            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

//        // Update DTO -> Entity
//        CreateMap<InvoiceUpdateDto, Invoice>()
//            .ForMember(d => d.Id, m => m.Ignore())
//            .ForMember(d => d.ContributorId, m => m.Ignore())
//            .ForMember(d => d.Ncf, m => m.Ignore()) 
//            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
//            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));

//    }
//}