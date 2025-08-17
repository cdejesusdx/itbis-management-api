using AutoMapper;
using Microsoft.Extensions.Logging;

using DGII.ItbisManagement.Domain.Entities;
using DGII.ItbisManagement.Application.DTOs;
using DGII.ItbisManagement.Application.Interfaces.Services;
using DGII.ItbisManagement.Application.Interfaces.Repositories;

namespace DGII.ItbisManagement.Application.Services;

/// <summary>Servicio para gestionar contribuyentes y sus comprobantes.</summary>
/// <remarks>Constructor con la inyección de dependencias.</remarks>
public class ContributorService(IContributorRepository contributorRepository,
                          IInvoiceRepository invoiceRepository, ILogger<ContributorService> logger, IMapper mapper) : IContributorService
{
    private readonly IContributorRepository _contributorRepository = contributorRepository;
    private readonly IInvoiceRepository _invoiceRepository = invoiceRepository;
    private readonly ILogger<ContributorService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ContributorDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var contributors = await _contributorRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IReadOnlyList<ContributorDto>>(contributors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al listar contribuyentes");
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<ContributorWithInvoicesDto?> GetWithInvoicesAsync(string taxId, CancellationToken cancellationToken = default)
    {
        try
        {
            var contributor = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
            if (contributor is null) return null;

            var invoice = await _invoiceRepository.GetByContributorAsync(taxId, cancellationToken);
            return new ContributorWithInvoicesDto
            {
                Contributor = _mapper.Map<ContributorDto>(contributor),
                Invoices = _mapper.Map<List<InvoiceDto>>(invoice),
                TotalItbis = invoice.Sum(i => i.Itbis18)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al obtener contribuyente por su RNC/Cédula con sus facturas {TaxId}", taxId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<ContributorDto?> GetAsync(string taxId, CancellationToken cancellationToken = default)
    {
        try
        {
            var contributor = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
            return contributor is null ? null : _mapper.Map<ContributorDto>(contributor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al obtener contribuyente {TaxId}", taxId);
            throw;
        }       
    }

    /// <inheritdoc/>
    public async Task<ContributorDto> CreateAsync(ContributorCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validar duplicado por TaxId
            var exists = await _contributorRepository.GetByIdAsync(dto.TaxId, cancellationToken);
            if (exists is not null)
            {
                _logger.LogWarning("Intento de crear contribuyente existente {TaxId}", dto.TaxId);
                throw new InvalidOperationException($"Ya existe el contribuyente {dto.TaxId}.");
            }

            var contributor = _mapper.Map<Contributor>(dto);
            await _contributorRepository.AddAsync(contributor, cancellationToken);

            return _mapper.Map<ContributorDto>(contributor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al crear contribuyente {TaxId}", dto.TaxId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<ContributorDto?> UpdateAsync(string taxId, ContributorUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var contributor = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
            if (contributor is null) return null;

            _mapper.Map(dto, contributor);
            await _contributorRepository.UpdateAsync(contributor, cancellationToken);

            _logger.LogInformation("Contribuyente {TaxId} actualizado", taxId);
            return _mapper.Map<ContributorDto>(contributor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al actualizar contribuyente {TaxId}", taxId);
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string taxId, CancellationToken cancellationToken = default)
    {
        try
        {
            var contributor = await _contributorRepository.GetByIdAsync(taxId, cancellationToken);
            if (contributor is null) return false;

            await _contributorRepository.DeleteAsync(taxId, cancellationToken);
            _logger.LogInformation("Contribuyente {TaxId} eliminado", taxId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en servicio al eliminar contribuyente {TaxId}", taxId);
            throw;
        }  
    }
}