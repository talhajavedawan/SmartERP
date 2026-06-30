using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.ChartofAccounts
{
    public class JournalVoucherRepo
    {
        DBContextERP context = new DBContextERP();
        public void AddJournalVoucher(JournalVoucher voucher)
        {
            if (voucher != null)
            {
                context.journalVouchers.Add(voucher);
                context.SaveChanges();
            }
        }
        public JournalVoucherStatus GetStatus()
        {
            return context.journalVoucherStatuses.FirstOrDefault(x => x.Id == 1);
        }
        public List<JournalVoucher> GetAllJournalVouchers()
        {
            return context.journalVouchers.ToList();
        }
        public JournalVoucher GetVoucherbyId(int id)
        {
            return context.journalVouchers.FirstOrDefault(x => x.Id == id);
        }
        public void UpdateVoucher(JournalVoucher voucher)
        {
            List<JournalTransaction> transactions = new List<JournalTransaction>();
            JournalVoucher dbVoucher = new JournalVoucher();
            dbVoucher = context.journalVouchers.
                FirstOrDefault(x => x.Id == voucher.Id);
            if (dbVoucher != null)
            {
                //dbVoucher = voucher;

                dbVoucher.ApprovedDate = voucher.ApprovedDate;
                dbVoucher.ClosingDate = voucher.ClosingDate;
                dbVoucher.coaTransactionsType = voucher.coaTransactionsType;
                dbVoucher.entryNo = voucher.entryNo;
                dbVoucher.isApproved = voucher.isApproved;
                dbVoucher.isReApproved = voucher.isReApproved;
                dbVoucher.isReviewed = voucher.isReviewed;
                dbVoucher.isVoid = voucher.isVoid;

                context.journalTransactions.RemoveRange(dbVoucher.journalTransactions);
                context.journalTransactions.AddRange(voucher.journalTransactions);
                dbVoucher.LastStatusChangeDate = voucher.LastStatusChangeDate;
                dbVoucher.MER = voucher.MER;
                dbVoucher.needReview = voucher.needReview;
                dbVoucher.PendingForClosing = voucher.PendingForClosing;
                dbVoucher.PendingForReApproval = voucher.PendingForReApproval;
                dbVoucher.postingDate = voucher.postingDate;
                dbVoucher.ReApprovalDate = voucher.ReApprovalDate;
                dbVoucher.stage = voucher.stage;
                if (voucher.userId != 0)
                {
                    dbVoucher.user = null;
                    var user = context.Users.FirstOrDefault(x => x.id == voucher.userId);
                    //dbVoucher.company = company;
                    dbVoucher.userId = user.id;
                }
                dbVoucher.voucherRefno = voucher.voucherRefno;
                if (voucher.statusId != 0)
                {
                    dbVoucher.JournalVoucherStatus = null;
                    var status = context.journalVoucherStatuses.FirstOrDefault(x => x.Id == voucher.statusId);
                    //dbVoucher.company = company;
                    dbVoucher.statusId = status.Id;
                    dbVoucher.JournalVoucherStatus = status;
                }
                if (voucher.company_Id != 0)
                {
                    dbVoucher.company = null;
                    var company = context.Companies.FirstOrDefault(x => x.Id == voucher.company_Id);
                    //dbVoucher.company = company;
                    dbVoucher.company_Id = company.Id;

                }
                if (voucher.dept_Id != 0)
                {
                    dbVoucher.department = null;
                    var department = context.Departments.FirstOrDefault(x => x.Id == voucher.dept_Id);
                    //dbVoucher.company = company;
                    dbVoucher.dept_Id = department.Id;
                    dbVoucher.department = department;
                }
                if (voucher.emp_Id != 0)
                {
                    dbVoucher.employee = null;
                    var employee = context.Employees.FirstOrDefault(x => x.EmpId == voucher.emp_Id);
                    //dbVoucher.company = company;
                    dbVoucher.emp_Id = employee.EmpId;

                }
                if (voucher.currencyId != 0)
                {
                    dbVoucher.Currency = null;
                    var currency = context.currencies.FirstOrDefault(x => x.Id == voucher.currencyId);
                    //dbVoucher.company = company;
                    dbVoucher.currencyId = currency.Id;
                }
                if (voucher.bill_Id != 0)
                {
                    dbVoucher.bill_Id = voucher.bill_Id;
                    dbVoucher.Bill = null;
                }
                if (voucher.purchaseOrder_Id != 0)
                {
                    dbVoucher.purchaseOrder_Id = voucher.purchaseOrder_Id;
                    dbVoucher.PurchaseOrder = null;
                }
                if(voucher.paymentGroupId!=0)
                {
                    dbVoucher.paymentGroupId = voucher.paymentGroupId;
                }
                context.SaveChanges();
            }

        }

        public int getAllActiveCount(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers =context.journalVouchers

            .Where(x => x.JournalVoucherStatus.isActive == true && x.isApproved == true && x.isVoid != true)
            .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }

        public int getAllInActiveCount(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers = context.journalVouchers
            .Where(x => x.JournalVoucherStatus.isActive == false && x.isApproved == true && x.isVoid != true)
            .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }

        public List<JournalVoucherStatus> getAllVoucherStatus()
        {
            return context.journalVoucherStatuses.ToList();
        }
        public List<JournalVoucherStatus> getAllActiveVoucherStatus()
        {

            return context.journalVoucherStatuses.Where(x => x.isActive == true).ToList();

        }
        public void addStatus(JournalVoucherStatus status)
        {
            context.journalVoucherStatuses.Add(status);
            context.SaveChanges();
        }
        /// <summary>
        /// Update Existing JV Status
        /// </summary>
        /// <param name="paymentStatus"></param>
        public void UpdateStatus(JournalVoucherStatus JVstatus)
        {
            JournalVoucherStatus _JVstatus = context.journalVoucherStatuses.FirstOrDefault(x => x.Id == JVstatus.Id);
            _JVstatus.Status = JVstatus.Status;
            _JVstatus.isActive = JVstatus.isActive;
            _JVstatus.forecolor = JVstatus.forecolor;
            _JVstatus.backcolor = JVstatus.backcolor;
            context.SaveChanges();
        }
        public JournalVoucherStatus getstatus(int voucherId)
        {
            return context.journalVoucherStatuses.FirstOrDefault(x => x.Id == voucherId);
        }
        public int getAllPendingForClosingAdministratorCount()
        {
            return context.journalVouchers

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true)
                .Count();
        }
        public int getAllPendingForApprovalCount(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers=context.journalVouchers
                .Where(x => x.isApproved == false && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }
        public int getAllPendingForReApprovalCount(int uid, List<int> companyIds, List<int> deptIds)
        {

            var vouchers= context.journalVouchers

                .Where(x => x.isApproved == true && x.isReApproved == false && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }
        public int getVoidRegisterCount(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers= context.journalVouchers
                .Where(x => x.isVoid == true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }
        public int getJVRegisterCount(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers= context.journalVouchers
                .Where(x => x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }
        public int getAllPendingForClosingCount(int uid, List<int> companyIds, List<int> deptIds)
        {

            var vouchers= context.journalVouchers

                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).Count();
        }
        public List<JournalVoucher> getAll()
        {
            return context.journalVouchers
            .Where(x => x.isVoid != true)
                .ToList();
        }
        public List<JournalVoucher> getAll(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers= context.journalVouchers
                .Where(x => x.isApproved == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) &&  deptIds.Contains((int)x.dept_Id) )
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
           
                
        }
        public List<JournalVoucher> getAllActive(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers= context.journalVouchers
                .Where(x => x.JournalVoucherStatus.isActive == true && x.isApproved == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public List<JournalVoucher> getAllInActive(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers= context.journalVouchers
                .Where(x => x.JournalVoucherStatus.isActive == false && x.isApproved == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public List<JournalVoucher> getAllPobyStatusId(int uid, int StatusId, List<int> companyIds, List<int> deptIds)
        {

            var vouchers= context.journalVouchers
                .Where(x => x.JournalVoucherStatus.Id == StatusId && x.isApproved == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public void updateStatusById(int voucherId, int statusId)
        {
            JournalVoucher journalVouchertoUpdate = context.journalVouchers.FirstOrDefault(x => x.Id == voucherId);
            journalVouchertoUpdate.JournalVoucherStatus.Id = statusId;
            context.SaveChanges();
        }
        public List<JournalVoucherStatus> getAllActiveJournalVoucherStatus()
        {
            return context.journalVoucherStatuses.Where(x => x.isActive == true).ToList();
        }
        public List<JournalVoucherStatus> getAllInActiveJournalVoucherStatus()
        {
            return context.journalVoucherStatuses.Where(x => x.isActive == false).ToList();
        }
        public JournalVoucher get(int journalVoucherId)
        {
            return context.journalVouchers
            .FirstOrDefault(x => x.Id == journalVoucherId);
        }
        public void updateStatus(int journalVoucherid, JournalVoucherStatus status)
        {
            JournalVoucher journalVouchertoUpdate = context.journalVouchers.FirstOrDefault(x => x.Id == journalVoucherid);
            journalVouchertoUpdate.JournalVoucherStatus = status;
            context.SaveChanges();
        }
        public List<JournalVoucher> getAllPendingForApproval(int uid, List<int> companyIds, List<int> deptIds)
        {
           var vouchers= context.journalVouchers
                .Where(x => x.isApproved != true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public JournalVoucher GetJournalVoucherById(int id)
        {
            return context.journalVouchers.
                FirstOrDefault(x => x.Id == id);

        }
        public List<JournalVoucher> getJVRegister(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers=context.journalVouchers
                .Where(x => x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public List<JournalVoucher> getAllPendingForReApproval(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers = context.journalVouchers
                .Where(x => x.isReApproved == false && x.isApproved == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public List<JournalVoucher> getAllPendingForClosing(int uid, List<int> companyIds, List<int> deptIds)
        {
            var vouchers= context.journalVouchers
                .Where(x => x.isApproved == true && x.PendingForClosing == true && x.isVoid != true && companyIds.Contains((int)x.company_Id) && deptIds.Contains((int)x.dept_Id))
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public List<JournalVoucher> getVoidRegister(int uid, List<int> companyIds, List<int> deptIds)
        {
           var vouchers= context.journalVouchers
                .Where(x => x.isVoid == true)
                .ToList();
            return vouchers.Where(x => x.journalTransactions.Count != 0).ToList();
        }
        public JournalVoucherStatus GetJournalVoucherStatus(int statusId)
        {
            return context.journalVoucherStatuses.FirstOrDefault(x=>x.Id == statusId);
        }
        public List<JournalVoucherStatus> GetAllJournalVoucherStatuses()
        {
            var statusList = context.journalVoucherStatuses
                .ToList();
            return statusList;

        }
        public List<JournalVoucherStatus> GetAllOpenJournalVoucherStatus()
        {
            var statusList = context.journalVoucherStatuses.Where(x => x.isActive == true)
                .ToList();
            return statusList;

        }
        public List<JournalVoucherStatus> GetAllCloseJournalVoucherStatus()
        {
            var statusList = context.journalVoucherStatuses.Where(x => x.isActive == false)
                .ToList();
            return statusList;
        }
        public List<JournalVoucher> GetAllJournalVoucherOpenAndClosed(int uid)
        {
            return context.journalVouchers
            .Where(x => x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true)
            .ToList();
        }
        public List<JournalVoucher> getAllActiveandUnapprovedJournalVoucher(int uid)
        {
            return context.journalVouchers
                .Where(x => x.JournalVoucherStatus.isActive == true && x.isApproved == true && x.isReApproved != false && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        public List<JournalVoucher> getAllInActiveandUnapprovedJournalVoucher(int uid)
        {
            return context.journalVouchers
                .Where(x => x.JournalVoucherStatus.isActive != true && x.isReApproved != false && x.isApproved == true && x.PendingForClosing != true && x.isVoid != true)
                .ToList();
        }
        public List<JournalVoucher> getAllJournalVouchersbyStatusId(int uid, int StatusId)
        {
            return context.journalVouchers
                .Where(x => x.JournalVoucherStatus.Id == StatusId && x.isApproved == true && x.isReApproved != false && x.isVoid != true && x.PendingForClosing != true)
                .ToList();
        }
        public JournalVoucher getVoucherbyId(int voucherId)
        {
            return context.journalVouchers.FirstOrDefault(x => x.Id == voucherId);
        }
        public List<JournalTransaction> getAllJournalTransactionsbyVoucherId(int id)
        {
            return context.journalTransactions.Where(x => x.coaTransactionsType == Enums.coaTransactionsType.JV && x.journalVoucher.Id == id).ToList();
        }
        public JournalVoucher GetJournalVoucher(int id)
        {
            return context.journalVouchers.FirstOrDefault(x => x.isVoid == false && x.Id == id);

        }
        public void setSotoVoid(int voucherId, bool isVoid)
        {
            JournalVoucher vouchertoUpdate = context.journalVouchers.FirstOrDefault(x => x.Id == voucherId);
            vouchertoUpdate.isVoid = isVoid;
            context.SaveChanges();
        }
        public List<JournalVoucher> GetVouchersByGroupId(int groupId)
        {
            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.journalVouchers
                .Where(x => x.paymentGroupId == groupId).ToList();
        }
        public List<JournalVoucher> GetVouchersByReceiptGroupId(int groupId)
        {
            if (groupId == 0)
                throw new NullReferenceException("Object can not be null");
            return context.journalVouchers
                .Where(x => x.receiptGroupId == groupId).ToList();
        }
        public void Approve(JournalVoucher voucher)
        {
            JournalVoucher vouchertoUpdate = context.journalVouchers.FirstOrDefault(x => x.Id == voucher.Id);
            vouchertoUpdate.isApproved = voucher.isApproved;
            vouchertoUpdate.stage = voucher.stage;
            context.SaveChanges();
        }
    }
}
