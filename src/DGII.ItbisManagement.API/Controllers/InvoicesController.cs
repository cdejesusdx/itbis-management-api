using Microsoft.AspNetCore.Mvc;

using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;

namespace DGII.ItbisManagement.API.Controllers;

/// <summary>Endpoints para comprobantes fiscales.</summary>
/// <remarks>Constructor con el servicio de facturas.</remarks>
[ApiController]
[Route("api/[controller]")]
public class InvoicesController(IInvoiceService invoiceService) : ControllerBase
{
    private readonly IInvoiceService _invoiceService = invoiceService;

    /// <summary>Listado de todos los comprobantes fiscales.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await _invoiceService.GetAllAsync(cancellationToken));

    /// <summary>Obtiene un comprobante por RNC/Cédula y NCF.</summary>
    [HttpGet("{taxId}/{ncf}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(string taxId, string ncf, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.GetAsync(taxId, ncf, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Listado de comprobantes por RNC/Cédula.</summary>
    [HttpGet("by-contributor/{taxId}")]
    [ProducesResponseType(typeof(IEnumerable<InvoiceDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByContributor(string taxId, CancellationToken cancellationToken) =>
        Ok(await _invoiceService.GetByContributorAsync(taxId, cancellationToken));

    /// <summary>Crea un nuevo comprobante.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] InvoiceCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var result = await _invoiceService.CreateAsync(dto, cancellationToken);
          
            return CreatedAtAction(nameof(GetOne), new { taxId = result.TaxId, ncf = result.Ncf }, result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Actualiza un comprobante existente (monto e ITBIS).</summary>
    [HttpPut("{taxId}/{ncf}")]
    [ProducesResponseType(typeof(InvoiceDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string taxId, string ncf, [FromBody] InvoiceUpdateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var result = await _invoiceService.UpdateAsync(taxId, ncf, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Elimina un comprobante por RNC/Cédula y NCF.</summary>
    [HttpDelete("{taxId}/{ncf}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string taxId, string ncf, CancellationToken cancellationToken)
    {
        var result = await _invoiceService.DeleteAsync(taxId, ncf, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}