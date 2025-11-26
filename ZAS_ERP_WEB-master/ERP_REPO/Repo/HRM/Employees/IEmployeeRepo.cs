using ERP_BL.Data;
using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Entities.HRM.Employees;
using ERP_BL.Entities.HRM.Employees.Dtos;
using ERP_BL.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ERP_REPO.Repo.HRM.Employees
{
    public interface IEmployeeRepo
    {
        Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync(string status = "all");
        Task<EmployeeDetailResponseDto?> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(EmployeeCreateDto dto);
        Task<bool> UpdateEmployeeAsync(EmployeeUpdateDto dto);
        Task<List<Employee>> GetAvailableEmployeesAsync();
        Task<List<Employee>> GetAvailableEmployeesForEditAsync(int userId);
        Task<(byte[]? Data, string? ContentType)?> GetEmployeeProfilePictureAsync(int employeeId);

        Task<bool> RemoveEmployeeProfilePictureAsync(int employeeId);
    }
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeeRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }
        private void EnsureAddressIsEmployee(Address? address)
        {
            if (address != null)
                address.AddressType = Enums.Employee;
        }

        // Process profile picture from file or byte array
        private void ProcessProfilePicture(EmployeeCreateDto dto, Employee employee)
        {
            if (dto.ProfilePicture != null)
            {
                employee.ProfilePicture = dto.ProfilePicture;
                employee.ProfilePictureContentType = dto.ProfilePictureContentType;
                employee.ProfilePictureSize = dto.ProfilePictureSize;
                employee.ProfilePictureFileName = dto.ProfilePictureFileName;
            }
        }

        // Remove profile picture
        private void RemoveProfilePicture(Employee employee)
        {
            employee.ProfilePicture = null;
            employee.ProfilePictureContentType = null;
            employee.ProfilePictureSize = null;
            employee.ProfilePictureFileName = null;
        }

        // GET ALL
        public async Task<IEnumerable<EmployeeResponseDto>> GetAllEmployeesAsync(string status = "all")
        {
            var query = _context.Employees
                .AsNoTracking()
                .Include(e => e.Manager)
                .Include(e => e.HRManager)
                .Include(e => e.CreatedByUser)
                .Include(e => e.LastModifiedByUser)
                .Include(e => e.Person)
                .Include(e => e.Contact)
                .Include(e => e.PermanentAddress)
                .Include(e => e.TemporaryAddress)
                .Include(e => e.Companies)
                .Include(e => e.Departments)
                .Select(e => new EmployeeResponseDto
                {
                    Id = e.Id,
                    SystemDisplayName = e.SystemDisplayName,
                    JobTitle = e.JobTitle,
                    HireDate = e.HireDate,
                    EmploymentType = e.EmploymentType,
                    EmployeeStatus = e.EmployeeStatus,
                    EmployeeStatusClass = e.EmployeeStatusClass,
                    IsActive = e.IsActive,
                    // Profile Picture
                    ProfilePictureUrl = e.ProfilePicture != null
                        ? $"/api/employees/{e.Id}/profile-picture"
                        : null,

                    ManagerId = e.ManagerId,
                    ManagerName = e.Manager != null ? e.Manager.SystemDisplayName : null,
                    HRManagerId = e.HRManagerId,
                    HRManagerName = e.HRManager != null ? e.HRManager.SystemDisplayName : null,
                    PayGrade = e.PayGrade,
                    ProbationPeriodEndDate = e.ProbationPeriodEndDate,
                    TerminationDate = e.TerminationDate,
                    Person = e.Person,
                    Contact = e.Contact,
                    PermanentAddress = e.PermanentAddress,
                    TemporaryAddress = e.TemporaryAddress,
                    Companies = e.Companies.Select(c => new CompanyDto { Id = c.Id, CompanyName = c.CompanyName }).ToList(),
                    Departments = e.Departments.Select(d => new DepartmentDto { Id = d.Id, DeptName = d.DeptName }).ToList(),
                    CreatedByUserId = e.CreatedByUserId,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.UserName : null,
                    LastModifiedByUserId = e.LastModifiedByUserId,
                    LastModifiedByUserName = e.LastModifiedByUser != null ? e.LastModifiedByUser.UserName : null,
                    CreatedDate = e.CreatedDate,
                    LastModifiedDate = e.LastModifiedDate
                });

            status = status?.Trim().ToLower() ?? "all";
            query = status switch
            {
                "active" => query.Where(e => e.IsActive),
                "inactive" => query.Where(e => !e.IsActive),
                _ => query
            };

            return await query.ToListAsync();
        }

        // GET BY ID
        public async Task<EmployeeDetailResponseDto?> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees
                .AsNoTracking()
                .Include(e => e.Manager)
                .Include(e => e.HRManager)
                .Include(e => e.CreatedByUser)
                .Include(e => e.LastModifiedByUser)
                .Include(e => e.Person)
                .Include(e => e.Contact)
                .Include(e => e.PermanentAddress)
                .Include(e => e.TemporaryAddress)
                .Include(e => e.Companies)
                .Include(e => e.Departments)
                .Where(e => e.Id == id)
                .Select(e => new EmployeeDetailResponseDto
                {
                    Id = e.Id,
                    SystemDisplayName = e.SystemDisplayName,
                    JobTitle = e.JobTitle,
                    HireDate = e.HireDate,
                    EmploymentType = e.EmploymentType,
                    EmployeeStatus = e.EmployeeStatus,
                    EmployeeStatusClass = e.EmployeeStatusClass,
                    IsActive = e.IsActive,
                    // Profile Picture
                    ProfilePicture = e.ProfilePicture,
                    ProfilePictureContentType = e.ProfilePictureContentType,
                    ProfilePictureSize = e.ProfilePictureSize,
                    ProfilePictureFileName = e.ProfilePictureFileName,

                    ManagerId = e.ManagerId,
                    ManagerName = e.Manager != null ? e.Manager.SystemDisplayName : null,
                    HRManagerId = e.HRManagerId,
                    HRManagerName = e.HRManager != null ? e.HRManager.SystemDisplayName : null,
                    PayGrade = e.PayGrade,
                    ProbationPeriodEndDate = e.ProbationPeriodEndDate,
                    TerminationDate = e.TerminationDate,
                    Person = e.Person,
                    Contact = e.Contact,
                    PermanentAddress = e.PermanentAddress,
                    TemporaryAddress = e.TemporaryAddress,
                    Companies = e.Companies.Select(c => new CompanyDto { Id = c.Id, CompanyName = c.CompanyName }).ToList(),
                    Departments = e.Departments.Select(d => new DepartmentDto { Id = d.Id, DeptName = d.DeptName }).ToList(),
                    CreatedByUserId = e.CreatedByUserId,
                    CreatedByUserName = e.CreatedByUser != null ? e.CreatedByUser.UserName : null,
                    LastModifiedByUserId = e.LastModifiedByUserId,
                    LastModifiedByUserName = e.LastModifiedByUser != null ? e.LastModifiedByUser.UserName : null,
                    CreatedDate = e.CreatedDate,
                    LastModifiedDate = e.LastModifiedDate
                })
                .FirstOrDefaultAsync();
        }
        // CREATE
        public async Task<Employee> CreateEmployeeAsync(EmployeeCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int userId = GetCurrentUserId();
                if (userId == 0) throw new UnauthorizedAccessException("User not authenticated.");

                var employee = new Employee
                {
                    SystemDisplayName = dto.SystemDisplayName,
                    JobTitle = dto.JobTitle,
                    HireDate = dto.HireDate,
                    ProbationPeriodEndDate = dto.ProbationPeriodEndDate,
                    TerminationDate = dto.TerminationDate,
                    EmploymentType = dto.EmploymentType,
                    EmployeeStatus = dto.EmployeeStatus,
                    EmployeeStatusClass = dto.EmployeeStatusClass,
                    ManagerId = dto.ManagerId,
                    HRManagerId = dto.HRManagerId,
                    PayGrade = dto.PayGrade,
                    IsActive = dto.IsActive,
                    CreatedByUserId = userId,
                    CreatedDate = DateTime.UtcNow
                };
                // Process profile picture
                ProcessProfilePicture(dto, employee);
                // Add related entities if provided
                if (dto.Person != null)
                {
                    _context.Persons.Add(dto.Person);
              
                }

                if (dto.Contact != null)
                {
                    _context.Contacts.Add(dto.Contact);
               
                }

                if (dto.PermanentAddress != null)
                {
                    EnsureAddressIsEmployee(dto.PermanentAddress);
                    _context.Addresses.Add(dto.PermanentAddress);
              
                }

                if (dto.TemporaryAddress != null)
                {
                    EnsureAddressIsEmployee(dto.TemporaryAddress);
                    _context.Addresses.Add(dto.TemporaryAddress);
          
                }

                await _context.SaveChangesAsync();
                // NOW link to employee (FKs are now valid) 
                employee.Person = dto.Person;
                employee.PersonId = dto.Person?.Id;

                employee.Contact = dto.Contact;
                employee.ContactId = dto.Contact?.Id;

                employee.PermanentAddress = dto.PermanentAddress;
                employee.PermanentAddressId = dto.PermanentAddress?.Id;

                employee.TemporaryAddress = dto.TemporaryAddress;
                employee.TemporaryAddressId = dto.TemporaryAddress?.Id; 

                //  Add employee and save
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                // Assign collections
                await AssignCollectionsAsync(employee, dto.CompanyIds, dto.DepartmentIds);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return employee;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateEmployeeAsync(EmployeeUpdateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.Employees
                    .Include(e => e.Person)
                    .Include(e => e.Contact)
                    .Include(e => e.PermanentAddress)
                    .Include(e => e.TemporaryAddress)
                    .Include(e => e.Companies)
                    .Include(e => e.Departments)
                    .FirstOrDefaultAsync(e => e.Id == dto.Id);

                if (existing == null) return false;

                int userId = GetCurrentUserId();
                if (userId == 0) throw new UnauthorizedAccessException();

                // --- Update main fields ---
                existing.SystemDisplayName = dto.SystemDisplayName;
                existing.JobTitle = dto.JobTitle;
                existing.HireDate = dto.HireDate;
                existing.ProbationPeriodEndDate = dto.ProbationPeriodEndDate;
                existing.TerminationDate = dto.TerminationDate;
                existing.EmploymentType = dto.EmploymentType;
                existing.EmployeeStatus = dto.EmployeeStatus;
                existing.EmployeeStatusClass = dto.EmployeeStatusClass;
                existing.ManagerId = dto.ManagerId;
                existing.HRManagerId = dto.HRManagerId;
                existing.PayGrade = dto.PayGrade;
                existing.IsActive = dto.IsActive;
                existing.LastModifiedByUserId = userId;
                existing.LastModifiedDate = DateTime.UtcNow;

                // Handle profile picture
                if (dto.RemoveProfilePicture)
                {
                    RemoveProfilePicture(existing);
                }
                else if (dto.ProfilePicture != null)
                {
                    ProcessProfilePicture(dto, existing);
                }

                // --- Update related entities safely ---
                await UpdateRelatedEntityAsync(
     existing.Person, dto.Person, _context.Persons, existing, "PersonId");

                await UpdateRelatedEntityAsync(
                    existing.Contact, dto.Contact, _context.Contacts, existing, "ContactId");

                await UpdateRelatedEntityAsync(
                    existing.PermanentAddress, dto.PermanentAddress, _context.Addresses, existing, "PermanentAddressId");

                await UpdateRelatedEntityAsync(
                    existing.TemporaryAddress, dto.TemporaryAddress, _context.Addresses, existing, "TemporaryAddressId");
                // --- Update company and department relationships ---
                existing.Companies.Clear();
                existing.Departments.Clear();
                await AssignCollectionsAsync(existing, dto.CompanyIds, dto.DepartmentIds);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        // GET PROFILE PICTURE
        public async Task<(byte[]? Data, string? ContentType)?> GetEmployeeProfilePictureAsync(int employeeId)
        {
            var pic = await _context.Employees
                .Where(e => e.Id == employeeId)
                .Select(e => new { e.ProfilePicture, e.ProfilePictureContentType })
                .FirstOrDefaultAsync();

            if (pic == null || pic.ProfilePicture == null) return null;
            return (pic.ProfilePicture, pic.ProfilePictureContentType);
        }

        // REMOVE PROFILE PICTURE
        public async Task<bool> RemoveEmployeeProfilePictureAsync(int employeeId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var employee = await _context.Employees.FindAsync(employeeId);
                if (employee == null) return false;

                RemoveProfilePicture(employee);
                employee.LastModifiedByUserId = GetCurrentUserId();
                employee.LastModifiedDate = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        private async Task UpdateRelatedEntityAsync<T>(
    T? existingEntity,
    T? incomingEntity,
    DbSet<T> dbSet,
    Employee employee,
    string fkPropertyName) // e.g., "TemporaryAddressId"
    where T : class
        {
            if (incomingEntity == null)
                return;

            // CASE 1: New entity
            if (existingEntity == null)
            {
                if (incomingEntity is Address addr)
                    addr.AddressType = Enums.Employee;

                dbSet.Add(incomingEntity);
                await _context.SaveChangesAsync(); // ← Generate Id

                // Set FK on employee
                var fkProp = typeof(Employee).GetProperty(fkPropertyName);
                var idProp = incomingEntity.GetType().GetProperty("Id");
                var newId = idProp?.GetValue(incomingEntity);
                fkProp?.SetValue(employee, newId);

                // Also set navigation
                var navProp = typeof(Employee).GetProperty(fkPropertyName.Replace("Id", ""));
                navProp?.SetValue(employee, incomingEntity);

                return;
            }

            // CASE 2: Update existing
            var entry = _context.Entry(existingEntity);
            var incomingProps = incomingEntity.GetType().GetProperties();

            foreach (var prop in incomingProps)
            {
                if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase)) continue;

                var type = prop.PropertyType;
                bool isSimple = type.IsValueType || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
                if (!isSimple) continue;

                var value = prop.GetValue(incomingEntity);
                entry.Property(prop.Name).CurrentValue = value;
            }

            entry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        // HELPER: Assign Collections
        private async Task AssignCollectionsAsync(Employee employee, List<int> companyIds, List<int> departmentIds)
        {
            if (companyIds?.Any() == true)
            {
                var companies = await _context.Companies
                    .Where(c => companyIds.Contains(c.Id) && !c.IsVoid)
                    .ToListAsync();
                employee.Companies = companies;
            }

            if (departmentIds?.Any() == true)
            {
                var departments = await _context.Departments
                    .Where(d => departmentIds.Contains(d.Id))
                    .ToListAsync();
                employee.Departments = departments;
            }
        }

        // AVAILABLE EMPLOYEES
        public async Task<List<Employee>> GetAvailableEmployeesAsync()
        {
            return await _context.Employees
                .Include(e => e.Person)
                .Include(e => e.Contact)
                .Where(e => e.IsActive && !_context.Users.Any(u => u.EmployeeId == e.Id))
                .ToListAsync();
        }

        public async Task<List<Employee>> GetAvailableEmployeesForEditAsync(int userId)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return await _context.Employees
                .Include(e => e.Person)
                .Include(e => e.Contact)
                .Where(e => e.IsActive || (currentUser != null && e.Id == currentUser.EmployeeId))
                .Where(e => !_context.Users.Any(u => u.EmployeeId == e.Id && u.Id != userId && !u.IsVoid))
                .ToListAsync();
        }
    }
}