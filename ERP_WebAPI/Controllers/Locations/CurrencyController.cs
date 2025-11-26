using AutoMapper;
using ERP_BL.Entities;
using ERP_REPO.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERP_WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CurrencyController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAllCurrencies")]
        public async Task<IActionResult> GetAllCurrencies(
       [FromQuery] string status = "All",
       [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 10,
       [FromQuery] string? sortColumn = null,
       [FromQuery] string? sortDirection = "asc",
       [FromQuery] string? searchTerm = null)
        {
            try
            {
                // Call the generic repo method (with pagination)
                var (currencies, totalCount) = await _unitOfWork.Currencies.GetAllAsync(
                    includeProperties: "Country",
                    status: status,
                    sortColumn: sortColumn,
                    sortDirection: sortDirection,
                    searchTerm: searchTerm,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );

                var result = _mapper.Map<IEnumerable<CurrencyGetDto>>(currencies);

                return Ok(new
                {
                    success = true,
                    totalCount,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching paginated currencies");
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }


        [HttpGet("GetCurrencyById/{id}")]
        public async Task<IActionResult> GetCurrencyById(int id)
        {
            try
            {
                var entity = await _unitOfWork.Currencies.GetByIdAsync(id, "Country");
                if (entity == null)
                {
                    _logger.LogWarning("Currency with ID {Id} not found", id);
                    return NotFound();
                }

                var result = _mapper.Map<CurrencyGetDto>(entity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while fetching currency with ID {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("CreateCurrency")]
        public async Task<IActionResult> CreateCurrency([FromBody] CurrencyCreateDto dto)
        {
            try
            {
                if (await _unitOfWork.Currencies.ExistsAsync(c => c.Name == dto.Name))
                    return BadRequest(new { status = "error", message = "Currency with the same name already exists." });

                var entity = _mapper.Map<Currency>(dto);
                await _unitOfWork.Currencies.AddAsync(entity, User);
                await _unitOfWork.SaveAsync();

                return Ok(new { status = "success", message = "Currency added successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new currency");
                return StatusCode(500, new { status = "error", message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpPut("UpdateCurrency/{id}")]
        public async Task<IActionResult> UpdateCurrency(int id, [FromBody] CurrencyUpdateDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Currencies.GetByIdAsync(id);
                if (entity == null)
                    return NotFound();

                if (entity.CountryId != dto.CountryId)
                {
                    if (await _unitOfWork.Currencies.ExistsAsync(c => c.CountryId == dto.CountryId && c.Id != id))
                        return BadRequest(new { status = "error", message = "Another currency exists for this country." });
                }

                _mapper.Map(dto, entity);
                _unitOfWork.Currencies.Update(entity, User);
                await _unitOfWork.SaveAsync();

                return Ok(new { status = "success", message = "Currency updated successfully" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while updating currency with ID {id}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
    
}
