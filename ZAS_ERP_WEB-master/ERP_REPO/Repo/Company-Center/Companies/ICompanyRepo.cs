using ERP_BL.Data;
using ERP_BL.Entities.CompanyCenter.Companies;
using ERP_BL.Entities.Locations.Cities;
using ERP_BL.Entities.Locations.Countries;
using ERP_BL.Entities.Locations.States;
using ERP_BL.Entities.Locations.Zones;
using Microsoft.EntityFrameworkCore;

namespace ERP_REPO.Repo.CompanyCenter.Companies
{
    public interface ICompanyRepo
    {
        Task<IEnumerable<Company>> GetAllCompaniesAsync();
        Task<Company?> GetCompanyByIdAsync(int id);
        Task<Company> CreateCompanyAsync(Company company);
        Task<Company?> UpdateCompanyAsync(int id, Company company);
        Task<bool> DeleteCompanyAsync(int id);
    }

    public class CompanyService : ICompanyRepo
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region 🟢 Get Methods

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .AsNoTracking()
                .Where(c => !c.IsVoid)
                .Include(c => c.BusinessType)
                .Include(c => c.IndustryType)
                .Include(c => c.Group)
                .Include(c => c.Address)
                    .ThenInclude(a => a.Zone)
                .Include(c => c.Address)
                    .ThenInclude(a => a.Country)
                .Include(c => c.Address)
                    .ThenInclude(a => a.State)
                .Include(c => c.Address)
                    .ThenInclude(a => a.City)
                .Include(c => c.Contact)
                .AsSplitQuery()
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies
                .AsNoTracking()
                .Include(c => c.BusinessType)
                .Include(c => c.IndustryType)
                .Include(c => c.Group)
                .Include(c => c.Address)
                    .ThenInclude(a => a.Zone)
                .Include(c => c.Address)
                    .ThenInclude(a => a.Country)
                .Include(c => c.Address)
                    .ThenInclude(a => a.State)
                .Include(c => c.Address)
                    .ThenInclude(a => a.City)
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsVoid)
                .ConfigureAwait(false);
        }

        #endregion

        #region 🟢 Create

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            company.CreationDate = DateTime.UtcNow;
            company.IsVoid = false;
            company.IsActive = true;

            // EF will automatically track relationships
            await _context.Companies.AddAsync(company).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return company;
        }

        #endregion

        #region 🟢 Update

        public async Task<Company?> UpdateCompanyAsync(int id, Company updatedCompany)
        {
            if (updatedCompany == null)
                throw new ArgumentNullException(nameof(updatedCompany));

            var existingCompany = await _context.Companies
                .Include(c => c.Address)
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsVoid)
                .ConfigureAwait(false);

            if (existingCompany == null)
                return null;

            // 🔹 Update basic fields
            existingCompany.CompanyName = updatedCompany.CompanyName;
            existingCompany.IndustryTypeId = updatedCompany.IndustryTypeId;
            existingCompany.BusinessTypeId = updatedCompany.BusinessTypeId;
            existingCompany.OpeningDate = updatedCompany.OpeningDate;
            existingCompany.ClosingDate = updatedCompany.ClosingDate;
            existingCompany.IsSubsidiary = updatedCompany.IsSubsidiary;
            existingCompany.ParentCompanyId = updatedCompany.ParentCompanyId;
            existingCompany.IsActive = updatedCompany.IsActive;
            existingCompany.Ntn = updatedCompany.Ntn;
            existingCompany.LastModified = DateTime.UtcNow;
            existingCompany.CompanyType = updatedCompany.CompanyType;
            existingCompany.GroupId = updatedCompany.GroupId;

            // 🔹 Address update (safe & tracked)
            if (updatedCompany.Address != null)
            {
                existingCompany.Address ??= new();
                existingCompany.Address.AddressLine1 = updatedCompany.Address.AddressLine1;
                existingCompany.Address.AddressLine2 = updatedCompany.Address.AddressLine2;
                existingCompany.Address.Zipcode = updatedCompany.Address.Zipcode;
                existingCompany.Address.ZoneId = updatedCompany.Address.ZoneId;
                existingCompany.Address.CountryId = updatedCompany.Address.CountryId;
                existingCompany.Address.StateId = updatedCompany.Address.StateId;
                existingCompany.Address.CityId = updatedCompany.Address.CityId;
            }

            // 🔹 Contact update
            if (updatedCompany.Contact != null)
            {
                existingCompany.Contact ??= new();
                existingCompany.Contact.Email = updatedCompany.Contact.Email;
                existingCompany.Contact.PhoneNumber = updatedCompany.Contact.PhoneNumber;
                existingCompany.Contact.WebsiteUrl = updatedCompany.Contact.WebsiteUrl;
                existingCompany.Contact.WhatsAppNumber = updatedCompany.Contact.WhatsAppNumber;
                existingCompany.Contact.Fax = updatedCompany.Contact.Fax;
                existingCompany.Contact.LinkedIn = updatedCompany.Contact.LinkedIn;
            }

        
            _context.Companies.Update(existingCompany);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return existingCompany;
        }

        #endregion

        #region 🟢 Delete (Soft Delete)

        public async Task<bool> DeleteCompanyAsync(int id)
        {
            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id).ConfigureAwait(false);
            if (company == null || company.IsVoid)
                return false;

            company.IsVoid = true;
            company.LastModified = DateTime.UtcNow;

            _context.Companies.Update(company);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }

        #endregion
    }
}