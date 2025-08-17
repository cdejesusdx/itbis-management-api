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

## 📌 Funcionalidades

- Crear, listar, actualizar y eliminar contribuyentes y comprobantes fiscales
- Manejo global de errores
- Validaciones con FluentValidation
- Documentación de endpoints con Swagger + XML comments
