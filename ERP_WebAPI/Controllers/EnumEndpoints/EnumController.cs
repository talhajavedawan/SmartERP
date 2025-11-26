using ERP_BL.Enums;
using ERP_BL.Enums.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ERP_WebAPI.Controllers.EnumEndpoints
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]

    public class EnumController : ControllerBase
    {
        [HttpGet("blood-groups")]
        public IActionResult GetBloodGroups()
        {
            var values = Enum.GetValues(typeof(BloodGroup))
                .Cast<BloodGroup>()
                .Select(bg => new
                {
                    value = bg.ToString(),
                    name = EnumHelper.GetDisplayName(bg)
                });

            return Ok(values);
        }

        [HttpGet("genders")]
        public IActionResult GetGenders()
        {
            var values = Enum.GetValues(typeof(Gender))
                             .Cast<Gender>()
                             .Select(e => new { Id = (int)e, Name = e.ToString() })
                             .ToList();
            return Ok(values);
        }

        [HttpGet("marital-statuses")]
        public IActionResult GetMaritalStatuses()
        {
            var values = Enum.GetValues(typeof(MaritalStatus))
                             .Cast<MaritalStatus>()
                             .Select(e => new { Id = (int)e, Name = e.ToString() })
                             .ToList();
            return Ok(values);
        }
    }
}
