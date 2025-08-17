using DGII.ItbisManagement.Domain.Enums;

namespace DGII.ItbisManagement.Domain.Entities
{
    /// <summary>Entidad que representa un contribuyente (persona física o jurídica).</summary>
    public class Contributor : BaseEntity
    {
        /// <summary>Identificador tributario (RNC o Cédula).</summary>
        public string TaxId { get; set; } = string.Empty;

        /// <summary>Nombre del contribuyente.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Tipo de contribuyente (individual o empresa).</summary>
        public ContributorType Type { get; set; }

        /// <summary>Estado del contribuyente (activo o inactivo).</summary>
        public Status Status { get; set; }

        /// <summary>Listado de facturas asociadas al contribuyente.</summary>
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}