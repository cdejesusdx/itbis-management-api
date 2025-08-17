using Microsoft.AspNetCore.Mvc;

using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;

namespace DGII.ItbisManagement.API.Controllers;

/// <summary>Endpoints para comprobantes fiscales.</summary>
/// <remarks>Constructor con el servicio de facturas.</remarks>
[ApiController]
[Route("api/[controller]")]
public class InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger) : ControllerBase
{
    private readonly IInvoiceService _invoiceService = invoiceService;
    private readonly ILogger<InvoicesController> _logger = logger;

    /// <summary>Listado de todos los comprobantes fiscales.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var result = await _invoiceService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /invoices");
            return Problem(title: "Ocurrió un error al obtener los comprobantes.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Obtiene un comprobante por RNC/Cédula y NCF.</summary>
    [HttpGet("{taxId}/{ncf}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(string taxId, string ncf, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _invoiceService.GetAsync(taxId, ncf, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /invoices/{TaxId}/{NCF}", taxId, ncf);
            return Problem(title: "Ocurrió un error al obtener el comprobante.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Listado de comprobantes por RNC/Cédula.</summary>
    [HttpGet("by-contributor/{taxId}")]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByContributor(string taxId, CancellationToken cancellationToken) 
    {
        try
        {
            var result =  await _invoiceService.GetByContributorAsync(taxId, cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /invoices/by-contributor/{TaxId}", taxId);
            return Problem(title: "Ocurrió un error al listar los comprobantes del contribuyente.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Crea un nuevo comprobante.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] InvoiceCreateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            
            var result = await _invoiceService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetAsync), new { taxId = result.TaxId, ncf = result.Ncf }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio al crear comprobante {TaxId}-{NCF}", dto.TaxId, dto.Ncf);
            return Problem(title: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en POST /invoices");
            return Problem(title: "Ocurrió un error al crear el comprobante.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Actualiza un comprobante existente (monto e ITBIS).</summary>
    [HttpPut("{taxId}/{ncf}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string taxId, string ncf, [FromBody] InvoiceUpdateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _invoiceService.UpdateAsync(taxId, ncf, dto, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en PUT /invoices/{TaxId}/{NCF}", taxId, ncf);
            return Problem(title: "Ocurrió un error al actualizar el comprobante.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Elimina un comprobante por RNC/Cédula y NCF.</summary>
    [HttpDelete("{taxId}/{ncf}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string taxId, string ncf, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _invoiceService.DeleteAsync(taxId, ncf, cancellationToken);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en DELETE /invoices/{TaxId}/{NCF}", taxId, ncf);
            return Problem(title: "Ocurrió un error al eliminar el comprobante.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}