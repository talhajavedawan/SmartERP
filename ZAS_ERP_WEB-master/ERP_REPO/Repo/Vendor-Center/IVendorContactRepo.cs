using ERP_BL.Data;
using ERP_BL.Entities;
using Microsoft.EntityFrameworkCore;
namespace ERP_REPO.Repo
{//
    public interface IVendorContactRepo : IGenericRepo<VendorContact>
    {
        Task<IEnumerable<VendorContact>> GetByVendorAsync(int vendorId);
    }

    public class VendorContactService : GenericService<VendorContact>, IVendorContactRepo
    {
        public VendorContactService(ApplicationDbContext context) : base(context)
        { }

        public async Task<IEnumerable<VendorContact>> GetByVendorAsync(int vendorId)
        {
            return await _context.VendorContacts
                .Where(c => c.VendorId == vendorId)
                .Include(c => c.Person)
                .Include(c => c.Contact)
                .ToListAsync();
        }

    } 
}
