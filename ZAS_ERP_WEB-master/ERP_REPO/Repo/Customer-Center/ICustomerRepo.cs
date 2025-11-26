using ERP_BL.Data;
using ERP_BL.Entities.Base.Addresses;
using ERP_BL.Entities.Base.Contacts;
using ERP_BL.Entities.Base.Persons;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.CompanyCenter.Customers;
using ERP_BL.Entities.Customer_Center;
using ERP_BL.Entities.Customer_Center.Dtos;
using ERP_BL.Enums;
using Microsoft.EntityFrameworkCore;

namespace ERP_REPO.Repo.CustomerCenter
{
    public interface ICustomerRepo
    {
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync(string status = "all");
        Task<CustomerCreateDto?> GetCustomerByIdAsync(int id);
        Task<CustomerDTO> CreateCustomerAsync(CustomerCreateDto dto, int userId);
        Task<CustomerDTO> UpdateCustomerAsync(int id, CustomerCreateDto dto, int userId);
        Task<bool> DeleteCustomerAsync(int id, int userId);
    }

    public class CustomerService : ICustomerRepo
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------- GET ALL --------------------
        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync(string status = "all")
        {
            var query = _context.Customers
                .Include(c => c.Company).ThenInclude(cmp => cmp.BusinessType)
                .Include(c => c.Company).ThenInclude(cmp => cmp.IndustryType)
                .Include(c => c.ContactPerson)
                .Include(c => c.Contact)
                .Include(c => c.BillingAddress).ThenInclude(a => a.City)
                .Include(c => c.ShippingAddress).ThenInclude(a => a.City)
                .Include(c => c.CreatedBy)
                .Include(c => c.LastModifiedBy)
                .AsQueryable();

            status = status?.Trim().ToLower() ?? "all";
            query = status switch
            {
                "active" => query.Where(c => c.IsActive),
                "inactive" => query.Where(c => !c.IsActive),
                _ => query
            };

            return await query
                .OrderByDescending(c => c.CreatedDate)
                .Select(c => new CustomerDTO
                {
                    Id = c.Id,
                    CompanyName = c.Company.CompanyName,
                    BusinessTypeName = c.Company.BusinessType.BusinessTypeName,
                    IndustryTypeName = c.Company.IndustryType.IndustryTypeName,
                    ContactEmail = c.Contact.Email,
                    ContactPhone = c.Contact.PhoneNumber,
                    ContactPersonName = c.ContactPerson.FirstName + " " + c.ContactPerson.LastName,
                    BillingAddress = $"{c.BillingAddress.AddressLine1}, {c.BillingAddress.AddressLine2}, {c.BillingAddress.City.Name}",
                    ShippingAddress = $"{c.ShippingAddress.AddressLine1}, {c.ShippingAddress.AddressLine2}, {c.ShippingAddress.City.Name}",
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate,
                    CreatedByName = c.CreatedBy != null ? c.CreatedBy.UserName : null,
                    LastModifiedByName = c.LastModifiedBy != null ? c.LastModifiedBy.UserName : null,
                    LastModified = c.LastModified
                })
                .AsNoTracking()
                .ToListAsync();
        }

        // -------------------- GET BY ID (for edit form) --------------------
        public async Task<CustomerCreateDto?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Company).ThenInclude(cmp => cmp.BusinessType)
                .Include(c => c.Company).ThenInclude(cmp => cmp.IndustryType)
                .Include(c => c.ContactPerson)
                .Include(c => c.Contact)
                .Include(c => c.BillingAddress).ThenInclude(a => a.City).ThenInclude(ci => ci.State).ThenInclude(st => st.Country)
                .Include(c => c.BillingAddress.Zone)
                .Include(c => c.ShippingAddress).ThenInclude(a => a.City).ThenInclude(ci => ci.State).ThenInclude(st => st.Country)
                .Include(c => c.ShippingAddress.Zone)
                .Include(c => c.CreatedBy)
                .Include(c => c.LastModifiedBy)
                .Where(c => c.Id == id)
                .Select(c => new CustomerCreateDto
                {
                    CompanyName = c.Company.CompanyName,
                    BusinessTypeId = c.Company.BusinessTypeId ?? 0,
                    BusinessTypeName = c.Company.BusinessType.BusinessTypeName,
                    IndustryTypeId = c.Company.IndustryTypeId ?? 0,
                    IndustryTypeName = c.Company.IndustryType.IndustryTypeName,
                    FirstName = c.ContactPerson.FirstName,
                    LastName = c.ContactPerson.LastName,
                    Email = c.Contact.Email,
                    PhoneNumber = c.Contact.PhoneNumber,
                    BillingAddressLine1 = c.BillingAddress.AddressLine1,
                    BillingAddressLine2 = c.BillingAddress.AddressLine2,
                    BillingCityId = c.BillingAddress.CityId ?? 0,
                    BillingCityName = c.BillingAddress.City.Name,
                    BillingStateId = c.BillingAddress.City.StateId,
                    BillingStateName = c.BillingAddress.City.State.Name,
                    BillingCountryId = c.BillingAddress.City.State.CountryId,
                    BillingCountryName = c.BillingAddress.City.State.Country.Name,
                    BillingZoneId = c.BillingAddress.ZoneId ?? 0,
                    BillingZoneName = c.BillingAddress.Zone.Name,
                    BillingZipCode = c.BillingAddress.Zipcode,
                    ShippingAddressLine1 = c.ShippingAddress.AddressLine1,
                    ShippingAddressLine2 = c.ShippingAddress.AddressLine2,
                    ShippingCityId = c.ShippingAddress.CityId ?? 0,
                    ShippingCityName = c.ShippingAddress.City.Name,
                    ShippingStateId = c.ShippingAddress.City.StateId,
                    ShippingStateName = c.ShippingAddress.City.State.Name,
                    ShippingCountryId = c.ShippingAddress.City.State.CountryId,
                    ShippingCountryName = c.ShippingAddress.City.State.Country.Name,
                    ShippingZoneId = c.ShippingAddress.ZoneId ?? 0,
                    ShippingZoneName = c.ShippingAddress.Zone.Name,
                    ShippingZipCode = c.ShippingAddress.Zipcode,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate,
                    CreatedByName = c.CreatedBy != null ? c.CreatedBy.UserName : null,
                    LastModified = c.LastModified,
                    LastModifiedByName = c.LastModifiedBy != null ? c.LastModifiedBy.UserName : null
                })
                .FirstOrDefaultAsync();
        }

        // -------------------- CREATE --------------------
        public async Task<CustomerDTO> CreateCustomerAsync(CustomerCreateDto dto, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var company = new Company
                {
                    CompanyName = dto.CompanyName,
                    BusinessTypeId = dto.BusinessTypeId,
                    IndustryTypeId = dto.IndustryTypeId,
                    CompanyType = CompanyType.CustomerCompany,
                    CreationDate = DateTime.UtcNow,
                    CreatedById = userId,
                    IsActive = dto.IsActive
                };

                var contactPerson = new Person { FirstName = dto.FirstName, LastName = dto.LastName };
                var contact = new Contact { Email = dto.Email, PhoneNumber = dto.PhoneNumber };

                var billingAddress = new Address
                {
                    AddressLine1 = dto.BillingAddressLine1,
                    AddressLine2 = dto.BillingAddressLine2,
                    CityId = dto.BillingCityId == 0 ? null : dto.BillingCityId,
                    StateId = dto.BillingStateId == 0 ? null : dto.BillingStateId,
                    CountryId = dto.BillingCountryId == 0 ? null : dto.BillingCountryId,
                    ZoneId = dto.BillingZoneId == 0 ? null : dto.BillingZoneId,
                    Zipcode = dto.BillingZipCode,
                    AddressType = Enums.Billing
                };

                var shippingAddress = new Address
                {
                    AddressLine1 = dto.ShippingAddressLine1,
                    AddressLine2 = dto.ShippingAddressLine2,
                    CityId = dto.ShippingCityId == 0 ? null : dto.ShippingCityId,
                    StateId = dto.ShippingStateId == 0 ? null : dto.ShippingStateId,
                    CountryId = dto.ShippingCountryId == 0 ? null : dto.ShippingCountryId,
                    ZoneId = dto.ShippingZoneId == 0 ? null : dto.ShippingZoneId,
                    Zipcode = dto.ShippingZipCode,
                    AddressType = Enums.Shipping
                };

                var customer = new Customer
                {
                    Company = company,
                    ContactPerson = contactPerson,
                    Contact = contact,
                    BillingAddress = billingAddress,
                    ShippingAddress = shippingAddress,
                    IsActive = dto.IsActive,
                    CreatedDate = DateTime.UtcNow,
                    CreatedById = userId
                };

                await _context.Customers.AddAsync(customer);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Map to CustomerDTO (output)
                var customerDto = new CustomerDTO
                {
                    Id = customer.Id,
                    CompanyName = company.CompanyName,
                    BusinessTypeName = await _context.BusinessTypes
                        .Where(bt => bt.Id == company.BusinessTypeId)
                        .Select(bt => bt.BusinessTypeName)
                        .FirstOrDefaultAsync(),
                    IndustryTypeName = await _context.IndustryTypes
                        .Where(it => it.Id == company.IndustryTypeId)
                        .Select(it => it.IndustryTypeName)
                        .FirstOrDefaultAsync(),
                    ContactPersonName = $"{contactPerson.FirstName} {contactPerson.LastName}",
                    ContactEmail = contact.Email,
                    ContactPhone = contact.PhoneNumber,
                    BillingAddress = $"{billingAddress.AddressLine1}, {billingAddress.AddressLine2}",
                    BillingZipcode = billingAddress.Zipcode,
                    ShippingAddress = $"{shippingAddress.AddressLine1}, {shippingAddress.AddressLine2}",
                    ShippingZipcode = shippingAddress.Zipcode,
                    IsActive = customer.IsActive,
                    CreatedDate = customer.CreatedDate,
                    CreatedByName = await _context.Users
                        .Where(u => u.Id == userId)
                        .Select(u => u.UserName)
                        .FirstOrDefaultAsync()
                };

                return customerDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error creating customer: {ex.Message}", ex);
            }
        }

        // -------------------- UPDATE --------------------
        public async Task<CustomerDTO> UpdateCustomerAsync(int id, CustomerCreateDto dto, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var existing = await _context.Customers
                    .Include(c => c.CreatedBy)
                    .Include(c => c.Company).ThenInclude(cmp => cmp.BusinessType)
                    .Include(c => c.Company).ThenInclude(cmp => cmp.IndustryType)
                    .Include(c => c.ContactPerson)
                    .Include(c => c.Contact)
                    .Include(c => c.BillingAddress)
                    .Include(c => c.ShippingAddress)
                    .Include(c => c.LastModifiedBy)
                    .FirstOrDefaultAsync(c => c.Id == id);


                if (existing == null) return null;

                existing.IsActive = dto.IsActive;
                existing.LastModified = DateTime.UtcNow;
                existing.LastModifiedById = userId;

                if (existing.Company != null)
                {
                    existing.Company.CompanyName = dto.CompanyName;
                    existing.Company.BusinessTypeId = dto.BusinessTypeId;
                    existing.Company.IndustryTypeId = dto.IndustryTypeId;
                    existing.Company.LastModified = DateTime.UtcNow;
                    existing.Company.LastModifiedById = userId;
                }

                if (existing.ContactPerson != null)
                {
                    existing.ContactPerson.FirstName = dto.FirstName;
                    existing.ContactPerson.LastName = dto.LastName;
                }

                if (existing.Contact != null)
                {
                    existing.Contact.Email = dto.Email;
                    existing.Contact.PhoneNumber = dto.PhoneNumber;
                }

                if (existing.BillingAddress != null)
                {
                    existing.BillingAddress.AddressLine1 = dto.BillingAddressLine1;
                    existing.BillingAddress.AddressLine2 = dto.BillingAddressLine2;
                    existing.BillingAddress.CityId = dto.BillingCityId == 0 ? null : dto.BillingCityId;
                    existing.BillingAddress.StateId = dto.BillingStateId == 0 ? null : dto.BillingStateId;
                    existing.BillingAddress.CountryId = dto.BillingCountryId == 0 ? null : dto.BillingCountryId;
                    existing.BillingAddress.ZoneId = dto.BillingZoneId == 0 ? null : dto.BillingZoneId;
                    existing.BillingAddress.Zipcode = dto.BillingZipCode;
                }

                if (existing.ShippingAddress != null)
                {
                    existing.ShippingAddress.AddressLine1 = dto.ShippingAddressLine1;
                    existing.ShippingAddress.AddressLine2 = dto.ShippingAddressLine2;
                    existing.ShippingAddress.CityId = dto.ShippingCityId == 0 ? null : dto.ShippingCityId;
                    existing.ShippingAddress.StateId = dto.ShippingStateId == 0 ? null : dto.ShippingStateId;
                    existing.ShippingAddress.CountryId = dto.ShippingCountryId == 0 ? null : dto.ShippingCountryId;
                    existing.ShippingAddress.ZoneId = dto.ShippingZoneId == 0 ? null : dto.ShippingZoneId;
                    existing.ShippingAddress.Zipcode = dto.ShippingZipCode;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                // Return updated DTO
                var customerDto = new CustomerDTO
                {
                    Id = existing.Id,
                    CompanyName = existing.Company?.CompanyName,
                    BusinessTypeName = existing.Company?.BusinessType?.BusinessTypeName,
                    IndustryTypeName = existing.Company?.IndustryType?.IndustryTypeName,

                    ContactPersonName = $"{existing.ContactPerson?.FirstName} {existing.ContactPerson?.LastName}",
                    ContactEmail = existing.Contact?.Email,
                    ContactPhone = existing.Contact?.PhoneNumber,
                    BillingAddress = $"{existing.BillingAddress?.AddressLine1}, {existing.BillingAddress?.AddressLine2}",
                    BillingZipcode = existing.BillingAddress?.Zipcode,
                    ShippingAddress = $"{existing.ShippingAddress?.AddressLine1}, {existing.ShippingAddress?.AddressLine2}",
                    ShippingZipcode = existing.ShippingAddress?.Zipcode,
                    IsActive = existing.IsActive,

                    CreatedDate = existing.CreatedDate,
                    CreatedByName = existing.CreatedBy?.UserName,


                    LastModified = existing.LastModified,
                    LastModifiedByName = existing.LastModifiedBy?.UserName
                };

                return customerDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error updating customer: {ex.Message}", ex);
            }
        }

        // -------------------- DELETE --------------------
        public async Task<bool> DeleteCustomerAsync(int id, int userId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer == null) return false;

            customer.IsActive = false;
            customer.LastModifiedById = userId;
            customer.LastModified = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}