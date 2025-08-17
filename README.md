# ITBIS Management API (.NET 9 + Clean Architecture)

API RESTful para la gestión de **contribuyentes** y **comprobantes fiscales (facturas)**, incluyendo el cálculo del **ITBIS (18%)** por RNC/Cédula.  

---

## 🚀 Tecnologías utilizadas

- **ASP.NET Core 9 (Web API)**
- **Entity Framework Core (SQL Server + Migraciones)**
- **AutoMapper** (perfiles y convertidores enum⇄texto)
- **Swagger / OpenAPI** (con XML comments)
- **xUnit + FluentAssertions** (tests)
- **Clean / Onion Architecture + Repository Pattern**
- Principios **SOLID** e **Inyección de dependencias**

---

## 📁 Estructura del proyecto

src/
 ├─ DGII.ItbisManagement.Domain/           # Entidades y enums
 ├─ DGII.ItbisManagement.Application/      # DTOs, Services, Interfaces, Mappings
 │   ├─ DTOs/
 │   │   ├─ Contributors/ (Dto/CreateDto/UpdateDto)
 │   │   └─ Invoices/     (Dto/CreateDto/UpdateDto)
 │   ├─ Interfaces/
 │   │   ├─ Repositories/ (IContributorRepository, IInvoiceRepository)
 │   │   └─ Services/     (IContributorService, IInvoiceService)
 │   ├─ Mappings/         (AutoMapper profiles + converters)
 │   └─ Services/         (ContributorService, InvoiceService)
 ├─ DGII.ItbisManagement.Infrastructure/   # DbContext, EF config, repos EF, Seeder, Migrations
 └─ DGII.ItbisManagement.API/              # Controllers, DI, Swagger, Program
tests/
 └─ DGII.ItbisManagement.Application.Tests/
 
 
 ## ⚙️ Configuración previa

Antes de ejecutar el proyecto, configura la cadena de conexión en:


Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ItbisDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

## 🧪 Instrucciones para ejecutar

```bash
git clone https://github.com/cdejesusdx/itbis-management-api.git
cd itbis-management-api

dotnet restore
dotnet run --project DGII.ItbisManagement.API
```

Una vez iniciado, accede a la documentación Swagger en:

```
https://localhost:7187/swagger/index.html
```

---

## Migraciones y base de datos

- Las migraciones ya están incluidas en el repositorio (Infrastructure/Migrations).
- Al ejecutar la API, se aplican automáticamente mediante Database.Migrate().
- Solo asegurarse de tener la cadena de conexión correctamente configurada.

---

## 📌 Funcionalidades

- Crear, listar, actualizar y eliminar contribuyentes y comprobantes fiscales
- Manejo global de errores
- Validaciones con FluentValidation
- Documentación de endpoints con Swagger + XML comments

---

## 📌 Endpoints principales

Contribuyentes

- GET /api/contributors - Listado de todos los contribuyentes.
- GET /api/contributors/{taxId} - Obtiene un contribuyente por RNC/Cédula.
- GET /api/contributors/{taxId}/invoices - Contribuyente con comprobantes e ITBIS total.
- GET /api/contributors/{taxId}/itbis-total - Total de ITBIS del contribuyente.
- POST /api/contributors - Crea un nuevo contribuyente.
- PUT /api/contributors/{taxId} - Actualiza los datos de un contribuyente.
- DELETE /api/contributors/{taxId} - Elimina un contribuyente por RNC/Cédula.

Comprobantes (facturas)

- GET /api/invoices - Listado de todos los comprobantes fiscales.
- GET /api/invoices/{taxId}/{ncf} - Obtiene un comprobante por RNC/Cédula y NCF.
- GET /api/invoices/by-contributor/{taxId} - Listado de comprobantes por RNC/Cédula.
- POST /api/invoices - Crea un nuevo comprobante.
- PUT /api/invoices/{taxId}/{ncf} - Actualiza un comprobante existente (monto e ITBIS).
- DELETE /api/invoices/{taxId}/{ncf} - Elimina un comprobante por RNC/Cédula y NCF.