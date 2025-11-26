using ERP_BL.Data;
using ERP_BL.Entities.Locations.Cities;
using ERP_BL.Entities.Locations.Cities.Dtos;
using ERP_BL.Entities.Locations.States;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CityController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CityController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAllCities")]
        public async Task<IActionResult> GetAllCities([FromQuery] CityQueryParameters query)
        {
            try
            {
                if (query.PageNumber <= 0) query.PageNumber = 1;
                if (query.PageSize <= 0) query.PageSize = 20;

                var citiesQuery = _context.Set<City>()
                    .Include(c => c.State)
                    .AsNoTracking()
                    .AsQueryable();

          
                if (!string.IsNullOrWhiteSpace(query.Search))
                {
                    string searchTerm = query.Search.Trim().ToLower();
                    citiesQuery = citiesQuery.Where(c =>
                        c.Name.ToLower().Contains(searchTerm) ||
                        c.State.Name.ToLower().Contains(searchTerm));
                }

                var totalCount = await citiesQuery.CountAsync();

                var cities = await citiesQuery
                    .OrderBy(c => c.Name)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(c => new GetCityDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StateId = c.StateId,
                        StateName = c.State.Name
                    })
                    .ToListAsync();

                if (!cities.Any())
                    return NoContent();

                return Ok(new
                {
                    Success = true,
                    Message = "Cities fetched successfully.",
                    Data = new
                    {
                        Items = cities,
                        TotalCount = totalCount,
                        CurrentPage = query.PageNumber,
                        PageSize = query.PageSize,
                        TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error fetching cities.",
                    Details = ex.Message
                });
            }
        }


        [HttpGet("GetStatesDropdown")]
        public async Task<IActionResult> GetStatesDropdown([FromQuery] string? search = null)
        {
            try
            {
                var query = _context.Set<State>().AsNoTracking();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    string searchTerm = search.Trim().ToLower();
                    query = query.Where(s => s.Name.ToLower().Contains(searchTerm));
                }

                var states = await query
                    .OrderBy(s => s.Name)
                    .Take(50)
                    .Select(s => new { s.Id, s.Name })
                    .ToListAsync();

                return Ok(new
                {
                    Success = true,
                    Message = "States fetched successfully.",
                    Data = states
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Error fetching states.",
                    Details = ex.Message
                });
            }
        }

 
        [HttpPost("CreateCity")]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Success = false, Message = "Invalid input.", Errors = ModelState });

            try
            {
                //  Validate State existence
                var stateExists = await _context.Set<State>().AnyAsync(s => s.Id == dto.StateId);
                if (!stateExists)
                    return BadRequest(new { Success = false, Message = "Invalid StateId. State does not exist." });

                string cityName = dto.Name.Trim().ToLower();

                //  Check for duplicate within the same state
                bool cityExistsInState = await _context.Set<City>()
                    .AnyAsync(c => c.Name.ToLower() == cityName && c.StateId == dto.StateId);
                if (cityExistsInState)
                    return Conflict(new { Success = false, Message = $"City '{dto.Name}' already exists in this state." });

                // 🔹 (Optional stricter rule) Prevent global duplicates
                bool cityExistsGlobally = await _context.Set<City>()
                    .AnyAsync(c => c.Name.ToLower() == cityName);
                if (cityExistsGlobally)
                {
                    
                     return Conflict(new { Success = false, Message = $"City '{dto.Name}' already exists in another state." });
                }

          
                var city = new City
                {
                    Name = dto.Name.Trim(),
                    StateId = dto.StateId
                };

                await _context.AddAsync(city);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAllCities), new { id = city.Id }, new
                {
                    Success = true,
                    Message = "City created successfully.",
                    Data = new { city.Id, city.Name, city.StateId }
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Database error while creating city.",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Unexpected error while creating city.",
                    Details = ex.Message
                });
            }
        }


        [HttpPut("UpdateCity/{id:int}")]
        public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { Success = false, Message = "City ID mismatch." });

            try
            {
                var city = await _context.Set<City>().FindAsync(id);
                if (city == null)
                    return NotFound(new { Success = false, Message = "City not found." });

                string newName = dto.Name.Trim().ToLower();

                // 🔹 Prevent same name in the same state
                bool duplicateInState = await _context.Set<City>()
                    .AnyAsync(c => c.Id != id &&
                                   c.Name.ToLower() == newName &&
                                   c.StateId == dto.StateId);
                if (duplicateInState)
                    return Conflict(new
                    {
                        Success = false,
                        Message = $"Another city named '{dto.Name}' already exists in this state."
                    });

                city.Name = dto.Name.Trim();
                city.StateId = dto.StateId;

                await _context.SaveChangesAsync();
                return Ok(new { Success = true, Message = "City updated successfully.", Data = city });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Database error while updating city.",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Unexpected error while updating city.",
                    Details = ex.Message
                });
            }
        }

        [HttpDelete("DeleteCity/{id:int}")]
        public async Task<IActionResult> DeleteCity(int id)
        {
            try
            {
                var city = await _context.Set<City>()
                    .Include(c => c.State)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (city == null)
                    return NotFound(new { Success = false, Message = "City not found." });

                // Optional: check if city is in use
                bool isLinked = await _context.Companies.AnyAsync(c => c.Address.CityId == id);
                if (isLinked)
                    return BadRequest(new { Success = false, Message = "City cannot be deleted because it is linked to existing companies." });

                _context.Remove(city);
                await _context.SaveChangesAsync();

                return Ok(new { Success = true, Message = "City deleted successfully." });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "Database constraint error while deleting city.",
                    Details = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    Message = "An unexpected error occurred while deleting city.",
                    Details = ex.Message
                });
            }
        }
    }
}
