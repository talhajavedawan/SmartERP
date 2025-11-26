using ERP_BL.Data;
using ERP_BL.Entities.Locations.States;
using ERP_BL.Entities.Locations.States.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("[controller]")]
[Authorize]
public class StateController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public StateController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("countries")]
    public async Task<IActionResult> GetCountries()
    {
        var countries = await _context.Countries
            .OrderBy(c => c.Name)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        return Ok(new { success = true, data = countries });
    }


    [HttpGet("cities/search")]
    public async Task<IActionResult> SearchCities(string search = "", int page = 1, int pageSize = 20)
    {
        var query = _context.Cities.AsQueryable();

        if (!string.IsNullOrEmpty(search))
            query = query.Where(c => c.Name.Contains(search));

        var total = await query.CountAsync();

        var cities = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new { c.Id, c.Name })
            .ToListAsync();

        return Ok(new { success = true, total, data = cities });
    }


    [HttpGet("GetAllStates")]
    public async Task<IActionResult> GetAllStates(string search = "", int page = 1, int pageSize = 50)
    {
        var query = _context.States
            .Include(s => s.Country)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            string term = search.Trim().ToLower();
            query = query.Where(s =>
                s.Name.ToLower().Contains(term) ||
                s.StateCode.ToLower().Contains(term) ||
                s.Country.Name.ToLower().Contains(term));
        }

        var totalRecords = await query.CountAsync();

        var states = await query
            .OrderBy(s => s.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.StateCode,
                Country = new { s.Country.Id, s.Country.Name }
            })
            .ToListAsync();

        return Ok(new
        {
            success = true,
            totalRecords,
            data = states
        });
    }


    [HttpGet("GetStateById/{id:int}")]
    public async Task<IActionResult> GetStateById(int id)
    {
        var state = await _context.States
            .Include(s => s.Cities)
            .Include(s => s.Country)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (state == null)
            return NotFound(new { success = false, message = $"State with ID {id} not found." });

        var dto = new StateDto
        {
            Id = state.Id,
            Name = state.Name,
            CountryId = state.CountryId,
            StateCode = state.StateCode,
            CityIds = state.Cities.Select(c => c.Id).ToList()
        };

        return Ok(new { success = true, data = dto });
    }


    [HttpPost("CreateState")]
    public async Task<IActionResult> CreateState([FromBody] StateDto model)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Invalid input.", errors = ModelState });

        // 🔹 Validate country
        bool countryExists = await _context.Countries.AnyAsync(c => c.Id == model.CountryId);
        if (!countryExists)
            return BadRequest(new { success = false, message = "Invalid CountryId. Country not found." });

        // 🔹 Check duplicate name in the same country
        string stateName = model.Name.Trim().ToLower();
        bool exists = await _context.States
            .AnyAsync(s => s.Name.ToLower() == stateName && s.CountryId == model.CountryId);
        if (exists)
            return Conflict(new { success = false, message = $"State '{model.Name}' already exists in this country." });

        var state = new State
        {
            Name = model.Name.Trim(),
            CountryId = model.CountryId,
            StateCode = model.StateCode
        };

        _context.States.Add(state);
        await _context.SaveChangesAsync();

        // 🔹 Assign cities if any
        if (model.CityIds != null && model.CityIds.Any())
        {
            var cities = await _context.Cities
                .Where(c => model.CityIds.Contains(c.Id))
                .ToListAsync();

            foreach (var city in cities)
                city.StateId = state.Id;

            await _context.SaveChangesAsync();
        }

        return Ok(new { success = true, message = "State created successfully.", data = state });
    }


    [HttpPut("UpdateState/{id:int}")]
    public async Task<IActionResult> UpdateState(int id, [FromBody] StateDto model)
    {
        if (id != model.Id)
            return BadRequest(new { success = false, message = "State ID mismatch." });

        var state = await _context.States
            .Include(s => s.Cities)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (state == null)
            return NotFound(new { success = false, message = "State not found." });

        // 🔹 Ensure unique within same country
        string stateName = model.Name.Trim().ToLower();
        bool exists = await _context.States
            .AnyAsync(s => s.Id != id && s.Name.ToLower() == stateName && s.CountryId == model.CountryId);
        if (exists)
            return Conflict(new { success = false, message = $"State '{model.Name}' already exists in this country." });

        // 🔹 Update fields
        state.Name = model.Name.Trim();
        state.CountryId = model.CountryId;
        state.StateCode = model.StateCode;

        // 🔹 Reassign cities safely
        var oldCities = await _context.Cities.Where(c => c.StateId == id).ToListAsync();
        foreach (var city in oldCities)
            city.StateId = 0;

        if (model.CityIds != null && model.CityIds.Any())
        {
            var newCities = await _context.Cities
                .Where(c => model.CityIds.Contains(c.Id))
                .ToListAsync();

            foreach (var city in newCities)
                city.StateId = id;
        }

        await _context.SaveChangesAsync();
        return Ok(new { success = true, message = "State updated successfully." });
    }


    [HttpDelete("DeleteState/{id:int}")]
    public async Task<IActionResult> DeleteState(int id)
    {
        var state = await _context.States.FindAsync(id);
        if (state == null)
            return NotFound(new { success = false, message = "State not found." });

        // Unassign cities first
        var cities = await _context.Cities.Where(c => c.StateId == id).ToListAsync();
        foreach (var city in cities)
            city.StateId = 0;

        _context.States.Remove(state);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "State deleted successfully." });
    }
}
