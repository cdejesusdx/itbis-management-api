using Microsoft.EntityFrameworkCore;

using DGII.ItbisManagement.Domain.Enums;
using DGII.ItbisManagement.Domain.Entities;

namespace DGII.ItbisManagement.Infrastructure.Persistence
{
    /// <summary>Inicializa la base de datos con datos de ejemplo.</summary>
    public class DbInitializer
    {
        /// <summary>Inserta datos solo si la BD está vacía.</summary>
        public static async Task SeedAsync(ItbisDbContext context, CancellationToken cancellationToken)
        {
            if (await context.Contributors.AnyAsync(cancellationToken)) return;

            // ========== CONTRIBUYENTES ==========
            var contributors = new List<Contributor>
            {
                new() { TaxId = "98754321012", Name = "JUAN PEREZ", Type = ContributorType.Individual, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "123456789",   Name = "FARMACIA TU SALUD", Type = ContributorType.LegalEntity, Status = Status.Inactive, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00145678901", Name = "MARIA GOMEZ", Type = ContributorType.Individual, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00298765432", Name = "SUPERMERCADO LA ECONOMIA", Type = ContributorType.LegalEntity, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00378965412", Name = "CLINICA NUEVA VIDA", Type = ContributorType.LegalEntity, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00432198765", Name = "PEDRO MARTINEZ", Type = ContributorType.Individual, Status = Status.Inactive, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00585214796", Name = "CONSTRUCTORA EL PROGRESO", Type = ContributorType.LegalEntity, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00696325874", Name = "LUISA FERNANDEZ", Type = ContributorType.Individual, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00715926348", Name = "EMPRESAS TECHDOM", Type = ContributorType.LegalEntity, Status = Status.Inactive, CreateBy = "System", Created = DateTime.Now },
                new() { TaxId = "00875395146", Name = "OSCAR RAMIREZ", Type = ContributorType.Individual, Status = Status.Active, CreateBy = "System", Created = DateTime.Now },
            };

            context.Contributors.AddRange(contributors);
            await context.SaveChangesAsync(cancellationToken);

            // ========== FACTURAS ==========
            var invoices = new List<Invoice>
            {
                new() { ContributorId = contributors[0].Id, Ncf = "E310000000001", Amount = 200.00m,  Itbis18 = 36.00m,  CreateBy = "System", Created = DateTime.Now },
                new() { ContributorId = contributors[0].Id, Ncf = "E310000000002", Amount = 1000.00m, Itbis18 = 180.00m, CreateBy = "System", Created = DateTime.Now },

                new() { ContributorId = contributors[2].Id, Ncf = "B010000000001", Amount = 150.00m,  Itbis18 = 27.00m,  CreateBy = "System", Created = DateTime.Now },
                new() { ContributorId = contributors[2].Id, Ncf = "B010000000002", Amount = 300.00m,  Itbis18 = 54.00m,  CreateBy = "System", Created = DateTime.Now },

                new() { ContributorId = contributors[3].Id, Ncf = "E320000000001", Amount = 750.00m,  Itbis18 = 135.00m, CreateBy = "System", Created = DateTime.Now },
                new() { ContributorId = contributors[3].Id, Ncf = "E320000000002", Amount = 1200.00m, Itbis18 = 216.00m, CreateBy = "System", Created = DateTime.Now },

                new() { ContributorId = contributors[4].Id, Ncf = "E330000000001", Amount = 500.00m,  Itbis18 = 90.00m,  CreateBy = "System", Created = DateTime.Now },

                new() { ContributorId = contributors[6].Id, Ncf = "E340000000001", Amount = 2200.00m, Itbis18 = 396.00m, CreateBy = "System", Created = DateTime.Now },

                new() { ContributorId = contributors[7].Id, Ncf = "B020000000001", Amount = 85.00m,   Itbis18 = 15.30m,  CreateBy = "System", Created = DateTime.Now },

                new() { ContributorId = contributors[9].Id, Ncf = "B030000000001", Amount = 450.00m,  Itbis18 = 81.00m,  CreateBy = "System", Created = DateTime.Now },
            };

            context.Invoices.AddRange(invoices);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}