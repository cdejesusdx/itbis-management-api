using Microsoft.EntityFrameworkCore;

using DGII.ItbisManagement.Application.Mappings;
using DGII.ItbisManagement.Application.Services;
using DGII.ItbisManagement.Infrastructure.Persistence;
using DGII.ItbisManagement.Infrastructure.Repositories;
using DGII.ItbisManagement.Application.Interfaces.Services;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Controllers + XML/JSON
builder.Services.AddControllers().AddXmlSerializerFormatters(); // Se habilita contenido para devolver XML además del JSON.

// AutoMapper
builder.Services.AddAutoMapper(typeof(AssemblyMarker).Assembly);

// EF Core + SQL Server
builder.Services.AddDbContext<ItbisDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositorios EF
builder.Services.AddScoped<IContributorRepository, ContributorRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();

// Servicios de aplicación
builder.Services.AddScoped<IContributorService, ContributorService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Swagger + XML docs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    var xmls = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
    foreach (var xml in xmls) opt.IncludeXmlComments(xml, includeControllerXmlComments: true);
});

// Swagger con inclusión de comentarios XML de todos los ensamblados
builder.Services.AddSwaggerGen(opt =>
{
    // Incluyir todos los archivos XML generados por los proyectos
    var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
    foreach (var xml in xmlFiles)
        opt.IncludeXmlComments(xml, includeControllerXmlComments: true);

    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "DGII – ITBIS Management API",
        Version = "v1",
        Description = "API para listar contribuyentes y comprobantes fiscales, incluyendo suma de ITBIS."
    });
});

var app = builder.Build();

var useInMemory = builder.Configuration.GetValue<bool>("UseInMemory");

// Migración + DbInitializer
if (useInMemory)
{
   // TODO
}
else
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ItbisDbContext>();
    await context.Database.MigrateAsync();
    await DbInitializer.SeedAsync(context, CancellationToken.None);
}

// Swagger UI siempre disponible
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DGII – ITBIS Management API v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
