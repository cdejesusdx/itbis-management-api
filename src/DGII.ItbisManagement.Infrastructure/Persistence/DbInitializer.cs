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

            var contributor1 = new Contributor { TaxId = "98754321012", Name = "JUAN PEREZ", Type = ContributorType.Individual, Status = Status.Active, CreateBy = "System", Created = DateTime.Now };
            var contributor2 = new Contributor { TaxId = "123456789", Name = "FARMACIA TU SALUD", Type = ContributorType.LegalEntity, Status = Status.Inactive, CreateBy = "System", Created = DateTime.Now };

            context.Contributors.AddRange(contributor1, contributor2);
            await context.SaveChangesAsync(cancellationToken);

            var invoice1 = new Invoice { ContributorId = contributor1.Id, Ncf = "E310000000001", Amount = 200.00m, Itbis18 = 36.00m, CreateBy = "System", Created = DateTime.Now };
            var invoice2 = new Invoice { ContributorId = contributor1.Id, Ncf = "E310000000002", Amount = 1000.00m, Itbis18 = 180.00m, CreateBy = "System", Created = DateTime.Now };

            context.Invoices.AddRange(invoice1, invoice2);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}