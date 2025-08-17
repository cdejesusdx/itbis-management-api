
namespace DGII.ItbisManagement.Domain.Entities
{
    /// <summary>Entidad que representa un comprobante fiscal (NCF).</summary>
    public class Invoice : BaseEntity
    {
        /// <summary>Identificador interno (PK) generado por la base de datos.</summary>
        public long Id { get; set; }

        /// <summary>Identificador del contribuyente (FK a Contributor.Id).</summary>
        public int ContributorId { get; set; }

        /// <summary>Referencia al contribuyente propietario (propiedad de navegación).</summary>
        public Contributor? Contributor { get; set; }

        /// <summary>Número de comprobante fiscal (NCF) único por contribuyente.</summary>
        public string Ncf { get; set; } = string.Empty;

        /// <summary>Monto de la factura.</summary>
        public decimal Amount { get; set; }

        /// <summary>Valor del ITBIS al 18% correspondiente a la factura.</summary>
        public decimal Itbis18 { get; set; }
    }
}
