using Microsoft.EntityFrameworkCore;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Infrastructure.EntityConfigs;

namespace DGII.ItbisManagement.Infrastructure.Persistence
{
    /// <summary>DbContext de EF Core para el dominio de ITBIS.</summary>
    public class ItbisDbContext : DbContext
    {
        /// <summary>Crea una instancia del contexto con opciones inyectadas.</summary>
        public ItbisDbContext(DbContextOptions<ItbisDbContext> options) : base(options) { }

        /// <summary>Tabla de contribuyentes.</summary>
        public DbSet<Contributor> Contributors => Set<Contributor>();

        /// <summary>Tabla de comprobantes fiscales.</summary>
        public DbSet<Invoice> Invoices => Set<Invoice>();

        /// <summary>Configura el modelo y las convenciones.</summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContributorConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
        }
    }
}