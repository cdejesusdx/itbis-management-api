namespace DGII.ItbisManagement.Domain.Entities
{
    public class BaseEntity
    {
        /// <summary>Obtiene o establece el identificador único de la entidad.</summary>
        public int Id { get; set; }

        /// <summary>Obtiene o establece el nombre de usuario del creador de la entidad.</summary>
        public string CreateBy { get; set; } = string.Empty;

        /// <summary>Obtiene o establece la fecha y hora en que se creó la entidad.</summary>
        public DateTime Created { get; set; }

        /// <summary>Obtiene o establece el nombre de usuario que actualizó la entidad por última vez.</summary>
        public string? UpdateBy { get; set; }

        /// <summary>Obtiene o establece la fecha y hora en que se actualizó la entidad por última vez.</summary>
        public DateTime? Updated { get; set; }

    }
}