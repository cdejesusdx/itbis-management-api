using AutoMapper;
using DGII.ItbisManagement.Domain.Enums;

namespace DGII.ItbisManagement.Application.Mappings;

/// <summary>
/// Convierte string en español a enum Status.
/// </summary>
public sealed class StringToStatusConverter : IValueConverter<string, Status>
{
    public Status Convert(string sourceMember, ResolutionContext context)
    {
        var s = (sourceMember ?? string.Empty).Trim().ToLowerInvariant();

        return s switch
        {
            "activo" => Status.Active,
            "inactivo" => Status.Inactive,
            _ => throw new ArgumentException($"Estatus no válido: '{sourceMember}'.")
        };
    }
}
