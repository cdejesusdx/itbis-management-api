using Microsoft.AspNetCore.Mvc;

using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;

namespace DGII.ItbisManagement.API.Controllers;

/// <summary>Endpoints para contribuyentes y sus comprobantes.</summary>
[ApiController]
[Route("api/[controller]")]
public class ContributorsController(IContributorService _contributorService) : ControllerBase
{
    /// <summary>Listado de todos los contribuyentes.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContributorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) =>
        Ok(await _contributorService.GetAllAsync(cancellationToken));

    /// <summary>Obtiene un contribuyente por RNC/Cédula.</summary>
    [HttpGet("{taxId}")]
    [ProducesResponseType(typeof(ContributorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne(string taxId, CancellationToken cancellationToken)
    {
        var result = await _contributorService.GetAsync(taxId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Contribuyente con comprobantes e ITBIS total.</summary>
    [HttpGet("{taxId}/invoices")]
    [ProducesResponseType(typeof(ContributorWithInvoicesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithInvoices(string taxId, CancellationToken cancellationToken)
    {
        var result = await _contributorService.GetWithInvoicesAsync(taxId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Total de ITBIS del contribuyente.</summary>
    [HttpGet("{taxId}/itbis-total")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItbisTotal(string taxId, CancellationToken cancellationToken)
    {
        var result = await _contributorService.GetWithInvoicesAsync(taxId, cancellationToken);
        return result is null ? NotFound() : Ok(result.TotalItbis);
    }

    /// <summary>Crea un nuevo contribuyente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContributorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ContributorCreateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var result = await _contributorService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetOne), new { taxId = result.TaxId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Actualiza los datos de un contribuyente.</summary>
    [HttpPut("{taxId}")]
    [ProducesResponseType(typeof(ContributorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string taxId, [FromBody] ContributorUpdateDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var result = await _contributorService.UpdateAsync(taxId, dto, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Elimina un contribuyente por RNC/Cédula.</summary>
    [HttpDelete("{taxId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string taxId, CancellationToken cancellationToken)
    {
        var result = await _contributorService.DeleteAsync(taxId, cancellationToken);
        return result ? NoContent() : NotFound();
    }
}