using ERP_BL.Data;
using ERP_BL.Entities;
namespace ERP_REPO.Repo
{
    public interface IVendorNatureRepo : IGenericRepo<VendorNature>
    {
    }
    public class VendorNatureService : GenericService<VendorNature>, IVendorNatureRepo
    {
        public VendorNatureService(ApplicationDbContext context) : base(context)
        {
        }

    }
}
