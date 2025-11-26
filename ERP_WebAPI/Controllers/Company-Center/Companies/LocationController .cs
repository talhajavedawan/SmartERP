using ERP_BL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//Location controller
namespace ERP_WebAPI.Controllers.CompanyCenter.Companies
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all Zones
        [HttpGet("zones")]
        public IActionResult GetZones() => Ok(_context.Zones.ToList());

        // Get countries by Zone
        [HttpGet("countries/{zoneId}")]
        public IActionResult GetCountries(int zoneId) =>
            Ok(_context.Countries.Where(c => c.ZoneId == zoneId).ToList());

        // Get states by Country
        [HttpGet("states/{countryId}")]
        public IActionResult GetStates(int countryId) =>
            Ok(_context.States.Where(s => s.CountryId == countryId).ToList());

        // Get cities by State
        [HttpGet("cities/{stateId}")]
        public IActionResult GetCities(int stateId) =>
        Ok(_context.Cities.Where(c => c.StateId == stateId).ToList());
    }
}
