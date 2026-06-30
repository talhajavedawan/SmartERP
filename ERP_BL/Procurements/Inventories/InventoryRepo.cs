using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Procurements.Inventories
{
    public interface IInventoryRepo
    {
        List<Inventory> getAll();
        List<Inventory> getAll(ERP_BL.Databases.User user);
        List<Inventory> GetByProduct(int prodId,List<int>compIds);
        Inventory get(int purchaseOrderId);
    }
    public class InventoryRepo
    {
        DBContextERP context = new DBContextERP();

        public List<Inventory> getAll()
        {
           return context.inventories.ToList();
        }
        public List<Inventory> GetByProduct(int prodId, List<int> compIds)
        {
           return context.inventories.Where(x => x.prodId == prodId && compIds.Contains((int)x.companyId)).ToList();
        }
    }
}
