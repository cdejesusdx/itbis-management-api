using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Infrastructure.EntityConfigs
{
    /// <summary>Configuración de EF Core para la entidad Invoice.</summary>
    internal sealed class InvoiceConfig : IEntityTypeConfiguration<Invoice>
    {
        /// <summary>Establece el mapeo y las restricciones de la entidad.</summary>
        public void Configure(EntityTypeBuilder<Invoice> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Invoices");

            entityTypeBuilder.HasKey(i => i.Id);
            entityTypeBuilder.Property(i => i.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(i => i.Ncf).HasMaxLength(20).IsRequired();

            entityTypeBuilder.Property(i => i.Amount).HasColumnType("decimal(18,2)");
            entityTypeBuilder.Property(i => i.Itbis18).HasColumnType("decimal(18,2)");

            entityTypeBuilder.HasOne(i => i.Contributor)
             .WithMany(c => c.Invoices)
             .HasForeignKey(i => i.ContributorId)
             .OnDelete(DeleteBehavior.Cascade);

            // Relacion unica de NCF por contribuyente
            entityTypeBuilder.HasIndex(i => new { i.ContributorId, i.Ncf }).IsUnique();
        }
    }
}