using AutoMapper;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>Perfil de mapeo para comprobantes fiscales.</summary>
public sealed class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceDto>()
            .ForMember(d => d.TaxId, m => m.MapFrom(s => s.Contributor != null ? s.Contributor.TaxId : string.Empty))
            .ForMember(d => d.Ncf, m => m.MapFrom(s => s.Ncf))
            .ForMember(d => d.Amount, m => m.MapFrom(s => s.Amount))
            .ForMember(d => d.Itbis18, m => m.MapFrom(s => s.Itbis18));
    }
}