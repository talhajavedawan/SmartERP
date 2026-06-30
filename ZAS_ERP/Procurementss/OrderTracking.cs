using ERP_BL;
using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZAS_ERP.Procurementss.SaleOrderss;

namespace ZAS_ERP.Procurementss
{
    public class OrderTracking
    {
        public int Id = 0;
        public int GroupId;
        public ERP_BL.Enums.TransactionItemType Type = TransactionItemType.Inquiry;
        ProcurementRepo repo = new ProcurementRepo();
        public bool isParent = false;
        List<AllOrdersView> allTransactions = new List<AllOrdersView>();
        public List<AllOrdersView> getTransactions(int id, TransactionItemType type)
        {
            List<SaleOrder> linkedSaleOrders = new List<SaleOrder>();
            if (type == TransactionItemType.Payments)
            {
                allTransactions = repo.getAllOrderTrackingNew(id, type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);
            }
            else if (type == TransactionItemType.TargetReward || type == TransactionItemType.STL)
            {
                allTransactions = repo.getAllOrderTrackingTransactions(id, type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.id);
            }
            else
            {
                allTransactions = repo.getAllOrderTrackingTransactions(id, type, SYSTEM_STATIC.AllowedPermissions, SYSTEM_STATIC.currentUser.employeeId);
            }
            return allTransactions;
        }
        public void getTransactions(int id, TransactionItemType type, bool _isParent)
        {
            List<SaleOrder> linkedSaleOrders = new List<SaleOrder>();

        }
    }
}
