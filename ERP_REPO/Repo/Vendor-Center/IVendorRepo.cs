using AutoMapper;
using ERP_BL.Data;
using ERP_BL.Entities;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.CompanyCenter.Departments;
using ERP_BL.Entities.Core.Users;
using ERP_BL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using System.Security.Claims;

namespace ERP_REPO.Repo
{//
    public interface IVendorRepo : IGenericRepo<Vendor>
    {
        Task<IEnumerable<Company>> GetSelectableCompaniesAsync();
        Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId);
        Task<bool> IsCircularRelationAsync(int vendorId, int? parentId);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task UpdateVendorAsync(VendorUpdateDto dto, ClaimsPrincipal user);
    }

    public class VendorService : GenericService<Vendor>, IVendorRepo
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public VendorService(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
            : base(context)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

    
        public async Task<IDbContextTransaction> BeginTransactionAsync() =>
            await _context.Database.BeginTransactionAsync();
        public async Task<bool> IsCircularRelationAsync(int vendorId, int? parentId)
        {
            if (!parentId.HasValue) return false;
            if (vendorId != 0 && vendorId == parentId.Value) return true;

            var visited = new HashSet<int>();
            var current = parentId;

            while (current.HasValue)
            {
                if (!visited.Add(current.Value)) return true;

                var parent = await _context.Vendors
                    .AsNoTracking()
                    .FirstOrDefaultAsync(v => v.Id == current.Value);

                if (parent == null) break;
                if (vendorId != 0 && parent.ParentVendorId == vendorId) return true;

                current = parent.ParentVendorId;
            }
            return false;
        }

  
        public async Task AddAsync(Vendor vendor, ClaimsPrincipal user)
        {
            if (vendor.Company == null)
                throw new InvalidOperationException("Vendor must have an associated Company.");

            vendor.Company.CompanyType = CompanyType.VendorCompany;

            if (await IsCircularRelationAsync(0, vendor.ParentVendorId))
                throw new InvalidOperationException("⚠ Invalid parent vendor — circular hierarchy detected!");

            await base.AddAsync(vendor, user);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateVendorAsync(VendorUpdateDto dto, ClaimsPrincipal user)
        {
            var vendor = await _context.Vendors
                .Include(v => v.Company)
                    .ThenInclude(c => c.Contact)
                .Include(v => v.ShippingAddress)
                .Include(v => v.BillingAddress)
                .Include(v => v.ClientCompanies)
                .Include(v => v.Departments)
                .FirstOrDefaultAsync(v => v.Id == dto.Id);

            if (vendor == null)
                throw new InvalidOperationException("Vendor not found.");

            if (vendor.Company == null)
                vendor.Company = new Company { CompanyType = CompanyType.VendorCompany };

            if (vendor.Company.Contact == null)
                vendor.Company.Contact = new ERP_BL.Entities.Base.Contacts.Contact();

            if (vendor.BillingAddress == null)
                vendor.BillingAddress = new ERP_BL.Entities.Base.Addresses.Address { AddressType = ERP_BL.Enums.Enums.Billing };

            if (vendor.ShippingAddress == null)
                vendor.ShippingAddress = new ERP_BL.Entities.Base.Addresses.Address { AddressType = ERP_BL.Enums.Enums.Shipping };

            _mapper.Map(dto, vendor);

            vendor.ShippingAddress?.ClearNavs();
            vendor.BillingAddress?.ClearNavs();

            vendor.ClientCompanies.Clear();
            if (dto.ClientCompanyIds != null && dto.ClientCompanyIds.Any())
            {
                var companies = await _context.Companies
                    .Where(c => dto.ClientCompanyIds.Contains(c.Id))
                    .ToListAsync();
                foreach (var company in companies)
                    vendor.ClientCompanies.Add(company);
            }

            vendor.Departments.Clear();
            if (dto.DepartmentIds != null && dto.DepartmentIds.Any())
            {
                var departments = await _context.Departments
                    .Where(d => dto.DepartmentIds.Contains(d.Id))
                    .ToListAsync();
                foreach (var dept in departments)
                    vendor.Departments.Add(dept);
            }

            if (int.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
                vendor.LastModifiedById = userId;

            vendor.ModifiedDate = DateTime.UtcNow;

            Update(vendor, user);
            await _context.SaveChangesAsync();
        }

        
        public async Task<IEnumerable<Company>> GetSelectableCompaniesAsync()
        {
            return await _context.Companies
                .Where(c => c.CompanyType == CompanyType.GroupCompany ||
                            c.CompanyType == CompanyType.IndividualCompany)
                .Select(c => new Company
                {
                    Id = c.Id,
                    CompanyName = c.CompanyName,
                    ParentCompanyId = c.ParentCompanyId  
                })
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId)
        {
            return await _context.Departments
                .Where(d => d.Companies.Any(c => c.Id == companyId))
                .Select(d => new Department
                {
                    Id = d.Id,
                    DeptName = d.DeptName,
                    ParentDepartmentId = d.ParentDepartmentId 
                })
                .AsNoTracking()
                .ToListAsync();
        }


        private async Task<(List<int> companyIds, List<int> departmentIds)> GetUserAssignmentsAsync(ClaimsPrincipal user)
        {
            var userIdStr = user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return (new List<int>(), new List<int>());

            var employee = await _context.Users
                .Include(u => u.Employee)
                    .ThenInclude(e => e.Companies)
                .Include(u => u.Employee)
                    .ThenInclude(e => e.Departments)
                .Where(u => u.Id == userId)
                .Select(u => u.Employee)
                .FirstOrDefaultAsync();

            var companyIds = employee?.Companies?.Select(c => c.Id).ToList() ?? new List<int>();
            var departmentIds = employee?.Departments?.Select(d => d.Id).ToList() ?? new List<int>();

            return (companyIds, departmentIds);
        }

        public async Task<(IEnumerable<Vendor> Data, int TotalCount)> GetAllForUserAsync(
            ClaimsPrincipal user,
            string status = "All",
            string? sortColumn = null,
            string? sortDirection = "asc",
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var (companyIds, departmentIds) = await GetUserAssignmentsAsync(user);

            if (!companyIds.Any() && !departmentIds.Any())
                return (Enumerable.Empty<Vendor>(), 0);

            Expression<Func<Vendor, bool>> filter = v =>
                (v.Company != null && companyIds.Contains(v.Company.Id)) ||
                (v.Departments.Any(d => departmentIds.Contains(d.Id)));

            string includeProperties =
                "Company,Company.Contact,Company.IndustryType,Company.BusinessType," +
                "Currency,VendorNature,ParentVendor,ParentVendor.Company," +
                "ClientCompanies,Departments," +
                "ShippingAddress,ShippingAddress.Country,ShippingAddress.State,ShippingAddress.City,ShippingAddress.Zone," +
                "BillingAddress,BillingAddress.Country,BillingAddress.State,BillingAddress.City,BillingAddress.Zone";

            return await base.GetAllAsync(
                filter: filter,
                includeProperties: includeProperties,
                status: status,
                sortColumn: sortColumn,
                sortDirection: sortDirection,
                searchTerm: searchTerm,
                pageNumber: pageNumber,
                pageSize: pageSize);
        }


    }
}

