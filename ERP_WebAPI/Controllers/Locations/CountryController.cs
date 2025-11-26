using ERP_BL.Data;
using ERP_BL.Entities.Locations.Countries;
using ERP_BL.Entities.Locations.Countries.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//
[ApiController]
[Route("[controller]")]
[Authorize]
public class CountryController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CountryController> _logger;

    public CountryController(ApplicationDbContext context, ILogger<CountryController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("GetAll")]
    public async Task<ActionResult<IEnumerable<Country>>> GetAll()
    {
        try
        {
            var countries = await _context.Countries
         .AsNoTracking()
         .Select(c => new CountryZoneDTO
         {
             Id = c.Id,
             Name = c.Name,
             Iso2 = c.Iso2,
             Iso3 = c.Iso3,
             ZoneId = c.ZoneId,
             ZoneName = c.Zone != null ? c.Zone.Name : null
         })
         .OrderBy(c => c.Name)
         .ToListAsync();


            return Ok(countries);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting countries");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("GetById/{id}")]
    public async Task<ActionResult<Country>> GetById(int id)
    {
        var country = await _context.Countries
         .Include(c => c.Zone) 
         .FirstOrDefaultAsync(c => c.Id == id);
        if (country == null)
            return NotFound();

        var zones = await _context.Zones.ToListAsync();

        var dto = new SelectCountryZoneDTO
        {
            Id = country.Id,
            Name = country.Name,
            Iso2 = country.Iso2,
            Iso3 = country.Iso3,
            Zones = zones
        };

        return Ok(dto);
    }
    [HttpPut("UpdateCountryZone")]
    public async Task<IActionResult> UpdateCountryZone([FromBody] CountryUpdateZoneDTO model)
    {
        if (model == null || model.CountryId <= 0)
            return BadRequest(new { Success = false, Message = "Invalid request data." });

        try
        {
            var country = await _context.Countries.FindAsync(model.CountryId);
            if (country == null)
                return NotFound(new { Success = false, Message = $"Country with ID {model.CountryId} not found." });

            var zoneExists = await _context.Zones.AnyAsync(z => z.Id == model.ZoneId);
            if (!zoneExists)
                return BadRequest(new { Success = false, Message = "Invalid ZoneId. Zone does not exist." });

            country.ZoneId = model.ZoneId;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = $"Country zone updated successfully.",
                Data = new { country.Id, country.Name, country.ZoneId }
            });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error while updating country zone");
            return StatusCode(500, new
            {
                Success = false,
                Message = "Database error while updating country zone.",
                Details = ex.InnerException?.Message ?? ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating country zone");
            return StatusCode(500, new
            {
                Success = false,
                Message = "Unexpected error while updating country zone.",
                Details = ex.Message
            });
        }
    }


}





