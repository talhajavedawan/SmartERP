using ERP_BL.Databases;
using ERP_BL.Procurements.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public interface IAdjustmentRepo
    {      
        //Inventory Adjustment CRUD

        void Add(InventoryAdjustment adjustment);
        void addStatus(InventoryAdjustmentStatus status);
        void update(InventoryAdjustment _adjustment );
        void updateStatus(InventoryAdjustmentStatus status);

        //Inventory Adjustment list
        List<InventoryAdjustment> getAll();
        List<InventoryAdjustment> getAll(ERP_BL.Databases.User user);
        List<InventoryAdjustment> getAll(int userId);
        List<InventoryAdjustment> getAllActive(int userId);
        List<InventoryAdjustment> getAllInActive(int userId);
        List<InventoryAdjustment> getAllPendingForApprovalDepartmental(int userId);
        List<InventoryAdjustment> getAllPendingForApproval(int userId);
        List<InventoryAdjustment> getAllPendingForApprovalOwn(int userId);
        List<InventoryAdjustment> getAllPendingForAdministrator(int userId);
        List<InventoryAdjustment> getAllPendingForClosingDepartmental(int userId);
        List<InventoryAdjustment> getAllPendingForClosing(int userId);
        List<InventoryAdjustment> getAllPendingForClosingOwn(int userId);
        List<InventoryAdjustment> getAllPendingForClosingAdministrator();
        List<InventoryAdjustment> getAllAdjustmentbyStatusId(int userId, int statusId);
        List<InventoryAdjustment> getAdjustmentRegisterAdministrator();
        List<InventoryAdjustment> getAdjustmentRegister(int userId);
        List<InventoryAdjustment> getVoidRegister(int userId);
        List<InventoryAdjustment> getVoidRegisterOwn(int userId);
        List<InventoryAdjustment> getVoidRegisterAdministrator();
        List<InventoryAdjustmentStatus> getAllInActiveInventoryAdjustmentStatus();
        List<InventoryAdjustmentStatus> getAllInventoryAdjustmentStatus();
        InventoryAdjustmentStatus getstatus(int inventoryAdjustmentstatusid);
        int getInventoryAdjustmentCountByStatusId(int uid, int statusId);


        InventoryAdjustment get(int _adjustmentId);
        //Inventory Adjustment counters
        int getAllPendingForApprovalDepartmentalCount(int userId);
        int getAllPendingForApprovalCount(int userId);
        int getAllPendingForApprovalCountOwn(int userId);
        int getAllPendingForAdministratorCount();
        int getInventoryAdjustmentRegisterCountOWn();
        int getAllPendingForClosingDepartmentalCount(int userId);
        int getAllPendingForClosingCount(int userId);
        int getAllPendingForClosingCountOwn(int userId);
        int getInventoryAdjustmentRegisterCount(int userId);
        int getAllPendingForClosingAdministratorCount();
        int getVoidRegisterAdministratorCount();
        int getVoidRegisterCount(int userId);
    }

    public class AdjustmentRepo : IAdjustmentRepo
    {
        DBContextERP context = new DBContextERP();

        public void Add(InventoryAdjustment adjustment)
        {
            context.inventoryAdjustments.Add(adjustment);
            context.SaveChanges();
        }
        public void addStatus(InventoryAdjustmentStatus status)
        {
            context.adjustmentStatuses.Add(status);
            context.SaveChanges();
        }
        public InventoryAdjustment get(int _adjustmentId)
        {
            return context.inventoryAdjustments.FirstOrDefault(x => x.Id == _adjustmentId);
        }

        public List<InventoryAdjustment> getAdjustmentRegister(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments
        .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true).ToList();
        }

        public List<InventoryAdjustment> getAdjustmentRegisterAdministrator()
        {
            return context.inventoryAdjustments
        .Where(x => x.isVoid != true).ToList();
        }

        public List<InventoryAdjustment> getAll()
        {
            return context.inventoryAdjustments.ToList();
        }
        public List<InventoryAdjustment> getAll(User user)
        {
            return context.inventoryAdjustments.Where(x => x.creator_Id == user.id).ToList();
        }

        public List<InventoryAdjustment> getAll(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true )
                .ToList();
        }

        public List<InventoryAdjustment> getAllActive(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.AdjustmentStatus.isActive == true && x.isApproved == true )
                .ToList();
        }

        public List<InventoryAdjustment> getAllAdjustmentbyStatusId(int userId, int statusId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.AdjustmentStatus.Id == statusId && x.isApproved == true  && x.isApproved == true)
                .ToList();
        }
        public List<InventoryAdjustment> getAllInActive(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.AdjustmentStatus.isActive == false && x.isApproved == true )
                .ToList();
        }

        public List<InventoryAdjustment> getAllPendingForAdministrator(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments

                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }

        public int getAllPendingForAdministratorCount()
        {
            return context.inventoryAdjustments

                 .Where(x => x.isApproved == false)
                 .Count();
        }

        public List<InventoryAdjustment> getAllPendingForApproval(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }

        public int getAllPendingForApprovalCount(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments.Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)   && x.isVoid != true && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false).Count();
        }

        public int getAllPendingForApprovalCountOwn(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments


                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
        }

        public List<InventoryAdjustment> getAllPendingForApprovalDepartmental(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .ToList();
        }

        public int getAllPendingForApprovalDepartmentalCount(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments


                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == false)
                .Count();
        }

        public List<InventoryAdjustment> getAllPendingForApprovalOwn(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments

                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .ToList();
        }

        public List<InventoryAdjustment> getAllPendingForClosing(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }

        public List<InventoryAdjustment> getAllPendingForClosingAdministrator()
        {
            return context.inventoryAdjustments
                .Where(x => x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }

      

        public int getAllPendingForClosingAdministratorCount()
        {
            return context.inventoryAdjustments

               .Where(x => x.isApproved == true && x.PendingForClosing == true)
               .Count();
        }

        public int getAllPendingForClosingCount(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments


                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && (x.Creator.employee.SupervisorId == user.employee.EmpId || x.Creator.employee.EmpId == user.employee.EmpId || x.Creator.employee.Supervisor.SupervisorId == user.employee.EmpId || x.Creator.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }

        public int getAllPendingForClosingCountOwn(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && (x.Creator.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }

        public List<InventoryAdjustment> getAllPendingForClosingDepartmental(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments

                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .ToList();
        }

        public int getAllPendingForClosingDepartmentalCount(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
        }

        public List<InventoryAdjustment> getAllPendingForClosingOwn(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && (x.Creator.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true)
                .ToList();
        }

        public int getInventoryAdjustmentRegisterCount(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);

            return context.inventoryAdjustments
                   .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true).Count();
        }

        public int getInventoryAdjustmentRegisterCountOWn()
        {
            return context.inventoryAdjustments

           .Where(x => x.isVoid != true).Count();
        }

        public List<InventoryAdjustment> getVoidRegister(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();
            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .ToList();
        }
        public int getVoidRegisterAdministratorCount()
        {
            return context.inventoryAdjustments
                    .Where(x => x.isVoid == true)
                    .Count();
        }

        public int getVoidRegisterCount(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);
            return context.inventoryAdjustments

                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true)
                .Count();
        }

        public List<InventoryAdjustment> getVoidRegisterOwn(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId);
            List<int> deptIds = new List<int>();

            foreach (var dpt in user.employee.departments)
                deptIds.Add(dpt.Id);
            List<int> companyIds = new List<int>();

            foreach (var comp in user.employee.Companies)
                companyIds.Add(comp.Id);

            return context.inventoryAdjustments
                .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id) && x.isVoid == true && (x.Creator.employee.EmpId == user.employee.EmpId))
                .ToList();
        }
        public List<InventoryAdjustment> getVoidRegisterAdministrator()
        {
            return context.inventoryAdjustments
                .Where(x => x.isVoid == true)
            .ToList();
        }

        public void update(InventoryAdjustment _adjustment)
        {
            var adjustment = context.inventoryAdjustments.FirstOrDefault(x => x.Id ==_adjustment.Id);
            adjustment = _adjustment;
            context.SaveChanges();
        }

        public void updateStatus(InventoryAdjustmentStatus status)
        {
            InventoryAdjustmentStatus dbStatus = context.adjustmentStatuses.FirstOrDefault(x => x.Id == status.Id);
            dbStatus = status;
            context.SaveChanges();
        }
        public List<InventoryAdjustmentStatus> getAllInActiveInventoryAdjustmentStatus()
        {
            List<InventoryAdjustmentStatus> statuses = new List<InventoryAdjustmentStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.adjustmentStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }
        public InventoryAdjustmentStatus getstatus(int inventoryAdjustmentstatusid)
        {
            return context.adjustmentStatuses.FirstOrDefault(x => x.Id == inventoryAdjustmentstatusid);
        }
        public List<InventoryAdjustmentStatus> getAllInventoryAdjustmentStatus()
        {
            return context.adjustmentStatuses.ToList();
        }
        public List<InventoryAdjustmentStatus> getAllActiveInventoryAdjustmentStatus()
        {
            List<InventoryAdjustmentStatus> statuses = new List<InventoryAdjustmentStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.adjustmentStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        public int getInventoryAdjustmentCountByStatusId(int uid, int statusId)
        {
            using (var _DbContext = new DBContextERP())
            {
                if (uid != 0)
                {
                    var user = _DbContext.Users.FirstOrDefault(x => x.id == uid);
                    List<int> deptIds = new List<int>();
                    List<int> companyIds = new List<int>();
                    foreach (var comp in user.employee.Companies)
                        companyIds.Add(comp.Id);
                    foreach (var dpt in user.employee.departments)
                        deptIds.Add(dpt.Id);
                    return _DbContext.inventoryAdjustments
                        .Where(x => deptIds.Contains((int)x.depId_Id) && companyIds.Contains((int)x.company_Id)  && x.isVoid != true && x.AdjustmentStatus.Id == statusId && x.isApproved == true)
                        .Count();
                }
                else
                    return _DbContext.inventoryAdjustments.Where(x => x.AdjustmentStatus.Id == statusId && x.isApproved == true).Count();
            }
        }

        public List<Inventory>  GetbyProductId(int _prodId, DateTime  purchaseInvoiceDate)
        {
            return context.inventories.Where(x => x.prodId == _prodId && x.creationDate < purchaseInvoiceDate.Date && x.TransactionsType==Enums.InventoryTransactionsType.Adjustment && x.adjustment_Id!=null).ToList();
        }
        public List<InventoryAdjustmentStatus> getAllActiveAdjustmentStatus()
        {
            List<InventoryAdjustmentStatus> statuses = new List<InventoryAdjustmentStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.adjustmentStatuses.Where(x => x.isActive == true).ToList();
            }
            return statuses;
        }
        public List<InventoryAdjustmentStatus> getAllInActiveAdjustmentStatus()
        {
            List<InventoryAdjustmentStatus> statuses = new List<InventoryAdjustmentStatus>();
            using (var _DbContext = new DBContextERP())
            {
                statuses = _DbContext.adjustmentStatuses.Where(x => x.isActive == false).ToList();
            }
            return statuses;
        }
        public void Approve(InventoryAdjustment adjustment)
        {
            InventoryAdjustment adjustmenttoUpdate = context.inventoryAdjustments.FirstOrDefault(x => x.Id == adjustment.Id);
            adjustmenttoUpdate.isApproved = adjustment.isApproved;
            adjustmenttoUpdate.stage = adjustment.stage;
            context.SaveChanges();
        }
        public void setAdjustmenttoVoid(int Id, bool isVoid)
        {
            InventoryAdjustment item = context.inventoryAdjustments.FirstOrDefault(x => x.Id == Id);
            item.isVoid = isVoid;
            context.SaveChanges();
        }
 
    }
}
