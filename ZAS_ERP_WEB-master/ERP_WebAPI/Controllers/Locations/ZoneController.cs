using ERP_BL.Data;
using ERP_BL.Entities.Locations.Zones;
using ERP_BL.Entities.Locations.Zones.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_WebAPI.Controllers.Locations
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ZoneController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ZoneController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        [HttpPost("Create")]
        public async Task<IActionResult> CreateZone([FromBody] CreateZoneDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid input data.", errors = ModelState });

            string zoneName = dto.Name.Trim().ToLower();

            //  Check for duplicate zone name
            bool zoneExists = await _context.Zones
                .AnyAsync(z => z.Name.ToLower() == zoneName);
            if (zoneExists)
                return Conflict(new { success = false, message = $"Zone '{dto.Name}' already exists." });

            //  Validate all country IDs exist
            var countries = await _context.Countries
                .Where(c => dto.CountryIds.Contains(c.Id))
                .ToListAsync();

            if (countries.Count != dto.CountryIds.Count)
                return BadRequest(new { success = false, message = "Some countries do not exist." });

            //  Prevent assigning countries already in another zone
            var conflictingCountries = await _context.Countries
                .Where(c => dto.CountryIds.Contains(c.Id) && c.ZoneId != null)
                .Select(c => c.Name)
                .ToListAsync();

            if (conflictingCountries.Any())
                return Conflict(new
                {
                    success = false,
                    message = $"These countries are already assigned to another zone: {string.Join(", ", conflictingCountries)}"
                });

      
            var zone = new Zone
            {
                Name = dto.Name.Trim(),
                Countries = countries
            };

            _context.Zones.Add(zone);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Zone created successfully.", data = new { zone.Id, zone.Name } });
        }

      
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllZones()
        {
            var zones = await _context.Zones
                .Include(z => z.Countries)
                .AsNoTracking()
                .Select(z => new
                {
                    z.Id,
                    z.Name,
                    Countries = z.Countries.Select(c => new { c.Id, c.Name }).ToList()
                })
                .OrderBy(z => z.Name)
                .ToListAsync();

            return Ok(new { success = true, data = zones });
        }


        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var zone = await _context.Zones
                .Include(z => z.Countries)
                .AsNoTracking()
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null)
                return NotFound(new { success = false, message = $"Zone {id} not found." });

            var dto = new
            {
                zone.Id,
                zone.Name,
                CountryIds = zone.Countries.Select(c => c.Id).ToList(),
                Countries = zone.Countries.Select(c => new { c.Id, c.Name }).ToList()
            };

            return Ok(new { success = true, data = dto });
        }

    
        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateZoneDTO dto)
        {
            var existingZone = await _context.Zones
                .Include(z => z.Countries)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (existingZone == null)
                return NotFound(new { success = false, message = $"Zone {id} not found." });

            string zoneName = dto.Name.Trim().ToLower();

            //  Prevent duplicate name
            bool nameExists = await _context.Zones
                .AnyAsync(z => z.Id != id && z.Name.ToLower() == zoneName);
            if (nameExists)
                return Conflict(new { success = false, message = $"Another zone with name '{dto.Name}' already exists." });

            //  Validate countries
            var countries = await _context.Countries
                .Where(c => dto.CountryIds.Contains(c.Id))
                .ToListAsync();

            if (countries.Count != dto.CountryIds.Count)
                return BadRequest(new { success = false, message = "Some countries do not exist." });

            //  Prevent assigning a country to multiple zones
            var conflictingCountries = await _context.Countries
                .Where(c => dto.CountryIds.Contains(c.Id) &&
                            c.ZoneId != null &&
                            c.ZoneId != existingZone.Id)
                .Select(c => c.Name)
                .ToListAsync();

            if (conflictingCountries.Any())
                return Conflict(new
                {
                    success = false,
                    message = $"These countries are already assigned to another zone: {string.Join(", ", conflictingCountries)}"
                });

            //  Update zone info
            existingZone.Name = dto.Name.Trim();
            existingZone.Countries.Clear();
            foreach (var country in countries)
                existingZone.Countries.Add(country);

            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Zone updated successfully." });
        }

      
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var zone = await _context.Zones
                .Include(z => z.Countries)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zone == null)
                return NotFound(new { success = false, message = $"Zone {id} not found." });

            foreach (var country in zone.Countries)
                country.ZoneId = null;

            _context.Zones.Remove(zone);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Zone deleted successfully." });
        }
    }
}
