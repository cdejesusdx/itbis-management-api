using Microsoft.AspNetCore.Mvc;

using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;

namespace DGII.ItbisManagement.API.Controllers;

/// <summary>Endpoints para contribuyentes y sus comprobantes.</summary>
[ApiController]
[Route("api/[controller]")]
public class ContributorsController(IContributorService contributorService, ILogger<ContributorsController> logger) : ControllerBase
{
    private readonly IContributorService _contributorService = contributorService;
    private readonly ILogger<ContributorsController> _logger = logger;

    /// <summary>Listado de todos los contribuyentes.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContributorDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken) 
    {
        try
        {
            var result = await _contributorService.GetAllAsync(cancellationToken);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /contributors");
            return Problem(title: "Ocurrió un error al obtener los contribuyentes.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Obtiene un contribuyente por RNC/Cédula.</summary>
    [HttpGet("{taxId}", Name = "GetContributorByTaxId")]
    [ProducesResponseType(typeof(ContributorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(string taxId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _contributorService.GetAsync(taxId, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /contributors/{TaxId}", taxId);
            return Problem(title: "Ocurrió un error al obtener el contribuyente.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Contribuyente con comprobantes e ITBIS total.</summary>
    [HttpGet("{taxId}/invoices")]
    [ProducesResponseType(typeof(ContributorWithInvoicesDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWithInvoices(string taxId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _contributorService.GetWithInvoicesAsync(taxId, cancellationToken);
            return result is null ? NotFound() : Ok(result);

        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /contributors/{TaxId}/invoices", taxId);
            return Problem(title: "Ocurrió un error al obtener los comprobantes del contribuyente.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Total de ITBIS del contribuyente.</summary>
    [HttpGet("{taxId}/itbis-total")]
    [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetItbisTotal(string taxId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _contributorService.GetWithInvoicesAsync(taxId, cancellationToken);
            return result is null ? NotFound() : Ok(result.TotalItbis);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en GET /contributors/{TaxId}/itbis-total", taxId);
            return Problem(title: "Ocurrió un error al calcular el ITBIS.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Crea un nuevo contribuyente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ContributorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] ContributorCreateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _contributorService.CreateAsync(dto, cancellationToken);
            return CreatedAtRoute("GetContributorByTaxId", new { taxId = result.TaxId }, result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio al crear {TaxId}", dto.TaxId);
            return Problem(title: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en POST /contributors");
            return Problem(title: "Ocurrió un error al crear el contribuyente.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Actualiza los datos de un contribuyente.</summary>
    [HttpPut("{taxId}")]
    [ProducesResponseType(typeof(ContributorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(string taxId, [FromBody] ContributorUpdateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _contributorService.UpdateAsync(taxId, dto, cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Validación de negocio al actualizar {TaxId}", taxId);
            return Problem(title: ex.Message, statusCode: StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en PUT /contributors/{TaxId}", taxId);
            return Problem(title: "Ocurrió un error al actualizar el contribuyente.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>Elimina un contribuyente por RNC/Cédula.</summary>
    [HttpDelete("{taxId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(string taxId, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _contributorService.DeleteAsync(taxId, cancellationToken);
            return result ? NoContent() : NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fallo en DELETE /contributors/{TaxId}", taxId);
            return Problem(title: "Ocurrió un error al eliminar el contribuyente.", statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}