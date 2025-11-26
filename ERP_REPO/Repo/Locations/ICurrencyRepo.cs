using ERP_BL.Data;
using ERP_BL.Entities;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace ERP_REPO.Repo
    {
    public interface ICurrencyRepo : IGenericRepo<Currency> { }

    public class CurrencyService : GenericService<Currency>, ICurrencyRepo
    {
        public CurrencyService(ApplicationDbContext context) : base(context) { }
    }

}
