using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Infrastructure.EntityConfigs
{
    /// <summary>Configuración de EF Core para la entidad Contributor.</summary>
    internal sealed class ContributorConfig : IEntityTypeConfiguration<Contributor>
    {
        /// <summary>Establece el mapeo y las restricciones de la entidad.</summary>
        public void Configure(EntityTypeBuilder<Contributor>  entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Contributors");
            
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.Property(x => x.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(x => x.TaxId).HasMaxLength(20).IsRequired();
            entityTypeBuilder.HasIndex(x => x.TaxId).IsUnique();

            entityTypeBuilder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entityTypeBuilder.Property(x => x.Type).IsRequired();
            entityTypeBuilder.Property(x => x.Status).IsRequired();

        }
    }
}