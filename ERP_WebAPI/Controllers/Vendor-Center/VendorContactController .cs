using AutoMapper;
using ERP_BL.Entities;
using ERP_REPO.Repo;
using Microsoft.AspNetCore.Mvc;
namespace ERP_WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VendorContactController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<VendorContactController> _logger;

        public VendorContactController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<VendorContactController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll(
            [FromQuery] string status = "All",
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? searchTerm = null,
            [FromQuery] int? vendorId = null     
        )
        {
            try
            {
                string includeProps = "Vendor.Company,Person,Contact,CreatedBy,LastModifiedBy";

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    sortColumn = sortColumn.ToLower() switch
                    {
                        "vendorname" => "Vendor.Company.CompanyName",
                        "firstname" => "Person.FirstName",
                        "lastname" => "Person.LastName",
                        _ => sortColumn
                    };
                }

              
                var result = await _unitOfWork.VendorContacts.GetAllAsync(
                    includeProperties: includeProps,
                    status: status,
                    sortColumn: sortColumn,
                    sortDirection: sortDirection,
                    searchTerm: searchTerm,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                );

                var contacts = result.Data.ToList();    
                int totalCount = result.TotalCount;

             
                if (vendorId.HasValue)
                {
                    contacts = contacts.Where(x => x.VendorId == vendorId.Value).ToList();
                    totalCount = contacts.Count;
                }

                var dtoList = _mapper.Map<IEnumerable<VendorContactGetDto>>(contacts);

                return Ok(new
                {
                    success = true,
                    totalCount,
                    pageNumber,
                    pageSize,
                    totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    data = dtoList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAll VendorContacts");
                return StatusCode(500, new
                {
                    error = "Failed to retrieve vendor contacts.",
                    details = ex.Message
                });
            }
        }


        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _unitOfWork.VendorContacts.GetByIdAsync(id, "Vendor.Company,Person,Contact,CreatedBy,LastModifiedBy");

            if (entity == null)
                return NotFound(new { message = $"Vendor Contact with ID {id} not found." });

            var dto = _mapper.Map<VendorContactGetDto>(entity);
            return Ok(dto);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] VendorContactCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vendor = await _unitOfWork.Vendors.GetByIdAsync(dto.VendorId, "Company");
            if (vendor == null)
                return BadRequest(new { message = "Invalid Vendor ID." });

            var entity = _mapper.Map<VendorContact>(dto);
            await _unitOfWork.VendorContacts.AddAsync(entity, User);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                success = true,
                message = "Vendor Contact created successfully.",
                data = new
                {
                    entity.Id,
                    entity.VendorId,
                    VendorName = vendor.Company?.CompanyName
                }
            });
        }

        [HttpPut("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] VendorContactUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "Mismatched contact ID." });

            var existing = await _unitOfWork.VendorContacts.GetByIdAsync(
                id, "Person,Contact,Vendor.Company"
            );

            if (existing == null)
                return NotFound(new { message = "Vendor Contact not found." });

            _mapper.Map(dto, existing);

            _unitOfWork.VendorContacts.Update(existing, User);
            await _unitOfWork.SaveAsync();

            return Ok(new
            {
                success = true,
                message = "Vendor Contact updated successfully.",
                data = new
                {
                    existing.Id,
                    existing.VendorId,
                    existing.PersonId,
                    existing.ContactId,
                    existing.IsActive,
                    VendorName = existing.Vendor?.Company?.CompanyName
                }
            });
        }


        [HttpGet("GetByVendor/{vendorId:int}")]
        public async Task<IActionResult> GetByVendor(int vendorId)
        {
            try
            {
                var contacts = await _unitOfWork.VendorContacts.GetByVendorAsync(vendorId);

                var dtoList = _mapper.Map<IEnumerable<VendorContactGetDto>>(contacts);

                return Ok(new
                {
                    success = true,
                    data = dtoList
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error GetByVendor");
                return StatusCode(500, new { message = "Failed to load contacts", details = ex.Message });
            }
        }


    }
}
