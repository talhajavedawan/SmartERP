using ERP_BL.Databases;
using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.TransactionCounter
{
  public  class TransactionCounterRepo
    {
        DBContextERP context = new DBContextERP();
        public int GetAllCounterForCompany(List<Company> companies, int uid, TransactionItemType transactionType, TransactionCounterType transactionCounterType)
        {
            int count = 0;
            List<int> deptIds = new List<int>();
            List<int> companyId = new List<int>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            deptIds = user.employee.departments.Select(x => x.Id).ToList();
            companyId = new List<int>();
            foreach (var v in companies)
            {
                companyId.Add(v.Id);
            }
            switch (transactionType)
            {
                case TransactionItemType.Inquiry:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                          
                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.inquiries

                              .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:
                           
                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.inquiries
                                    .Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:

                            break;
                        case TransactionCounterType.PendingForReapproval:

                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:

                            break;

                    }
                    break;
                case TransactionItemType.Offer:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.offers
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.offers
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:

                            break;
                        case TransactionCounterType.PendingForReapproval:

                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:

                            break;

                    }
                    break;
                case TransactionItemType.Sale_Order:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.saleOrders
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.saleOrders
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.saleOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
               .Count();
                            break;
                            case TransactionCounterType.PendingForReapproval:
                            count = context.saleOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.saleOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
               .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Memorandum_Sale:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.memorandumSales
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.memorandumSales
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;

                      

                    }
                    break;
                case TransactionItemType.Sale_Invoice:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.saleInvoices
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.saleInvoices

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.saleInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.saleInvoices
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.saleInvoices
             .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.saleInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.saleInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.saleInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;             

                    }
                    break;
                case TransactionItemType.Purchase_Order:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.purchaseOrders
                                    .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.purchaseOrders

            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.purchaseOrders

             .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.purchaseOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Purchase_Invoice:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.purchaseInvoices

            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.purchaseInvoices

             .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.purchaseInvoices

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.purchaseInvoices
                                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.CostCenter:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.inquiries

                              .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            context.inquiries
                                    .Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.SummarySheet:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.inquiries

                              .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            context.inquiries
                                    .Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Bill:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.bills

            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.bills

            .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.BillStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Sale_Receipt:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.salesReceipts
                                     .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                                     .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:
                            count = context.salesReceipts
                                  .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)                
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:
                            count = context.salesReceipts
                                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                .Count();
                            break;

                        case TransactionCounterType.Open:
                            count = context.salesReceipts
                             .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == true && x.PendingForClosing != true)
                             .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.salesReceipts
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == false && x.PendingForClosing != true)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.salesReceipts
                                 .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.salesReceipts       
                              .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) /*&& (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                              .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.salesReceipts
                             .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                             .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.salesReceipts
                         .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                         .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.salesReceipts
                             .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.salesReceipts
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true) 
                            .Count();
                            break;

                    }
                    break;
                case TransactionItemType.FixedAssets:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.Assets
                .Where(x => deptIds.Contains((int)x.deptId) && companyId.Contains(x.companyId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id)
                && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId
                || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId
                || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
               
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
             
                .Count();
                            break;


                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.Assets

                
                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
               
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
              
                .Count();
                            break;

                       

                    }
                    break;
                case TransactionItemType.InterBank_Transfer:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                           count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                case TransactionCounterType.PendingForApproval:
                           count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                case TransactionCounterType.PendingForApprovalOwn:
                    count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.Open:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.interBankTransfers
                                .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                            .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                            .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Employee:
                    switch (transactionCounterType)
                    {
                    }
                    break;
                case TransactionItemType.Leave:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.leaveApplications
                            .Where(x => x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;

                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.leaveApplications
                            .Where(x =>  x.PendingForClosing == true )
                            .Count();
                            break;


                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.leaveApplications
                            .Where(x => x.isReApproved == false && x.isVoid != true)
                            .Count();
                            break;
                      
                    }
                    break;
                case TransactionItemType.JV:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.journalVouchers
                            .Where(x => x.isApproved == false && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id) && x.journalTransactions.Count != 0)
                            .Count();
                           
                            break;
                        case TransactionCounterType.PendingForApproval:


                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            break;

                        case TransactionCounterType.Open:
                            count = context.journalVouchers
                            .Where(x => x.JournalVoucherStatus.isActive == true && x.isApproved == true && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id) && x.journalTransactions.Count != 0)
                            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.journalVouchers
                             .Where(x => x.JournalVoucherStatus.isActive == false && x.isApproved == true && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id) && x.journalTransactions.Count != 0)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.journalVouchers
                              .Where(x =>x.journalTransactions.Count!=0 && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                              .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:

                            break;
                        case TransactionCounterType.PendingForClosingOwn:

                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count =  context.journalVouchers
                                .Where(x =>x.journalTransactions.Count!=0 && x.isApproved == true && x.isReApproved == false && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:

                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:

                            break;

                    }
                    break;
                case TransactionItemType.Admin_Bill:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.adminBills
                                   .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && x.isApproved == false && x.isVoid != true)
                                   .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.adminBills
                              .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.adminBills.Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:
                            count = context.adminBills         
                                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.BillStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.adminBills
                                  .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.BillStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                                  .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.adminBills
               .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && x.isApproved != false && x.PendingForClosing == true && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.adminBills

                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.adminBills

                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count =   context.adminBills
                                .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.adminBills
                .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count =   context.adminBills
                .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.InterCompanyBank_Transfer:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.interCompanyBankTransfers
                            .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:
                            count = context.interCompanyBankTransfers
                            .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                    .Count();
                            break;

                        case TransactionCounterType.Open:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.Close:
                            count =   context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true) 
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.interCompanyBankTransfers
                                        .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                        .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.interCompanyBankTransfers
                                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count =  context.interCompanyBankTransfers
                                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.interCompanyBankTransfers
                                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                                    .Count();
                            break;

                    }
                    break;
            }
            return count;
        }





        //For Multi Company and department
        public int GetAllCounterForCompanyDepartment(List<Company> companies,List<Department> departments, int uid, TransactionItemType transactionType, TransactionCounterType transactionCounterType)
        {
            int count = 0;
            List<int> deptIds = new List<int>();
            List<int> companyId = new List<int>();
            var user = context.Users.FirstOrDefault(x => x.id == uid);
            //deptIds = user.employee.departments.Select(x => x.Id).ToList();
            
            foreach (var v in companies)
            {
                companyId.Add(v.Id);
            }
            foreach (var _dept in departments)
            {
                deptIds.Add(_dept.Id);
            }
            switch (transactionType)
            {
                case TransactionItemType.Inquiry:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.inquiries

                              .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.inquiries
                                    .Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:

                            break;
                        case TransactionCounterType.PendingForReapproval:

                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:

                            break;

                    }
                    break;
                case TransactionItemType.Offer:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.offers
                                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.offers
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.offers
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.offerStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.offers
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.offers

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:

                            break;
                        case TransactionCounterType.PendingForReapproval:

                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:

                            break;

                    }
                    break;
                case TransactionItemType.Sale_Order:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.saleOrders
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.saleOrders
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleOrderStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.saleOrders
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.saleOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.saleOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.saleOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.saleOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
               .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Memorandum_Sale:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.memorandumSales
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.memorandumSales
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.memorandumSaleStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.memorandumSales
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.memorandumSales

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;



                    }
                    break;
                case TransactionItemType.Sale_Invoice:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.saleInvoices
                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.saleInvoices

                .Where(x => x.CreationDate.Value.Month <= DateTime.Today.Month && x.CreationDate.Value.Month >= DateTime.Today.Month - 1 && x.CreationDate.Value.Year == DateTime.Today.Year && (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.saleInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.saleInvoices
            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == true && x.isApproved == true && x.PendingForClosing != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.saleInvoices
             .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.saleInvoiceStatus.isActive == false && x.isApproved == true && x.PendingForClosing != true)
             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.saleInvoices
                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.saleInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.saleInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Purchase_Order:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.purchaseOrders
                                    .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.purchaseOrders

            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseOrderStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.purchaseOrders

             .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.PurchaseOrderStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.purchaseOrders

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.purchaseOrders

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Purchase_Invoice:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.purchaseInvoices

            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.PurchaseInvoiceStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.purchaseInvoices

             .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.PurchaseInvoiceStatus.isActive == false && x.isApproved == true && x.isVoid != true)
             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.purchaseInvoices

               .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.purchaseInvoices

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.purchaseInvoices
                                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.CostCenter:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.inquiries

                              .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            context.inquiries
                                    .Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.SummarySheet:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == false)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.inquiries

                              .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.inquiries.Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.inquiries
                                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isVoid != true && x.PendingForClosing != true && x.inquiryStatus.isActive == true && x.isApproved == true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            context.inquiries
                                    .Where(x => x.inquiryStatus.isActive == false && deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.PendingForClosing != true && x.isApproved == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.inquiries

                .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Bill:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.Open:

                            count = context.bills

            .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.BillStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.bills

            .Where(x => deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) && x.BillStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.bills

                .Where(x => (deptIds.Contains(x.dept_Id) && companyId.Contains((int)x.company_Id) || (deptIds.Contains((int)x.InterDepartment_Id) && companyId.Contains((int)x.InterCompany_Id) && x.isInterCompany == true)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Sale_Receipt:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.salesReceipts
                                     .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isApproved == false && x.isVoid != true)
                                     .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:
                            count = context.salesReceipts
                                  .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:
                            count = context.salesReceipts
                                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                .Count();
                            break;

                        case TransactionCounterType.Open:
                            count = context.salesReceipts
                             .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == true && x.PendingForClosing != true)
                             .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.salesReceipts
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && x.isApproved == true && x.isReApproved != false && x.saleReceiptStatus.isActive == false && x.PendingForClosing != true)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.salesReceipts
                                 .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.salesReceipts
                              .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) /*&& (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                              .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.salesReceipts
                             .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) /*&& (x.user.employee.EmpId == user.employee.EmpId)*/ && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                             .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.salesReceipts
                         .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                         .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.salesReceipts
                             .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.salesReceipts
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                            .Count();
                            break;

                    }
                    break;
                case TransactionItemType.FixedAssets:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.Assets
                .Where(x => deptIds.Contains((int)x.deptId) && companyId.Contains(x.companyId) && x.isApproved == false && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id)
                && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId
                || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId
                || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)

                .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)

                .Count();
                            break;


                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.Assets


                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.Assets

                .Where(x => deptIds.Contains(x.managingDept.Id) && companyId.Contains(x.OwnerCompany.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)

                .Count();
                            break;



                    }
                    break;
                case TransactionItemType.InterBank_Transfer:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.interBankTransfers
                             .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isApproved == false && x.isVoid != true)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:
                            count = context.interBankTransfers
                             .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:
                            count = context.interBankTransfers
                                    .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.Open:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.interBankTransfers
                                .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                            .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.interBankTransfers
                            .Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                            .Count();
                            break;

                    }
                    break;
                case TransactionItemType.Employee:
                    switch (transactionCounterType)
                    {
                    }
                    break;
                case TransactionItemType.Leave:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.leaveApplications
                            .Where(x => x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;

                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.leaveApplications
                            .Where(x => x.PendingForClosing == true)
                            .Count();
                            break;


                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.leaveApplications
                            .Where(x => x.isReApproved == false && x.isVoid != true)
                            .Count();
                            break;

                    }
                    break;
                case TransactionItemType.JV:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.journalVouchers
                            .Where(x => x.isApproved == false && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id) && x.journalTransactions.Count != 0)
                            .Count();

                            break;
                        case TransactionCounterType.PendingForApproval:


                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            break;

                        case TransactionCounterType.Open:
                            count = context.journalVouchers
                            .Where(x => x.JournalVoucherStatus.isActive == true && x.isApproved == true && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id) && x.journalTransactions.Count != 0)
                            .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.journalVouchers
                             .Where(x => x.JournalVoucherStatus.isActive == false && x.isApproved == true && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id) && x.journalTransactions.Count != 0)
                             .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.journalVouchers
                              .Where(x => x.journalTransactions.Count != 0 && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                              .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:

                            break;
                        case TransactionCounterType.PendingForClosingOwn:

                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.journalVouchers
                                .Where(x => x.journalTransactions.Count != 0 && x.isApproved == true && x.isReApproved == false && x.isVoid != true && companyId.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:

                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:

                            break;

                    }
                    break;
                case TransactionItemType.Admin_Bill:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:

                            count = context.adminBills
                                   .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && x.isApproved == false && x.isVoid != true)
                                   .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:

                            count = context.adminBills
                              .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false)
                                 .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:

                            count = context.adminBills.Where(x => deptIds.Contains((int)x.dept_Id) && companyId.Contains(x.company.Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isVoid != true && x.isApproved == false)
                                      .Count();
                            break;

                        case TransactionCounterType.Open:
                            count = context.adminBills
                                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.BillStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.adminBills
                                  .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.BillStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                                  .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.adminBills
               .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && x.isApproved != false && x.PendingForClosing == true && x.isVoid != true)
               .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.adminBills

                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.adminBills

                .Where(x => deptIds.Contains(x.department.Id) && companyId.Contains(x.company.Id) && x.isVoid != true && (x.user.employee.EmpId == user.employee.EmpId) && x.PendingForClosing == true && x.isApproved == true)
                .Count();
                            break;

                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.adminBills
                                .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.adminBills
                .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            count = context.adminBills
                .Where(x => (deptIds.Contains(x.department.Id) || x.user_Id == uid) && companyId.Contains((int)x.company_Id) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                .Count();
                            break;

                    }
                    break;
                case TransactionItemType.InterCompanyBank_Transfer:
                    switch (transactionCounterType)
                    {
                        case TransactionCounterType.PendingForApprovalDepartmental:
                            count = context.interCompanyBankTransfers
                            .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForApproval:
                            count = context.interCompanyBankTransfers
                            .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                            .Count();
                            break;
                        case TransactionCounterType.PendingForApprovalOwn:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == false && x.isVoid != true)
                                    .Count();
                            break;

                        case TransactionCounterType.Open:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.Close:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.interBankTransStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForClosingDepartmental:
                            count = context.interCompanyBankTransfers
                                        .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                        .Count();
                            break;
                        case TransactionCounterType.PendingForClosing:
                            count = context.interCompanyBankTransfers
                                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForClosingOwn:
                            count = context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                                    .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalDepartmental:
                            count = context.interCompanyBankTransfers
                                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && x.isApproved == true && x.isReApproved == false && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapproval:
                            count = context.interCompanyBankTransfers
                                .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.SupervisorId == user.employee.EmpId || x.user.employee.EmpId == user.employee.EmpId || x.user.employee.Supervisor.Supervisor.SupervisorId == user.employee.EmpId || x.user.employee.Supervisor.SupervisorId == user.employee.EmpId) && x.isReApproved == false && x.isApproved == true && x.isVoid != true)
                                .Count();
                            break;
                        case TransactionCounterType.PendingForReapprovalOwn:
                            context.interCompanyBankTransfers
                                    .Where(x => (deptIds.Contains(x.departmentFrom.Id) || deptIds.Contains(x.departmentTo.Id)) && (companyId.Contains(x.companyFrom.Id) || companyId.Contains(x.companyTo.Id)) && (x.user.employee.EmpId == user.employee.EmpId) && x.isReApproved == false && x.isVoid != true && x.isApproved == true)
                                    .Count();
                            break;

                    }
                    break;
            }
            return count;
        }
    }
}
