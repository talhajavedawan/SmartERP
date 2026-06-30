using DevExpress.Utils;
using ERP_BL.AssetsRentals;
using ERP_BL.AssetsRentals.RentalContracts;
using ERP_BL.AssetsRentals.RentalInvoices;
using ERP_BL.AssetsRentals.RentalOrders;
using ERP_BL.AssetsRentals.TenantRentals;
using ERP_BL.ChartofAccounts;
using ERP_BL.Enums;
using ERP_BL.Payments;
using ERP_BL.Procurements.AdminBills;
using ERP_BL.Procurements.InterBankTransfers;
using ERP_BL.Procurements.LoansAdvances;
using ERP_BL.ToDoTasks;
using ERP_BL.ToDoTasks.Taskss;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{

    public class ProcurementRepo
    {


        DBContextERP context = new DBContextERP();

        public CommentLog GetComment(int commentId)
        {
            var comment = context.CommentLogs.FirstOrDefault(x=>x.Id == commentId);
            return comment;
        }

        /// <summary>
        /// Add new Comment
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object</param>
        public void Add(CommentLog viewInfo)
        {
            context.CommentLogs.Add(viewInfo);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Comment for  employee = " + viewInfo.employee.person.FName + " Id= " + viewInfo.Id);

        }
        /// <summary>
        /// Add new Comment for transaction
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        public void Addaa(int Transactionid, TransactionItemType Transactiontype, string comment, List<User> users, int empID)
        {
            CommentLog viewInfo = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                viewInfo.employeeId = empID;
                viewInfo.TransactionId = Transactionid;
                viewInfo.TransactionType = Transactiontype;
                viewInfo.Timestamp = System.DateTime.Now;
                viewInfo.ReadTimestamp = System.DateTime.Now;
                viewInfo.TaggedList = new List<User>();
                viewInfo.ReplyCommentId = 116;
                if (users != null)
                    foreach (var use in users)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            viewInfo.TaggedList.Add(user);
                    }
                //viewInfo.Info = Info.ToString();
                viewInfo.Comment = comment;
                context.CommentLogs.Add(viewInfo);

                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
            }
        }


        public int? AddCommentLinkNotification(int Transactionid, TransactionItemType Transactiontype, CommentLog comment, int empID, int? flagId)
        {
            CommentLog commentLog = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                commentLog.employeeId = empID;
                if (comment.AssigneeId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.AssigneeId = comment.AssigneeId;
                }
                if (comment.Category != null || comment.CategoryId != null)
                {
                    commentLog.CategoryId = comment.CategoryId;
                    commentLog.Category = comment.Category;

                }
                if (comment.managerId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.managerId = comment.managerId;
                }
                if (comment.salesPersonId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.salesPersonId = comment.salesPersonId;
                }
                if (comment.financePersonId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.financePersonId = comment.financePersonId;
                }

                commentLog.FlagId = flagId;
                commentLog.TransactionId = Transactionid;
                commentLog.TransactionType = Transactiontype;
                commentLog.Timestamp = System.DateTime.Now;
                commentLog.ReadTimestamp = System.DateTime.Now;
                commentLog.Subject = comment.Subject;
                if (comment.TaggedList != null)
                {
                    commentLog.TaggedList = new List<User>();


                    foreach (var use in comment.TaggedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedList.Add(user);
                    }
                }
                if (comment.CCUsersList != null)
                {
                    commentLog.CCUsersList = new List<User>();
                    foreach (var use in comment.CCUsersList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCUsersList.Add(user);
                    }
                }
                if (comment.TaggedRecomenndedList != null)
                {
                    commentLog.TaggedRecomenndedList = new List<User>();
                    foreach (var use in comment.TaggedRecomenndedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedRecomenndedList.Add(user);
                    }
                }
                if (comment.CCRecomenndedList != null)
                {
                    commentLog.CCRecomenndedList = new List<User>();
                    foreach (var use in comment.CCRecomenndedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCRecomenndedList.Add(user);
                    }
                }

                commentLog.ReplyCommentId = comment.ReplyCommentId;

                //commentLog.Info = Info.ToString();
                commentLog.Comment = comment.Comment;
                context.CommentLogs.Add(commentLog);
                try
                {
                    context.SaveChanges();
                }

                catch (Exception ex)
                {
                    throw;
                }
                SystemLog.LogInfo(this.GetType(), "Added commentLog  Id= " + commentLog.Id);
                return commentLog.Id;
            }
            else
            {
                return null;
            }
        }


        public void UpdateCommentLinkNotification(CommentLog comment)
        {
            DBContextERP dBContext = new DBContextERP();
            bool flagGlow = false;
            if (comment.FlagId != null && comment.FlagId != 0)
            {
                NotificationFlag notificationFlag = dBContext.notificationFlags.FirstOrDefault(x => x.Id == comment.FlagId);
                flagGlow = notificationFlag.canGlow;
            }
            CommentLog commentLog = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                var notifications = dBContext.Notifications.Where(x => x.commentLogId == comment.Id).ToList();
                commentLog.employeeId = comment.employeeId;
                if (comment.AssigneeId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.AssigneeId = comment.AssigneeId;
                }
                if (comment.Category != null || comment.CategoryId != null)
                {
                    commentLog.CategoryId = comment.CategoryId;
                    //commentLog.Category = comment.Category;

                }

                commentLog.FlagId = comment.FlagId;
                commentLog.TransactionId = comment.TransactionId;
                commentLog.TransactionType = comment.TransactionType;
                commentLog.Timestamp = System.DateTime.Now;
                commentLog.ReadTimestamp = System.DateTime.Now;
                commentLog.Subject = comment.Subject;
                if (comment.TaggedList != null)
                {
                    commentLog.TaggedList = new List<User>();


                    foreach (var use in comment.TaggedList)
                    {
                        User user = new User();
                        user = dBContext.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedList.Add(user);
                    }
                }
                if (comment.CCUsersList != null)
                {
                    commentLog.CCUsersList = new List<User>();
                    foreach (var use in comment.CCUsersList)
                    {
                        User user = new User();
                        user = dBContext.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCUsersList.Add(user);
                    }
                }
                if (comment.TaggedRecomenndedList != null)
                {
                    commentLog.TaggedRecomenndedList = new List<User>();


                    foreach (var use in comment.TaggedRecomenndedList)
                    {
                        User user = new User();
                        user = dBContext.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedRecomenndedList.Add(user);
                    }
                }
                if (comment.CCRecomenndedList != null)
                {
                    commentLog.CCRecomenndedList = new List<User>();
                    foreach (var use in comment.CCRecomenndedList)
                    {
                        User user = new User();
                        user = dBContext.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCRecomenndedList.Add(user);
                    }
                }
                commentLog.ReplyCommentId = comment.ReplyCommentId;
                //commentLog.Info = Info.ToString();
                commentLog.Comment = comment.Comment;
                dBContext.CommentLogs.Add(commentLog);
                try
                {
                    dBContext.SaveChanges();

                    foreach (var not in notifications)
                    {
                        if (not.UserId != null)
                        {
                            not.commentLogId = commentLog.Id;
                            not.FlagId = commentLog.FlagId;
                            not.Glow = flagGlow;
                            not.Timestamp = DateTime.Now;
                            not.isRead = false;
                        }
                        else
                        {
                            not.commentLogId = commentLog.Id;
                        }


                    }
                    dBContext.SaveChanges();
                }

                catch (Exception ex)
                {
                    throw;
                }
                SystemLog.LogInfo(this.GetType(), "Added commentLog  Id= " + commentLog.Id);
            }
            SystemLog.LogInfo(this.GetType(), "Added commentLog  Id= " + comment.Id);
        }

        /// <summary>
        /// Add new Comment for transaction
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        public void Add(int Transactionid, TransactionItemType Transactiontype, CommentLog comment, int empID)
        {
            CommentLog commentLog = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                commentLog.employeeId = empID;
                if (comment.AssigneeId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.AssigneeId = comment.AssigneeId;
                }
                if (comment.Category != null || comment.CategoryId != null)
                {
                    commentLog.CategoryId = comment.CategoryId;
                    commentLog.Category = comment.Category;

                }

                commentLog.TransactionId = Transactionid;
                commentLog.TransactionType = Transactiontype;
                commentLog.Timestamp = System.DateTime.Now;
                commentLog.ReadTimestamp = System.DateTime.Now;
                commentLog.Subject = comment.Subject;
                if (comment.TaggedList != null)
                {
                    commentLog.TaggedList = new List<User>();


                    foreach (var use in comment.TaggedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedList.Add(user);
                    }
                }
                if (comment.CCUsersList != null)
                {
                    commentLog.CCUsersList = new List<User>();
                    foreach (var use in comment.CCUsersList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCUsersList.Add(user);
                    }
                }
                if (comment.TaggedRecomenndedList != null)
                {
                    commentLog.TaggedRecomenndedList = new List<User>();


                    foreach (var use in comment.TaggedRecomenndedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedRecomenndedList.Add(user);
                    }
                }
                if (comment.CCRecomenndedList != null)
                {
                    commentLog.CCRecomenndedList = new List<User>();
                    foreach (var use in comment.CCRecomenndedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCRecomenndedList.Add(user);
                    }
                }

                commentLog.ReplyCommentId = comment.ReplyCommentId;

                //commentLog.Info = Info.ToString();
                commentLog.Comment = comment.Comment;
                context.CommentLogs.Add(commentLog);
                try
                {
                    context.SaveChanges();
                }

                catch (Exception ex)
                {
                    throw;
                }
                SystemLog.LogInfo(this.GetType(), "Added commentLog  Id= " + commentLog.Id);
            }
        }
        public void Add(int Transactionid, TransactionItemType Transactiontype, CommentLog comment, int empID, string billSyetemRef, string poSystemRef)
        {
            CommentLog commentLog = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                commentLog.employeeId = empID;
                if (comment.AssigneeId != null)
                {
                    //commentLog.Assignee = comment.Assignee;
                    commentLog.AssigneeId = comment.AssigneeId;
                }
                if (comment.Category != null || comment.CategoryId != null)
                {
                    commentLog.CategoryId = comment.CategoryId;
                    commentLog.Category = comment.Category;

                }

                commentLog.TransactionId = Transactionid;
                commentLog.TransactionType = Transactiontype;
                commentLog.Timestamp = System.DateTime.Now;
                commentLog.ReadTimestamp = System.DateTime.Now;
                commentLog.Subject = comment.Subject;
                if (comment.TaggedList != null)
                {
                    commentLog.TaggedList = new List<User>();


                    foreach (var use in comment.TaggedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedList.Add(user);
                    }
                }
                if (comment.CCUsersList != null)
                {
                    commentLog.CCUsersList = new List<User>();
                    foreach (var use in comment.CCUsersList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCUsersList.Add(user);
                    }
                }
                if (comment.TaggedRecomenndedList != null)
                {
                    commentLog.TaggedRecomenndedList = new List<User>();
                    foreach (var use in comment.TaggedRecomenndedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.TaggedRecomenndedList.Add(user);
                    }
                }
                if (comment.CCRecomenndedList != null)
                {
                    commentLog.CCRecomenndedList = new List<User>();
                    foreach (var use in comment.CCRecomenndedList)
                    {
                        User user = new User();
                        user = context.Users.FirstOrDefault(x => x.id == use.id);
                        if (user != null && user.id != 0)
                            commentLog.CCRecomenndedList.Add(user);
                    }
                }
                commentLog.ReplyCommentId = comment.ReplyCommentId;
                if(billSyetemRef!=null)
                {
                    commentLog.billSystemRef = billSyetemRef;
                }
                if(poSystemRef!=null)
                {
                    commentLog.poSystemRef = poSystemRef;
                }



                //commentLog.Info = Info.ToString();
                commentLog.Comment = comment.Comment;
                context.CommentLogs.Add(commentLog);
                try
                {
                    context.SaveChanges();
                }

                catch (Exception ex)
                {
                    throw;
                }
                SystemLog.LogInfo(this.GetType(), "Added commentLog  Id= " + commentLog.Id);
            }
        }
        /// <summary>
        /// Add new Comment for transaction
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        public void Add(int Transactionid, TransactionItemType Transactiontype, string comment, int empID)
        {
            CommentLog viewInfo = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                viewInfo.employeeId = empID;
                viewInfo.TransactionId = Transactionid;
                viewInfo.TransactionType = Transactiontype;
                viewInfo.Timestamp = System.DateTime.Now;
                viewInfo.ReadTimestamp = System.DateTime.Now;

                //viewInfo.Info = Info.ToString();
                viewInfo.Comment = comment;
                context.CommentLogs.Add(viewInfo);

                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
            }
        }
        /// <summary>
        /// Add new Comment for transaction
        /// </summary>
        /// <param name="viewInfo">viewInfo  Object for Inquiry</param>
        public void AddComment(int Transactionid, TransactionItemType Transactiontype, string comment, List<User> TaggedUsers, int empID)
        {
            CommentLog viewInfo = new CommentLog();
            if (SystemLog.CurrentUserId != 0)
            {
                viewInfo.employeeId = empID;
                viewInfo.TransactionId = Transactionid;
                viewInfo.TransactionType = Transactiontype;
                viewInfo.Timestamp = System.DateTime.Now;
                viewInfo.ReadTimestamp = System.DateTime.Now;
                //foreach(var user in TaggedUsers)
                if (TaggedUsers.Count > 0)
                {
                    viewInfo.TaggedList = new List<User>();
                    viewInfo.TaggedList = TaggedUsers;//.Add(new User() { id = id });

                }
                //viewInfo.Info = Info.ToString();
                viewInfo.Comment = comment;
                context.CommentLogs.Add(viewInfo);

                context.SaveChanges();
                SystemLog.LogInfo(this.GetType(), "Added viewInfo  Id= " + viewInfo.Id);
            }
        }

        public List<CommentCategory> getCommentCategories(/*TransactionItemType transactionItemType*/)
        {
            return context.commentCategories
               .Where(x => x.isActive == true).ToList();
        }

        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslog(int TransactionId, TransactionItemType transactiontype)
        {
            return context.CommentLogs.Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        }
     
        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogAsc(int TransactionId, TransactionItemType transactiontype)
        {
            Console.WriteLine(transactiontype);

            DateTime _creationDate = DateTime.Today;

            if (transactiontype == TransactionItemType.ToDo_Task)
                _creationDate = context.toDoTasks.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.toDoTasks.FirstOrDefault(x => x.Id == TransactionId).creationDate;
            if (transactiontype == TransactionItemType.Loans)
                _creationDate = context.loans.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.loans.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            if (transactiontype == TransactionItemType.Payments)
                _creationDate = context.payments.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.payments.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            if (transactiontype == TransactionItemType.Admin_Bill)
                _creationDate = context.adminBills.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.adminBills.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Bill)
                _creationDate = context.bills.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.bills.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            //else if (transactiontype == TransactionItemType.CostCenter)
            //_creationDate = context.costSheets.FirstOrDefault(x => x.Id == TransactionId) == null? _creationDate : (DateTime)context.costSheets.FirstOrDefault(x => x.Id == TransactionId).Timestamp; 
            else if (transactiontype == TransactionItemType.Inquiry)
            {
                //_creationDate = context.inquiries.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.inquiries.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
                return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();
            }
            else if (transactiontype == TransactionItemType.InterBank_Transfer)
                _creationDate = context.interBankTransfers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.interBankTransfers.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.InterCompanyBank_Transfer)
                _creationDate = context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.JV)
                _creationDate = context.journalVouchers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.journalVouchers.FirstOrDefault(x => x.Id == TransactionId).postingDate;
            else if (transactiontype == TransactionItemType.Leave)
                _creationDate = context.leaveApplications.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.leaveApplications.FirstOrDefault(x => x.Id == TransactionId).ApplyDate;
            else if (transactiontype == TransactionItemType.Memorandum_Sale)
                _creationDate = context.memorandumSales.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.memorandumSales.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Offer)
            {
                //_creationDate = context.offers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.offers.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
                return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();
            }
            else if (transactiontype == TransactionItemType.Purchase_Invoice)
                _creationDate = context.purchaseInvoices.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.purchaseInvoices.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Purchase_Order)
                _creationDate = context.purchaseOrders.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.purchaseOrders.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Sale_Invoice)
                _creationDate = context.saleInvoices.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.saleInvoices.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Sale_Order)
                _creationDate = context.saleOrders.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.saleOrders.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Sale_Receipt)
                _creationDate = context.salesReceipts.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.salesReceipts.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.SummarySheet)
                _creationDate = context.summarySheetFields.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.summarySheetFields.FirstOrDefault(x => x.Id == TransactionId).Timestamp;
            else if (transactiontype == TransactionItemType.Budget)
                _creationDate = context.budgetCostSheets.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.budgetCostSheets.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            if (transactiontype == TransactionItemType.CostCenter)
            {
                return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();
            }
            else
            {
                var t = context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype /*&& x.Timestamp >= _creationDate*/).ToList();
                return t;
            }

        }
        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogBeforeApproval(int TransactionId, TransactionItemType transactiontype, DateTime? date)
        {
            return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype && x.Timestamp <= date).ToList();

        }
        /// <summary>
        /// Get List of comments for currenttransaction by date
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogAfterApproval(int TransactionId, TransactionItemType transactiontype, DateTime? date)
        {
            return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype && x.Timestamp >= date).ToList();

        }
        /// <summary>
        /// Get List of comments for currenttransaction between date
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogBetweenDates(int TransactionId, TransactionItemType transactiontype, DateTime? datefrom, DateTime datetill)
        {
            return context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype && x.Timestamp >= datefrom && x.Timestamp >= datetill).ToList();

        }

        /// <summary>
        /// Get List of comments for currenttransaction
        /// </summary>
        /// 
        public List<CommentLog> getcommentslogDsc(int TransactionId, TransactionItemType transactiontype)
        {
            return context.CommentLogs.OrderByDescending(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();

        }
        /// <summary>
        /// Get all Warranties
        /// </summary>
        /// <returns>List of Warranties Objects</returns>
        public List<ERP_BL.Databases.Warranty> GetWarranties()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.Warranties.ToList();

        }
        /// <summary>
        /// Get all Active Warranties
        /// </summary>
        /// <returns>List of Warranties Objects</returns>
        public List<ERP_BL.Databases.Warranty> GetActiveWarranties()
        {
            SystemLog.LogInfo(this.GetType(), "Retrive list of Industry Types=");
            return context.Warranties.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get  Warranty  by Id
        /// </summary>
        /// <param name="warrantyid"></param>
        /// <returns>List of Warranties Objects</returns>
        public ERP_BL.Databases.Warranty GetWarranty(int warrantyid)
        {
            SystemLog.LogInfo(this.GetType(), "Retrived Industry Type by Id=" + warrantyid);
            return context.Warranties.FirstOrDefault(x => x.Id == warrantyid); ;
        }
        /// <summary>
        /// Get  Warranty by Id
        /// </summary>
        /// <returns>Warranty Object</returns>
        public void updateWarranty(Warranty warranty)
        {
            Warranty industry = new Warranty();
            industry = warranty;

            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Industry Type  Id=" + warranty.Id);

        }
        /// <summary>
        /// Add new company in DB
        /// </summary>
        /// <param name="company">Company Object</param>
        public void addWarranty(Warranty warranty)
        {
            context.Warranties.Add(warranty);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added New Industry Type  Id=" + warranty.Id);

        }
        /// <summary>
        /// Get purchaseOrder based on SaleOrder ID.
        /// </summary>
        /// <param name="SaleOrderId"></param>
        /// <returns></returns>
        public SaleOrder getSaleOrder(int SaleOrderId)
        {
            return context.saleOrders

                        .FirstOrDefault(x => x.Id == SaleOrderId);
        }
        /// <summary>
        /// Get purchaseOrder based on SaleOrder ID.
        /// </summary>
        /// <param name="SaleOrderId"></param>
        /// <returns></returns>
        public List<AllTransactionsView> getAllTransactions(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {
            Inquiry inquiry = new Inquiry();
            Offer offer = new Offer();
            List<Offer> offers = new List<Offer>();
            InterBankTransfer bankTransfer = new InterBankTransfer();
            InterCompanyBankTransfer companyBankTransfer = new InterCompanyBankTransfer();
            JournalVoucher voucher = new JournalVoucher();
            SaleOrder saleOrder = new SaleOrder();
            PurchaseOrder invoicedPurchaseOrder = new PurchaseOrder();

            List<SaleInvoice> saleInvoices = new List<SaleInvoice>();
            List<PurchaseInvoice> purchaseInvoices = new List<PurchaseInvoice>();
            PurchaseInvoice PInvoice = new PurchaseInvoice();
            List<MemorandumSale> memorandumSales = new List<MemorandumSale>();
            List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            List<Bill> bills = new List<Bill>();
            Bill billl = new Bill();
            List<JournalVoucher> journalVouchers1 = new List<JournalVoucher>();
            List<JournalVoucher> vouchers = new List<JournalVoucher>();
            AdminBill adminBill = new AdminBill();
            List<AdminBill> adminBills = new List<AdminBill>();

            AssetRental assetRental = new AssetRental();
            List<AssetRental> assetRentals = new List<AssetRental>();

            LoansAdvance loansAdvance = new LoansAdvance();
            TargetRewards targetReward = new TargetRewards();
            List<LoansAdvance> loansAdvances = new List<LoansAdvance>();

            Payment payment = new Payment();
            List<Payment> payments = new List<Payment>();
            STL stl = new STL();

            List<STL> stls = new List<STL>();

            List<SaleOrder> saleOrders = new List<SaleOrder>();
            List<AdminBill> adminBillsInterBank = new List<AdminBill>();
            List<InterBankTransfer> bankTransfers = new List<InterBankTransfer>();
            List<InterCompanyBankTransfer> companyBankTransfers = new List<InterCompanyBankTransfer>();
           
            
            InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();
            JournalVoucherRepo voucherRepo = new JournalVoucherRepo();

            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            
            EmployeeRepo empRepo = new EmployeeRepo();

            ERP_BL.ToDoTasks.Taskss.Tasks task = new ToDoTasks.Taskss.Tasks();
            List<ERP_BL.ToDoTasks.Taskss.Tasks> tasks = new List<ERP_BL.ToDoTasks.Taskss.Tasks>();
            TaskRepo taskRepo = new TaskRepo();
            //Employee currEmp = new Employee();
            //currEmp = empRepo.GetEmployeeForPayments(empID);

            switch (type)
            {
                case Enums.TransactionItemType.AssetRental:
                    assetRental = context.assetRentals.FirstOrDefault(x=>x.Id == Id);

                    adminBills = assetRental.adminBills;
                    break;
                case Enums.TransactionItemType.InterCompanyBank_Transfer:
                    //bankTransfer = bankTransferRepo.GetInterBankTransfer(Id);

                    companyBankTransfer = context.interCompanyBankTransfers

                       
                .FirstOrDefault(x => x.Id == Id);
        
                    //}
                    if (companyBankTransfer.Id != 0)
                    {
                        companyBankTransfers.Add(companyBankTransfer);
                    }

                    if (companyBankTransfer.paymentGroupId != 0)
                    {
                        companyBankTransfers = context.interCompanyBankTransfers
  

                        .Where(x=>x.paymentGroupId==companyBankTransfer.paymentGroupId).ToList();
                    }
                    else
                    {
                        if (companyBankTransfer.receiptGroupId != 0)
                        {
                            companyBankTransfers = context.interCompanyBankTransfers
             

                                .Where(x=>x.receiptGroupId== companyBankTransfer.receiptGroupId).ToList();

                        }
                    }
                    if (companyBankTransfers.Count > 0)
                    {
                        foreach (var transfer in companyBankTransfers)
                        {
                            if (transfer.paymentGroupId != 0)
                            {
                                payments = context.payments.Where(x => x.transactionGroupId == transfer.paymentGroupId).ToList();
                                foreach (var paym in payments)
                                {

                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                                    {

                                        if (paym.AdminBill_Id != null)
                                        {
                                            Employee empUser = new Employee();
                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                            adminBill = context.adminBills
                                            .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                            if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                                adminBill = null;
                                        }
                                        else
                                        {
                                            adminBill = null;
                                        }
                                        if (adminBill != null)
                                        {
                                            payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                        }
                                        billl = null;
                                        PInvoice = null;
                                        saleOrder = null;
                                        offer = null;
                                        inquiry = null;
                                        payment = null;
                                        invoicedPurchaseOrder = null;

                                    }

                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                                    {
                                        Employee empUser = new Employee();
                                        if (paym.Bill_Id != null)
                                        {

                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                            billl = context.bills
                                            .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                            if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                                billl = null;
                                        }
                                        else
                                        {
                                            billl = null;
                                        }

                                        if (billl != null)
                                        {

                                            if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                            {
                                                saleOrder = context.saleOrders
                                                
                                                    .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                                if (saleOrder != null)
                                                    if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                        saleOrder = null;
                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }

                                            if (saleOrder == null)
                                            {
                                                PurchaseOrder purchaseOrder = new PurchaseOrder();
                                                if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                                {
                                                    purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                                    if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                        purchaseOrder = null;
                                                }
                                                else
                                                {
                                                    purchaseOrder = null;
                                                }

                                                if (purchaseOrder != null)
                                                {
                                                    saleOrder = context.saleOrders
                                                   
                                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                                    purchaseOrders.Add(purchaseOrder);
                                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                               
                                                }
                                            }

                                            journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                            payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                            offer = context.offers
                                                    .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                        }
                                        else
                                        {
                                            saleInvoices = null;
                                            offer = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            purchaseOrders = context.purchaseOrders
                                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                            if (purchaseOrders.Count != 0)
                                            {
                                                foreach (var purchaseOrder in purchaseOrders)
                                                {
                                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                }
                                            }
                                        }
                                        if (purchaseInvoices != null)
                                        {
                                            foreach (var _PI in purchaseInvoices)
                                                if (_PI.Payments.Count > 0)
                                                    payments.AddRange(_PI.Payments);
                                        }
                                        //else
                                        //{
                                        //    purchaseOrders = null;
                                        //}

                                        if (offer != null)
                                        {
                                            inquiry = context.inquiries
                                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                        }
                                        else
                                        {

                                            inquiry = null;
                                        }
                                        if (saleInvoices != null)
                                        {
                                            saleReceipts = context.salesReceipts
                                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                        }
                                        else
                                        {
                                            saleReceipts = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                        }
                                        else if (offer != null)
                                        {
                                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                        }
                                        else
                                        {
                                            memorandumSales = null;
                                        }

                                        PInvoice = null;
                                        //payments = null;
                                        payment = null;
                                        invoicedPurchaseOrder = null;
                                        adminBill = null;
                                    }



                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                                    {

                                        if (paym.PInvoice_Id != null)
                                        {
                                            Employee empUser = new Employee();
                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                            PInvoice = context.purchaseInvoices
                                            .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                            if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                                PInvoice = null;
                                        }
                                        else
                                        {
                                            PInvoice = null;
                                        }
                                        if (PInvoice != null)
                                        {
                                            invoicedPurchaseOrder = context.purchaseOrders
                                      
                                            .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                            payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                        }
                                        else
                                        {
                                            invoicedPurchaseOrder = null;
                                        }
                                        if (invoicedPurchaseOrder != null)
                                        {
                                            saleOrder = context.saleOrders
                                 
                                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                            if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                                purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                            if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                            {
                                                purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                            }

                                            if (purchaseOrders != null)
                                                foreach (var _po in purchaseOrders)
                                                    purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                            purchaseInvoices.Distinct();

                                            if (purchaseInvoices != null)
                                                foreach (var _pi in purchaseInvoices)
                                                    payments.AddRange(_pi.Payments);

                                            payments.Distinct();
                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                        }
                                        else
                                        {
                                            saleInvoices = null;
                                            offer = null;
                                        }
                                        if (offer != null)
                                        {
                                            inquiry = context.inquiries
                                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                        }
                                        else
                                        {

                                            inquiry = null;
                                        }
                                        if (saleInvoices != null)
                                        {
                                            saleReceipts = context.salesReceipts
                                           .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                        }
                                        else
                                        {
                                            saleReceipts = null;
                                        }



                                        adminBill = null;
                                        billl = null;
                                        bills = null;
                                        //purchaseInvoices = null;
                                        payment = null;
                                        journalVouchers1 = null;
                                    }

                                }
                            }
                            else
                            {
                                saleReceipts = context.salesReceipts
                               .Where(x => x.transactionGroupId == transfer.receiptGroupId).ToList();
                                if(transfer.STL!=null)
                                stls.Add(transfer.STL);

                            }
                        }
                    }
         
                    break;
                case Enums.TransactionItemType.InterBank_Transfer:
                    //bankTransfer = bankTransferRepo.GetInterBankTransfer(Id);

                    bankTransfer= context.interBankTransfers

                .FirstOrDefault(x => x.Id == Id);
                    if(bankTransfer.adminBillId!=null)
                    {
                        adminBill = context.adminBills
                       .FirstOrDefault(x => x.Id == bankTransfer.adminBillId);
                    }
                    if(bankTransfer.Id!=0)
                    {
                        bankTransfers.Add(bankTransfer);
                    }

                    if (bankTransfer.paymentGroupId!=0)
                    {
                        bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(bankTransfer.paymentGroupId, empID, (int) bankTransfer.company_Id);

                    }
                    else
                    {
                        if (bankTransfer.receiptGroupId != 0)
                        {
                            bankTransfers = bankTransferRepo.GetReceiptInterBankTransfers(bankTransfer.receiptGroupId, empID, (int)bankTransfer.company_Id);

                        }
                    }
                    if (bankTransfers .Count> 0)
                    {

                        foreach (var transfer in bankTransfers)
                        {
                            if (transfer.paymentGroupId != 0)
                            {
                                payments = context.payments.Where(x => x.transactionGroupId == transfer.paymentGroupId).ToList();
                                foreach (var paym in payments)
                                {

                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                                    {

                                        if (paym.AdminBill_Id != null)
                                        {
                                            Employee empUser = new Employee();
                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                            adminBill = context.adminBills
                                            .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                            if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                                adminBill = null;
                                        }
                                        else
                                        {
                                            adminBill = null;
                                        }
                                        if (adminBill != null)
                                        {
                                            payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                        }
                                        billl = null;
                                        PInvoice = null;
                                        saleOrder = null;
                                        offer = null;
                                        inquiry = null;
                                        payment = null;
                                        invoicedPurchaseOrder = null;

                                    }

                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                                    {
                                        Employee empUser = new Employee();
                                        if (paym.Bill_Id != null)
                                        {

                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                            billl = context.bills
                                            .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                            if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                                billl = null;
                                        }
                                        else
                                        {
                                            billl = null;
                                        }

                                        if (billl != null)
                                        {

                                            if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                            {
                                                saleOrder = context.saleOrders
                                          
                                                    .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                                if (saleOrder != null)
                                                    if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                        saleOrder = null;
                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }

                                            if (saleOrder == null)
                                            {
                                                PurchaseOrder purchaseOrder = new PurchaseOrder();
                                                if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                                {
                                                    purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                                    if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                        purchaseOrder = null;
                                                }
                                                else
                                                {
                                                    purchaseOrder = null;
                                                }

                                                if (purchaseOrder != null)
                                                {
                                                    saleOrder = context.saleOrders
                                                   
                                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                              
                                                    purchaseOrders.Add(purchaseOrder);
                                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                    //}
                                                }
                                            }

                                            journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                            payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                        }
                                        else
                                        {
                                            saleInvoices = null;
                                            offer = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            purchaseOrders = context.purchaseOrders
                                         
                                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                            if (purchaseOrders.Count != 0)
                                            {
                                                foreach (var purchaseOrder in purchaseOrders)
                                                {
                                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                }
                                            }
                                        }
                                        if (purchaseInvoices != null)
                                        {
                                            foreach (var _PI in purchaseInvoices)
                                                if (_PI.Payments.Count > 0)
                                                    payments.AddRange(_PI.Payments);
                                        }

                                        if (offer != null)
                                        {
                                            inquiry = context.inquiries
                                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                        }
                                        else
                                        {

                                            inquiry = null;
                                        }
                                        if (saleInvoices != null)
                                        {
                                            saleReceipts = context.salesReceipts
                                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                        }
                                        else
                                        {
                                            saleReceipts = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                        }
                                        else if (offer != null)
                                        {
                                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                        }
                                        else
                                        {
                                            memorandumSales = null;
                                        }

                                        PInvoice = null;
                                        //payments = null;
                                        payment = null;
                                        invoicedPurchaseOrder = null;
                                        adminBill = null;
                                    }



                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                                    {

                                        if (paym.PInvoice_Id != null)
                                        {
                                            Employee empUser = new Employee();
                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                            PInvoice = context.purchaseInvoices
                                            .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                            if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                                PInvoice = null;
                                        }
                                        else
                                        {
                                            PInvoice = null;
                                        }
                                        if (PInvoice != null)
                                        {
                                            invoicedPurchaseOrder = context.purchaseOrders
                                        
                                            .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                            payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                        }
                                        else
                                        {
                                            invoicedPurchaseOrder = null;
                                        }
                                        if (invoicedPurchaseOrder != null)
                                        {
                                            saleOrder = context.saleOrders
                                   
                                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                            if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                                purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                            if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                            {
                                                purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                            }

                                            if (purchaseOrders != null)
                                                foreach (var _po in purchaseOrders)
                                                    purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                            purchaseInvoices.Distinct();

                                            if (purchaseInvoices != null)
                                                foreach (var _pi in purchaseInvoices)
                                                    payments.AddRange(_pi.Payments);

                                            payments.Distinct();
                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                        }
                                        else
                                        {
                                            saleInvoices = null;
                                            offer = null;
                                        }
                                        if (offer != null)
                                        {
                                            inquiry = context.inquiries
                                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                        }
                                        else
                                        {

                                            inquiry = null;
                                        }
                                        if (saleInvoices != null)
                                        {
                                            saleReceipts = context.salesReceipts
                                    .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                        }
                                        else
                                        {
                                            saleReceipts = null;
                                        }



                                        adminBill = null;
                                        billl = null;
                                        bills = null;
                                        //purchaseInvoices = null;
                                        payment = null;
                                        journalVouchers1 = null;
                                    }

                                }
                            }
                            else
                            {
                                saleReceipts = context.salesReceipts
                    .Where(x => x.transactionGroupId == transfer.receiptGroupId).ToList();


                            }
                        }
                    }
                    if(bankTransfer.vendorBillId!=null)
                    {
                        billl = bankTransfer.Bill;
                    }
                    stl = bankTransfer.STL;
                    break;



                case Enums.TransactionItemType.JV:
                    voucher = voucherRepo.GetJournalVoucherById(Id);
                    if (voucher.paymentGroupId != 0)
                    {
                        vouchers = voucherRepo.GetVouchersByGroupId(voucher.paymentGroupId);

                        foreach (var paym in payments)
                        {

                            if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                            {

                                if (paym.AdminBill_Id != null)
                                {
                                    Employee empUser = new Employee();
                                    empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                    adminBill = context.adminBills
                                    .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                    if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                        adminBill = null;
                                }
                                else
                                {
                                    adminBill = null;
                                }
                                if (adminBill != null)
                                {
                                    payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                }
                                billl = null;
                                PInvoice = null;
                                saleOrder = null;
                                offer = null;
                                inquiry = null;
                                payment = null;
                                invoicedPurchaseOrder = null;

                            }

                            if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                            {
                                Employee empUser = new Employee();
                                if (paym.Bill_Id != null)
                                {

                                    empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                    billl = context.bills
                                    .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                    if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                        billl = null;
                                }
                                else
                                {
                                    billl = null;
                                }

                                if (billl != null)
                                {

                                    if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                    {
                                        saleOrder = context.saleOrders
                                          
                                            .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                        if (saleOrder != null)
                                            if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                saleOrder = null;
                                    }
                                    else
                                    {
                                        saleOrder = null;
                                    }

                                    if (saleOrder == null)
                                    {
                                        PurchaseOrder purchaseOrder = new PurchaseOrder();
                                        if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                        {
                                            purchaseOrder = context.purchaseOrders
                                    .FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                            if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                purchaseOrder = null;
                                        }
                                        else
                                        {
                                            purchaseOrder = null;
                                        }

                                        if (purchaseOrder != null)
                                        {
                                            saleOrder = context.saleOrders
                                     
                                            .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                  
                                            purchaseOrders.Add(purchaseOrder);
                                            if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                            //}
                                        }
                                    }

                                    journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                    payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                }
                                else
                                {
                                    saleOrder = null;
                                }
                                if (saleOrder != null)
                                {
                                    saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                    offer = context.offers
                                            .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                }
                                else
                                {
                                    saleInvoices = null;
                                    offer = null;
                                }
                                if (saleOrder != null)
                                {
                                    purchaseOrders = context.purchaseOrders
                                    .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                    if (purchaseOrders.Count != 0)
                                    {
                                        foreach (var purchaseOrder in purchaseOrders)
                                        {
                                            if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                        }
                                    }
                                }
                                if (purchaseInvoices != null)
                                {
                                    foreach (var _PI in purchaseInvoices)
                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments);
                                }
                                //else
                                //{
                                //    purchaseOrders = null;
                                //}

                                if (offer != null)
                                {
                                    inquiry = context.inquiries
                                   .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                }
                                else
                                {

                                    inquiry = null;
                                }
                                if (saleInvoices != null)
                                {
                                    saleReceipts = context.salesReceipts
                                 .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                }
                                else
                                {
                                    saleReceipts = null;
                                }
                                if (saleOrder != null)
                                {
                                    memorandumSales = context.memorandumSales
                               .Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                }
                                else if (offer != null)
                                {
                                    memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                }
                                else
                                {
                                    memorandumSales = null;
                                }

                                PInvoice = null;
                                //payments = null;
                                payment = null;
                                invoicedPurchaseOrder = null;
                                adminBill = null;
                            }



                            if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                            {

                                if (paym.PInvoice_Id != null)
                                {
                                    Employee empUser = new Employee();
                                    empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                    PInvoice = context.purchaseInvoices
                                    .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                    if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                        PInvoice = null;
                                }
                                else
                                {
                                    PInvoice = null;
                                }
                                if (PInvoice != null)
                                {
                                    invoicedPurchaseOrder = context.purchaseOrders
                                  
                                    .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                    payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                }
                                else
                                {
                                    invoicedPurchaseOrder = null;
                                }
                                if (invoicedPurchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders
                    
                                    .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                    if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                        purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                    if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                    {
                                        purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                    }

                                    if (purchaseOrders != null)
                                        foreach (var _po in purchaseOrders)
                                            purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                    purchaseInvoices.Distinct();

                                    if (purchaseInvoices != null)
                                        foreach (var _pi in purchaseInvoices)
                                            payments.AddRange(_pi.Payments);

                                    payments.Distinct();
                                }
                                else
                                {
                                    saleOrder = null;
                                }
                                if (saleOrder != null)
                                {
                                    //purchaseOrders = context.purchaseOrders
                                    //.Include("SaleOrder")
                                    //.Include("company")
                                    //.Include("AllocateTo")
                                    //.Include("department").Include("purchaseInvoices").Include("PurchaseOrderStatus")
                                    //.Where(x => x.saleOrder_Id == saleOrder.Id).ToList();


                                    saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                    offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                }
                                else
                                {
                                    saleInvoices = null;
                                    offer = null;
                                }
                                if (offer != null)
                                {
                                    inquiry = context.inquiries
                                   .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                }
                                else
                                {

                                    inquiry = null;
                                }
                                if (saleInvoices != null)
                                {
                                    saleReceipts = context.salesReceipts
                                    .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                }
                                else
                                {
                                    saleReceipts = null;
                                }



                                adminBill = null;
                                billl = null;
                                bills = null;
                     
                                payment = null;
                                journalVouchers1 = null;
                            }

                        }
                        
                    }
                    else
                    if (voucher.receiptGroupId != 0)
                    {
                        vouchers = voucherRepo.GetVouchersByReceiptGroupId(voucher.receiptGroupId);
                        saleReceipts = context.salesReceipts
                   .Where(x => x.transactionGroupId == voucher.receiptGroupId).ToList();
                        
                    }
                    payments .AddRange( context.payments
                        .Where(x => x.transactionGroupId == voucher.paymentGroupId).ToList());
                    payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                    break;


                case Enums.TransactionItemType.Inquiry:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    inquiry = context.inquiries
                        .FirstOrDefault(x => x.Id == Id);
                    if (inquiry != null)
                    {
                        offer = context.offers
                               .FirstOrDefault(x => x.inquiry_Id == Id);
                    }
                    else
                    {
                        offer = null;
                    }
                    if (offer != null)
                    {
                        saleOrder = context.saleOrders
                 
                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                    }
                    else
                    {
                        saleInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count != 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count != 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                               
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers
                                                 
                                                               .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                         
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                      .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));

                         
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers
                     
                                   .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }

                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)

                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;

                //Offer
                case Enums.TransactionItemType.Offer:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    offer = context.offers.FirstOrDefault(x => x.Id == Id);

                    if(offer.offertype==InquiryType.DistributionBiz)
                    {
                        offers = context.offers.Where(x => x.uniqueNumber == offer.uniqueNumber).ToList();

                    }




                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                        saleOrder = context.saleOrders
                
                        .FirstOrDefault(x => x.offer_Id == offer.Id);


                        var _tasks = context.tasks.Where(x => x.offerId == offer.Id).ToList();
                        //var _task = taskRepo.GetSOTask(saleOrder.Id);
                        if (_tasks != null)
                            tasks.AddRange(_tasks);
                    }
                    else
                    {
                        saleOrder = null;
                        inquiry = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                        //var _task = taskRepo.GetSOTask(saleOrder.Id);
                        if (_tasks != null)
                            tasks.AddRange(_tasks);
                    }
                    else
                    {
                        saleInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count!=0)
                                purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                            }

                        }

                        if (purchaseInvoices.Count != 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count != 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers
                                                   
                              .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }

                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);

                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            companyBankTransfers.AddRange(context.interCompanyBankTransfers
                                                    

                              .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)

                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;


                case Enums.TransactionItemType.Tasks:

                    task = context.tasks.FirstOrDefault(x=>x.Id==Id);
                    if(task.saleOrderId != null)
                    {
                        tasks = context.tasks.Where(x => x.saleOrderId == task.saleOrderId).ToList();
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;

                        saleOrder = context.saleOrders
        
                        .FirstOrDefault(x => x.Id == task.saleOrderId);


                        //var parentOrder = context.saleOrders.FirstOrDefault(x => x.Id == x.ParentSO_Id);
                        //if(parentOrder!=null)
                        //saleOrders.Add(parentOrder);
                        //saleOrders.Distinct();

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
    
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                    //var _task = taskRepo.GetPOTask(purchaseOrder.Id);
                                    if (_tasks != null)
                                        tasks.AddRange(_tasks);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }

                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }

                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }

                            foreach (var SI in saleInvoices)
                            {
                                var _tasks = context.tasks.Where(x=>x.saleInvoiceId==SI.Id).ToList();
                                //var _task = taskRepo.GetSITask(SI.Id);
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        if (bills != null && bills.Count > 0)
                        {
                            if (payments == null)
                                payments = new List<Payment>();
                            foreach (var __bill in bills)
                            {
                                if (__bill.Payments != null && __bill.Payments.Count > 0)
                                {
                                    payments.AddRange(__bill.Payments);

                                }
                            }
                        }
                        if (payments != null && payments.Count > 0)
                            payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    }
                    else if(task.saleInvoiceId != null)
                    {
                        tasks = context.tasks.Where(x => x.saleInvoiceId == task.saleInvoiceId).ToList();
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var invoicee = context.saleInvoices.FirstOrDefault(x => x.Id == task.saleInvoiceId);
                        if (invoicee != null)
                        {
                            saleOrder = context.saleOrders
                           
                            .FirstOrDefault(x => x.Id == invoicee.SaleOrderId);
                        }
                        else
                        {
                            saleOrder = null;
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                    //var _task = taskRepo.GetPOTask(purchaseOrder.Id);
                                    if (_tasks != null)
                                        tasks.AddRange(_tasks);
                                       
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }

                        }
                        else
                        {
                            purchaseOrders = null;
                        }


                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                            .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                            foreach (var SI in saleInvoices)
                            {
                                var _tasks = context.tasks.Where(x => x.saleInvoiceId == SI.Id).ToList();
                                //var _task = taskRepo.GetSITask(SI.Id);
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        if(saleOrder != null)
                        {
                            var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                            //var _task = taskRepo.GetSOTask(saleOrder.Id);
                            if (_tasks != null)
                                tasks.AddRange(_tasks);
                        }
                        
                    }
                    else if(task.purchaseOrderId != null)
                    {
                        tasks = context.tasks.Where(x => x.purchaseOrderId == task.purchaseOrderId).ToList();
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var PurchaseOrderr = context.purchaseOrders
                   
                            .FirstOrDefault(x => x.Id == task.purchaseOrderId);
                        if (PurchaseOrderr != null)
                        {
                            // purchaseInvoices = context.purchaseInvoices.Include("PurchaseInvoiceStatus")
                            //.Include("customerCompany.company").Include("PurchaseOrder")
                            //.Include("company").Include("employee")
                            //.Include("department").Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                            saleOrder = context.saleOrders
                      
                            .FirstOrDefault(x => x.Id == PurchaseOrderr.saleOrder_Id);
                        }
                        else
                        {
                            saleOrder = null;
                            purchaseInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();

                            if (bills.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _bill in bills)
                                {
                                    if (_bill.Payments.Count > 0)
                                    {
                                        payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());

                                    }
                                    //payments.AddRange(_bill.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else if (PurchaseOrderr != null)
                        {
                            bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrderr.Id)).ToList();

                            if (bills.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _bill in bills)
                                {
                                    if (_bill.Payments.Count > 0)
                                        payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                    //payments.AddRange(_bill.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (payments == null)
                                        payments = new List<Payment>();

                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }

                            }
                        }
                        else if (PurchaseOrderr != null)
                        {
                            purchaseOrders.Add(PurchaseOrderr);
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                    //var _task = taskRepo.GetPOTask(purchaseOrder.Id);
                                    if (_tasks != null)
                                        tasks.AddRange(_tasks);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                if (payments == null)
                                    payments = new List<Payment>();

                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                            purchaseInvoices = null;
                        }


                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                           .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                            foreach (var SI in saleInvoices)
                            {
                                var _tasks = context.tasks.Where(x => x.saleInvoiceId == SI.Id).ToList();
                                //var _task = taskRepo.GetSITask(SI.Id);
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        if(PurchaseOrderr != null)
                            journalVouchers1 = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrderr.Id).ToList();

                        if (saleOrder != null)
                        {
                            var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                            //var _task = taskRepo.GetSOTask(saleOrder.Id);
                            if (_tasks != null)
                                tasks.AddRange(_tasks);
                        }
                    }
                    else if(task.offerId != null)
                    {
                        tasks = context.tasks.Where(x => x.offerId == task.offerId).ToList();
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        offer = context.offers.FirstOrDefault(x => x.Id == task.offerId);

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                            saleOrder = context.saleOrders
                      
                            .FirstOrDefault(x => x.offer_Id == offer.Id);
                        }
                        else
                        {
                            saleOrder = null;
                            inquiry = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        }
                        else
                        {
                            saleInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                    var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                    //var _task = taskRepo.GetPOTask(purchaseOrder.Id);
                                    if (_tasks != null)
                                        tasks.AddRange(_tasks);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count != 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count != 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                           .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                            foreach (var SI in saleInvoices)
                            {
                                var _tasks = context.tasks.Where(x => x.saleInvoiceId == SI.Id).ToList();
                                //var _task = taskRepo.GetSITask(SI.Id);
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)

                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        if (saleOrder != null)
                        {
                            var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                            //var _task = taskRepo.GetSOTask(saleOrder.Id);
                            if (_tasks != null)
                                tasks.AddRange(_tasks);
                        }
                    }

                    if (tasks != null && tasks.Count > 0)
                    {
                        tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        if (task != null && task.Id != 0 && tasks.FirstOrDefault(x=>x.Id == task.Id) != null)
                        {
                            var itemToRemove = tasks.Single(r => r.Id == task.Id);
                            tasks.Remove(itemToRemove);
                        }     
                    }
                        
                    break;

                //SaleOrder
                case Enums.TransactionItemType.Sale_Order:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    saleOrder = context.saleOrders

                        .FirstOrDefault(x => x.Id == Id);
                  
                
                    //var parentOrder = context.saleOrders.FirstOrDefault(x => x.Id == x.ParentSO_Id);
                    //if(parentOrder!=null)
                    //saleOrders.Add(parentOrder);
                    //saleOrders.Distinct();

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        //bankTransfer = context.interBankTransfers.Include("interBankTransStatus").Include("company")
                       
                        //.Include("department").Include("employee").FirstOrDefault(x => x.Id == saleOrder.interBankTransfer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                        //var _task = taskRepo.GetSOTask(saleOrder.Id);
                        if (_tasks != null)
                            tasks.AddRange(_tasks);

                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                var _taskk = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                //var _taskk = taskRepo.GetPOTask(purchaseOrder.Id);
                                if (_taskk != null)
                                    tasks.AddRange(_taskk);
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                companyBankTransfers .AddRange( context.interCompanyBankTransfers
         

                                .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                            
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                 
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            companyBankTransfers .AddRange( context.interCompanyBankTransfers
                                                 

                              .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                        }

                        foreach (var _SI in saleInvoices)
                        {
                            var _tasks = context.tasks.Where(x => x.saleInvoiceId == _SI.Id).ToList();
                            //var _taskk = taskRepo.GetSITask(_SI.Id);
                            if (_tasks != null)
                                tasks.AddRange(_tasks);
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    if(bills != null && bills.Count>0)
                    {
                        if (payments == null)
                            payments = new List<Payment>();
                        foreach (var __bill in bills)
                        {
                            if (__bill.Payments != null && __bill.Payments.Count > 0)
                            {
                                payments.AddRange(__bill.Payments);

                                //foreach (Payment _pymnnt in __bill.Payments)
                                //{
                                //    if(_pymnnt!=null)
                                //    payments.Add(payment);
                                //}
                            }
                        }
                    }
                    if( payments!=null && payments.Count > 0)
                        payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                    if (tasks != null && tasks.Count > 0)
                    {
                        tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    }
                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Invoice:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var invoice = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
                    if (invoice != null)
                    {
                        saleOrder = context.saleOrders
                      
                        .FirstOrDefault(x => x.Id == invoice.SaleOrderId);
                    }
                    else
                    {
                        saleOrder = null;
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                        var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                        //var _taskk = taskRepo.GetSOTask(saleOrder.Id);
                        if (_tasks != null)
                            tasks.AddRange(_tasks);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if(invoice.LoansAdvances.Count>0)
                    {
                        loansAdvances.AddRange(invoice.LoansAdvances);
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                //var _taskk = taskRepo.GetPOTask(purchaseOrder.Id);
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                            }
                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers
                              
                  

                                        .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                            }
                        }

                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                   

                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            companyBankTransfers.AddRange(context.interCompanyBankTransfers
                                          
                       .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                        }

                        foreach (var _SI in saleInvoices)
                        {
                            var _tasks = context.tasks.Where(x => x.saleInvoiceId == _SI.Id).ToList();
                            //var _taskk = taskRepo.GetSITask(_SI.Id);
                            if (_tasks != null)
                                tasks.AddRange(_tasks);
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    if (tasks != null && tasks.Count > 0)
                    {
                        tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    }
                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Receipt:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var receipt = context.salesReceipts.FirstOrDefault(x => x.Id == Id);
                  
                        
                    if(receipt.receiptType == ReceiptType.Loans_Advances)
                    {
                        loansAdvances.Add(receipt.loansAdvance);
                        PaymentRepo paymentRepo = new PaymentRepo();
                        payments = new List<Payment>();
                        payments.AddRange(paymentRepo.getPaymentsByLAid((int)receipt.LoansAdvanceId, empID));
                        foreach(var pymnt in payments.ToList())
                        {
                            if(pymnt.loansAdvance != null)
                            {
                                loansAdvances.Add(pymnt.loansAdvance);
                                if(pymnt.loansAdvance.SalesReceipts != null && pymnt.loansAdvance.SalesReceipts.Count > 0)
                                    saleReceipts.AddRange(pymnt.loansAdvance.SalesReceipts);
                                if (pymnt.loansAdvance.Payments != null && pymnt.loansAdvance.Payments.Count > 0)
                                    payments.AddRange(pymnt.loansAdvance.Payments);
                            }
                        }
                        if (saleReceipts.Find(x => x.Id == receipt.Id) != null)
                            saleReceipts.Remove(receipt);
                        saleReceipts = saleReceipts.Distinct().ToList();
                        foreach(var _receipt in saleReceipts)
                        {
                            if (_receipt.loansAdvance != null)
                                loansAdvances.Add(_receipt.loansAdvance);
                        }
                        payments = payments.Distinct().ToList();
                        loansAdvances =  loansAdvances.Distinct().ToList();

                    }
                    else if(receipt.receiptType != ReceiptType.Direct_Receipt)
                    {
                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(receipt.transactionGroupId, empID, (int)receipt.companyId));
                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(receipt.transactionGroupId));
                        companyBankTransfers.AddRange(context.interCompanyBankTransfers
               
                            .Where(x=>x.receiptGroupId==receipt.transactionGroupId).ToList());

                        if (receipt != null)
                        {
                            saleOrder = context.saleOrders
                   
                            .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                        }
                        else
                        {
                            saleOrder = null;
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                    }
                    
                    break;

                case Enums.TransactionItemType.Admin_Bill:

                    adminBill = context.adminBills
                        .FirstOrDefault(x => x.Id == Id);

                    if(adminBill.InterBankTransfers.Count>0)
                    {
                        bankTransfers = adminBill.InterBankTransfers;
                    }


                    AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
                    if (adminBill.Payments != null)
                    {
                        var paymentss = adminBill.Payments;

                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach(var _paymnt in paymentss)
                        {
                            payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                        }
                        foreach (var paym in payments)
                        {

                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            if (paym.LoansAdvanceId != null)
                                loansAdvances.Add(loansAdvanceRepo.GetLoansAdvance(paym.LoansAdvanceId.Value));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        payments = null;
                    }
                    
                    if (adminBill.LoansAdvanceId != null)
                    {
                        loansAdvances.Add(loansAdvanceRepo.GetLoansAdvance(adminBill.LoansAdvanceId.Value));
                    }
                    if(loansAdvances != null && loansAdvances.Count > 0)
                    {
                        foreach (var _LA in loansAdvances)
                        {
                            if(_LA.AdminBills != null && _LA.AdminBills.Count > 0)
                                adminBills.AddRange(_LA.AdminBills);
                        }
                    }
                    if(adminBills != null && adminBills.Count > 0)
                    {
                        foreach (var _adminBill in adminBills)
                        {
                            if (_adminBill.Payments != null && _adminBill.Payments.Count > 0)
                                payments.AddRange(_adminBill.Payments);

                            if (_adminBill.assetRentalId != null)
                            {
                                if (assetRentals == null)
                                    assetRentals = new List<AssetRental>();
                                assetRentals.Add(context.assetRentals.FirstOrDefault(x => x.Id == _adminBill.assetRentalId));
                            }
                        }
                    }
                    if (adminBills.FirstOrDefault(x => x.Id == adminBill.Id) != null)
                        adminBills.Remove(adminBills.FirstOrDefault(x=>x.Id == adminBill.Id));

                    if (adminBill.assetRentalId != null)
                    {
                        if (assetRentals == null)
                            assetRentals = new List<AssetRental>();
                        assetRentals.Add(context.assetRentals.FirstOrDefault(x => x.Id == adminBill.assetRentalId));
                    }

                    adminBills = adminBills.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    loansAdvances = loansAdvances.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    payment = null;
                    billl = null;
                    inquiry = null;
                    offer = null;
                    saleOrder = null;
                    saleInvoices = null;
                    purchaseOrders = null;
                    bills = null;
                    purchaseInvoices = null;
                    journalVouchers1 = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    //if (adminBill != null)
                    //{
                    //    saleOrder = context.saleOrders
                    //    .Include("offer")
                    //    .Include("offer.inquiry")
                    //    .Include("company")
                    //    .Include("employee")
                    //    .Include("department").Include("SaleInvoices").Include("SaleOrderStatus")
                    //    .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                    //}
                    break;

                case Enums.TransactionItemType.LoansAdvances:

                    loansAdvance = context.loansAdvances
                        .Include("company")
                        .Include("department")
                        .FirstOrDefault(x => x.Id == Id);
                    if (loansAdvance.Payments != null)
                    {
                        var paymentss = loansAdvance.Payments;

                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _paymnt in paymentss)
                        {
                            payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                        }
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        payments = null;
                    }
                    if (loansAdvance.SaleInvoice != null)
                    {
                        saleInvoices.Add(loansAdvance.SaleInvoice);
                    }

                    if (loansAdvance.SalesReceipts != null)
                    {
                        var saleReceiptss = loansAdvance.SalesReceipts;

                        SalesReceiptRepo saleReceiptRepo = new SalesReceiptRepo();
                        foreach (var _receipt in saleReceiptss)
                        {
                            saleReceipts.Add(saleReceiptRepo.GetSalesReceipt(_receipt.Id));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (loansAdvance.AdminBills != null)
                    {
                        adminBills = loansAdvance.AdminBills;
                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _adminBill in adminBills)
                        {
                            if(_adminBill.Payments != null && _adminBill.Payments.Count > 0)
                                payments.AddRange(_adminBill.Payments);
                        }
                    }
                    payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                    if (loansAdvance.bills != null)
                    {
                        var billss = loansAdvance.bills;

                        BillRepo billRepo = new BillRepo();
                        foreach (var _bill in billss)
                        {
                            bills.Add(billRepo.get(_bill.Id));
                        }
                    }
                    else
                    {
                        bills = null;
                    }

                    //adminBill = null;
                    //payment = null;
                    //billl = null;
                    //inquiry = null;
                    //offer = null;
                    //saleOrder = null;
                    //saleInvoices = null;
                    //purchaseOrders = null;
                    ////bills = null;
                    //purchaseInvoices = null;
                    //journalVouchers1 = null;
                    //PInvoice = null;
                    //invoicedPurchaseOrder = null;
                    //if (adminBill != null)
                    //{
                    //    saleOrder = context.saleOrders
                    //    .Include("offer")
                    //    .Include("offer.inquiry")
                    //    .Include("company")
                    //    .Include("employee")
                    //    .Include("department").Include("SaleInvoices").Include("SaleOrderStatus")
                    //    .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                    //}
                    break;

                case Enums.TransactionItemType.TargetReward:

                    targetReward = context.targetRewards
                        .FirstOrDefault(x => x.Id == Id);
                    if (targetReward.Payments != null)
                    {
                        var paymentss = targetReward.Payments;

                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _paymnt in paymentss)
                        {
                            payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                        }
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        payments = null;
                    }
                    
                   
                    payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                    saleReceipts = null;
                    adminBill = null;
                    payment = null;
                    billl = null;
                    inquiry = null;
                    offer = null;
                    saleOrder = null;
                    saleInvoices = null;
                    purchaseOrders = null;
                    bills = null;
                    purchaseInvoices = null;
                    journalVouchers1 = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    //if (adminBill != null)
                    //{
                    //    saleOrder = context.saleOrders
                    //    .Include("offer")
                    //    .Include("offer.inquiry")
                    //    .Include("company")
                    //    .Include("employee")
                    //    .Include("department").Include("SaleInvoices").Include("SaleOrderStatus")
                    //    .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                    //}
                    break;

                case Enums.TransactionItemType.Payments:

                    payment = context.payments.FirstOrDefault(x => x.Id == Id);
                    

                    if(payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Admin_Bills)
                    {

                        if (payment.AdminBill_Id != null)
                        {
                            adminBill = context.adminBills.FirstOrDefault(x => x.Id == payment.AdminBill_Id);
                        }
                        else
                        {
                            adminBill = null;
                        }
                        billl = null;
                        PInvoice = null;
                        saleOrder = null;
                        offer = null;
                        inquiry = null;
                        invoicedPurchaseOrder = null;

                    }

                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Vendor_Bills)
                    {

                        if (payment.Bill_Id != null)
                        {
                            billl = context.bills.FirstOrDefault(x => x.Id == payment.Bill_Id);
                        }
                        else
                        {
                            billl = null;
                        }

                        if (billl != null)
                        {
                            saleOrder = context.saleOrders
                          
                 
                            .FirstOrDefault(x => x.Id == billl.saleOrder_Id);
                            if (saleOrder == null)
                            {
                                var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);
                                if (purchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders
                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                    purchaseOrders.Add(purchaseOrder);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                                //else
                                //{
                                //    bills.Add(billl);
                                //}
                            }
                            if (saleOrder != null)
                            {
                                //bills = context.bills.Include("BillStatus")
                                //.Include("customerCompany.company").Include("SaleOrder")
                                //.Include("company").Include("AllocateTo")
                                //.Include("department").Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                            payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                        }
                        //else
                        //{
                        //    purchaseOrders = null;
                        //}

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                         .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        PInvoice = null;
                        payments = null;
                        invoicedPurchaseOrder = null;
                        adminBill = null;
                    }
                    


                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                    {
 
                        if (payment.PInvoice_Id != null)
                        {
                            PInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == payment.PInvoice_Id);
                        }
                        else
                        {
                            PInvoice = null;
                        }
                        if (PInvoice != null)
                        {
                            invoicedPurchaseOrder = context.purchaseOrders
                      
                            .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);
                        }
                        else
                        {
                            invoicedPurchaseOrder = null;
                        }
                        if (invoicedPurchaseOrder != null)
                        {
                            saleOrder = context.saleOrders
                   
                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            //purchaseOrders = context.purchaseOrders
                            //.Include("SaleOrder")
                            //.Include("company")
                            //.Include("AllocateTo")
                            //.Include("department").Include("purchaseInvoices").Include("PurchaseOrderStatus")
                            //.Where(x => x.saleOrder_Id == saleOrder.Id).ToList();


                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        
                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }



                        adminBill = null;
                        billl = null;
                        bills = null;
                        purchaseInvoices = null;
                        journalVouchers1 = null;
                    }
                    break;


                case Enums.TransactionItemType.STL:

                    stl = context.STLs.FirstOrDefault(x => x.Id == Id);
                    bankTransfers.AddRange(stl.InterBankTransfers);
                    companyBankTransfers.AddRange(stl.InterCompanyBankTransfers);
                    payment = context.payments.FirstOrDefault(x => x.transactionGroupId == stl.paymentGroupId);
                    break;



                case Enums.TransactionItemType.Purchase_Order:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var PurchaseOrder = context.purchaseOrders
                        .FirstOrDefault(x => x.Id == Id);
                    if (PurchaseOrder != null)
                    {
                       // purchaseInvoices = context.purchaseInvoices.Include("PurchaseInvoiceStatus")
                       //.Include("customerCompany.company").Include("PurchaseOrder")
                       //.Include("company").Include("employee")
                       //.Include("department").Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                        saleOrder = context.saleOrders
       
                        .FirstOrDefault(x => x.Id == PurchaseOrder.saleOrder_Id);
                    }
                    else
                    {
                        saleOrder = null;
                        purchaseInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        var _tasks = context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList();
                        //var _taskk = taskRepo.GetSOTask(saleOrder.Id);
                        if (_tasks != null)
                            tasks.AddRange(_tasks);
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();

                        if (bills.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _bill in bills)
                            {
                                if (_bill.Payments.Count > 0)
                                {
                                    payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                   
                                }
                                //payments.AddRange(_bill.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else if (PurchaseOrder != null)
                    {
                        bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrder.Id)).ToList();

                        if (bills.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _bill in bills)
                            {
                                if (_bill.Payments.Count > 0)
                                    payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                //payments.AddRange(_bill.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                var _tasks = context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList();
                                //var _taskk = taskRepo.GetPOTask(purchaseOrder.Id);
                                if (_tasks != null)
                                    tasks.AddRange(_tasks);
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }

                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (payments == null)
                                    payments = new List<Payment>();

                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }

                        }
                    }
                    else if (PurchaseOrder != null)
                    {
                        purchaseOrders.Add(PurchaseOrder);
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                        if(purchaseInvoices.Count > 0)
                        {
                            if(payments == null)
                                payments = new List<Payment>();

                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                        purchaseInvoices = null;
                    }


                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                      .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                        foreach (var _SI in saleInvoices)
                        {
                            var _tasks = context.tasks.Where(x => x.saleInvoiceId == _SI.Id).ToList();
                            //var _taskk = taskRepo.GetSITask(_SI.Id);
                            if (_tasks != null)
                                tasks.AddRange(_tasks);
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales
                        .Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    journalVouchers1 = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                    if (tasks != null && tasks.Count > 0)
                    {
                        tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    }
                    break;
                case Enums.TransactionItemType.Bill:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    
                    var bill = context.bills
                    .FirstOrDefault(x => x.Id == Id);
                    if (bill != null)
                    {
                        saleOrder = context.saleOrders
                
                        .FirstOrDefault(x => x.Id == bill.saleOrder_Id);
                        if (saleOrder == null)
                        {
                            var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == bill.purchaseOrder_Id);
                            if (purchaseOrder != null)
                            {
                                saleOrder = context.saleOrders
           
                                .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList();
                                purchaseOrders.Add(purchaseOrder);
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                            else
                            {
                                bills.Add(bill);
                            }
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        if (bill.interBankTransfers!=null)
                            bankTransfers.AddRange(bill.interBankTransfers);
                        journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == bill.Id).ToList();

                        payments = context.payments.Where(x => x.Bill_Id == bill.Id).ToList();
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            companyBankTransfers.AddRange(context.interCompanyBankTransfers

                       .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());

                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }

                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices
                        .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                    }

                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                   .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            companyBankTransfers.AddRange(context.interCompanyBankTransfers
                              

                       .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    if (bill.loansAdvance != null)
                        loansAdvance = bill.loansAdvance;
                    break;
                //Memorandum Sale
                case Enums.TransactionItemType.Memorandum_Sale:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var memorandumSale = context.memorandumSales.Where(x => x.Id == Id).FirstOrDefault();
                    if (memorandumSale != null)
                    {
                        saleOrder = context.saleOrders
                   
                        .FirstOrDefault(x => x.Id == memorandumSale.SaleOrder_Id);
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = context.offers.FirstOrDefault(x => x.Id == memorandumSale.Offer_Id);
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
        
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                 .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else if(memorandumSale!=null)
                    {
                        memorandumSales.Add(memorandumSale);
                    }
                    break;


                case Enums.TransactionItemType.Purchase_Invoice:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    var purchaseInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == Id);
                    if (purchaseInvoice != null)
                    {
                        invoicedPurchaseOrder = context.purchaseOrders
              
                        .FirstOrDefault(x => x.Id == purchaseInvoice.purchaseOrder_Id);

                        payments = context.payments
                  
                        .Where(x=>x.PInvoice_Id == purchaseInvoice.Id).ToList();
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        invoicedPurchaseOrder = null;
                    }
                    if (invoicedPurchaseOrder != null)
                    {
                        saleOrder = context.saleOrders
      
                        .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    if (saleOrder != null)
                    {
                        //purchaseOrders = context.purchaseOrders
                        //.Include("SaleOrder")
                        //.Include("company")
                        //.Include("AllocateTo")
                        //.Include("department").Include("purchaseInvoices").Include("PurchaseOrderStatus")
                        //.Where(x => x.saleOrder_Id == saleOrder.Id).ToList();

                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                   .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    purchaseInvoices = context.purchaseInvoices.Where(x => x.isVoid != true && x.purchaseOrder_Id == purchaseInvoice.purchaseOrder_Id).ToList();

                    if (purchaseInvoices != null)
                        foreach (var _pi in purchaseInvoices)
                            payments.AddRange(_pi.Payments.Except(payments));

                    payments.Distinct();
              

                    break;
           


            }
            bankTransfers = bankTransfers.GroupBy(x => x.Id)
                                   .Select(g => g.First())
                                   .ToList();
            companyBankTransfers = companyBankTransfers.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
            stls = stls.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
            if (bankTransfers.Count > 0)
            {
                foreach (var transfer in bankTransfers)
                {
                    if (transfer.vendorBillId != null)
                    {
                        billl = transfer.Bill;
                    }
                }
            }
            if (assetRental != null && assetRental.Id != 0 && allowedPermissions.Find(x => x.Name == "View List of Assets") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = assetRental.Id.ToString() + "_" + TransactionItemType.AssetRental.ToString(),
                    TransactionType = TransactionItemType.AssetRental.ToString(),
                    Company = assetRental.company.CompanyName,
                    Employee = assetRental.assetHolderEmployee?.person.FName + " " + assetRental.assetHolderEmployee?.person.FName,
                    CreationDate = assetRental.CreationDate,
                    //Currency = assetRental..CurrencyName,
                    Department = (assetRental.department.parentDepartment == null) ? assetRental.department.DeptName : assetRental.department.parentDepartment.DeptName + " " + assetRental.department.DeptName,
                    //Customer = inquiry.customerCompany.company.CompanyName,
                    //SalesReferenceNo = inquiry.SalesReferenceNo,
                    BackColor = assetRental.Status.backcolor,
                    Status = assetRental.Status.Status,
                    totalCFRValue = assetRental.PurchasingCost

                });
            }
            if (assetRentals.Count > 0 && allowedPermissions.Find(x => x.Name == "View List of Assets") != null)
            {

                foreach (var asset in assetRentals)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = asset.Id.ToString() + "_" + TransactionItemType.AssetRental.ToString(),
                        TransactionType = TransactionItemType.AssetRental.ToString(),
                        Company = asset.company.CompanyName,
                        Employee = assetRental.assetHolderEmployee?.person.FName + " " + assetRental.assetHolderEmployee?.person.FName,
                        CreationDate = asset.CreationDate,
                        //Currency = asset.cur.CurrencyName,
                        Department = (asset.department.parentDepartment == null) ? asset.department.DeptName : asset.department.parentDepartment.DeptName + " " + asset.department.DeptName,
                        //Customer = inquiry.customerCompany.company.CompanyName,
                        //SalesReferenceNo = inquiry.SalesReferenceNo,
                        BackColor = asset.Status.backcolor,
                        Status = asset.Status.Status,
                        totalCFRValue = asset.PurchasingCost

                    });
                }
            }
            if (stl != null && stl.Id != 0 && allowedPermissions.Find(x => x.Name == "List of STL") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = stl.Id.ToString() + "_" + TransactionItemType.STL.ToString(),
                    TransactionType = TransactionItemType.STL.ToString(),
                    Company = stl.company.CompanyName,
                    Employee = stl.user.employee.person.FName + " " + stl.user.employee.person.FName,
                    CreationDate = stl.CreationDate,
                    Currency = stl.stlCurrency.CurrencyName,
                    Department = (stl.department.parentDepartment == null) ? stl.department.DeptName : stl.department.parentDepartment.DeptName + " " + stl.department.DeptName,
                    //Customer = inquiry.customerCompany.company.CompanyName,
                    //SalesReferenceNo = inquiry.SalesReferenceNo,
                    BackColor = stl.stlStatus.backcolor,
                    Status = stl.stlStatus.Status,
                    totalCFRValue = stl.paymentAmountOC

                });
            }
            if (stls.Count> 0 && allowedPermissions.Find(x => x.Name == "List of STL") != null)
            {

                foreach (var STL in stls)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = STL.Id.ToString() + "_" + TransactionItemType.STL.ToString(),
                        TransactionType = TransactionItemType.STL.ToString(),
                        Company = STL.company.CompanyName,
                        //Employee = STL.user.employee.person.FName + " " + stl.user.employee.person.FName,
                        CreationDate = STL.CreationDate,
                        Currency = STL.stlCurrency.CurrencyName,
                        Department = (STL.department.parentDepartment == null) ? STL.department.DeptName : STL.department.parentDepartment.DeptName + " " + STL.department.DeptName,
                        //Customer = inquiry.customerCompany.company.CompanyName,
                        //SalesReferenceNo = inquiry.SalesReferenceNo,
                        BackColor = STL.stlStatus.backcolor,
                        Status = STL.stlStatus.Status,
                        totalCFRValue = STL.paymentAmountOC

                    });
                }
            }
            // Adding all transactions in same structure
            if (adminBill != null && adminBill.Id!=0 && allowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = adminBill.Id.ToString() + "_" + TransactionItemType.Admin_Bill.ToString()+"_"+adminBill.transactionGroupId,
                    TransactionType = TransactionItemType.Admin_Bill.ToString(),
                    Company = adminBill.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                    CreationDate = adminBill.CreationDate,
                    Currency = adminBill.currency.CurrencyName,
                    Department = (adminBill.department.parentDepartment == null) ? adminBill.department.DeptName : adminBill.department.parentDepartment.DeptName + " " + adminBill.department.DeptName,
                    SyetmReferenceNo = adminBill.SystemRefNo,
                    BackColor = adminBill.BillStatus.backcolor,
                    Status = adminBill.BillStatus.Status,
                    amountSOC = adminBill.AmountOC,
                    amountME = adminBill.AmountMER,
                    loanAdjustment = adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount)
                    //totalCFRValue = 0

                });
            }

            if(adminBills != null && adminBills.Count > 0 && allowedPermissions.Find(x=>x.Name== "List of Admin Bills") != null)
            {
                foreach(var _adminBill in adminBills)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = _adminBill.Id.ToString() + "_" + TransactionItemType.Admin_Bill.ToString() + "_" + _adminBill.transactionGroupId,
                        TransactionType = TransactionItemType.Admin_Bill.ToString(),
                        Company = _adminBill.company.CompanyName,
                        //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                        CreationDate = _adminBill.CreationDate,
                        Currency = _adminBill.currency == null ? null : _adminBill.currency.CurrencyName,
                        Department = (_adminBill.department == null) ? null : _adminBill.department.DeptName,
                        SyetmReferenceNo = _adminBill.SystemRefNo,
                        BackColor = _adminBill.BillStatus == null ? null :_adminBill.BillStatus.backcolor,
                        Status = _adminBill.BillStatus == null? null:_adminBill.BillStatus.Status,
                        amountSOC = _adminBill.AmountOC,
                        amountME = _adminBill.AmountMER,
                        loanAdjustment = _adminBill.Adjustments.Where(x => x.isApproved == true).Sum(x => x.AdjustmentAmount)
                        //totalCFRValue = 0

                    });
                }
            }

            if (task != null && task.Id != 0 && allowedPermissions.Find(x => x.Name == "List of User Tasks") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = task.Id.ToString() + "_" + TransactionItemType.Tasks.ToString(),
                    TransactionType = TransactionItemType.Tasks.ToString(),
                    Company = task.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                    CreationDate = task.creationDate,
                    Currency = task.currency == null ? null : task.currency.CurrencyName,
                    Department = (task.department.parentDepartment == null) ? task.department.DeptName : task.department.parentDepartment.DeptName + " " + task.department.DeptName,
                    SyetmReferenceNo = task.SystemRef,
                    BackColor = task.Status.backcolor,
                    Status = task.Status.Status,
                    totalCFRValue = task.ManualAmount,
                    //amountME = task.MER,
                    //totalCFRValue = 0

                });
            }

            if (tasks != null && tasks.Count > 0 && allowedPermissions.Find(x => x.Name == "List of User Tasks") != null)
            {
                foreach (var _task in tasks)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = _task.Id.ToString() + "_" + TransactionItemType.Tasks.ToString(),
                        TransactionType = TransactionItemType.Tasks.ToString(),
                        Company = _task.company.CompanyName,
                        //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                        CreationDate = _task.creationDate,
                        Currency = _task.currency.CurrencyName,
                        Department = (_task.department.parentDepartment == null) ? _task.department.DeptName : _task.department.parentDepartment.DeptName + " " + _task.department.DeptName,
                        SyetmReferenceNo = _task.SystemRef,
                        BackColor = _task.Status == null ? null : _task.Status.backcolor,
                        Status = _task.Status == null ? null : _task.Status.Status,
                        totalCFRValue = _task.ManualAmount,
                        //amountME = _task.MER,
                        //loanAdjustment = _task.AdminBills.Where(a => a.isVoid != true).Sum(b => b.Adjustments.Where(c => c.isApproved == true).Sum(d => d.AdjustmentAmount))
                        //totalCFRValue = 0

                    });
                }
            }

            if (loansAdvance != null && loansAdvance.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
            {
                if(loansAdvance.loansAdvanceType == LoansAdvanceType.Admin_Bill)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = loansAdvance.Id.ToString() + "_" + TransactionItemType.LoansAdvances.ToString(),
                        TransactionType = TransactionItemType.LoansAdvances.ToString(),
                        Company = loansAdvance.company.CompanyName,
                        //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                        CreationDate = loansAdvance.CreationDate,
                        Currency = loansAdvance.currency.CurrencyName,
                        Department = (loansAdvance.department.parentDepartment == null) ? loansAdvance.department.DeptName : loansAdvance.department.parentDepartment.DeptName + " " + loansAdvance.department.DeptName,
                        SyetmReferenceNo = loansAdvance.SystemRef,
                        BackColor = loansAdvance.Status.backcolor,
                        Status = loansAdvance.Status.Status,
                        totalCFRValue = loansAdvance.LoanAmountOC,
                        amountME = loansAdvance.MER,
                        loanAdjustment = loansAdvance.AdminBills.Where(a => a.isVoid != true).Sum(b => b.Adjustments.Where(c => c.isApproved == true).Sum(d => d.AdjustmentAmount))
                        //totalCFRValue = 0

                    });
                }
                else if (loansAdvance.loansAdvanceType == LoansAdvanceType.Vendor_Bill)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = loansAdvance.Id.ToString() + "_" + TransactionItemType.LoansAdvances.ToString(),
                        TransactionType = TransactionItemType.LoansAdvances.ToString(),
                        Company = loansAdvance.company?.CompanyName,
                        //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                        CreationDate = loansAdvance.CreationDate,
                        Currency = loansAdvance.currency.CurrencyName,
                        Department = (loansAdvance.department.parentDepartment == null) ? loansAdvance.department.DeptName : loansAdvance.department.parentDepartment.DeptName + " " + loansAdvance.department.DeptName,
                        SyetmReferenceNo = loansAdvance.SystemRef,
                        BackColor = loansAdvance.Status.backcolor,
                        Status = loansAdvance.Status.Status,
                        totalCFRValue = loansAdvance.LoanAmountOC,
                        amountME = loansAdvance.MER,
                        loanAdjustment = loansAdvance.bills.Where(a => a.isVoid != true).Sum(b => b.Adjustments.Where(c => c.isApproved == true).Sum(d => d.AdjustmentAmount))
                        //totalCFRValue = 0

                    });
                }
                
            }

            if(loansAdvances != null && loansAdvances.Count > 0 && allowedPermissions.Find(x=>x.Name== "List of Loans and Advances") != null)
            {
                foreach(var _LA in loansAdvances)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = _LA.Id.ToString() + "_" + TransactionItemType.LoansAdvances.ToString(),
                        TransactionType = TransactionItemType.LoansAdvances.ToString(),
                        Company = _LA.company == null ? null : _LA.company.CompanyName,
                        //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                        CreationDate = _LA.CreationDate,
                        Currency = _LA.currency.CurrencyName,
                        Department = (_LA.department.parentDepartment == null) ? _LA.department.DeptName : _LA.department.parentDepartment.DeptName + " " + _LA.department.DeptName,
                        SyetmReferenceNo = _LA.SystemRef,
                        BackColor = _LA.Status == null ? null : _LA.Status.backcolor,
                        Status = _LA.Status == null ? null : _LA.Status.Status,
                        totalCFRValue = _LA.AppliedAmountOC,
                        amountME = _LA.MER,
                        loanAdjustment = _LA.AdminBills.Where(a => a.isVoid != true).Sum(b => b.Adjustments.Where(c => c.isApproved == true).Sum(d => d.AdjustmentAmount))
                        //totalCFRValue = 0

                    });
                }
            }

            if (targetReward != null && targetReward.Id != 0 && allowedPermissions.Find(x => x.Name == "View List of Target Rewards") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = targetReward.Id.ToString() + "_" + TransactionItemType.TargetReward.ToString(),
                    TransactionType = TransactionItemType.TargetReward.ToString(),
                    TaskGroup = targetReward.toDoTask.taskGroup == null ? null : targetReward.toDoTask.taskGroup.GroupName,
                    //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                    CreationDate = targetReward.CreationDate,
                    Currency = targetReward.currency.CurrencyName,
                    //Department = (loansAdvance.department.parentDepartment == null) ? loansAdvance.department.DeptName : loansAdvance.department.parentDepartment.DeptName + " " + loansAdvance.department.DeptName,
                    //SyetmReferenceNo = loansAdvance.SystemRef,
                    //BackColor = loansAdvance.Status.backcolor,
                    //Status = loansAdvance.Status.Status,
                    totalCFRValue = targetReward.RewardAmount,
                    //amountME = loansAdvance.MER,
                    //totalCFRValue = 0

                });
            }


            if (payments != null  && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                foreach (var _payment in payments)
                {
                    string parentId = "";
                    if (_payment.PInvoice_Id != null)
                    {

                        parentId = (_payment.PInvoice_Id != null ? _payment.PInvoice_Id.ToString() + "_" + TransactionItemType.Purchase_Invoice.ToString() : "");


                        //parentId = (int)_payment.PInvoice_Id;
                    }
                    else
                    if (_payment.AdminBill_Id != null)
                    {
                        parentId = (_payment.AdminBill_Id != null ? _payment.AdminBill_Id.ToString() + "_" + TransactionItemType.Admin_Bill.ToString() : "");

                        //parentId = (int)_payment.AdminBill_Id;

                    }
                    else
                    if (_payment.Bill_Id != null)
                    {
                        parentId = (_payment.Bill_Id != null ? _payment.Bill_Id.ToString() + "_" + TransactionItemType.Bill.ToString() : "");

                    }
                    else
                        if (_payment.LoansAdvanceId != null)
                    {
                        parentId = (_payment.LoansAdvanceId != null ? _payment.LoansAdvanceId.ToString() + "_" + TransactionItemType.LoansAdvances.ToString() : "");

                    }
                    else
                        if (_payment.TargetReward_Id != null)
                    {
                        parentId = (_payment.TargetReward_Id != null ? _payment.TargetReward_Id.ToString() + "_" + TransactionItemType.TargetReward.ToString() : "");
                    }


                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = _payment.Id.ToString() + "_" + TransactionItemType.Payments.ToString(),
                        TransactionType = TransactionItemType.Payments.ToString(),
                        Company = _payment.company.CompanyName,
                        Parent_Id = parentId.ToString(),
                        CreationDate = _payment.CreationDate,

                        BackColor = _payment.Status == null ? null : _payment.Status.backcolor,
                        Status = _payment.Status == null ? null : _payment.Status.Status,
                        totalCFRValue = _payment.DebitedAmount,


                    }) ;
                }
            }

            if (billl!= null && billl.Id!=0 && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = billl.Id.ToString() + "_" + TransactionItemType.Bill.ToString(),
                    TransactionType = TransactionItemType.Bill.ToString(),
                    Company = billl.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + billl.AllocateTo.person.LName,
                    CreationDate = billl.CreationDate,
                    Currency = billl.currency.CurrencyName,
                    Department = (billl.department.parentDepartment == null) ? billl.department.DeptName : billl.department.parentDepartment.DeptName + " " + billl.department.DeptName,
                    SyetmReferenceNo = billl.SyetmReferenceNo,
                    BackColor = billl.BillStatus.backcolor,
                    Status = billl.BillStatus.Status,
                  
                    totalCFRValue = billl.totalCFRValue
                    //amountME = billl.SoAmountSOC_ER,
                    //totalCFRValue = 0

                });
            }
            if (payment != null && payment.Id!=0 && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                AllTransactionsView transaction = new AllTransactionsView();

                if(adminBill != null)
                    transaction.Id = payment.Id.ToString() + "_" + TransactionItemType.Payments.ToString() + "_" + adminBill.transactionGroupId;
                else
                    transaction.Id = payment.Id.ToString() + "_" + TransactionItemType.Payments.ToString();
                transaction.TransactionType = TransactionItemType.Payments.ToString();
                transaction.Company = payment.company.CompanyName;
                transaction.CreationDate = payment.CreationDate;
                transaction.Currency = payment.currency.CurrencyName;
                transaction.SyetmReferenceNo = payment.SystemRefNo;
                transaction.BackColor = payment.Status.backcolor;
                transaction.Status = payment.Status.Status;
                transaction.totalCFRValue = payment.DebitedAmount;
                allTransactions.Add(transaction);
            }

            if (PInvoice != null && PInvoice.Id!=0 && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = PInvoice.Id.ToString() + "_" + TransactionItemType.Purchase_Invoice.ToString(),
                    TransactionType = TransactionItemType.Purchase_Invoice.ToString(),
                    Company = PInvoice.company.CompanyName,
                 
                    CreationDate = PInvoice.CreationDate,
                    Currency = PInvoice.currency.CurrencyName,
                    Department = (PInvoice.department.parentDepartment == null) ? PInvoice.department.DeptName : PInvoice.department.parentDepartment.DeptName + " " + PInvoice.department.DeptName,
                    SyetmReferenceNo = PInvoice.PIReferenceNo,
                    BackColor = PInvoice.PurchaseInvoiceStatus.backcolor,
                    Status = PInvoice.PurchaseInvoiceStatus.Status,
                    amountSOC = PInvoice.totalInvoiceAmount
                   

                });
            }



            if (inquiry != null && inquiry.Id!=0 && allowedPermissions.Find(x => x.Name == "List Of Inquiries") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = inquiry.Id.ToString() + "_" + TransactionItemType.Inquiry.ToString(),
                    TransactionType = TransactionItemType.Inquiry.ToString(),
                    Company = inquiry.company.CompanyName,
                    Employee = inquiry.employee.person.FName + " " + inquiry.employee.person.FName,
                    CreationDate = inquiry.CreationDate,
                    Currency = inquiry.customerCompany.company.currency.CurrencyName,
                    Department = (inquiry.department.parentDepartment == null) ? inquiry.department.DeptName : inquiry.department.parentDepartment.DeptName + " " + inquiry.department.DeptName,
                    Customer = inquiry.customerCompany.company.CompanyName,
                    SalesReferenceNo = inquiry.SalesReferenceNo,
                    BackColor = inquiry.inquiryStatus.backcolor,
                    Status = inquiry.inquiryStatus.Status,
                    totalCFRValue = 0

                });
            }
            if (offer!= null && offer.Id!=0  && allowedPermissions.Find(x => x.Name == "List Of Offers") != null)
            {
                if (offer.offertype == InquiryType.DistributionBiz)
                {
                    if(offers.Count>0 && allowedPermissions.Find(x => x.Name == "List Of Offers") != null)
                    {
                        foreach(var off in offers)
                        {
                            allTransactions.Add(new AllTransactionsView()
                            {
                                Id = off.Id.ToString() + "_" + TransactionItemType.Offer.ToString(),
                                TransactionType = TransactionItemType.Offer.ToString(),
                                Company = off.company.CompanyName,
                                Employee = off.employee.person.FName + " " + off.employee.person.FName,
                                CreationDate = off.CreationDate,
                                Currency = off.customerCompany.company.currency.CurrencyName,
                                Department = (off.department.parentDepartment == null) ? off.department.DeptName : off.department.parentDepartment.DeptName + " " + off.department.DeptName,
                                Customer = off.customerCompany.company.CompanyName,
                                SalesReferenceNo = off.SalesReferenceNo,
                                BackColor = off.offerStatus.backcolor,
                                Status = off.offerStatus.Status,
                                totalCFRValue = off.totalCFRValue


                            });
                        }
                    }
                }
                else
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = offer.Id.ToString() + "_" + TransactionItemType.Offer.ToString(),
                        TransactionType = TransactionItemType.Offer.ToString(),
                        Company = offer.company.CompanyName,
                        Employee = offer.employee.person.FName + " " + offer.employee.person.FName,
                        CreationDate = offer.CreationDate,
                        Currency = offer.customerCompany.company.currency.CurrencyName,
                        Department = (offer.department.parentDepartment == null) ? offer.department.DeptName : offer.department.parentDepartment.DeptName + " " + offer.department.DeptName,
                        Customer = offer.customerCompany.company.CompanyName,
                        SalesReferenceNo = offer.SalesReferenceNo,
                        BackColor = offer.offerStatus.backcolor,
                        Status = offer.offerStatus.Status,
                        totalCFRValue = offer.totalCFRValue


                    });
                }
            }
          
            if (saleOrder != null && saleOrder.Id!=0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = saleOrder.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                    TransactionType = TransactionItemType.Sale_Order.ToString(),
                    Company = saleOrder.company.CompanyName,
                    Employee = saleOrder.employee.person.FName + " " + saleOrder.employee.person.FName,
                    CreationDate = saleOrder.CreationDate,
                    Currency = saleOrder.customerCompany.company.currency.CurrencyName,
                    Department = (saleOrder.department.parentDepartment == null) ? saleOrder.department.DeptName : saleOrder.department.parentDepartment.DeptName + " " + saleOrder.department.DeptName,
                    Customer = saleOrder.customerCompany.company.CompanyName,
                    SalesReferenceNo = saleOrder.SalesReferenceNo,
                    BackColor = saleOrder.saleOrderStatus.backcolor,
                    Status = saleOrder.saleOrderStatus.Status,
                    totalCFRValue = saleOrder.totalCFRValue,
                    amountSOC = saleOrder.totalCFRValue,
                    amountME = saleOrder.totalBaseCFRValue

                }); 
            }
            if (saleOrders.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                foreach (var order in saleOrders)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = order.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                        TransactionType = TransactionItemType.Sale_Order.ToString(),
                        Company = order.company.CompanyName,
                        Employee = order.employee.person.FName + " " + order.employee.person.FName,
                        CreationDate = order.CreationDate,
                        Currency = order.customerCompany.company.currency.CurrencyName,
                        Department = (order.department.parentDepartment == null) ? order.department.DeptName : order.department.parentDepartment.DeptName + " " + order.department.DeptName,
                        Customer = order.customerCompany.company.CompanyName,
                        SalesReferenceNo = order.SalesReferenceNo,
                        BackColor = order.saleOrderStatus.backcolor,
                        Status = order.saleOrderStatus.Status,
                        totalCFRValue = order.totalCFRValue,
                        amountSOC = order.totalCFRValue,
                        amountME = order.totalBaseCFRValue,
                        Parent_Id=order.ParentSO_Id.ToString()
                        

                    });
                }
            }
            
            if (invoicedPurchaseOrder != null  && invoicedPurchaseOrder.Id!=0 && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = invoicedPurchaseOrder.Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString(),
                    TransactionType = TransactionItemType.Purchase_Order.ToString(),
                    Company = invoicedPurchaseOrder.company.CompanyName,
                    Employee = invoicedPurchaseOrder.AllocateTo.person.FName + " " + invoicedPurchaseOrder.AllocateTo.person.FName,
                    CreationDate = invoicedPurchaseOrder.CreationDate,
                    Currency = invoicedPurchaseOrder.customerCompany.company.currency.CurrencyName,
                    Department = (invoicedPurchaseOrder.department.parentDepartment == null) ? invoicedPurchaseOrder.department.DeptName : invoicedPurchaseOrder.department.parentDepartment.DeptName + " " + invoicedPurchaseOrder.department.DeptName,
                    Customer = invoicedPurchaseOrder.customerCompany.company.CompanyName,
                    SalesReferenceNo = invoicedPurchaseOrder.SalesReferenceNo,
                    BackColor = invoicedPurchaseOrder.PurchaseOrderStatus.backcolor,
                    Status = invoicedPurchaseOrder.PurchaseOrderStatus.Status,
                    totalCFRValue = invoicedPurchaseOrder.totalCFRValue,
                    amountSOC = invoicedPurchaseOrder.SoAmountSOC_ER,
                    amountME = invoicedPurchaseOrder.totalBaseCFRValue,
                    Parent_Id = (invoicedPurchaseOrder.saleOrder_Id != null ? invoicedPurchaseOrder.saleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : ""),
                    SyetmReferenceNo = invoicedPurchaseOrder.SyetmReferenceNo

                });
            }

            if (saleInvoices != null && allowedPermissions.Find(x => x.Name == "List of Sale Invoices") != null)
            {
                foreach (var saleInvoice in saleInvoices)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = saleInvoice.Id.ToString() + "_" + TransactionItemType.Sale_Invoice.ToString(),
                        TransactionType = TransactionItemType.Sale_Invoice.ToString(),
                        Company = saleInvoice.company.CompanyName,
                        Employee = saleInvoice.employee.person.FName + " " + saleInvoice.employee.person.FName,
                        CreationDate = saleInvoice.CreationDate,
                        Currency = saleInvoice.customerCompany.company.currency.CurrencyName,
                        Department = (saleInvoice.department.parentDepartment == null) ? saleInvoice.department.DeptName : saleInvoice.department.parentDepartment.DeptName + " " + saleInvoice.department.DeptName,
                        Customer = saleInvoice.customerCompany.company.CompanyName,
                        SalesReferenceNo = saleInvoice.SalesReferenceNo,
                        BackColor = saleInvoice.saleInvoiceStatus.backcolor,
                        Status = saleInvoice.saleInvoiceStatus.Status,
                        totalCFRValue = saleInvoice.totalInvoiceAmount,
                        amountSOC = saleInvoice.SOCFRValue,
                        amountME = saleInvoice.totalBaseAmount,
                        Parent_Id = (saleInvoice.SaleOrderId != null ? saleInvoice.SaleOrderId.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : "")

                    });
                }
            }
            if (saleReceipts != null && allowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
            {
                foreach (var receipt in saleReceipts)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = receipt.Id.ToString() + "_" + TransactionItemType.Sale_Receipt.ToString(),
                        TransactionType = TransactionItemType.Sale_Receipt.ToString(),
                        Company = receipt.company.CompanyName,
                        //Employee = receipt.employee.person.FName + " " + receipt.employee.person.FName,
                        CreationDate = receipt.CreationDate,
                        Currency = receipt.Currency.CurrencyName,
                        Department = (receipt.department.parentDepartment == null) ? receipt.department.DeptName : receipt.department.parentDepartment.DeptName + " " + receipt.department.DeptName,
                        Customer = receipt.Customer == null ? null: receipt.Customer.company.CompanyName,
                        SalesReferenceNo = receipt.SystemRefNo,
                        BackColor = (receipt.saleReceiptStatus == null)? null : receipt.saleReceiptStatus.backcolor,
                        Status = (receipt.saleReceiptStatus == null) ? null : receipt.saleReceiptStatus.Status,
                        totalCFRValue = receipt.CollectionAmount,
                        Parent_Id = (receipt.saleInvoice != null ? receipt.saleInvoice.Id.ToString() + "_" + TransactionItemType.Sale_Invoice.ToString() : "")

                        //amountSOC = receipt.SOCFRValue,
                        //amountME = receipt.totalBaseAmount

                    });
                }
            }
            if (purchaseOrders != null && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
            {
                foreach (var purchaseOrder in purchaseOrders)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = purchaseOrder.Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString(),
                        TransactionType = TransactionItemType.Purchase_Order.ToString(),
                        Company = purchaseOrder.company.CompanyName,
                        Employee = purchaseOrder.AllocateTo.person.FName + " " + purchaseOrder.AllocateTo.person.FName,
                        CreationDate = purchaseOrder.CreationDate,
                        Currency = purchaseOrder.customerCompany.company.currency.CurrencyName,
                        Department = (purchaseOrder.department.parentDepartment == null) ? purchaseOrder.department.DeptName : purchaseOrder.department.parentDepartment.DeptName + " " + purchaseOrder.department.DeptName,
                        Customer = purchaseOrder.customerCompany.company.CompanyName,
                        SalesReferenceNo = purchaseOrder.SalesReferenceNo,
                        BackColor = purchaseOrder.PurchaseOrderStatus.backcolor,
                        Status = purchaseOrder.PurchaseOrderStatus.Status,
                        totalCFRValue = purchaseOrder.totalCFRValue,
                        amountSOC = purchaseOrder.SoAmountSOC_ER,
                        amountME = purchaseOrder.totalBaseCFRValue,
                        Parent_Id = (purchaseOrder.saleOrder_Id != null ? purchaseOrder.saleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : ""),
                        SyetmReferenceNo = purchaseOrder.SyetmReferenceNo
                        

                    });
                }
            }
            if (journalVouchers1 != null )
            {
                foreach (var vouche in journalVouchers1)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = vouche.Id.ToString() + "_" + TransactionItemType.JV.ToString(),
                        TransactionType = TransactionItemType.JV.ToString(),
                        Company = vouche.company.CompanyName,
                        Employee = vouche.employee.person.FName + " " + vouche.employee.person.FName,
                        CreationDate = vouche.postingDate,
                        Currency = vouche.Currency.CurrencyName,
                        Department = (vouche.department.parentDepartment == null) ? vouche.department.DeptName : vouche.department.parentDepartment.DeptName + " " + vouche.department.DeptName,
                        //Customer = purchaseOrder.customerCompany.company.CompanyName,
                        SalesReferenceNo = vouche.voucherRefno,
                        BackColor = vouche.JournalVoucherStatus.backcolor,
                        Status = vouche.JournalVoucherStatus.Status,
                        //totalCFRValue = purchaseOrder.totalCFRValue,
                        //amountSOC = purchaseOrder.SoAmountSOC_ER,
                        //amountME = purchaseOrder.totalBaseCFRValue,
                        Parent_Id = (vouche.bill_Id != null ? vouche.bill_Id.ToString() + "_" + TransactionItemType.Bill.ToString() : "")

                    });
                }
            }
            if (vouchers.Count != 0 )
            {
                foreach (var jv in vouchers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = jv.Id.ToString() + "_" + TransactionItemType.InterBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.JV.ToString(),
                        Company = jv.company.CompanyName,
                        Employee = jv.employee.person.FName + " " + jv.employee.person.FName,
                        CreationDate = jv.postingDate,
                        Currency = jv.company.currency.CurrencyName,
                        Department = (jv.department.parentDepartment == null) ? jv.department.DeptName : jv.department.parentDepartment.DeptName + " " + jv.department.DeptName,
                        SalesReferenceNo = jv.voucherRefno,
                        BackColor = jv.JournalVoucherStatus.backcolor,
                        Status = jv.JournalVoucherStatus.Status,
                        totalCFRValue = jv.journalTransactions.Sum(x => x.debit)
                    });
                }
            }
            if (bills != null && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
            {
                foreach (var bill in bills)
                {
                    double? hasvalue = bill.SoAmountSOC_ER;

                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = bill.Id.ToString() + "_" + TransactionItemType.Bill.ToString(),
                        TransactionType = TransactionItemType.Bill.ToString(),
                        Company = bill.company.CompanyName,
                        Employee = bill.AllocateTo.person.FName + " " + bill.AllocateTo.person.FName,
                        CreationDate = bill.CreationDate,
                        Currency = bill.customerCompany.company.currency.CurrencyName,
                        Department = (bill.department.parentDepartment == null) ? bill.department.DeptName : bill.department.parentDepartment.DeptName + " " + bill.department.DeptName,
                        Customer = bill.customerCompany.company.CompanyName,
                        SalesReferenceNo = bill.SalesReferenceNo,
                        BackColor = bill.BillStatus.backcolor,
                        Status = bill.BillStatus.Status,
                        totalCFRValue = bill.totalCFRValue,
                        Parent_Id = bill.purchaseOrder_Id != null ? bill.purchaseOrder_Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString() : (bill.saleOrder_Id != null ? bill.saleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : ""),
                        //amountSOC = hasvalue.HasValue?  hasvalue: null,                        
                        amountME = bill.totalBaseCFRValue,
                        SyetmReferenceNo = bill.SyetmReferenceNo
                    });
                }
            }
            if (memorandumSales != null && allowedPermissions.Find(x => x.Name == "List of Memorandum Sales") != null)
            {
                foreach (var memorandumSale in memorandumSales)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = memorandumSale.Id.ToString() + "_" + TransactionItemType.Memorandum_Sale.ToString(),
                        TransactionType = TransactionItemType.Memorandum_Sale.ToString(),
                        Company = memorandumSale.company.CompanyName,
                        CreationDate = memorandumSale.CreationDate,
                        Currency = memorandumSale.customerCompany.company.currency.CurrencyName,
                        Department = (memorandumSale.department.parentDepartment == null) ? memorandumSale.department.DeptName : memorandumSale.department.parentDepartment.DeptName + " " + memorandumSale.department.DeptName,
                        Customer = memorandumSale.customerCompany.company.CompanyName,
                        SalesReferenceNo = memorandumSale.referenceNo,
                        BackColor = memorandumSale.memorandumSaleStatus.backcolor,
                        Status = memorandumSale.memorandumSaleStatus.Status,
                        totalCFRValue = memorandumSale.totalCFRValue,
                        amountSOC = memorandumSale.totalCFRValue,
                        amountME = memorandumSale.totalCFRValue,
                        Parent_Id = (memorandumSale.SaleOrder_Id != null ? memorandumSale.SaleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : (memorandumSale.Offer_Id != null ? memorandumSale.Offer_Id.ToString() + "_" + TransactionItemType.Offer.ToString() : ""))

                    });
                }
            }
            
            if (purchaseInvoices != null && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
            {
                foreach (var purchaseInvoice in purchaseInvoices)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = purchaseInvoice.Id.ToString() + "_" + TransactionItemType.Purchase_Invoice.ToString(),
                        TransactionType = TransactionItemType.Purchase_Invoice.ToString(),
                        Company = purchaseInvoice.company.CompanyName,
                        Employee = purchaseInvoice.employee.person.FName + " " + purchaseInvoice.employee.person.FName,
                        CreationDate = purchaseInvoice.CreationDate,
                        Currency = purchaseInvoice.customerCompany.company.currency.CurrencyName,
                        Department = (purchaseInvoice.department.parentDepartment == null) ? purchaseInvoice.department.DeptName : purchaseInvoice.department.parentDepartment.DeptName + " " + purchaseInvoice.department.DeptName,
                        Customer = purchaseInvoice.customerCompany.company.CompanyName,
                        SalesReferenceNo = purchaseInvoice.SalesReferenceNo,
                        BackColor = purchaseInvoice.PurchaseInvoiceStatus.backcolor,
                        Status = purchaseInvoice.PurchaseInvoiceStatus.Status,
                        totalCFRValue = purchaseInvoice.totalInvoiceAmount,
                        amountSOC = purchaseInvoice.POCFRValue,
                        amountME = purchaseInvoice.totalBaseAmount,
                        Parent_Id = (purchaseInvoice.purchaseOrder_Id != null ? purchaseInvoice.purchaseOrder_Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString() : "")
                    });
                }
            }
            if (bankTransfers.Count != 0  && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                foreach (var transfer in bankTransfers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = transfer.Id.ToString() + "_" + TransactionItemType.InterBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.InterBank_Transfer.ToString(),
                        Company = transfer.company.CompanyName,
                        Employee = transfer.employee.person.FName + " " + transfer.employee.person.FName,
                        CreationDate = transfer.CreationDate,
                        Currency = transfer.company.currency.CurrencyName,
                        Department = (transfer.department.parentDepartment == null) ? transfer.department.DeptName : transfer.department.parentDepartment.DeptName + " " + transfer.department.DeptName,
                       
                        SalesReferenceNo = transfer.FinanceRefNo,
                        BackColor = transfer.interBankTransStatus.backcolor,
                        Status = transfer.interBankTransStatus.Status,
                        totalCFRValue = transfer.AmountFrom


                    });
                }
            }
            if (companyBankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                foreach (var transfer in companyBankTransfers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = transfer.Id.ToString() + "_" + TransactionItemType.InterCompanyBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer.ToString(),
                        Company = transfer.companyTo == null ? null : transfer.companyTo.CompanyName,
                        //Employee = transfer.employee.person.FName + " " + transfer.employee.person.FName,
                        CreationDate = transfer.CreationDate,
                        Currency = transfer.companyFrom.currency.CurrencyName,
                        Department = (transfer.departmentFrom.parentDepartment == null) ? transfer.departmentFrom.DeptName : transfer.departmentFrom.parentDepartment.DeptName + " " + transfer.departmentFrom.DeptName,
                        SalesReferenceNo = transfer.FinanceRefNo,
                        BackColor = transfer.interBankTransStatus.backcolor,
                        Status = transfer.interBankTransStatus.Status,
                        totalCFRValue = transfer.AmountOC
                    });
                }
            }
            return allTransactions;
        }
        /// <summary>
        /// Get purchaseOrder based on SaleOrder ID.
        /// </summary>
        /// <param name="SaleOrderId"></param>
        /// <returns></returns>
        public List<AllTransactionsView> getAllTransactionsNew(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {
            Inquiry inquiry = new Inquiry();
            Offer offer = new Offer();
            InterBankTransfer bankTransfer = new InterBankTransfer();
            SaleOrder saleOrder = new SaleOrder();
            PurchaseOrder invoicedPurchaseOrder = new PurchaseOrder();

            List<SaleInvoice> saleInvoices = new List<SaleInvoice>();
            List<PurchaseInvoice> purchaseInvoices = new List<PurchaseInvoice>();
            PurchaseInvoice PInvoice = new PurchaseInvoice();
            List<MemorandumSale> memorandumSales = new List<MemorandumSale>();
            List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            List<Bill> bills = new List<Bill>();
            Bill billl = new Bill();
            List<JournalVoucher> journalVouchers = new List<JournalVoucher>();
            AdminBill adminBill = new AdminBill();
            LoansAdvance loansAdvance = new LoansAdvance();
            TargetRewards targetReward = new TargetRewards();
            Payment payment = new Payment();
            List<Payment> payments = new List<Payment>();
            List<STL> Stls = new List<STL>();
            List<InterBankTransfer> bankTransfers = new List<InterBankTransfer>();  
            List<JournalVoucher> vouchers = new List<JournalVoucher>();
            List<InterCompanyBankTransfer> companyBankTransfers = new List<InterCompanyBankTransfer>();

            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();
            JournalVoucherRepo voucherRepo = new JournalVoucherRepo();
            switch (type)
            {
                case Enums.TransactionItemType.Inquiry:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    inquiry = context.inquiries
                        .FirstOrDefault(x => x.Id == Id);
                    if (inquiry != null)
                    {
                        offer = context.offers.FirstOrDefault(x => x.inquiry_Id == Id);
                    }
                    else
                    {
                        offer = null;
                    }
                    if (offer != null)
                    {
                        saleOrder = context.saleOrders
               
                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                    }
                    else
                    {
                        saleInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders

                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count != 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count != 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)

                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    break;

                //Offer
                case Enums.TransactionItemType.Offer:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    offer = context.offers.FirstOrDefault(x => x.Id == Id);

                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                        saleOrder = context.saleOrders
             
                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                    }
                    else
                    {
                        saleOrder = null;
                        inquiry = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                    }
                    else
                    {
                        saleInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills                           .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count != 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count != 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                  .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)

                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;

                //SaleOrder
                case Enums.TransactionItemType.Sale_Order:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    saleOrder = context.saleOrders
           
                        .FirstOrDefault(x => x.Id == Id);
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        bankTransfer = context.interBankTransfers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }

                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Invoice:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var invoice = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
                    if (invoice != null)
                    {
                        saleOrder = context.saleOrders
      
                        .FirstOrDefault(x => x.Id == invoice.SaleOrderId);
                    }
                    else
                    {
                        saleOrder = null;
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }

                    }
                    else
                    {
                        purchaseOrders = null;
                    }


                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                      .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Receipt:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var receipt = context.salesReceipts
                   .FirstOrDefault(x => x.Id == Id);
                    if (receipt != null)
                    {
                        saleOrder = context.saleOrders
                 
                        .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                    }
                    else
                    {
                        saleOrder = null;
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                      .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;

                case Enums.TransactionItemType.Admin_Bill:

                    adminBill = context.adminBills.FirstOrDefault(x => x.Id == Id);
                    if (adminBill.Payments != null)
                    {
                        var paymentss = adminBill.Payments;

                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _paymnt in paymentss)
                        {
                            payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                        }
                    }
                    else
                    {
                        payments = null;
                    }

                    payment = null;
                    billl = null;
                    inquiry = null;
                    offer = null;
                    saleOrder = null;
                    saleInvoices = null;
                    purchaseOrders = null;
                    bills = null;
                    purchaseInvoices = null;
                    journalVouchers = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    //if (adminBill != null)
                    //{
                    //    saleOrder = context.saleOrders
                    //    .Include("offer")
                    //    .Include("offer.inquiry")
                    //    .Include("company")
                    //    .Include("employee")
                    //    .Include("department").Include("SaleInvoices").Include("SaleOrderStatus")
                    //    .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                    //}
                    break;

                case Enums.TransactionItemType.Payments:

                    payment = context.payments.FirstOrDefault(x => x.Id == Id);
                    if (payment != null && payment.transactionGroupId != 0)
                    {
                        companyBankTransfers = context.interCompanyBankTransfers
                                               
                            .Where(x => x.paymentGroupId == payment.transactionGroupId).ToList();
                       
                            vouchers = context.journalVouchers
                                .Where(x=>x.paymentGroupId==payment.transactionGroupId).ToList();

                        Stls = context.STLs.Where(x => x.paymentGroupId == payment.transactionGroupId).ToList();
                        foreach(var stl in Stls)
                        {
                            bankTransfers.AddRange(stl.InterBankTransfers);
                        }
                            

                    }

                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Admin_Bills)
                    {

                        if (payment.AdminBill_Id != null)
                        {
                            Employee empUser = new Employee();
                            EmployeeRepo empRepo = new EmployeeRepo();
                            empUser = empRepo.GetEmployeeForPayments(empID);
                            adminBill = context.adminBills
                            .FirstOrDefault(x => x.Id == payment.AdminBill_Id);
                            if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                adminBill = null;
                        }
                        else
                        {
                            adminBill = null;
                        }
                        if(adminBill != null)
                        {
                            payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                        }
                        bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                        vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                        billl = null;
                        PInvoice = null;
                        saleOrder = null;
                        offer = null;
                        inquiry = null;
                        payment = null;
                        invoicedPurchaseOrder = null;

                    }

                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Vendor_Bills)
                    {
                        Employee empUser = new Employee();
                        EmployeeRepo empRepo = new EmployeeRepo();
                        if (payment.Bill_Id != null)
                        {
                            
                            empUser = empRepo.GetEmployeeForPayments(empID);
                            
                            billl = context.bills
                            .FirstOrDefault(x => x.Id == payment.Bill_Id);

                            if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                billl = null;
                        }
                        else
                        {
                            billl = null;
                        }

                        if (billl != null)
                        {

                            if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                            {
                                saleOrder = context.saleOrders
                            
                                    .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                if (saleOrder != null)
                                    if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                        saleOrder = null;
                            }
                            else
                            {
                                saleOrder = null;
                            }

                            if (saleOrder == null)
                            {
                                PurchaseOrder purchaseOrder = new PurchaseOrder();
                                if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                {
                                    purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                    if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                        purchaseOrder = null;
                                }
                                else
                                {
                                    purchaseOrder = null;
                                }
                                
                                if (purchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders
                              
                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                    purchaseOrders.Add(purchaseOrder);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                           
                                }
                            }
                            
                            journalVouchers = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                            payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers
                                 
                              .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                         
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                        }
                        if(purchaseInvoices != null)
                        {
                            foreach (var _PI in purchaseInvoices)
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments);
                        }
                        //else
                        //{
                        //    purchaseOrders = null;
                        //}

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                      .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                        vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                        PInvoice = null;
                        //payments = null;
                        payment = null;
                        invoicedPurchaseOrder = null;
                        adminBill = null;
                        loansAdvance = null;
                    }



                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                    {

                        if (payment.PInvoice_Id != null)
                        {
                            Employee empUser = new Employee();
                            EmployeeRepo empRepo = new EmployeeRepo();
                            empUser = empRepo.GetEmployeeForPayments(empID);
                            PInvoice = context.purchaseInvoices
                            .FirstOrDefault(x => x.Id == payment.PInvoice_Id);
                            if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                PInvoice = null;
                        }
                        else
                        {
                            PInvoice = null;
                        }
                        if (PInvoice != null)
                        {
                            invoicedPurchaseOrder = context.purchaseOrders
                              
                            .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                            payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                        }
                        else
                        {
                            invoicedPurchaseOrder = null;
                        }
                        if (invoicedPurchaseOrder != null)
                        {
                            saleOrder = context.saleOrders
               
                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                            if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                            if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                            {
                                purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                            }

                            if (purchaseOrders != null)
                                foreach (var _po in purchaseOrders)
                                    purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x=>x.Id != PInvoice.Id).ToList());

                            purchaseInvoices.Distinct();

                            if (purchaseInvoices != null)
                                foreach (var _pi in purchaseInvoices)
                                    payments.AddRange(_pi.Payments);

                            payments.Distinct();
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {

                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id));


                        adminBill = null;
                        billl = null;
                        bills = null;
                        //purchaseInvoices = null;
                        payment = null;
                        journalVouchers = null;
                    }

                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Loans_Advances)
                    {

                        if (payment.LoansAdvanceId != null)
                        {
                            Employee empUser = new Employee();
                            EmployeeRepo empRepo = new EmployeeRepo();
                            empUser = empRepo.GetEmployeeForPayments(empID);
                            loansAdvance = context.loansAdvances
                              
                            .FirstOrDefault(x => x.Id == payment.LoansAdvanceId);
                            if (empUser.departments.FirstOrDefault(x => x.Id == loansAdvance.department.Id) == null)
                                loansAdvance = null;
                        }
                        else
                        {
                            loansAdvance = null;
                        }
                        if (loansAdvance != null)
                        {
                            payments = context.payments.Where(x => x.LoansAdvanceId == loansAdvance.Id).ToList();
                        }
                        bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                        vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                        billl = null;
                        PInvoice = null;
                        saleOrder = null;
                        offer = null;
                        inquiry = null;
                        payment = null;
                        invoicedPurchaseOrder = null;
                        adminBill = null;

                    }
                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Target_Reward)
                    {

                        if (payment.TargetReward_Id != null)
                        {
                            Employee empUser = new Employee();
                            EmployeeRepo empRepo = new EmployeeRepo();
                            empUser = empRepo.GetEmployeeForPayments(empID);
                            targetReward = context.targetRewards

                            .FirstOrDefault(x => x.Id == payment.TargetReward_Id);
                            //if (empUser.departments.FirstOrDefault(x => x.Id == loansAdvance.department.Id) == null)
                            //    loansAdvance = null;
                        }
                        else
                        {
                            targetReward = null;
                        }
                        if (targetReward != null)
                        {
                            payments = context.payments.Where(x => x.TargetReward_Id == targetReward.Id).ToList();
                        }
                        bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                        vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                        billl = null;
                        PInvoice = null;
                        saleOrder = null;
                        offer = null;
                        inquiry = null;
                        payment = null;
                        invoicedPurchaseOrder = null;
                        adminBill = null;

                    }
                    break;

                case Enums.TransactionItemType.Purchase_Order:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var PurchaseOrder = context.purchaseOrders
                        .FirstOrDefault(x => x.Id == Id);
                    if (PurchaseOrder != null)
                    {
                        // purchaseInvoices = context.purchaseInvoices.Include("PurchaseInvoiceStatus")
                        //.Include("customerCompany.company").Include("PurchaseOrder")
                        //.Include("company").Include("employee")
                        //.Include("department").Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                        saleOrder = context.saleOrders
                      
                        .FirstOrDefault(x => x.Id == PurchaseOrder.saleOrder_Id);
                    }
                    else
                    {
                        saleOrder = null;
                        purchaseInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else if (PurchaseOrder != null)
                    {
                        bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }
                    }
                    else if (PurchaseOrder != null)
                    {
                        purchaseOrders.Add(PurchaseOrder);
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                        purchaseInvoices = null;
                    }


                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                   .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    journalVouchers = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();

                    break;
                case Enums.TransactionItemType.Bill:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;

                    var bill = context.bills.FirstOrDefault(x => x.Id == Id);
                    if (bill != null)
                    {
                        saleOrder = context.saleOrders
                    
                        .FirstOrDefault(x => x.Id == bill.saleOrder_Id);
                        if (saleOrder == null)
                        {
                            var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == bill.purchaseOrder_Id);
                            if (purchaseOrder != null)
                            {
                                saleOrder = context.saleOrders
                      
                                .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList();
                                purchaseOrders.Add(purchaseOrder);
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                            else
                            {
                                bills.Add(bill);
                            }
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        journalVouchers = context.journalVouchers.Where(x => x.bill_Id == bill.Id).ToList();

                        payments = context.payments.Where(x => x.Bill_Id == bill.Id).ToList();

                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers
                              .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                    }
 
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
               .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }










                    break;
                //Memorandum Sale
                case Enums.TransactionItemType.Memorandum_Sale:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var memorandumSale = context.memorandumSales.Where(x => x.Id == Id).FirstOrDefault();
                    if (memorandumSale != null)
                    {
                        saleOrder = context.saleOrders
   
                        .FirstOrDefault(x => x.Id == memorandumSale.SaleOrder_Id);
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices
                       .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = context.offers.FirstOrDefault(x => x.Id == memorandumSale.Offer_Id);
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    //else
                    //{
                    //    purchaseOrders = null;
                    //}
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else if (memorandumSale != null)
                    {
                        memorandumSales.Add(memorandumSale);
                    }
                    break;


                case Enums.TransactionItemType.Purchase_Invoice:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    var purchaseInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == Id);
                    if (purchaseInvoice != null)
                    {
                        invoicedPurchaseOrder = context.purchaseOrders
                       
                        .FirstOrDefault(x => x.Id == purchaseInvoice.purchaseOrder_Id);

                        payments = context.payments
                     
                        .Where(x => x.PInvoice_Id == purchaseInvoice.Id).ToList();
                    }
                    else
                    {
                        invoicedPurchaseOrder = null;
                    }
                    if (invoicedPurchaseOrder != null)
                    {
                        saleOrder = context.saleOrders
                 
                        .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                 
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    purchaseInvoices = context.purchaseInvoices.Where(x => x.isVoid != true && x.purchaseOrder_Id == purchaseInvoice.purchaseOrder_Id).ToList();

                    //purchaseInvoices.Add(purchaseInvoice);

                    break;
                    //Purchase Invoices


            }
            if (Stls.Count != 0)
            {
                foreach (var sTL in Stls)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = sTL.Id.ToString() + "_" + TransactionItemType.STL.ToString(),
                        TransactionType = TransactionItemType.STL.ToString(),
                        Company = sTL.company.CompanyName,
                        Employee = sTL.user.employee.person.FName + " " + sTL.user.employee.person.FName,
                        CreationDate = sTL.CreationDate,
                        Currency = sTL.stlCurrency.CurrencyName,
                        Department = (sTL.department.parentDepartment == null) ? sTL.department.DeptName : sTL.department.parentDepartment.DeptName + " " + sTL.department.DeptName,
                        //SalesReferenceNo = sTL.voucherRefno,
                        BackColor = sTL.stlStatus.backcolor,
                        Status = sTL.stlStatus.Status,
                        totalCFRValue = sTL.stlPaymentAmountOC
                    });
                }
            }
            if (vouchers.Count != 0)
            {
                foreach (var jv in vouchers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = jv.Id.ToString() + "_" + TransactionItemType.InterBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.JV.ToString(),
                        Company = jv.company.CompanyName,
                        Employee = jv.employee.person.FName + " " + jv.employee.person.FName,
                        CreationDate = jv.postingDate,
                        Currency = jv.company.currency.CurrencyName,
                        Department = (jv.department.parentDepartment == null) ? jv.department.DeptName : jv.department.parentDepartment.DeptName + " " + jv.department.DeptName,
                        SalesReferenceNo = jv.voucherRefno,
                        BackColor = jv.JournalVoucherStatus.backcolor,
                        Status = jv.JournalVoucherStatus.Status,
                        totalCFRValue = jv.journalTransactions.Sum(x => x.debit)
                    });
                }
            }
            companyBankTransfers = companyBankTransfers.GroupBy(x => x.Id)
                                .Select(g => g.First())
                                .ToList();
            if (loansAdvance != null && loansAdvance.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = loansAdvance.Id.ToString() + "_" + TransactionItemType.LoansAdvances.ToString(),
                    TransactionType = TransactionItemType.LoansAdvances.ToString(),
                    Company = loansAdvance.company == null ? null : loansAdvance.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                    CreationDate = loansAdvance.CreationDate,
                    Currency = loansAdvance.currency.CurrencyName,
                    Department = (loansAdvance.department.parentDepartment == null) ? loansAdvance.department.DeptName : loansAdvance.department.parentDepartment.DeptName + " " + loansAdvance.department.DeptName,
                    SyetmReferenceNo = loansAdvance.SystemRef,
                    BackColor = loansAdvance.Status.backcolor,
                    Status = loansAdvance.Status.Status,
                    totalCFRValue = loansAdvance.LoanAmountOC,
                    amountME = loansAdvance.MER,
                    //totalCFRValue = 0

                });
            }

            if (targetReward != null && targetReward.Id != 0 && allowedPermissions.Find(x => x.Name == "View List of Target Rewards") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = targetReward.Id.ToString() + "_" + TransactionItemType.TargetReward.ToString(),
                    TransactionType = TransactionItemType.TargetReward.ToString(),
                    TaskGroup = targetReward.toDoTask.taskGroup == null ? null : targetReward.toDoTask.taskGroup.GroupName,
                    //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                    CreationDate = targetReward.CreationDate,
                    Currency = targetReward.currency.CurrencyName,
                    //Department = (loansAdvance.department.parentDepartment == null) ? loansAdvance.department.DeptName : loansAdvance.department.parentDepartment.DeptName + " " + loansAdvance.department.DeptName,
                    //SyetmReferenceNo = loansAdvance.SystemRef,
                    //BackColor = loansAdvance.Status.backcolor,
                    //Status = loansAdvance.Status.Status,
                    totalCFRValue = targetReward.RewardAmount,
                    //amountME = loansAdvance.MER,
                    //totalCFRValue = 0

                });
            }



            // Adding all transactions in same structure
            if (adminBill != null && allowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = adminBill.Id.ToString() + "_" + TransactionItemType.Admin_Bill.ToString() + "_" + adminBill.transactionGroupId,
                    TransactionType = TransactionItemType.Admin_Bill.ToString(),
                    Company = adminBill.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + adminBill.employee.person.LName,
                    CreationDate = adminBill.CreationDate,
                    Currency = adminBill.currency.CurrencyName,
                    Department = (adminBill.department.parentDepartment == null) ? adminBill.department.DeptName : adminBill.department.parentDepartment.DeptName + " " + adminBill.department.DeptName,
                    SyetmReferenceNo = adminBill.SystemRefNo,
                    BackColor = adminBill.BillStatus.backcolor,
                    Status = adminBill.BillStatus.Status,
                    amountSOC = adminBill.AmountOC,
                    amountME = adminBill.AmountMER,
                    loanAdjustment = adminBill.Adjustments.Where(x=>x.isApproved==true).Sum(x=>x.AdjustmentAmount)
                    //totalCFRValue = 0

                });
            }

            if (payments != null && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                foreach (var _payment in payments)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = _payment.Id.ToString() + "_" + TransactionItemType.Payments.ToString(),
                        TransactionType = TransactionItemType.Payments.ToString(),
                        Company = _payment.company.CompanyName,
                        //Employee = _payment.employee.person.FName + " " + saleInvoice.employee.person.FName,
                        CreationDate = _payment.CreationDate,
                        //Currency = _payment.currency.CurrencyName,
                        //Department = (_payment.departments.parentDepartment == null) ? saleInvoice.department.DeptName : saleInvoice.department.parentDepartment.DeptName + " " + saleInvoice.department.DeptName,
                        //Customer = saleInvoice.customerCompany.company.CompanyName,
                        //SalesReferenceNo = saleInvoice.SalesReferenceNo,
                        BackColor = _payment.Status != null?_payment.Status.backcolor: null,
                        Status = _payment.Status != null? _payment.Status.Status:null,
                        totalCFRValue = _payment.DebitedAmount,
                        //amountSOC = saleInvoice.SOCFRValue,
                        //amountME = saleInvoice.totalBaseAmount,
                        //Parent_Id = (saleInvoice.SaleOrderId != null ? saleInvoice.SaleOrderId.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : "")

                    });
                }
            }

            if (billl != null && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = billl.Id.ToString() + "_" + TransactionItemType.Bill.ToString(),
                    TransactionType = TransactionItemType.Bill.ToString(),
                    Company = billl.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + billl.AllocateTo.person.LName,
                    CreationDate = billl.CreationDate,
                    Currency = billl.currency.CurrencyName,
                    Department = (billl.department.parentDepartment == null) ? billl.department.DeptName : billl.department.parentDepartment.DeptName + " " + billl.department.DeptName,
                    SyetmReferenceNo = billl.SyetmReferenceNo,
                    BackColor = billl.BillStatus.backcolor,
                    Status = billl.BillStatus.Status,

                    totalCFRValue = billl.totalCFRValue
                    //amountME = billl.SoAmountSOC_ER,
                    //totalCFRValue = 0

                });
            }
            if (payment != null && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
            {
                AllTransactionsView transaction = new AllTransactionsView();

                if (adminBill != null)
                    transaction.Id = payment.Id.ToString() + "_" + TransactionItemType.Payments.ToString() + "_" + adminBill.transactionGroupId;
                else
                    transaction.Id = payment.Id.ToString() + "_" + TransactionItemType.Payments.ToString();
                transaction.TransactionType = TransactionItemType.Payments.ToString();
                transaction.Company = payment.company.CompanyName;
                transaction.CreationDate = payment.CreationDate;
                transaction.Currency = payment.currency.CurrencyName;
                transaction.SyetmReferenceNo = payment.SystemRefNo;
                transaction.BackColor = payment.Status.backcolor;
                transaction.Status = payment.Status.Status;
                transaction.totalCFRValue = payment.DebitedAmount;
                allTransactions.Add(transaction);
            }

            if (PInvoice != null && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = PInvoice.Id.ToString() + "_" + TransactionItemType.Purchase_Invoice.ToString(),
                    TransactionType = TransactionItemType.Purchase_Invoice.ToString(),
                    Company = PInvoice.company.CompanyName,
                    //Employee = adminBill.employee.person.FName + " " + billl.AllocateTo.person.LName,
                    CreationDate = PInvoice.CreationDate,
                    Currency = PInvoice.currency.CurrencyName,
                    Department = (PInvoice.department.parentDepartment == null) ? PInvoice.department.DeptName : PInvoice.department.parentDepartment.DeptName + " " + PInvoice.department.DeptName,
                    SyetmReferenceNo = PInvoice.PIReferenceNo,
                    BackColor = PInvoice.PurchaseInvoiceStatus.backcolor,
                    Status = PInvoice.PurchaseInvoiceStatus.Status,
                     totalCFRValue = PInvoice.totalInvoiceAmount,
                        amountSOC = PInvoice.POCFRValue,
                        amountME = PInvoice.totalBaseAmount,

                });
            }
            if (bankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                foreach (var transfer in bankTransfers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = transfer.Id.ToString() + "_" + TransactionItemType.InterBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.InterBank_Transfer.ToString(),
                        Company = transfer.company.CompanyName,
                        Employee = transfer.employee.person.FName + " " + transfer.employee.person.FName,
                        CreationDate = transfer.CreationDate,
                        Currency = transfer.company.currency.CurrencyName,
                        Department = (transfer.department.parentDepartment == null) ? transfer.department.DeptName : transfer.department.parentDepartment.DeptName + " " + transfer.department.DeptName,

                        SalesReferenceNo = transfer.FinanceRefNo,
                        BackColor = transfer.interBankTransStatus.backcolor,
                        Status = transfer.interBankTransStatus.Status,
                        totalCFRValue = transfer.AmountOC


                    });
                }
            }
            if (vouchers.Count != 0 && allowedPermissions.Find(x => x.Name == "View JV") != null)
            {
                foreach (var jv in vouchers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = jv.Id.ToString() + "_" + TransactionItemType.InterBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.JV.ToString(),
                        Company = jv.company.CompanyName,
                        Employee = jv.employee.person.FName + " " + jv.employee.person.FName,
                        CreationDate = jv.postingDate,
                        Currency = jv.company.currency.CurrencyName,
                        Department = (jv.department.parentDepartment == null) ? jv.department.DeptName : jv.department.parentDepartment.DeptName + " " + jv.department.DeptName,
                        SalesReferenceNo = jv.voucherRefno,
                        BackColor = jv.JournalVoucherStatus.backcolor,
                        Status = jv.JournalVoucherStatus.Status,
                        totalCFRValue = jv.journalTransactions.Sum(x=>x.debit)
                    });
                }
            }
            if (inquiry != null && allowedPermissions.Find(x => x.Name == "List Of Inquiries") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = inquiry.Id.ToString() + "_" + TransactionItemType.Inquiry.ToString(),
                    TransactionType = TransactionItemType.Inquiry.ToString(),
                    Company = inquiry.company.CompanyName,
                    Employee = inquiry.employee.person.FName + " " + inquiry.employee.person.FName,
                    CreationDate = inquiry.CreationDate,
                    Currency = inquiry.customerCompany.company.currency.CurrencyName,
                    Department = (inquiry.department.parentDepartment == null) ? inquiry.department.DeptName : inquiry.department.parentDepartment.DeptName + " " + inquiry.department.DeptName,
                    Customer = inquiry.customerCompany.company.CompanyName,
                    SalesReferenceNo = inquiry.SalesReferenceNo,
                    BackColor = inquiry.inquiryStatus.backcolor,
                    Status = inquiry.inquiryStatus.Status,
                    totalCFRValue = 0

                });
            }
            if (offer != null && allowedPermissions.Find(x => x.Name == "List Of Offers") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = offer.Id.ToString() + "_" + TransactionItemType.Offer.ToString(),
                    TransactionType = TransactionItemType.Offer.ToString(),
                    Company = offer.company.CompanyName,
                    Employee = offer.employee.person.FName + " " + offer.employee.person.FName,
                    CreationDate = offer.CreationDate,
                    Currency = offer.customerCompany.company.currency.CurrencyName,
                    Department = (offer.department.parentDepartment == null) ? offer.department.DeptName : offer.department.parentDepartment.DeptName + " " + offer.department.DeptName,
                    Customer = offer.customerCompany.company.CompanyName,
                    SalesReferenceNo = offer.SalesReferenceNo,
                    BackColor = offer.offerStatus.backcolor,
                    Status = offer.offerStatus.Status,
                    totalCFRValue = offer.totalCFRValue


                });
            }
            if (saleOrder != null && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = saleOrder.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                    TransactionType = TransactionItemType.Sale_Order.ToString(),
                    Company = saleOrder.company.CompanyName,
                    Employee = saleOrder.employee.person.FName + " " + saleOrder.employee.person.FName,
                    CreationDate = saleOrder.CreationDate,
                    Currency = saleOrder.customerCompany.company.currency.CurrencyName,
                    Department = (saleOrder.department.parentDepartment == null) ? saleOrder.department.DeptName : saleOrder.department.parentDepartment.DeptName + " " + saleOrder.department.DeptName,
                    Customer = saleOrder.customerCompany.company.CompanyName,
                    SalesReferenceNo = saleOrder.SalesReferenceNo,
                    BackColor = saleOrder.saleOrderStatus.backcolor,
                    Status = saleOrder.saleOrderStatus.Status,
                    totalCFRValue = saleOrder.totalCFRValue,
                    amountSOC = saleOrder.totalCFRValue,
                    amountME = saleOrder.totalBaseCFRValue

                });
            }
            if (invoicedPurchaseOrder != null && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = invoicedPurchaseOrder.Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString(),
                    TransactionType = TransactionItemType.Purchase_Order.ToString(),
                    Company = invoicedPurchaseOrder.company.CompanyName,
                    Employee = invoicedPurchaseOrder.AllocateTo.person.FName + " " + invoicedPurchaseOrder.AllocateTo.person.FName,
                    CreationDate = invoicedPurchaseOrder.CreationDate,
                    Currency = invoicedPurchaseOrder.customerCompany.company.currency.CurrencyName,
                    Department = (invoicedPurchaseOrder.department.parentDepartment == null) ? invoicedPurchaseOrder.department.DeptName : invoicedPurchaseOrder.department.parentDepartment.DeptName + " " + invoicedPurchaseOrder.department.DeptName,
                    Customer = invoicedPurchaseOrder.customerCompany.company.CompanyName,
                    SalesReferenceNo = invoicedPurchaseOrder.SalesReferenceNo,
                    BackColor = invoicedPurchaseOrder.PurchaseOrderStatus.backcolor,
                    Status = invoicedPurchaseOrder.PurchaseOrderStatus.Status,
                    totalCFRValue = invoicedPurchaseOrder.totalCFRValue,
                    amountSOC = invoicedPurchaseOrder.SoAmountSOC_ER,
                    amountME = invoicedPurchaseOrder.totalBaseCFRValue,
                    Parent_Id = (invoicedPurchaseOrder.saleOrder_Id != null ? invoicedPurchaseOrder.saleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : ""),
                    SyetmReferenceNo = invoicedPurchaseOrder.SyetmReferenceNo

                });
            }

            if (saleInvoices != null && allowedPermissions.Find(x => x.Name == "List of Sale Invoices") != null)
            {
                foreach (var saleInvoice in saleInvoices)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = saleInvoice.Id.ToString() + "_" + TransactionItemType.Sale_Invoice.ToString(),
                        TransactionType = TransactionItemType.Sale_Invoice.ToString(),
                        Company = saleInvoice.company.CompanyName,
                        Employee = saleInvoice.employee.person.FName + " " + saleInvoice.employee.person.FName,
                        CreationDate = saleInvoice.CreationDate,
                        Currency = saleInvoice.customerCompany.company.currency.CurrencyName,
                        Department = (saleInvoice.department.parentDepartment == null) ? saleInvoice.department.DeptName : saleInvoice.department.parentDepartment.DeptName + " " + saleInvoice.department.DeptName,
                        Customer = saleInvoice.customerCompany.company.CompanyName,
                        SalesReferenceNo = saleInvoice.SalesReferenceNo,
                        BackColor = saleInvoice.saleInvoiceStatus.backcolor,
                        Status = saleInvoice.saleInvoiceStatus.Status,
                        totalCFRValue = saleInvoice.totalInvoiceAmount,
                        amountSOC = saleInvoice.SOCFRValue,
                        amountME = saleInvoice.totalBaseAmount,
                        Parent_Id = (saleInvoice.SaleOrderId != null ? saleInvoice.SaleOrderId.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : "")

                    });
                }
            }
            if (saleReceipts != null && allowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
            {
                foreach (var receipt in saleReceipts)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = receipt.Id.ToString() + "_" + TransactionItemType.Sale_Receipt.ToString(),
                        TransactionType = TransactionItemType.Sale_Receipt.ToString(),
                        Company = receipt.company.CompanyName,
                        //Employee = receipt.employee.person.FName + " " + receipt.employee.person.FName,
                        CreationDate = receipt.CreationDate,
                        Currency = receipt.Currency.CurrencyName,
                        Department = (receipt.department.parentDepartment == null) ? receipt.department.DeptName : receipt.department.parentDepartment.DeptName + " " + receipt.department.DeptName,
                        Customer = receipt.Customer == null? null : receipt.Customer.company.CompanyName,
                        SalesReferenceNo = receipt.SystemRefNo,
                        BackColor = receipt.saleReceiptStatus.backcolor,
                        Status = receipt.saleReceiptStatus.Status,
                        totalCFRValue = receipt.CollectionAmount,
                        Parent_Id = (receipt.saleInvoice != null ? receipt.saleInvoice.Id.ToString() + "_" + TransactionItemType.Sale_Invoice.ToString() : "")

                        //amountSOC = receipt.SOCFRValue,
                        //amountME = receipt.totalBaseAmount

                    });
                }
            }
            if (purchaseOrders != null && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
            {
                foreach (var purchaseOrder in purchaseOrders)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = purchaseOrder.Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString(),
                        TransactionType = TransactionItemType.Purchase_Order.ToString(),
                        Company = purchaseOrder.company.CompanyName,
                        Employee = purchaseOrder.AllocateTo.person.FName + " " + purchaseOrder.AllocateTo.person.FName,
                        CreationDate = purchaseOrder.CreationDate,
                        Currency = purchaseOrder.customerCompany.company.currency.CurrencyName,
                        Department = (purchaseOrder.department.parentDepartment == null) ? purchaseOrder.department.DeptName : purchaseOrder.department.parentDepartment.DeptName + " " + purchaseOrder.department.DeptName,
                        Customer = purchaseOrder.customerCompany.company.CompanyName,
                        SalesReferenceNo = purchaseOrder.SalesReferenceNo,
                        BackColor = purchaseOrder.PurchaseOrderStatus.backcolor,
                        Status = purchaseOrder.PurchaseOrderStatus.Status,
                        totalCFRValue = purchaseOrder.totalCFRValue,
                        amountSOC = purchaseOrder.SoAmountSOC_ER,
                        amountME = purchaseOrder.totalBaseCFRValue,
                        Parent_Id = (purchaseOrder.saleOrder_Id != null ? purchaseOrder.saleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : ""),
                        SyetmReferenceNo = purchaseOrder.SyetmReferenceNo


                    });
                }
            }
            if (journalVouchers != null)
            {
                foreach (var voucher in journalVouchers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = voucher.Id.ToString() + "_" + TransactionItemType.JV.ToString(),
                        TransactionType = TransactionItemType.JV.ToString(),
                        Company = voucher.company.CompanyName,
                        Employee = voucher.employee.person.FName + " " + voucher.employee.person.FName,
                        CreationDate = voucher.postingDate,
                        Currency = voucher.Currency.CurrencyName,
                        Department = (voucher.department.parentDepartment == null) ? voucher.department.DeptName : voucher.department.parentDepartment.DeptName + " " + voucher.department.DeptName,
                        //Customer = purchaseOrder.customerCompany.company.CompanyName,
                        SalesReferenceNo = voucher.voucherRefno,
                        BackColor = voucher.JournalVoucherStatus.backcolor,
                        Status = voucher.JournalVoucherStatus.Status,
                        //totalCFRValue = purchaseOrder.totalCFRValue,
                        //amountSOC = purchaseOrder.SoAmountSOC_ER,
                        //amountME = purchaseOrder.totalBaseCFRValue,
                        Parent_Id = (voucher.bill_Id != null ? voucher.bill_Id.ToString() + "_" + TransactionItemType.Bill.ToString() : "")

                    });
                }
            }
            if (bills != null && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
            {
                foreach (var bill in bills)
                {
                    double? hasvalue = bill.SoAmountSOC_ER;

                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = bill.Id.ToString() + "_" + TransactionItemType.Bill.ToString(),
                        TransactionType = TransactionItemType.Bill.ToString(),
                        Company = bill.company.CompanyName,
                        Employee = bill.AllocateTo.person.FName + " " + bill.AllocateTo.person.FName,
                        CreationDate = bill.CreationDate,
                        Currency = bill.customerCompany.company.currency.CurrencyName,
                        Department = (bill.department.parentDepartment == null) ? bill.department.DeptName : bill.department.parentDepartment.DeptName + " " + bill.department.DeptName,
                        Customer = bill.customerCompany.company.CompanyName,
                        SalesReferenceNo = bill.SalesReferenceNo,
                        BackColor = bill.BillStatus.backcolor,
                        Status = bill.BillStatus.Status,
                        totalCFRValue = bill.totalCFRValue,
                        Parent_Id = bill.purchaseOrder_Id != null ? bill.purchaseOrder_Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString() : (bill.saleOrder_Id != null ? bill.saleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : ""),
                        //amountSOC = hasvalue.HasValue?  hasvalue: null,                        
                        amountME = bill.totalBaseCFRValue,
                        SyetmReferenceNo = bill.SyetmReferenceNo
                    });
                }
            }
            if (memorandumSales != null && allowedPermissions.Find(x => x.Name == "List of Memorandum Sales") != null)
            {
                foreach (var memorandumSale in memorandumSales)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = memorandumSale.Id.ToString() + "_" + TransactionItemType.Memorandum_Sale.ToString(),
                        TransactionType = TransactionItemType.Memorandum_Sale.ToString(),
                        Company = memorandumSale.company.CompanyName,
                        CreationDate = memorandumSale.CreationDate,
                        Currency = memorandumSale.customerCompany.company.currency.CurrencyName,
                        Department = (memorandumSale.department.parentDepartment == null) ? memorandumSale.department.DeptName : memorandumSale.department.parentDepartment.DeptName + " " + memorandumSale.department.DeptName,
                        Customer = memorandumSale.customerCompany.company.CompanyName,
                        SalesReferenceNo = memorandumSale.referenceNo,
                        BackColor = memorandumSale.memorandumSaleStatus.backcolor,
                        Status = memorandumSale.memorandumSaleStatus.Status,
                        totalCFRValue = memorandumSale.totalCFRValue,
                        amountSOC = memorandumSale.totalCFRValue,
                        amountME = memorandumSale.totalCFRValue,
                        Parent_Id = (memorandumSale.SaleOrder_Id != null ? memorandumSale.SaleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : (memorandumSale.Offer_Id != null ? memorandumSale.Offer_Id.ToString() + "_" + TransactionItemType.Offer.ToString() : ""))

                    });
                }
            }
            if (purchaseInvoices != null && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
            {
                foreach (var purchaseInvoice in purchaseInvoices)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = purchaseInvoice.Id.ToString() + "_" + TransactionItemType.Purchase_Invoice.ToString(),
                        TransactionType = TransactionItemType.Purchase_Invoice.ToString(),
                        Company = purchaseInvoice.company.CompanyName,
                        Employee = purchaseInvoice.employee.person.FName + " " + purchaseInvoice.employee.person.FName,
                        CreationDate = purchaseInvoice.CreationDate,
                        Currency = purchaseInvoice.customerCompany.company.currency.CurrencyName,
                        Department = (purchaseInvoice.department.parentDepartment == null) ? purchaseInvoice.department.DeptName : purchaseInvoice.department.parentDepartment.DeptName + " " + purchaseInvoice.department.DeptName,
                        Customer = purchaseInvoice.customerCompany.company.CompanyName,
                        SalesReferenceNo = purchaseInvoice.SalesReferenceNo,
                        BackColor = purchaseInvoice.PurchaseInvoiceStatus.backcolor,
                        Status = purchaseInvoice.PurchaseInvoiceStatus.Status,
                        totalCFRValue = purchaseInvoice.totalInvoiceAmount,
                        amountSOC = purchaseInvoice.POCFRValue,
                        amountME = purchaseInvoice.totalBaseAmount,
                        Parent_Id = (purchaseInvoice.purchaseOrder_Id != null ? purchaseInvoice.purchaseOrder_Id.ToString() + "_" + TransactionItemType.Purchase_Order.ToString() : "")
                    });
                }
            }
            if (companyBankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
            {
                foreach (var transfer in companyBankTransfers)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = transfer.Id.ToString() + "_" + TransactionItemType.InterCompanyBank_Transfer.ToString(),
                        TransactionType = TransactionItemType.InterCompanyBank_Transfer.ToString(),
                        Company = transfer.companyTo.CompanyName,
                        //Employee = transfer.employee.person.FName + " " + transfer.employee.person.FName,
                        CreationDate = transfer.CreationDate,
                        Currency = transfer.companyFrom.currency.CurrencyName,
                        Department = (transfer.departmentFrom.parentDepartment == null) ? transfer.departmentFrom.DeptName : transfer.departmentFrom.parentDepartment.DeptName + " " + transfer.departmentFrom.DeptName,
                        SalesReferenceNo = transfer.FinanceRefNo,
                        BackColor = transfer.interBankTransStatus.backcolor,
                        Status = transfer.interBankTransStatus.Status,
                        totalCFRValue = transfer.AmountFrom
                    });
                }
            }
            return allTransactions;
        }





        public List<AllOrdersView> getAllOrderTrackingTransactions(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {
            try
            {
                Inquiry inquiry = new Inquiry();
                Offer offer = new Offer();
                ERP_BL.Procurements.ModuleContract moduleContract = new ERP_BL.Procurements.ModuleContract();
                List<Offer> offers = new List<Offer>();
                List<ERP_BL.Procurements.ModuleContract> moduleContracts = new List<ERP_BL.Procurements.ModuleContract>();
                InterBankTransfer bankTransfer = new InterBankTransfer();
                InterCompanyBankTransfer companyBankTransfer = new InterCompanyBankTransfer();
                JournalVoucher voucher = new JournalVoucher();
                SaleOrder saleOrder = new SaleOrder();
                PurchaseOrder invoicedPurchaseOrder = new PurchaseOrder();
                List<SaleInvoice> saleInvoices = new List<SaleInvoice>();
                List<PurchaseInvoice> purchaseInvoices = new List<PurchaseInvoice>();
                PurchaseInvoice PInvoice = new PurchaseInvoice();
                List<MemorandumSale> memorandumSales = new List<MemorandumSale>();
                List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
                List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
                List<Bill> bills = new List<Bill>();
                Bill billl = new Bill();
                List<JournalVoucher> journalVouchers1 = new List<JournalVoucher>();
                List<JournalVoucher> vouchers = new List<JournalVoucher>();
                AdminBill adminBill = new AdminBill();
                List<AdminBill> adminBills = new List<AdminBill>();

                List<AssetRental> assetRentals = new List<AssetRental>();
                List<TenantRental> tenantRentals = new List<TenantRental>();
                List<RentalContract> rentalContracts = new List<RentalContract>();
                List<RentalOrder> rentalOrders = new List<RentalOrder>();
                List<RentalInvoice> rentalInvoices = new List<RentalInvoice>();

                LoansAdvance loansAdvance = new LoansAdvance();
                TargetRewards targetReward = new TargetRewards();
                List<LoansAdvance> loansAdvances = new List<LoansAdvance>();
                Payment payment = new Payment();
                List<Payment> payments = new List<Payment>();
                STL stl = new STL();
                List<STL> stls = new List<STL>();
                List<SaleOrder> saleOrders = new List<SaleOrder>();
                List<AdminBill> adminBillsInterBank = new List<AdminBill>();
                List<InterBankTransfer> bankTransfers = new List<InterBankTransfer>();
                List<InterCompanyBankTransfer> companyBankTransfers = new List<InterCompanyBankTransfer>();
                InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();
                JournalVoucherRepo voucherRepo = new JournalVoucherRepo();
                List<AllOrdersView> allTransactions = new List<AllOrdersView>();
                EmployeeRepo empRepo = new EmployeeRepo();
                ERP_BL.ToDoTasks.Taskss.Tasks task = new ToDoTasks.Taskss.Tasks();
                List<ERP_BL.ToDoTasks.Taskss.Tasks> tasks = new List<ERP_BL.ToDoTasks.Taskss.Tasks>();
                TaskRepo taskRepo = new TaskRepo();
                //Employee currEmp = new Employee();
                //currEmp = empRepo.GetEmployeeForPayments(empID);
                switch (type)
                {
                    case Enums.TransactionItemType.AssetRental:

                        if (assetRentals == null)
                            assetRentals = new List<AssetRental>();

                        var assetRental = context.assetRentals.FirstOrDefault(x => x.Id == Id);
                        if (assetRental != null)
                            assetRentals.Add(assetRental);

                        if (rentalContracts == null)
                            rentalContracts = new List<RentalContract>();

                        rentalContracts.AddRange(assetRentals.Where(a => a.RentalContracts?.Count > 0)
                                                             .SelectMany(a => a.RentalContracts));

                        if (rentalOrders == null)
                            rentalOrders = new List<RentalOrder>();

                        rentalOrders.AddRange(rentalContracts.Where(c => c.rentalOrders?.Count > 0)
                                                             .SelectMany(c => c.rentalOrders));

                        if (rentalInvoices == null)
                            rentalInvoices = new List<RentalInvoice>();

                        rentalInvoices.AddRange(rentalOrders.Where(o => o.rentalInvoices?.Count > 0)
                                                            .SelectMany(o => o.rentalInvoices));

                        if (tenantRentals == null)
                            tenantRentals = new List<TenantRental>();

                        tenantRentals.AddRange(rentalContracts.Where(c => c.tenantRental != null && !tenantRentals.Any(tr => tr.Id == c.tenantRentalId))
                                                               .Select(c => c.tenantRental));

                        if (saleReceipts == null)
                            saleReceipts = new List<SalesReceipt>();

                        saleReceipts.AddRange(rentalInvoices.Where(i => i.salesReceipts?.Count > 0)
                                                            .SelectMany(i => i.salesReceipts));

                        assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        break;


                    case Enums.TransactionItemType.TenantRental:

                        if (tenantRentals == null)
                            tenantRentals = new List<TenantRental>();

                        var tenantRental = context.tenantRentals.FirstOrDefault(x => x.Id == Id);
                        if (tenantRental != null)
                            tenantRentals.Add(tenantRental);

                        if (rentalContracts == null)
                            rentalContracts = new List<RentalContract>();

                        rentalContracts.AddRange(tenantRentals.Where(t => t.RentalContracts?.Count > 0)
                                                              .SelectMany(t => t.RentalContracts));

                        if (rentalOrders == null)
                            rentalOrders = new List<RentalOrder>();

                        rentalOrders.AddRange(rentalContracts.Where(c => c.rentalOrders?.Count > 0)
                                                             .SelectMany(c => c.rentalOrders));

                        if (rentalInvoices == null)
                            rentalInvoices = new List<RentalInvoice>();

                        rentalInvoices.AddRange(rentalOrders.Where(o => o.rentalInvoices?.Count > 0)
                                                            .SelectMany(o => o.rentalInvoices));

                        if (assetRentals == null)
                            assetRentals = new List<AssetRental>();

                        assetRentals.AddRange(rentalContracts.Where(c => c.assetRental != null && !assetRentals.Any(ar => ar.Id == c.assetRentalId))
                                                             .Select(c => c.assetRental));

                        if (saleReceipts == null)
                            saleReceipts = new List<SalesReceipt>();

                        saleReceipts.AddRange(rentalInvoices.Where(i => i.salesReceipts?.Count > 0)
                                                            .SelectMany(i => i.salesReceipts));

                        assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();


                        break;


                    case Enums.TransactionItemType.RentalContract:

                        if (rentalContracts == null)
                            rentalContracts = new List<RentalContract>();

                        var rentalContract = context.rentalContracts.FirstOrDefault(x => x.Id == Id);
                        if (rentalContract != null)
                            rentalContracts.Add(rentalContract);

                        if (rentalOrders == null)
                            rentalOrders = new List<RentalOrder>();

                        rentalOrders.AddRange(rentalContracts.Where(c => c.rentalOrders?.Count > 0)
                                                             .SelectMany(c => c.rentalOrders));

                        if (rentalInvoices == null)
                            rentalInvoices = new List<RentalInvoice>();

                        rentalInvoices.AddRange(rentalOrders.Where(o => o.rentalInvoices?.Count > 0)
                                                            .SelectMany(o => o.rentalInvoices));

                        if (assetRentals == null)
                            assetRentals = new List<AssetRental>();

                        assetRentals.AddRange(rentalContracts.Where(c => c.assetRental != null && !assetRentals.Any(ar => ar.Id == c.assetRentalId))
                                                             .Select(c => c.assetRental));

                        if (tenantRentals == null)
                            tenantRentals = new List<TenantRental>();

                        tenantRentals.AddRange(rentalContracts.Where(c => c.tenantRental != null && !tenantRentals.Any(tr => tr.Id == c.tenantRentalId))
                                                               .Select(c => c.tenantRental));

                        if (saleReceipts == null)
                            saleReceipts = new List<SalesReceipt>();

                        saleReceipts.AddRange(rentalInvoices.Where(i => i.salesReceipts?.Count > 0)
                                                            .SelectMany(i => i.salesReceipts));

                        assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();


                        break;

                    case Enums.TransactionItemType.RentalOrder:

                        if (rentalOrders == null)
                            rentalOrders = new List<RentalOrder>();

                        var rentalOrder = context.rentalOrders.FirstOrDefault(x => x.Id == Id);
                        if (rentalOrder != null)
                            rentalOrders.Add(rentalOrder);

                        if (rentalInvoices == null)
                            rentalInvoices = new List<RentalInvoice>();

                        rentalInvoices.AddRange(rentalOrders.Where(x => x.rentalInvoices?.Count > 0)
                                                            .SelectMany(x => x.rentalInvoices));

                        if (rentalContracts == null)
                            rentalContracts = new List<RentalContract>();

                        rentalContracts.AddRange(rentalOrders.Where(x => x.rentalContract != null && !rentalContracts.Any(rc => rc.Id == x.rentalContractId))
                                                             .Select(x => x.rentalContract));

                        if (assetRentals == null)
                            assetRentals = new List<AssetRental>();

                        assetRentals.AddRange(rentalContracts.Where(c => c.assetRental != null && !assetRentals.Any(ar => ar.Id == c.assetRentalId))
                                                             .Select(c => c.assetRental));

                        if (tenantRentals == null)
                            tenantRentals = new List<TenantRental>();

                        tenantRentals.AddRange(rentalContracts.Where(c => c.tenantRental != null && !tenantRentals.Any(tr => tr.Id == c.tenantRentalId))
                                                               .Select(c => c.tenantRental));

                        if (saleReceipts == null)
                            saleReceipts = new List<SalesReceipt>();

                        saleReceipts.AddRange(rentalInvoices.Where(x => x.salesReceipts?.Count > 0)
                                                            .SelectMany(x => x.salesReceipts));

                        assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        break;

                    case Enums.TransactionItemType.RentalInvoice:

                        if (rentalInvoices == null)
                            rentalInvoices = new List<RentalInvoice>();

                        var rentalInvoice = context.rentalInvoices.FirstOrDefault(x => x.Id == Id);
                        if (rentalInvoice != null)
                            rentalInvoices.Add(rentalInvoice);

                        if (rentalOrders == null)
                            rentalOrders = new List<RentalOrder>();

                        rentalOrders.AddRange(rentalInvoices.Where(x => x.rentalOrder != null && !rentalOrders.Any(ro => ro.Id == x.rentalOrderId))
                                                             .Select(x => x.rentalOrder));

                        if (rentalContracts == null)
                            rentalContracts = new List<RentalContract>();

                        rentalContracts.AddRange(rentalOrders.Where(x => x.rentalContract != null && !rentalContracts.Any(rc => rc.Id == x.rentalContractId))
                                                             .Select(x => x.rentalContract));

                        if (assetRentals == null)
                            assetRentals = new List<AssetRental>();

                        assetRentals.AddRange(rentalContracts.Where(c => c.assetRental != null && !assetRentals.Any(ar => ar.Id == c.assetRentalId))
                                                             .Select(c => c.assetRental));

                        if (tenantRentals == null)
                            tenantRentals = new List<TenantRental>();

                        tenantRentals.AddRange(rentalContracts.Where(c => c.tenantRental != null && !tenantRentals.Any(tr => tr.Id == c.tenantRentalId))
                                                               .Select(c => c.tenantRental));

                        if (saleReceipts == null)
                            saleReceipts = new List<SalesReceipt>();

                        saleReceipts.AddRange(rentalInvoices.Where(x => x.salesReceipts?.Count > 0)
                                                            .SelectMany(x => x.salesReceipts));

                        assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        break;

                    case Enums.TransactionItemType.InterCompanyBank_Transfer:
                        companyBankTransfer = context.interCompanyBankTransfers
                    .FirstOrDefault(x => x.Id == Id);

                        //}
                        if (companyBankTransfer.Id != 0)
                        {
                            companyBankTransfers.Add(companyBankTransfer);
                        }

                        if (companyBankTransfer.paymentGroupId != 0)
                        {
                             companyBankTransfers = context.interCompanyBankTransfers.Where(x => x.paymentGroupId == companyBankTransfer.paymentGroupId).ToList();
                        }
                        else
                        {
                            if (companyBankTransfer.receiptGroupId != 0)
                            {
                                companyBankTransfers = context.interCompanyBankTransfers.Where(x => x.receiptGroupId == companyBankTransfer.receiptGroupId).ToList();
                            }
                        }
                        if (companyBankTransfers.Count > 0)
                        {
                            foreach (var transfer in companyBankTransfers)
                            {
                                if (transfer.paymentGroupId != 0)
                                {
                                    payments = context.payments.Where(x => x.transactionGroupId == transfer.paymentGroupId).ToList();
                                    foreach (var paym in payments)
                                    {
                                        if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                                        {
                                            if (paym.AdminBill_Id != null)
                                            {
                                                Employee empUser = new Employee();
                                                empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                                adminBill = context.adminBills
                                                .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                                if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                                    adminBill = null;
                                            }
                                            else
                                            {
                                                adminBill = null;
                                            }
                                            if (adminBill != null)
                                            {
                                                payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                            }
                                            billl = null;
                                            PInvoice = null;
                                            saleOrder = null;
                                            offer = null;
                                            moduleContract = null;
                                            inquiry = null;
                                            payment = null;
                                            invoicedPurchaseOrder = null;
                                        }
                                        if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                                        {
                                            Employee empUser = new Employee();
                                            if (paym.Bill_Id != null)
                                            {

                                                empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                                billl = context.bills
                                                .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                                if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                                    billl = null;
                                            }
                                            else
                                            {
                                                billl = null;
                                            }

                                            if (billl != null)
                                            {

                                                if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                                {
                                                    saleOrder = context.saleOrders

                                                        .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                                    if (saleOrder != null)
                                                        if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                            saleOrder = null;
                                                }
                                                else
                                                {
                                                    saleOrder = null;
                                                }

                                                if (saleOrder == null)
                                                {
                                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                                    if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                                    {
                                                        purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                                        if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                            purchaseOrder = null;
                                                    }
                                                    else
                                                    {
                                                        purchaseOrder = null;
                                                    }

                                                    if (purchaseOrder != null)
                                                    {
                                                        saleOrder = context.saleOrders

                                                        .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                                        purchaseOrders.Add(purchaseOrder);
                                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                                    }
                                                }

                                                journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                                payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                                offer = context.offers
                                                        .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                              
                                            }
                                            else
                                            {
                                                saleInvoices = null;
                                                offer = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                purchaseOrders = context.purchaseOrders
                                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                                if (purchaseOrders.Count != 0)
                                                {
                                                    foreach (var purchaseOrder in purchaseOrders)
                                                    {
                                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                    }
                                                }
                                            }
                                            if (purchaseInvoices != null)
                                            {
                                                foreach (var _PI in purchaseInvoices)
                                                    if (_PI.Payments.Count > 0)
                                                        payments.AddRange(_PI.Payments);
                                            }
                                            if (offer != null)
                                            {
                                                inquiry = context.inquiries.FirstOrDefault(x => x.Id == offer.inquiry_Id);
                                            }
                                            else
                                            {
                                                inquiry = null;
                                            }
                                            if (saleInvoices != null)
                                            {
                                                saleReceipts = context.salesReceipts
                                              .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                            }
                                            else
                                            {
                                                saleReceipts = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                            }
                                            else if (offer != null)
                                            {
                                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                            }
                                            else
                                            {
                                                memorandumSales = null;
                                            }
                                            PInvoice = null;
                                            payment = null;
                                            invoicedPurchaseOrder = null;
                                            adminBill = null;
                                        }
                                        if (purchaseOrders != null)
                                        {
                                            if (purchaseOrders.Count != 0)
                                            {
                                                foreach (var purchaseOrder in purchaseOrders)
                                                {
                                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                                }
                                            }
                                        }
                                        if (offer.Id != 0)
                                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());

                                        if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                                        {

                                            if (paym.PInvoice_Id != null)
                                            {
                                                Employee empUser = new Employee();
                                                empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                                PInvoice = context.purchaseInvoices
                                                .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                                if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                                    PInvoice = null;
                                            }
                                            else
                                            {
                                                PInvoice = null;
                                            }
                                            if (PInvoice != null)
                                            {
                                                invoicedPurchaseOrder = context.purchaseOrders

                                                .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                                payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                            }
                                            else
                                            {
                                                invoicedPurchaseOrder = null;
                                            }
                                            if (invoicedPurchaseOrder != null)
                                            {
                                                saleOrder = context.saleOrders

                                                .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                                if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                                    purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                                if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                                {
                                                    purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                                }

                                                if (purchaseOrders != null)
                                                    foreach (var _po in purchaseOrders)
                                                        purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                                purchaseInvoices.Distinct();

                                                if (purchaseInvoices != null)
                                                    foreach (var _pi in purchaseInvoices)
                                                        payments.AddRange(_pi.Payments);

                                                payments.Distinct();
                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                            }
                                            else
                                            {
                                                saleInvoices = null;
                                                offer = null;
                                            }
                                            if (offer != null)
                                            {
                                                inquiry = context.inquiries
                                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                            }
                                            else
                                            {

                                                inquiry = null;
                                            }
                                            if (saleInvoices != null)
                                            {
                                                saleReceipts = context.salesReceipts
                                               .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                            }
                                            else
                                            {
                                                saleReceipts = null;
                                            }
                                            if (offer.Id != 0)
                                                moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());


                                            adminBill = null;
                                            billl = null;
                                            bills = null;
                                            //purchaseInvoices = null;
                                            payment = null;
                                            journalVouchers1 = null;
                                        }

                                    }
                                }
                                else
                                {
                                    saleReceipts = context.salesReceipts
                                   .Where(x => x.transactionGroupId == transfer.receiptGroupId).ToList();


                                }
                            }
                        }

                        break;
                    case Enums.TransactionItemType.InterBank_Transfer:
                        //bankTransfer = bankTransferRepo.GetInterBankTransfer(Id);

                        bankTransfer = context.interBankTransfers

                    .FirstOrDefault(x => x.Id == Id);
                        if (bankTransfer.adminBillId != null)
                        {
                            adminBill = context.adminBills
                           .FirstOrDefault(x => x.Id == bankTransfer.adminBillId);
                        }
                        if (bankTransfer.Id != 0)
                        {
                            bankTransfers.Add(bankTransfer);
                        }

                        if (bankTransfer.paymentGroupId != 0)
                        {
                            bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(bankTransfer.paymentGroupId, empID, (int)bankTransfer.company_Id);

                        }
                        else
                        {
                            if (bankTransfer.receiptGroupId != 0)
                            {
                                bankTransfers = bankTransferRepo.GetReceiptInterBankTransfers(bankTransfer.receiptGroupId, empID,(int) bankTransfer.company_Id);

                            }
                        }
                        if (bankTransfers.Count > 0)
                        {

                            foreach (var transfer in bankTransfers)
                            {
                                if (transfer.paymentGroupId != 0)
                                {
                                    payments = context.payments.Where(x => x.transactionGroupId == transfer.paymentGroupId).ToList();
                                    foreach (var paym in payments)
                                    {

                                        if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                                        {

                                            if (paym.AdminBill_Id != null)
                                            {
                                                Employee empUser = new Employee();
                                                empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                                adminBill = context.adminBills
                                                .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                                if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                                    adminBill = null;
                                            }
                                            else
                                            {
                                                adminBill = null;
                                            }
                                            if (adminBill != null)
                                            {
                                                payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                            }
                                            billl = null;
                                            PInvoice = null;
                                            saleOrder = null;
                                            offer = null;
                                            inquiry = null;
                                            payment = null;
                                            invoicedPurchaseOrder = null;

                                        }

                                        if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                                        {
                                            Employee empUser = new Employee();
                                            if (paym.Bill_Id != null)
                                            {

                                                empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                                billl = context.bills
                                                .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                                if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                                    billl = null;
                                            }
                                            else
                                            {
                                                billl = null;
                                            }

                                            if (billl != null)
                                            {

                                                if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                                {
                                                    saleOrder = context.saleOrders

                                                        .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                                    if (saleOrder != null)
                                                        if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                            saleOrder = null;
                                                }
                                                else
                                                {
                                                    saleOrder = null;
                                                }

                                                if (saleOrder == null)
                                                {
                                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                                    if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                                    {
                                                        purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                                        if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                            purchaseOrder = null;
                                                    }
                                                    else
                                                    {
                                                        purchaseOrder = null;
                                                    }

                                                    if (purchaseOrder != null)
                                                    {
                                                        saleOrder = context.saleOrders

                                                        .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);

                                                        purchaseOrders.Add(purchaseOrder);
                                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                        //}
                                                    }
                                                }

                                                journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                                payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                            }
                                            else
                                            {
                                                saleInvoices = null;
                                                offer = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                purchaseOrders = context.purchaseOrders

                                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                                if (purchaseOrders.Count != 0)
                                                {
                                                    foreach (var purchaseOrder in purchaseOrders)
                                                    {
                                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                    }
                                                }
                                            }
                                            if (purchaseInvoices != null)
                                            {
                                                foreach (var _PI in purchaseInvoices)
                                                    if (_PI.Payments.Count > 0)
                                                        payments.AddRange(_PI.Payments);
                                            }

                                            if (offer != null)
                                            {
                                                inquiry = context.inquiries
                                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                            }
                                            else
                                            {

                                                inquiry = null;
                                            }
                                            if (saleInvoices != null)
                                            {
                                                saleReceipts = context.salesReceipts
                                           .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                            }
                                            else
                                            {
                                                saleReceipts = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                            }
                                            else if (offer != null)
                                            {
                                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                            }
                                            else
                                            {
                                                memorandumSales = null;
                                            }

                                            PInvoice = null;
                                            //payments = null;
                                            payment = null;
                                            invoicedPurchaseOrder = null;
                                            adminBill = null;
                                        }
                                        if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                                        {

                                            if (paym.PInvoice_Id != null)
                                            {
                                                Employee empUser = new Employee();
                                                empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                                PInvoice = context.purchaseInvoices
                                                .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                                if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                                    PInvoice = null;
                                            }
                                            else
                                            {
                                                PInvoice = null;
                                            }
                                            if (PInvoice != null)
                                            {
                                                invoicedPurchaseOrder = context.purchaseOrders
                                                .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);
                                                payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                            }
                                            else
                                            {
                                                invoicedPurchaseOrder = null;
                                            }
                                            if (invoicedPurchaseOrder != null)
                                            {
                                                saleOrder = context.saleOrders.FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                                                if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                                    purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());
                                                if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                                {
                                                    purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                                }
                                                if (purchaseOrders != null)
                                                    foreach (var _po in purchaseOrders)
                                                        purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());
                                                purchaseInvoices.Distinct();
                                                if (purchaseInvoices != null)
                                                    foreach (var _pi in purchaseInvoices)
                                                        payments.AddRange(_pi.Payments);
                                                payments.Distinct();
                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }
                                            if (saleOrder != null)
                                            {
                                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                            }
                                            else
                                            {
                                                saleInvoices = null;
                                                offer = null;
                                            }
                                            if (offer != null)
                                            {
                                                inquiry = context.inquiries
                                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                            }
                                            else
                                            {

                                                inquiry = null;
                                            }
                                            if (saleInvoices != null)
                                            {
                                                saleReceipts = context.salesReceipts
                                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                            }
                                            else
                                            {
                                                saleReceipts = null;
                                            }



                                            adminBill = null;
                                            billl = null;
                                            bills = null;
                                            //purchaseInvoices = null;
                                            payment = null;
                                            journalVouchers1 = null;
                                        }

                                    }
                                }
                                else
                                {
                                    saleReceipts = context.salesReceipts
                        .Where(x => x.transactionGroupId == transfer.receiptGroupId).ToList();


                                }
                            }
                        }
                        if (bankTransfer.vendorBillId != null)
                        {
                            billl = bankTransfer.Bill;
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (bills == null)
                                    {
                                        bills = new List<Bill>();
                                    }
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null)
                        {
                            if (offer.Id != 0)
                                moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        }
                        break;



                    case Enums.TransactionItemType.JV:
                        voucher = voucherRepo.GetJournalVoucherById(Id);
                        if (voucher.paymentGroupId != 0)
                        {
                            vouchers = voucherRepo.GetVouchersByGroupId(voucher.paymentGroupId);

                            foreach (var paym in payments)
                            {

                                if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                                {

                                    if (paym.AdminBill_Id != null)
                                    {
                                        Employee empUser = new Employee();
                                        empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                        adminBill = context.adminBills
                                        .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                        if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                            adminBill = null;
                                    }
                                    else
                                    {
                                        adminBill = null;
                                    }
                                    if (adminBill != null)
                                    {
                                        payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                    }
                                    billl = null;
                                    PInvoice = null;
                                    saleOrder = null;
                                    offer = null;
                                    inquiry = null;
                                    payment = null;
                                    invoicedPurchaseOrder = null;

                                }

                                if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                                {
                                    Employee empUser = new Employee();
                                    if (paym.Bill_Id != null)
                                    {

                                        empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                        billl = context.bills
                                        .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                        if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                            billl = null;
                                    }
                                    else
                                    {
                                        billl = null;
                                    }

                                    if (billl != null)
                                    {

                                        if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                        {
                                            saleOrder = context.saleOrders

                                                .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                            if (saleOrder != null)
                                                if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                    saleOrder = null;
                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }

                                        if (saleOrder == null)
                                        {
                                            PurchaseOrder purchaseOrder = new PurchaseOrder();
                                            if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                            {
                                                purchaseOrder = context.purchaseOrders
                                        .FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                                if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                    purchaseOrder = null;
                                            }
                                            else
                                            {
                                                purchaseOrder = null;
                                            }

                                            if (purchaseOrder != null)
                                            {
                                                saleOrder = context.saleOrders

                                                .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);

                                                purchaseOrders.Add(purchaseOrder);
                                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                //}
                                            }
                                        }

                                        journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                        payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                    }
                                    else
                                    {
                                        saleOrder = null;
                                    }
                                    if (saleOrder != null)
                                    {
                                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                        offer = context.offers
                                                .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                    }
                                    else
                                    {
                                        saleInvoices = null;
                                        offer = null;
                                    }
                                    if (saleOrder != null)
                                    {
                                        purchaseOrders = context.purchaseOrders
                                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                        if (purchaseOrders.Count != 0)
                                        {
                                            foreach (var purchaseOrder in purchaseOrders)
                                            {
                                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                            }
                                        }
                                    }
                                    if (purchaseInvoices != null)
                                    {
                                        foreach (var _PI in purchaseInvoices)
                                            if (_PI.Payments.Count > 0)
                                                payments.AddRange(_PI.Payments);
                                    }
                                    //else
                                    //{
                                    //    purchaseOrders = null;
                                    //}

                                    if (offer != null)
                                    {
                                        inquiry = context.inquiries
                                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                    }
                                    else
                                    {

                                        inquiry = null;
                                    }
                                    if (saleInvoices != null)
                                    {
                                        saleReceipts = context.salesReceipts
                                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                    }
                                    else
                                    {
                                        saleReceipts = null;
                                    }
                                    if (saleOrder != null)
                                    {
                                        memorandumSales = context.memorandumSales
                                   .Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                    }
                                    else if (offer != null)
                                    {
                                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                    }
                                    else
                                    {
                                        memorandumSales = null;
                                    }

                                    PInvoice = null;
                                    //payments = null;
                                    payment = null;
                                    invoicedPurchaseOrder = null;
                                    adminBill = null;
                                }
                                if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                                {

                                    if (paym.PInvoice_Id != null)
                                    {
                                        Employee empUser = new Employee();
                                        empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                        PInvoice = context.purchaseInvoices
                                        .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                        if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                            PInvoice = null;
                                    }
                                    else
                                    {
                                        PInvoice = null;
                                    }
                                    if (PInvoice != null)
                                    {
                                        invoicedPurchaseOrder = context.purchaseOrders

                                        .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                        payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                    }
                                    else
                                    {
                                        invoicedPurchaseOrder = null;
                                    }
                                    if (invoicedPurchaseOrder != null)
                                    {
                                        saleOrder = context.saleOrders

                                        .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                        if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                            purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                        if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                        {
                                            purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                        }

                                        if (purchaseOrders != null)
                                            foreach (var _po in purchaseOrders)
                                                purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                        purchaseInvoices.Distinct();

                                        if (purchaseInvoices != null)
                                            foreach (var _pi in purchaseInvoices)
                                                payments.AddRange(_pi.Payments);

                                        payments.Distinct();
                                    }
                                    else
                                    {
                                        saleOrder = null;
                                    }
                                    if (saleOrder != null)
                                    {
                                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                    }
                                    else
                                    {
                                        saleInvoices = null;
                                        offer = null;
                                    }
                                    if (offer != null)
                                    {
                                        inquiry = context.inquiries
                                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                                    }
                                    else
                                    {

                                        inquiry = null;
                                    }
                                    if (saleInvoices != null)
                                    {
                                        saleReceipts = context.salesReceipts
                                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                    }
                                    else
                                    {
                                        saleReceipts = null;
                                    }
                                    adminBill = null;
                                    billl = null;
                                    bills = null;

                                    payment = null;
                                    journalVouchers1 = null;
                                }
                            }
                        }
                        else
                        if (voucher.receiptGroupId != 0)
                        {
                            vouchers = voucherRepo.GetVouchersByReceiptGroupId(voucher.receiptGroupId);
                            saleReceipts = context.salesReceipts
                       .Where(x => x.transactionGroupId == voucher.receiptGroupId).ToList();

                        }
                        payments.AddRange(context.payments
                            .Where(x => x.transactionGroupId == voucher.paymentGroupId).ToList());
                        payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        break;
                    case Enums.TransactionItemType.Inquiry:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        inquiry = context.inquiries
                            .FirstOrDefault(x => x.Id == Id);
                        if (inquiry != null)
                        {
                            offer = context.offers
                            .FirstOrDefault(x => x.inquiry_Id == Id);
                        }
                        else
                        {
                            offer = null;
                        }
                        if (offer != null)
                        {

                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                            if (moduleContracts.Count > 0)
                            {
                                foreach (var contract in moduleContracts)
                                {
                                    saleOrder = context.saleOrders.FirstOrDefault(x => x.moduleContract_Id == contract.Id);
                                }
                            }
                            else
                            {
                                saleOrder = context.saleOrders

                                .FirstOrDefault(x => x.offer_Id == offer.Id);
                            }

                       
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        }
                        else
                        {
                            saleInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders

                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count != 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count != 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    companyBankTransfers.AddRange(context.interCompanyBankTransfers.Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers

                                   .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)

                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer!=null)
                        if (offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());

                        break;
                    //Offer
                    case Enums.TransactionItemType.Offer:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        offer = context.offers.FirstOrDefault(x => x.Id == Id);

                        if (offer.offertype == InquiryType.DistributionBiz)
                        {
                            offers = context.offers.Where(x => x.Id == offer.Id).ToList();
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                            if (moduleContracts.Count > 0)
                            {
                                foreach (var contract in moduleContracts)
                                {
                                    saleOrder = context.saleOrders.FirstOrDefault(x => x.moduleContract_Id == contract.Id);
                                }
                            }
                            else
                            {
                                saleOrder = context.saleOrders

                                .FirstOrDefault(x => x.offer_Id == offer.Id);
                            }
                            if (saleOrder!=null)
                            tasks.AddRange(context.tasks.Where(x => x.saleOrderId == saleOrder.Id).ToList());

                        }
                        else
                        {
                            saleOrder = null;
                            inquiry = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        }
                        else
                        {
                            saleInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                            if (purchaseInvoices.Count != 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count != 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    companyBankTransfers.AddRange(context.interCompanyBankTransfers

                                  .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                         .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));


                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);

                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers
                                  .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)

                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                    tasks.AddRange(context.tasks.Where(x => x.purchaseOrderId == purchaseOrder.Id).ToList());
                                }
                            }
                        }
                        tasks.AddRange(context.tasks.Where(x => x.offerId == offer.Id).ToList());
                     
                        break;
                    case Enums.TransactionItemType.Tasks:
                        task = context.tasks.FirstOrDefault(x => x.Id == Id);
                        if(task.offerId!=null)
                        {
                            tasks.AddRange(context.tasks.Where(x => x.offerId == (int)task.offerId).ToList());
                        }
                        if (task.saleOrderId != null)
                        {
                            adminBill = null;
                            payment = null;
                            billl = null;
                            payments = null;
                            PInvoice = null;
                            invoicedPurchaseOrder = null;
                            saleOrder = context.saleOrders
                            .FirstOrDefault(x => x.Id == task.saleOrderId);
                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();

                                if (saleOrder.moduleContract_Id == null)
                                {
                                    offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                }
                                else
                                {
                                    moduleContract = context.ModuleContracts.FirstOrDefault(x => x.Id == saleOrder.moduleContract_Id);
                                    offer = context.offers.FirstOrDefault(x => x.Id == moduleContract.Id);
                                    moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                                    if (moduleContracts.Count > 0)
                                    {
                                        foreach (var contract in moduleContracts)
                                        {
                                            saleOrder = context.saleOrders.FirstOrDefault(x => x.moduleContract_Id == contract.Id);
                                        }
                                    }
                                    else
                                    {
                                        saleOrder = context.saleOrders

                                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                                    }
                                }
                                
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders
                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                        if (_tasks.Count> 0)
                                            tasks.AddRange(_tasks);
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }
                                if (purchaseInvoices.Count > 0)
                                {
                                    payments = new List<Payment>();
                                    foreach (var _PI in purchaseInvoices)
                                    {
                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments.Distinct());
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }
                                }
                            }
                            else
                            {
                                purchaseOrders = null;
                            }

                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            else
                            {
                                bills = null;
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                            }
                            else
                            {

                                inquiry = null;
                            }

                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                            .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                foreach (var recpt in saleReceipts)
                                {
                                    var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (trans == null)
                                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                                }
                                foreach (var recpt in saleReceipts)
                                {
                                    var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (vouch == null)
                                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                }

                                foreach (var SI in saleInvoices)
                                {
                                    var _tasks = taskRepo.GetSITask(SI.Id, true);
                                    if (_tasks.Count> 0)
                                        tasks.AddRange(_tasks);
                                }
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }

                            if (bills != null && bills.Count > 0)
                            {
                                if (payments == null)
                                    payments = new List<Payment>();
                                foreach (var __bill in bills)
                                {
                                    if (__bill.Payments != null && __bill.Payments.Count > 0)
                                    {
                                        payments.AddRange(__bill.Payments);

                                    }
                                }
                            }
                            if (payments != null && payments.Count > 0)
                                payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        }
                        else if (task.saleInvoiceId != null)
                        {
                            adminBill = null;
                            payment = null;
                            billl = null;
                            payments = null;
                            PInvoice = null;
                            invoicedPurchaseOrder = null;
                            var invoicee = context.saleInvoices.FirstOrDefault(x => x.Id == task.saleInvoiceId);
                            if (invoicee != null)
                            {
                                saleOrder = context.saleOrders

                                .FirstOrDefault(x => x.Id == invoicee.SaleOrderId);
                            }
                            else
                            {
                                saleOrder = null;
                            }

                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders
                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {

                                        var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                        if (_tasks.Count > 0)
                                            tasks.AddRange(_tasks);

                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }
                                if (purchaseInvoices.Count > 0)
                                {
                                    payments = new List<Payment>();
                                    foreach (var _PI in purchaseInvoices)
                                    {
                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments.Distinct());
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }
                                }
                            }
                            else
                            {
                                purchaseOrders = null;
                            }
                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            else
                            {
                                bills = null;
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                            }
                            else
                            {
                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                                .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                foreach (var recpt in saleReceipts)
                                {
                                    var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (trans == null)
                                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                                }
                                foreach (var recpt in saleReceipts)
                                {
                                    var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (vouch == null)
                                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                }
                                foreach (var SI in saleInvoices)
                                {
                                    var _tasks = taskRepo.GetSITask(SI.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                }
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }
                            if (saleOrder != null)
                            {
                                var _tasks = taskRepo.GetSOTask(saleOrder.Id, true);
                                if (_tasks.Count > 0)
                                    tasks.AddRange(_tasks);
                            }

                        }
                        else if (task.purchaseOrderId != null)
                        {
                            adminBill = null;
                            payment = null;
                            billl = null;
                            payments = null;
                            PInvoice = null;
                            invoicedPurchaseOrder = null;
                            var PurchaseOrderr = context.purchaseOrders

                                .FirstOrDefault(x => x.Id == task.purchaseOrderId);
                            if (PurchaseOrderr != null)
                            {
                                saleOrder = context.saleOrders
                                .FirstOrDefault(x => x.Id == PurchaseOrderr.saleOrder_Id);
                            }
                            else
                            {
                                saleOrder = null;
                                purchaseInvoices = null;
                            }
                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();

                                if (bills.Count > 0)
                                {
                                    payments = new List<Payment>();
                                    foreach (var _bill in bills)
                                    {
                                        if (_bill.Payments.Count > 0)
                                        {
                                            payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                        }
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }
                                }
                            }
                            else if (PurchaseOrderr != null)
                            {
                                bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrderr.Id)).ToList();

                                if (bills.Count > 0)
                                {
                                    payments = new List<Payment>();
                                    foreach (var _bill in bills)
                                    {
                                        if (_bill.Payments.Count > 0)
                                            payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                        //payments.AddRange(_bill.Payments.Distinct());
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }
                                }
                            }
                            else
                            {
                                bills = null;
                            }
                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders
                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }

                                if (purchaseInvoices.Count > 0)
                                {
                                    foreach (var _PI in purchaseInvoices)
                                    {
                                        if (payments == null)
                                            payments = new List<Payment>();

                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments.Distinct());
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }

                                }
                            }
                            else if (PurchaseOrderr != null)
                            {
                                purchaseOrders.Add(PurchaseOrderr);
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                        if (_tasks.Count > 0)
                                            tasks.AddRange(_tasks);
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }
                                if (purchaseInvoices.Count > 0)
                                {
                                    if (payments == null)
                                        payments = new List<Payment>();

                                    foreach (var _PI in purchaseInvoices)
                                    {
                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments.Distinct());
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }
                                }
                            }
                            else
                            {
                                purchaseOrders = null;
                                purchaseInvoices = null;
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                            }
                            else
                            {

                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                               .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                                foreach (var recpt in saleReceipts)
                                {
                                    var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (trans == null)
                                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                                }
                                foreach (var recpt in saleReceipts)
                                {
                                    var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (vouch == null)
                                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                }
                                foreach (var SI in saleInvoices)
                                {
                                    var _tasks = taskRepo.GetSITask(SI.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                }
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }

                            if (PurchaseOrderr != null)
                                journalVouchers1 = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrderr.Id).ToList();

                            if (saleOrder != null)
                            {
                                var _tasks = taskRepo.GetSOTask(saleOrder.Id, true);
                                if (_tasks.Count > 0)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else if (task.offerId != null)
                        {
                            adminBill = null;
                            payment = null;
                            billl = null;
                            payments = null;
                            PInvoice = null;
                            invoicedPurchaseOrder = null;
                            offer = context.offers.FirstOrDefault(x => x.Id == task.offerId);

                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);




                                moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                                if (moduleContracts.Count > 0)
                                {
                                    foreach (var contract in moduleContracts)
                                    {
                                        saleOrder = context.saleOrders.FirstOrDefault(x => x.moduleContract_Id == contract.Id);
                                    }
                                }
                                else
                                {
                                    saleOrder = context.saleOrders

                                    .FirstOrDefault(x => x.offer_Id == offer.Id);
                                }
                            }
                            else
                            {
                                saleOrder = null;
                                inquiry = null;
                            }
                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            }
                            else
                            {
                                saleInvoices = null;
                            }
                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            else
                            {
                                bills = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders
                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                        var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                        if (_tasks.Count > 0)
                                            tasks.AddRange(_tasks);
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }

                                if (purchaseInvoices.Count != 0)
                                {
                                    payments = new List<Payment>();
                                    foreach (var _PI in purchaseInvoices)
                                    {
                                        if (_PI.Payments.Count != 0)
                                            payments.AddRange(_PI.Payments.Distinct());
                                    }
                                }
                            }
                            else
                            {
                                purchaseOrders = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                               .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                                foreach (var SI in saleInvoices)
                                {
                                    var _tasks = taskRepo.GetSITask(SI.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                }
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)

                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }
                            if (saleOrder != null)
                            {
                                var _tasks = taskRepo.GetSOTask(saleOrder.Id, true);
                                if (_tasks.Count> 0)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        if (tasks != null && tasks.Count > 0)
                        {
                            tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            if (task != null && task.Id != 0 && tasks.FirstOrDefault(x => x.Id == task.Id) != null)
                            {
                                var itemToRemove = tasks.Single(r => r.Id == task.Id);
                                tasks.Remove(itemToRemove);
                            }
                        }
                        if (purchaseOrders != null)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                            }
                        }
                        if (task.offerId != null)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == task.offerId).ToList());
                        break;

                    case Enums.TransactionItemType.ModuleContract:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        moduleContract = context.ModuleContracts.FirstOrDefault(x => x.Id == Id);
                        moduleContracts.AddRange(context.ModuleContracts.Where(x => x.Id == Id).ToList());
                        offer = context.offers.FirstOrDefault(x=>x.Id==moduleContract.offer_Id);
                        offers.Add(moduleContract.offer);
                        inquiry = context.inquiries.FirstOrDefault(x=>x.Id==offer.inquiry_Id);
                        saleOrders = context.saleOrders.Where(x => x.moduleContract_Id == Id).ToList();
                        foreach(var so in saleOrders)
                        {
                            saleInvoices.AddRange(so.SaleInvoices);
                            foreach(var inv in saleInvoices)
                            {
                                saleReceipts.AddRange(inv.salesReceipts);
                            }
                            var _taskks = taskRepo.GetSOTask(so.Id, true);
                            if (_taskks.Count > 0)
                                tasks.AddRange(_taskks);

                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(so.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    companyBankTransfers.AddRange(context.interCompanyBankTransfers


                                    .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                                bills.AddRange(context.bills.Where(x => x.saleOrder_Id == (int?)(so.Id)).ToList());
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }


                            saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(so.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers


                                  .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            }
                            foreach (var _SI in saleInvoices)
                            {
                                var _tasks = taskRepo.GetSITask(_SI.Id, true);
                                if (_tasks.Count > 0)
                                    tasks.AddRange(_tasks);
                            }
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == so.Id).ToList();
                            if (payments == null)
                                payments = new List<Payment>();
                            foreach (var __bill in bills)
                            {
                                if (__bill.Payments != null && __bill.Payments.Count > 0)
                                {
                                    payments.AddRange(__bill.Payments);
                                }
                            }
                        }
                        if (payments != null && payments.Count > 0)
                            payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        if (tasks != null && tasks.Count > 0)
                        {
                            tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        } 
                        if (moduleContracts != null && moduleContracts.Count > 0)
                        {
                            moduleContracts = moduleContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        }



                        break;


                    //SaleOrder
                    case Enums.TransactionItemType.Sale_Order:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == Id);
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            if(saleOrder.moduleContract!=null)
                            moduleContracts.Add(saleOrder.moduleContract);

                            loansAdvances = saleOrder.LoansAdvances;
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            var _taskks = taskRepo.GetSOTask(saleOrder.Id, true);
                            if (_taskks.Count > 0)
                                tasks.AddRange(_taskks);

                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id ));
                                    companyBankTransfers.AddRange(context.interCompanyBankTransfers


                                    .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }

                        if (saleOrder != null)
                        {
                            bills.AddRange(context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList());
                        }
                        else
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                            else
                            {
                                bills = null;
                            }
                        }

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                             .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers


                                  .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            }
                            foreach (var _SI in saleInvoices)
                            {
                                var _tasks = taskRepo.GetSITask(_SI.Id, true);
                                if (_tasks.Count > 0)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        if (bills != null && bills.Count > 0)
                        {
                            if (payments == null)
                                payments = new List<Payment>();
                            foreach (var __bill in bills)
                            {
                                if (__bill.Payments != null && __bill.Payments.Count > 0)
                                {
                                    payments.AddRange(__bill.Payments);
                                }
                            }
                        }
                        if (payments != null && payments.Count > 0)
                        {
                            payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                            foreach(var _pymnt in payments)
                            {
                                if (saleReceipts == null)
                                    saleReceipts = new List<SalesReceipt>();

                                saleReceipts.AddRange(_pymnt.salesReceipts);
                            }

                            if (saleReceipts != null && saleReceipts.Count > 0)
                                saleReceipts = saleReceipts.GroupBy(x => x.Id).Select(y => y.First()).ToList();    
                        }
                            



                        if (tasks != null && tasks.Count > 0)
                        {
                            tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        }
                        if (offer != null&& offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;
                    //Sale Invoices
                    case Enums.TransactionItemType.Sale_Invoice:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var invoice = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
                        if (invoice != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == invoice.SaleOrderId);
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if(invoice.LoansAdvances.Count>0)
                        {
                            loansAdvances.AddRange(invoice.LoansAdvances);
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                            var _taskks = taskRepo.GetSOTask(saleOrder.Id, true);
                            if (_taskks.Count > 0)
                                tasks.AddRange(_taskks);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    companyBankTransfers.AddRange(context.interCompanyBankTransfers



                                            .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());
                                }
                            }

                        }
                        else
                        {
                            purchaseOrders = null;
                        }


                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                            .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers

                           .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            }

                            foreach (var _SI in saleInvoices)
                            {
                                var _tasks = taskRepo.GetSITask(_SI.Id, true);
                                if (_tasks.Count > 0)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        if (tasks != null && tasks.Count > 0)
                        {
                            tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;
                    //Sale Invoices
                    case Enums.TransactionItemType.Sale_Receipt:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;

                        var receipt = context.salesReceipts.FirstOrDefault(x => x.Id == Id);
                        if (receipt.receiptType == ReceiptType.Rental_Receipt)
                        {
                            if (saleReceipts == null)
                                saleReceipts = new List<SalesReceipt>();

                            saleReceipts.Add(receipt);

                            foreach (var _receipt in saleReceipts)
                            {
                                if (_receipt.rentalInvoice != null)
                                {
                                    if (rentalInvoices == null)
                                        rentalInvoices = new List<RentalInvoice>();

                                    rentalInvoices.Add(_receipt.rentalInvoice);
                                }
                            }

                            rentalInvoices = rentalInvoices.Distinct().ToList();

                            foreach (var _rentalInvoice in rentalInvoices)
                            {
                                if (_rentalInvoice.rentalOrder != null)
                                {
                                    if (rentalOrders == null)
                                        rentalOrders = new List<RentalOrder>();

                                    rentalOrders.Add(_rentalInvoice.rentalOrder);
                                }
                            }

                            foreach (var _rentalOrder in rentalOrders)
                            {
                                if (_rentalOrder.rentalContract != null)
                                {
                                    if (rentalContracts == null)
                                        rentalContracts = new List<RentalContract>();

                                    rentalContracts.Add(_rentalOrder.rentalContract);
                                }
                            }

                            foreach (var _rentalContract in rentalContracts)
                            {
                                if (_rentalContract.assetRental != null)
                                {
                                    if (assetRentals == null)
                                        assetRentals = new List<AssetRental>();

                                    assetRentals.Add(_rentalContract.assetRental);
                                }
                            }

                            assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        }
                        else if(receipt.receiptType == ReceiptType.Loans_Advances)
                        {
                            loansAdvances.Add(receipt.loansAdvance);
                            PaymentRepo paymentRepo = new PaymentRepo();
                            payments = new List<Payment>();
                            payments.AddRange(paymentRepo.getPaymentsByLAid((int)receipt.LoansAdvanceId, empID));
                            foreach (var pymnt in payments.ToList())
                            {
                                if (pymnt.loansAdvance != null)
                                {
                                    loansAdvances.Add(pymnt.loansAdvance);
                                    if (pymnt.loansAdvance.SalesReceipts != null && pymnt.loansAdvance.SalesReceipts.Count > 0)
                                        saleReceipts.AddRange(pymnt.loansAdvance.SalesReceipts);
                                    if (pymnt.loansAdvance.Payments != null && pymnt.loansAdvance.Payments.Count > 0)
                                        payments.AddRange(pymnt.loansAdvance.Payments);
                                }
                            }
                            if (saleReceipts.Find(x => x.Id == receipt.Id) != null)
                                saleReceipts.Remove(receipt);
                            saleReceipts = saleReceipts.Distinct().ToList();
                            foreach (var _receipt in saleReceipts)
                            {
                                if (_receipt.loansAdvance != null)
                                    loansAdvances.Add(_receipt.loansAdvance);

                                if (_receipt.rentalInvoice != null)
                                {
                                    if (rentalInvoices == null)
                                        rentalInvoices = new List<RentalInvoice>();

                                    rentalInvoices.Add(_receipt.rentalInvoice);
                                }
                            }
                            payments = payments.Distinct().ToList();
                            loansAdvances = loansAdvances.Distinct().ToList();
                            rentalInvoices = rentalInvoices.Distinct().ToList();

                            foreach (var _rentalInvoice in rentalInvoices)
                            {
                                if (_rentalInvoice.rentalOrder != null)
                                {
                                    if (rentalOrders == null)
                                        rentalOrders = new List<RentalOrder>();

                                    rentalOrders.Add(_rentalInvoice.rentalOrder);
                                }
                            }

                            foreach (var _rentalOrder in rentalOrders)
                            {
                                if (_rentalOrder.rentalContract != null)
                                {
                                    if (rentalContracts == null)
                                        rentalContracts = new List<RentalContract>();

                                    rentalContracts.Add(_rentalOrder.rentalContract);
                                }
                            }

                            foreach (var _rentalContract in rentalContracts)
                            {
                                if (_rentalContract.assetRental != null)
                                {
                                    if (assetRentals == null)
                                        assetRentals = new List<AssetRental>();

                                    assetRentals.Add(_rentalContract.assetRental);
                                }
                            }

                            assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            rentalContracts = rentalContracts.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            rentalOrders = rentalOrders.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                            rentalInvoices = rentalInvoices.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        }
                        else if (receipt.receiptType != ReceiptType.Direct_Receipt)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(receipt.transactionGroupId, empID, (int)receipt.companyId));
                            vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(receipt.transactionGroupId));
                            companyBankTransfers.AddRange(context.interCompanyBankTransfers
                                .Where(x => x.receiptGroupId == receipt.transactionGroupId).ToList());

                            if (receipt != null)
                            {
                                saleOrder = context.saleOrders
                                .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                            }
                            else
                            {
                                saleOrder = null;
                            }

                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders
                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }
                                if (purchaseInvoices.Count > 0)
                                {
                                    payments = new List<Payment>();
                                    foreach (var _PI in purchaseInvoices)
                                    {
                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments.Distinct());
                                    }
                                    foreach (var paym in payments)
                                    {
                                        bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                    }
                                    foreach (var paym in payments)
                                    {
                                        vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                    }
                                }
                            }
                            else
                            {
                                purchaseOrders = null;
                            }
                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            else
                            {
                                bills = null;
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                            }
                            else
                            {

                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                              .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }
                        }
                        else if(receipt.receiptType == ReceiptType.Direct_Receipt && receipt.payment != null)
                        {
                            adminBill = null;
                            billl = null;
                            payments = null;
                            PInvoice = null;
                            invoicedPurchaseOrder = null;


                            payment = receipt.payment;
                        }
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;

                    case Enums.TransactionItemType.Admin_Bill:

                        adminBill = context.adminBills
                            .FirstOrDefault(x => x.Id == Id);

                        if (adminBill.InterBankTransfers.Count > 0)
                        {
                            bankTransfers = adminBill.InterBankTransfers;
                        }
                        AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
                        if (adminBill.Payments != null)
                        {
                            var paymentss = adminBill.Payments;

                            PaymentRepo paymentRepo = new PaymentRepo();
                            foreach (var _paymnt in paymentss)
                            {
                                payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                            }
                            foreach (var paym in payments)
                            {

                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                if (paym.LoansAdvanceId != null)
                                    loansAdvances.Add(loansAdvanceRepo.GetLoansAdvance(paym.LoansAdvanceId.Value));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                        else
                        {
                            payments = null;
                        }
                        if (adminBill.LoansAdvanceId != null)
                        {
                            loansAdvances.Add(loansAdvanceRepo.GetLoansAdvance(adminBill.LoansAdvanceId.Value));
                        }
                        if (loansAdvances != null && loansAdvances.Count > 0)
                        {
                            foreach (var _LA in loansAdvances)
                            {
                                if (_LA.AdminBills != null && _LA.AdminBills.Count > 0)
                                    adminBills.AddRange(_LA.AdminBills);
                            }
                        }
                        if (adminBills != null && adminBills.Count > 0)
                        {
                            foreach (var _adminBill in adminBills)
                            {
                                if (_adminBill.Payments != null && _adminBill.Payments.Count > 0)
                                    payments.AddRange(_adminBill.Payments);
                                if (_adminBill.assetRentalId != null)
                                {
                                    if (assetRentals == null)
                                        assetRentals = new List<AssetRental>();
                                    assetRentals.Add(context.assetRentals.FirstOrDefault(x => x.Id == _adminBill.assetRentalId));
                                }
                            }
                        }
                        if (adminBills.FirstOrDefault(x => x.Id == adminBill.Id) != null)
                            adminBills.Remove(adminBills.FirstOrDefault(x => x.Id == adminBill.Id));

                        assetRentals = assetRentals.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        adminBills = adminBills.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        loansAdvances = loansAdvances.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        payment = null;
                        billl = null;
                        inquiry = null;
                        offer = null;
                        saleOrder = null;
                        saleInvoices = null;
                        purchaseOrders = null;
                        bills = null;
                        purchaseInvoices = null;
                        journalVouchers1 = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        break;

                    case Enums.TransactionItemType.LoansAdvances:

                        loansAdvance = context.loansAdvances
                            .Include("company")
                            .Include("department")
                            .FirstOrDefault(x => x.Id == Id);
                        if (loansAdvance.Payments != null)
                        {
                            var paymentss = loansAdvance.Payments;

                            PaymentRepo paymentRepo = new PaymentRepo();
                            foreach (var _paymnt in paymentss)
                            {
                                payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                        else
                        {
                            payments = null;
                        }
                        if(loansAdvance.saleOrder != null)
                        {
                            saleOrder = loansAdvance.saleOrder;
                        }
                        if (loansAdvance.purchaseOrder != null)
                        {
                            if (purchaseOrders == null)
                                purchaseOrders = new List<PurchaseOrder>();
                            purchaseOrders.Add(loansAdvance.purchaseOrder);
                        }
                        if (loansAdvance.SalesReceipts != null)
                        {
                            var saleReceiptss = loansAdvance.SalesReceipts;

                            SalesReceiptRepo saleReceiptRepo = new SalesReceiptRepo();
                            foreach (var _receipt in saleReceiptss)
                            {
                                saleReceipts.Add(saleReceiptRepo.GetSalesReceipt(_receipt.Id));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (loansAdvance.AdminBills != null)
                        {
                            adminBills = loansAdvance.AdminBills;
                            PaymentRepo paymentRepo = new PaymentRepo();
                            foreach (var _adminBill in adminBills)
                            {
                                if (_adminBill.Payments != null && _adminBill.Payments.Count > 0)
                                    payments.AddRange(_adminBill.Payments);
                            }
                        }
                        payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();

                        if (loansAdvance.bills != null)
                        {
                            bills = loansAdvance.bills;
                        }

                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        adminBill = null;
                        payment = null;
                        billl = null;
                        inquiry = null;
                        offer = null;
                        //saleOrder = null;
                        saleInvoices = null;
                        //purchaseOrders = null;
                        //bills = null;
                        purchaseInvoices = null;
                        journalVouchers1 = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        break;

                    case Enums.TransactionItemType.TargetReward:

                        targetReward = context.targetRewards
                            .FirstOrDefault(x => x.Id == Id);
                        if (targetReward.Payments != null)
                        {
                            var paymentss = targetReward.Payments;

                            PaymentRepo paymentRepo = new PaymentRepo();
                            foreach (var _paymnt in paymentss)
                            {
                                payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                        else
                        {
                            payments = null;
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        saleReceipts = null;
                        adminBill = null;
                        payment = null;
                        billl = null;
                        inquiry = null;
                        offer = null;
                        saleOrder = null;
                        saleInvoices = null;
                        purchaseOrders = null;
                        bills = null;
                        purchaseInvoices = null;
                        journalVouchers1 = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        break;

                    case Enums.TransactionItemType.Payments:

                        payment = context.payments.FirstOrDefault(x => x.Id == Id);


                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Admin_Bills)
                        {

                            if (payment.AdminBill_Id != null)
                            {
                                adminBill = context.adminBills.FirstOrDefault(x => x.Id == payment.AdminBill_Id);
                            }
                            else
                            {
                                adminBill = null;
                            }
                            billl = null;
                            PInvoice = null;
                            saleOrder = null;
                            offer = null;
                            inquiry = null;
                            invoicedPurchaseOrder = null;

                        }

                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Vendor_Bills)
                        {

                            if (payment.Bill_Id != null)
                            {
                                billl = context.bills.FirstOrDefault(x => x.Id == payment.Bill_Id);
                            }
                            else
                            {
                                billl = null;
                            }

                            if (billl != null)
                            {
                                saleOrder = context.saleOrders
                                .FirstOrDefault(x => x.Id == billl.saleOrder_Id);
                                if (saleOrder == null)
                                {
                                    var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);
                                    if (purchaseOrder != null)
                                    {
                                        saleOrder = context.saleOrders
                                        .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                        purchaseOrders.Add(purchaseOrder);
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                    //else
                                    //{
                                    //    bills.Add(billl);
                                    //}
                                }
                                if (saleOrder != null)
                                {
                                    //bills = context.bills.Include("BillStatus")
                                    //.Include("customerCompany.company").Include("SaleOrder")
                                    //.Include("company").Include("AllocateTo")
                                    //.Include("department").Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                }
                                journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                            }
                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders
                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                            }
                            else
                            {

                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                             .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                foreach (var recpt in saleReceipts)
                                {
                                    var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (trans == null)
                                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                                }
                                foreach (var recpt in saleReceipts)
                                {
                                    var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (vouch == null)
                                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                }
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }

                            PInvoice = null;
                            payments = null;
                            invoicedPurchaseOrder = null;
                            adminBill = null;
                        }



                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                        {

                            if (payment.PInvoice_Id != null)
                            {
                                PInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == payment.PInvoice_Id);
                            }
                            else
                            {
                                PInvoice = null;
                            }
                            if (PInvoice != null)
                            {
                                invoicedPurchaseOrder = context.purchaseOrders

                                .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);
                            }
                            else
                            {
                                invoicedPurchaseOrder = null;
                            }
                            if (invoicedPurchaseOrder != null)
                            {
                                saleOrder = context.saleOrders

                                .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                            }
                            else
                            {
                                saleOrder = null;
                            }
                            if (saleOrder != null)
                            {
                                //purchaseOrders = context.purchaseOrders
                                //.Include("SaleOrder")
                                //.Include("company")
                                //.Include("AllocateTo")
                                //.Include("department").Include("purchaseInvoices").Include("PurchaseOrderStatus")
                                //.Where(x => x.saleOrder_Id == saleOrder.Id).ToList();


                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                            }
                            else
                            {

                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                            .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                foreach (var recpt in saleReceipts)
                                {
                                    var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (trans == null)
                                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                                }
                                foreach (var recpt in saleReceipts)
                                {
                                    var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                    if (vouch == null)
                                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                }
                            }
                            else
                            {
                                saleReceipts = null;
                            }



                            adminBill = null;
                            billl = null;
                            bills = null;
                            purchaseInvoices = null;
                            journalVouchers1 = null;
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;


                    case Enums.TransactionItemType.STL:

                        stl = context.STLs.FirstOrDefault(x => x.Id == Id);

                        payment = context.payments.FirstOrDefault(x => x.transactionGroupId == stl.paymentGroupId);
                        bankTransfers.AddRange(stl.InterBankTransfers);
                        companyBankTransfers.AddRange(stl.InterCompanyBankTransfers);
                   
                        break;



                    case Enums.TransactionItemType.Purchase_Order:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var PurchaseOrder = context.purchaseOrders
                            .FirstOrDefault(x => x.Id == Id);
                        if (PurchaseOrder != null)
                        {
                            // purchaseInvoices = context.purchaseInvoices.Include("PurchaseInvoiceStatus")
                            //.Include("customerCompany.company").Include("PurchaseOrder")
                            //.Include("company").Include("employee")
                            //.Include("department").Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == PurchaseOrder.saleOrder_Id);

                            //loansAdvances = PurchaseOrder.LoansAdvances;

                            if (PurchaseOrder.LoansAdvances != null && PurchaseOrder.LoansAdvances.Count > 0)
                            {
                                if (loansAdvances == null)
                                    loansAdvances = new List<LoansAdvance>();
                                loansAdvances.AddRange(PurchaseOrder.LoansAdvances);
                            }
                        }
                        else
                        {
                            saleOrder = null;
                            purchaseInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            var _taskks = taskRepo.GetSOTask(saleOrder.Id, true);
                            if (_taskks.Count> 0)
                                tasks.AddRange(_taskks);
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();

                            if (bills.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _bill in bills)
                                {
                                    if (_bill.Payments.Count > 0)
                                    {
                                        payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());

                                    }
                                    //payments.AddRange(_bill.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }

                            if(saleOrder.LoansAdvances != null && saleOrder.LoansAdvances.Count > 0)
                            {
                                if (loansAdvances == null)
                                    loansAdvances = new List<LoansAdvance>();
                                loansAdvances.AddRange(saleOrder.LoansAdvances);
                            }
                        }
                        else if (PurchaseOrder != null)
                        {
                            bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrder.Id)).ToList();

                            if (bills.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _bill in bills)
                                {
                                    if (_bill.Payments.Count > 0)
                                        payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                    //payments.AddRange(_bill.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    var _tasks = taskRepo.GetPOTask(purchaseOrder.Id, true);
                                    if (_tasks.Count > 0)
                                        tasks.AddRange(_tasks);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }

                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (payments == null)
                                        payments = new List<Payment>();

                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }

                            }
                        }
                        else if (PurchaseOrder != null)
                        {
                            purchaseOrders.Add(PurchaseOrder);
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                if (payments == null)
                                    payments = new List<Payment>();

                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                            purchaseInvoices = null;
                        }


                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                            foreach (var _SI in saleInvoices)
                            {
                                var _tasks = taskRepo.GetSITask(_SI.Id, true);
                                if (_tasks.Count > 0)
                                    tasks.AddRange(_tasks);
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales
                            .Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        if (PurchaseOrder != null)
                            journalVouchers1 = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                        if (tasks != null && tasks.Count > 0)
                        {
                            tasks = tasks.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;
                    case Enums.TransactionItemType.Bill:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;

                        var bill = context.bills
                        .FirstOrDefault(x => x.Id == Id);
                        if (bill != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == bill.saleOrder_Id);
                            if (saleOrder == null)
                            {
                                var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == bill.purchaseOrder_Id);
                                if (purchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders

                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                    bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList();
                                    purchaseOrders.Add(purchaseOrder);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                                else
                                {
                                    bills.Add(bill);
                                }
                            }
                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            if (bill.interBankTransfers != null)
                                bankTransfers.AddRange(bill.interBankTransfers);
                            journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == bill.Id).ToList();

                            payments = context.payments.Where(x => x.Bill_Id == bill.Id).ToList();
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers

                           .Where(x => x.paymentGroupId == paym.transactionGroupId).ToList());

                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }

                            if (bill.loansAdvance != null)
                                loansAdvances.Add(bill.loansAdvance);



                        }

                        if (loansAdvances != null && loansAdvances.Count > 0)
                        {
                            foreach (var _LA in loansAdvances)
                            {
                                if (_LA.saleOrder != null)
                                    saleOrders.Add(_LA.saleOrder);
                            }
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices
                            .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            if (saleOrder.LoansAdvances != null && saleOrder.LoansAdvances.Count > 0)
                                loansAdvances = saleOrder.LoansAdvances;

                            
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                        }

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                                companyBankTransfers.AddRange(context.interCompanyBankTransfers


                           .Where(x => x.receiptGroupId == recpt.transactionGroupId).ToList());
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;
                    //Memorandum Sale
                    case Enums.TransactionItemType.Memorandum_Sale:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var memorandumSale = context.memorandumSales.Where(x => x.Id == Id).FirstOrDefault();
                        if (memorandumSale != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == memorandumSale.SaleOrder_Id);
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = context.offers.FirstOrDefault(x => x.Id == memorandumSale.Offer_Id);
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }

                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else if (memorandumSale != null)
                        {
                            memorandumSales.Add(memorandumSale);
                        }
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;


                    case Enums.TransactionItemType.Purchase_Invoice:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        var purchaseInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == Id);
                        if (purchaseInvoice != null)
                        {
                            invoicedPurchaseOrder = context.purchaseOrders

                            .FirstOrDefault(x => x.Id == purchaseInvoice.purchaseOrder_Id);

                            payments = context.payments

                            .Where(x => x.PInvoice_Id == purchaseInvoice.Id).ToList();
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                        else
                        {
                            invoicedPurchaseOrder = null;
                        }
                        if (invoicedPurchaseOrder != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            //purchaseOrders = context.purchaseOrders
                            //.Include("SaleOrder")
                            //.Include("company")
                            //.Include("AllocateTo")
                            //.Include("department").Include("purchaseInvoices").Include("PurchaseOrderStatus")
                            //.Where(x => x.saleOrder_Id == saleOrder.Id).ToList();

                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        purchaseInvoices = context.purchaseInvoices.Where(x => x.isVoid != true && x.purchaseOrder_Id == purchaseInvoice.purchaseOrder_Id).ToList();

                        if (purchaseInvoices != null)
                            foreach (var _pi in purchaseInvoices)
                                payments.AddRange(_pi.Payments.Except(payments));

                        payments.Distinct();
                        if (purchaseOrders != null)
                        {
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    bills.AddRange(context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList());
                                }
                            }
                        }
                        if (offer != null && offer.Id != 0)
                            moduleContracts.AddRange(context.ModuleContracts.Where(x => x.offer_Id == offer.Id).ToList());
                        break;



                }
                if (offer != null && offer.Id != 0)
                {
                    offers.Add(offer);
                }
                if (saleOrder != null)
                    if (saleOrder.Id != 0)
                    {
                        if (saleOrder.moduleContract != null)
                        {
                            moduleContracts.Add(saleOrder.moduleContract);
                        }
                    }
                if(saleOrders.Count>0)
                foreach (var so in saleOrders)
                {
                    if (so.moduleContract_Id != null)
                    {
                        moduleContracts.Add(context.ModuleContracts.FirstOrDefault(x=>x.Id==so.moduleContract_Id));
                    }
                }
                if (moduleContracts.Count > 0)
                {
                    foreach (var contract in moduleContracts)
                    {
                        if(contract!=null)
                        if (contract.offer_Id != null)
                        {
                            offer = contract.offer;
                            if (contract.offer.inquiry_Id != null)
                                inquiry = contract.offer.inquiry;
                        }
                    }
                }
                if (moduleContracts.Count > 0)
                {
                    foreach (var contract in moduleContracts)
                    {
                        if (contract != null)
                            if (contract.offer_Id != null)
                            {
                                offer = contract.offer;
                                if (contract.offer.inquiry_Id != null)
                                    inquiry = contract.offer.inquiry;
                            }
                    }
                }
                if(moduleContracts.Count>0)
                moduleContracts = moduleContracts.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
                saleOrders = saleOrders.GroupBy(x => x.Id)
                                   .Select(g => g.First())
                                   .ToList();
                offers = offers.GroupBy(x => x.Id)
                                   .Select(g => g.First())
                                   .ToList();
                bankTransfers = bankTransfers.GroupBy(x => x.Id)
                                       .Select(g => g.First())
                                       .ToList();
                companyBankTransfers = companyBankTransfers.GroupBy(x => x.Id)
                                      .Select(g => g.First())
                                      .ToList();
                if (payment != null && payment.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
                {
                    int parentId = 0;
                    if (payment.PInvoice_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.PInvoice_Id);
                    }
                    if (payment.Bill_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.Bill_Id);
                    }
                    if (payment.TargetReward_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.TargetReward_Id);
                    }
                    if (payment.AdminBill_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.AdminBill_Id);
                    }
                    if (payment.LoansAdvanceId != null)
                    {
                        parentId = Convert.ToInt32(payment.LoansAdvanceId);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (payment.statusClass_Id != null)
                    {
                        statusClass = payment.StatusClass.ClassName;
                        statusClassBackColor = payment.StatusClass.backcolor;
                    }
                    if (payment.isVoid == true)
                        stage = "Void";
                    else if (payment.isReApproved == false)
                        stage = "Under Approval";
                    else if (payment.isApproved == true && payment.stage == "Closed")
                        stage = "Closed";
                    else if (payment.isApproved == true && payment.Status.isActive == false && payment.PendingForClosing != true)
                        stage = "Closed";
                    else if (payment.isApproved == true && payment.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (payment.isApproved == true)
                        stage = "Approved";
                    else if (payment.isApproved == false)
                        stage = "Under Approval";
                    else if (payment.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (payment.vendor != null)
                    {
                        vendor = payment.vendor.company.CompanyName;
                    }
                    stls.AddRange(context.STLs.Where(x => x.paymentGroupId == payment.transactionGroupId).ToList());

                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = payment.Id,
                        transactionType = TransactionItemType.Payments,
                        Company = payment.company.CompanyName,
                        //Department = payment.departments.DeptName,
                        CreationDate = (DateTime)payment.CreationDate,
                        Currency = payment.currency.CurrencyName,
                        SalesReference = payment.BillFinanceRefNo,
                        Stage = stage,
                        Status = payment.Status.Status,
                        AmountOC = payment.DebitedAmount,
                        GroupId = payment.transactionGroupId,
                        BackColor = payment.Status.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (payments != null && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
                {
                    foreach (var _payment in payments)
                    {
                        string parentId = "";
                        if (_payment.PInvoice_Id != null)
                        {
                            parentId = (_payment.PInvoice_Id != null ? _payment.PInvoice_Id.ToString() : "");
                        }
                        else
                        if (_payment.AdminBill_Id != null)
                        {
                            parentId = (_payment.AdminBill_Id != null ? _payment.AdminBill_Id.ToString() : "");
                        }
                        else
                        if (_payment.Bill_Id != null)
                        {
                            parentId = (_payment.Bill_Id != null ? _payment.Bill_Id.ToString() : "");
                        }
                        else
                            if (_payment.LoansAdvanceId != null)
                        {
                            parentId = (_payment.LoansAdvanceId != null ? _payment.LoansAdvanceId.ToString() : "");
                        }
                        else
                            if (_payment.TargetReward_Id != null)
                        {
                            parentId = (_payment.TargetReward_Id != null ? _payment.TargetReward_Id.ToString() : "");
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (_payment.statusClass_Id != null)
                        {
                            statusClass = _payment.StatusClass.ClassName;
                            statusClassBackColor = _payment.StatusClass.backcolor;
                        }
                        if (_payment.isVoid == true)
                            stage = "Void";
                        else if (_payment.isReApproved == false)
                            stage = "Under Approval";
                        else if (_payment.isApproved == true && _payment.stage == "Closed")
                            stage = "Closed";
                        else if (_payment.isApproved == true && _payment.Status.isActive == false && _payment.PendingForClosing != true)
                            stage = "Closed";
                        else if (_payment.isApproved == true && _payment.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_payment.isApproved == true)
                            stage = "Approved";
                        else if (_payment.isApproved == false)
                            stage = "Under Approval";
                        else if (_payment.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (_payment.vendor != null)
                        {
                            vendor = _payment.vendor.company.CompanyName;
                        }
                        stls.AddRange(context.STLs.Where(x => x.paymentGroupId == _payment.transactionGroupId).ToList());

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _payment.Id,
                            transactionType = TransactionItemType.Payments,
                            Company = _payment.company.CompanyName,
                            //Department = payment.departments.DeptName,
                            CreationDate = (DateTime)_payment.CreationDate,
                            Currency = _payment.currency.CurrencyName,
                            SalesReference = _payment.BillFinanceRefNo,
                            Stage = stage,
                            Status = _payment.Status.Status,
                            AmountOC = _payment.DebitedAmount,
                            GroupId = _payment.transactionGroupId,
                            BackColor = _payment.Status.backcolor,
                            ParentId = Convert.ToInt32(parentId),
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (bankTransfers.Count > 0)
                {
                    foreach (var transfer in bankTransfers)
                    {
                        if (transfer.vendorBillId != null)
                        {
                            billl = transfer.Bill;
                        }
                    }
                }
                if (moduleContracts.Count != 0 && allowedPermissions.Find(x => x.Name == "List Of ModuleContracts") != null)
                {
                    moduleContracts = moduleContracts.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                    foreach (var transfer in moduleContracts)
                    {
                        int parentId = 0;
                        if (transfer.offer_Id != null)
                        {
                            parentId = Convert.ToInt32(transfer.offer_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (transfer.StatusClass != null)
                        {
                            statusClass = transfer.StatusClass.ClassName;
                            statusClassBackColor = transfer.StatusClass.backcolor;
                        }
                        if (transfer.isVoid == true)
                            stage = "Void";
                        //else if (transfer.isReApproved == false)
                        //    stage = "Under Approval";
                        else if (transfer.isApproved == true && transfer.stage == "Closed")
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.ModuleContractStatus.isActive == false && transfer.PendingForClosing != true)
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (transfer.isApproved == true)
                            stage = "Approved";
                        else if (transfer.isApproved == false)
                            stage = "Under Approval";
                        else if (transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        string currency = "";
                        double amount = 0;
                        currency = transfer.currency.CurrencyName;
                        amount = transfer.totalCFRValue;
                       
                        string vendor = "";
                        //if (transfer.vendor != null)
                        //{
                        //    vendor = transfer.vendor.company.CompanyName;
                        //}

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = transfer.Id,
                            transactionType = TransactionItemType.ModuleContract,
                            Company = transfer.company.CompanyName,
                            Department = transfer.department.DeptName,
                            CreationDate = (DateTime)transfer.CreationDate,
                            Currency = currency,
                            SalesReference = transfer.SalesReferenceNo,
                            Stage = stage,
                            Status = transfer.ModuleContractStatus.Status,
                            AmountOC = amount,
                            GroupId = null,
                            BackColor = transfer.ModuleContractStatus.backcolor,
                            ParentId = parentId,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }

                //---------Asset Rental--------------
                
                if (assetRentals != null && assetRentals.Count > 0 && allowedPermissions.Find(x => x.Name == "View List of Assets") != null)
                {
                    foreach (var _assetRental in assetRentals)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        //if (_assetRental.statusClass_Id != null)
                        //{
                        //    statusClass = _assetRental.StatusClass.ClassName;
                        //    statusClassBackColor = _assetRental.StatusClass.backcolor;
                        //}

                        if (_assetRental.isVoid == true)
                            stage = "Void";
                        else if (_assetRental.isReApproved == false)
                            stage = "Under Approval";
                        else if (_assetRental.isApproved == true && _assetRental.stage == "Closed")
                            stage = "Closed";
                        else if (_assetRental.isApproved == true && _assetRental.Status.isActive == false && _assetRental.PendingForClosing != true)
                            stage = "Closed";
                        else if (_assetRental.isApproved == true && _assetRental.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_assetRental.isApproved == true)
                            stage = "Approved";
                        else if (_assetRental.isApproved == false)
                            stage = "Under Approval";
                        else if (_assetRental.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (_assetRental.vendor != null)
                        {
                            vendor = _assetRental.vendor.company.CompanyName;
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _assetRental.Id,
                            transactionType = TransactionItemType.AssetRental,
                            Company = _assetRental.company.CompanyName,
                            Department = _assetRental.department.DeptName,
                            CreationDate = (DateTime)_assetRental.CreationDate,
                            //Currency = _assetRental.currency.CurrencyName,
                            SalesReference = _assetRental.SystemRef,
                            Stage = stage,
                            Status = _assetRental.Status.Status,
                            AmountOC = _assetRental.PurchasingCost,
                            GroupId = null,
                            BackColor = _assetRental.Status.backcolor,
                            //ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }


                if (rentalContracts != null && rentalContracts.Count > 0 && allowedPermissions.Find(x => x.Name == "List of Rental Contracts") != null)
                {
                    foreach (var _rentalContract in rentalContracts)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        //if (_assetRental.statusClass_Id != null)
                        //{
                        //    statusClass = _assetRental.StatusClass.ClassName;
                        //    statusClassBackColor = _assetRental.StatusClass.backcolor;
                        //}

                        if (_rentalContract.isVoid == true)
                            stage = "Void";
                        else if (_rentalContract.isReApproved == false)
                            stage = "Under Approval";
                        else if (_rentalContract.isApproved == true && _rentalContract.stage == "Closed")
                            stage = "Closed";
                        else if (_rentalContract.isApproved == true && _rentalContract.Status.isActive == false && _rentalContract.PendingForClosing != true)
                            stage = "Closed";
                        else if (_rentalContract.isApproved == true && _rentalContract.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_rentalContract.isApproved == true)
                            stage = "Approved";
                        else if (_rentalContract.isApproved == false)
                            stage = "Under Approval";
                        else if (_rentalContract.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (_rentalContract.vendor != null)
                        //{
                        //    vendor = _rentalContract.vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _rentalContract.Id,
                            transactionType = TransactionItemType.RentalContract,
                            Company = _rentalContract.company.CompanyName,
                            Department = _rentalContract.department.DeptName,
                            CreationDate = (DateTime)_rentalContract.CreationDate,
                            //Currency = _assetRental.currency.CurrencyName,
                            SalesReference = _rentalContract.SystemRef,
                            Stage = stage,
                            Status = _rentalContract.Status.Status,
                            AmountOC = _rentalContract.RentAmount,
                            GroupId = null,
                            BackColor = _rentalContract.Status.backcolor,
                            //ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }

                if (rentalOrders != null && rentalOrders.Count > 0 && allowedPermissions.Find(x => x.Name == "List of Rental Orders") != null)
                {
                    foreach (var _rentalOrder in rentalOrders)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        //if (_assetRental.statusClass_Id != null)
                        //{
                        //    statusClass = _assetRental.StatusClass.ClassName;
                        //    statusClassBackColor = _assetRental.StatusClass.backcolor;
                        //}

                        if (_rentalOrder.isVoid == true)
                            stage = "Void";
                        else if (_rentalOrder.isReApproved == false)
                            stage = "Under Approval";
                        else if (_rentalOrder.isApproved == true && _rentalOrder.stage == "Closed")
                            stage = "Closed";
                        else if (_rentalOrder.isApproved == true && _rentalOrder.Status.isActive == false && _rentalOrder.PendingForClosing != true)
                            stage = "Closed";
                        else if (_rentalOrder.isApproved == true && _rentalOrder.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_rentalOrder.isApproved == true)
                            stage = "Approved";
                        else if (_rentalOrder.isApproved == false)
                            stage = "Under Approval";
                        else if (_rentalOrder.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (_rentalOrder.vendor != null)
                        //{
                        //    vendor = _rentalOrder.vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _rentalOrder.Id,
                            transactionType = TransactionItemType.RentalOrder,
                            Company = _rentalOrder.company.CompanyName,
                            Department = _rentalOrder.department.DeptName,
                            CreationDate = (DateTime)_rentalOrder.CreationDate,
                            //Currency = _assetRental.currency.CurrencyName,
                            SalesReference = _rentalOrder.SystemRef,
                            Stage = stage,
                            Status = _rentalOrder.Status.Status,
                            AmountOC = _rentalOrder.RentAmount,
                            GroupId = null,
                            BackColor = _rentalOrder.Status.backcolor,
                            //ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }

                if (rentalInvoices != null && rentalInvoices.Count > 0 && allowedPermissions.Find(x => x.Name == "List of Rental Invoices") != null)
                {
                    foreach (var _rentalInvoice in rentalInvoices)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        //if (_assetRental.statusClass_Id != null)
                        //{
                        //    statusClass = _assetRental.StatusClass.ClassName;
                        //    statusClassBackColor = _assetRental.StatusClass.backcolor;
                        //}

                        if (_rentalInvoice.isVoid == true)
                            stage = "Void";
                        else if (_rentalInvoice.isReApproved == false)
                            stage = "Under Approval";
                        else if (_rentalInvoice.isApproved == true && _rentalInvoice.stage == "Closed")
                            stage = "Closed";
                        else if (_rentalInvoice.isApproved == true && _rentalInvoice.Status.isActive == false && _rentalInvoice.PendingForClosing != true)
                            stage = "Closed";
                        else if (_rentalInvoice.isApproved == true && _rentalInvoice.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_rentalInvoice.isApproved == true)
                            stage = "Approved";
                        else if (_rentalInvoice.isApproved == false)
                            stage = "Under Approval";
                        else if (_rentalInvoice.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (_rentalOrder.vendor != null)
                        //{
                        //    vendor = _rentalOrder.vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _rentalInvoice.Id,
                            transactionType = TransactionItemType.RentalInvoice,
                            Company = _rentalInvoice.company.CompanyName,
                            Department = _rentalInvoice.department.DeptName,
                            CreationDate = (DateTime)_rentalInvoice.CreationDate,
                            //Currency = _assetRental.currency.CurrencyName,
                            SalesReference = _rentalInvoice.SystemRef,
                            Stage = stage,
                            Status = _rentalInvoice.Status.Status,
                            AmountOC = _rentalInvoice.InvoiceAmount,
                            GroupId = null,
                            BackColor = _rentalInvoice.Status.backcolor,
                            //ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                //---------Asset Rental--------------

                // Adding all transactions in same structure
                if (adminBill != null && adminBill.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
                {
                    int parentId = 0;

                    if (adminBill.assetRentalId != null)
                        parentId = adminBill.assetRentalId.Value;

                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (adminBill.statusClass_Id != null)
                    {
                        statusClass = adminBill.StatusClass.ClassName;
                        statusClassBackColor = adminBill.StatusClass.backcolor;
                    }



                    if (adminBill.isVoid == true)
                        stage = "Void";
                    else if (adminBill.isReApproved == false)
                        stage = "Under Approval";
                    else if (adminBill.isApproved == true && adminBill.stage == "Closed")
                        stage = "Closed";
                    else if (adminBill.isApproved == true && adminBill.BillStatus.isActive == false && adminBill.PendingForClosing != true)
                        stage = "Closed";
                    else if (adminBill.isApproved == true && adminBill.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (adminBill.isApproved == true)
                        stage = "Approved";
                    else if (adminBill.isApproved == false)
                        stage = "Under Approval";
                    else if (adminBill.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (adminBill.vendor != null)
                    {
                        vendor = adminBill.vendor.company.CompanyName;
                    }
                    


                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = adminBill.Id,
                        transactionType = TransactionItemType.Admin_Bill,
                        Company = adminBill.company.CompanyName,
                        Department = adminBill.department.DeptName,
                        CreationDate = (DateTime)adminBill.CreationDate,
                        Currency = adminBill.currency.CurrencyName,
                        SalesReference = adminBill.FinanceRefNo2,
                        Stage = stage,
                        Status = adminBill.BillStatus.Status,
                        AmountOC = adminBill.AmountOC,
                        GroupId = null,
                        BackColor = adminBill.BillStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass= statusClass,
                        BackColorStatusClass= statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (adminBills != null && adminBills.Count > 0 && allowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
                {
                    foreach (var _adminBill in adminBills)
                    {
                        int parentId = 0;
                        if (_adminBill.assetRentalId != null)
                            parentId = _adminBill.assetRentalId.Value;

                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (_adminBill.statusClass_Id != null)
                        {
                            statusClass = _adminBill.StatusClass.ClassName;
                            statusClassBackColor = _adminBill.StatusClass.backcolor;
                        }

                        if (_adminBill.isVoid == true)
                            stage = "Void";
                        else if (_adminBill.isReApproved == false)
                            stage = "Under Approval";
                        else if (_adminBill.isApproved == true && _adminBill.stage == "Closed")
                            stage = "Closed";
                        else if (_adminBill.isApproved == true && _adminBill.BillStatus.isActive == false && _adminBill.PendingForClosing != true)
                            stage = "Closed";
                        else if (_adminBill.isApproved == true && _adminBill.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_adminBill.isApproved == true)
                            stage = "Approved";
                        else if (_adminBill.isApproved == false)
                            stage = "Under Approval";
                        else if (_adminBill.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (_adminBill.vendor != null)
                        {
                            vendor = _adminBill.vendor.company.CompanyName;
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _adminBill.Id,
                            transactionType = TransactionItemType.Admin_Bill,
                            Company = _adminBill.company.CompanyName,
                            Department = _adminBill.department.DeptName,
                            CreationDate = (DateTime)_adminBill.CreationDate,
                            Currency = _adminBill.currency.CurrencyName,
                            SalesReference = _adminBill.FinanceRefNo2,
                            Stage = stage,
                            Status = _adminBill.BillStatus.Status,
                            AmountOC = _adminBill.AmountOC,
                            GroupId = null,
                            BackColor = _adminBill.BillStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (task != null && task.Id != 0 && allowedPermissions.Find(x => x.Name == "List of User Tasks") != null)
                {
                    int parentId = 0;

                    if (task.inquiryId != null)
                    {
                        parentId = Convert.ToInt32(task.inquiryId);
                    }
                    else
                        if (task.offerId != null)
                    {
                        parentId = Convert.ToInt32(task.offerId);
                    }
                    else
                        if (task.saleInvoiceId != null)
                    {
                        parentId = Convert.ToInt32(task.saleInvoiceId);
                    }
                    else
                        if (task.purchaseOrderId != null)
                    {
                        parentId = Convert.ToInt32(task.purchaseOrderId);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (task.statusClass_Id != null)
                    {
                        statusClass = task.StatusClass.ClassName;
                        statusClassBackColor = task.StatusClass.backcolor;
                    }
                    if (task.isVoid == true)
                        stage = "Void";
                    else if (task.isReApproved == false)
                        stage = "Under Approval";
                    else if (task.isApproved == true && task.stage == "Closed")
                        stage = "Closed";
                    else if (task.isApproved == true && task.Status.isActive == false && task.PendingForClosing != true)
                        stage = "Closed";
                    else if (task.isApproved == null && task.Status.isActive == false && task.PendingForClosing != true)
                        stage = "Closed";
                    else if (task.isApproved == true && task.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (task.isApproved == true)
                        stage = "Approved";
                    else if (task.isApproved == false)
                        stage = "Under Approval";
                    else if (task.PendingForClosing == true)
                        stage = "Under Closing";
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = task.Id,
                        transactionType = TransactionItemType.Tasks,
                        Company = task.company.CompanyName,
                        Department = task.department.DeptName,
                        CreationDate = (DateTime)task.creationDate,
                        Currency = task.currency?.CurrencyName,
                        SalesReference = task.SystemRef,
                        Stage = stage,
                        Status = task.Status.Status,
                        AmountOC = task.ManualAmount,
                        GroupId = null,
                        BackColor = task.Status.backcolor,
                        ParentId = parentId,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (tasks != null && tasks.Count > 0 && allowedPermissions.Find(x => x.Name == "List of User Tasks") != null)
                {
                    foreach (var _task in tasks)
                    {
                        int parentId = 0;

                        if (_task.inquiryId != null)
                        {
                            parentId = Convert.ToInt32(_task.inquiryId);
                        }
                        else
                            if (_task.offerId != null)
                        {
                            parentId = Convert.ToInt32(_task.offerId);
                        }
                        else
                            if (_task.saleInvoiceId != null)
                        {
                            parentId = Convert.ToInt32(_task.saleInvoiceId);
                        }
                        else
                            if (_task.purchaseOrderId != null)
                        {
                            parentId = Convert.ToInt32(_task.purchaseOrderId);
                        }
                        else

                            if (_task.saleOrderId != null)
                        {
                            parentId = Convert.ToInt32(_task.saleOrderId);
                        }




                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (_task.statusClass_Id != null)
                        {
                            statusClass = _task.StatusClass.ClassName;
                            statusClassBackColor = _task.StatusClass.backcolor;
                        }
                        if (_task.isVoid == true)
                            stage = "Void";
                        else if (_task.isReApproved == false)
                            stage = "Under Approval";
                        else if (_task.isApproved == true && _task.stage == "Closed")
                            stage = "Closed";
                        else if (_task.isApproved == true && _task.Status.isActive == false && _task.PendingForClosing != true)
                            stage = "Closed";
                        else if (_task.isApproved == null && _task.Status.isActive == false && _task.PendingForClosing != true)
                            stage = "Closed";
                        else if (_task.isApproved == true && _task.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_task.isApproved == true)
                            stage = "Approved";
                        else if (_task.isApproved == false)
                            stage = "Under Approval";
                        else if (_task.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _task.Id,
                            transactionType = TransactionItemType.Tasks,
                            Company = _task.company.CompanyName,
                            Department = _task.department.DeptName,
                            CreationDate = (DateTime)_task.creationDate,
                            Currency = _task.currency.CurrencyName,
                            SalesReference = _task.SystemRef,
                            Stage = stage,
                            Status = _task.Status.Status,
                            AmountOC = _task.ManualAmount,
                            GroupId = null,
                            BackColor = _task.Status.backcolor,
                            ParentId = parentId,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (loansAdvance != null && loansAdvance.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
                {
                    int parentId = 0;
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (loansAdvance.statusClass_Id != null)
                    {
                        statusClass = loansAdvance.StatusClass.ClassName;
                        statusClassBackColor = loansAdvance.StatusClass.backcolor;
                    }
                    if (loansAdvance.isVoid == true)
                        stage = "Void";
                    else if (loansAdvance.isReApproved == false)
                        stage = "Under Approval";
                    else if (loansAdvance.isApproved == true && loansAdvance.stage == "Closed")
                        stage = "Closed";
                    else if (loansAdvance.isApproved == true && loansAdvance.Status.isActive == false && loansAdvance.PendingForClosing != true)
                        stage = "Closed";
                    else if (loansAdvance.isApproved == true && loansAdvance.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (loansAdvance.isApproved == true)
                        stage = "Approved";
                    else if (loansAdvance.isApproved == false)
                        stage = "Under Approval";
                    else if (loansAdvance.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";

                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = loansAdvance.Id,
                        transactionType = TransactionItemType.LoansAdvances,
                        Company = loansAdvance.company.CompanyName,
                        Department = loansAdvance.department.DeptName,
                        CreationDate = (DateTime)loansAdvance.CreationDate,
                        Currency = loansAdvance.currency.CurrencyName,
                        SalesReference = loansAdvance.SystemRef,
                        Stage = stage,
                        Status = loansAdvance.Status.Status,
                        AmountOC = loansAdvance.LoanAmountOC,
                        GroupId = null,
                        BackColor = loansAdvance.Status.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (loansAdvances != null && loansAdvances.Count > 0 && allowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
                {
                    foreach (var _LA in loansAdvances)
                    {
                        int parentId = 0;
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (_LA.statusClass_Id != null)
                        {
                            statusClass = _LA.StatusClass.ClassName;
                            statusClassBackColor = _LA.StatusClass.backcolor;
                        }
                        if (_LA.isVoid == true)
                            stage = "Void";
                        else if (_LA.isReApproved == false)
                            stage = "Under Approval";
                        else if (_LA.isApproved == true && _LA.stage == "Closed")
                            stage = "Closed";
                        else if (_LA.isApproved == true && _LA.Status.isActive == false && _LA.PendingForClosing != true)
                            stage = "Closed";
                        else if (_LA.isApproved == true && _LA.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_LA.isApproved == true)
                            stage = "Approved";
                        else if (_LA.isApproved == false)
                            stage = "Under Approval";
                        else if (_LA.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";

                        if(_LA.SaleInvoiceId!=null)
                        {
                            parentId = Convert.ToInt32(_LA.SaleInvoiceId);
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _LA.Id,
                            transactionType = TransactionItemType.LoansAdvances,
                            Company = _LA.company.CompanyName,
                            Department = _LA.department.DeptName,
                            CreationDate = (DateTime)_LA.CreationDate,
                            Currency = _LA.currency.CurrencyName,
                            SalesReference = _LA.SystemRef,
                            Stage = stage,
                            Status = _LA.Status.Status,
                            AmountOC = _LA.AppliedAmountOC,
                            GroupId = null,
                            BackColor = _LA.Status.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (loansAdvance != null && loansAdvance.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
                {
                    int parentId = 0;
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (loansAdvance.statusClass_Id != null)
                    {
                        statusClass = loansAdvance.StatusClass.ClassName;
                        statusClassBackColor = loansAdvance.StatusClass.backcolor;
                    }
                    if (loansAdvance.isVoid == true)
                        stage = "Void";
                    else if (loansAdvance.isReApproved == false)
                        stage = "Under Approval";
                    else if (loansAdvance.isApproved == true && loansAdvance.stage == "Closed")
                        stage = "Closed";
                    else if (loansAdvance.isApproved == true && loansAdvance.Status.isActive == false && loansAdvance.PendingForClosing != true)
                        stage = "Closed";
                    else if (loansAdvance.isApproved == true && loansAdvance.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (loansAdvance.isApproved == true)
                        stage = "Approved";
                    else if (loansAdvance.isApproved == false)
                        stage = "Under Approval";
                    else if (loansAdvance.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";

                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = loansAdvance.Id,
                        transactionType = TransactionItemType.LoansAdvances,
                        Company = loansAdvance.company.CompanyName,
                        Department = loansAdvance.department.DeptName,
                        CreationDate = (DateTime)loansAdvance.CreationDate,
                        Currency = loansAdvance.currency.CurrencyName,
                        SalesReference = loansAdvance.SystemRef,
                        Stage = stage,
                        Status = loansAdvance.Status.Status,
                        AmountOC = loansAdvance.LoanAmountOC,
                        GroupId = null,
                        BackColor = loansAdvance.Status.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (targetReward != null && targetReward.Id != 0 && allowedPermissions.Find(x => x.Name == "View List of Target Rewards") != null)
                {
                    int parentId = 0;
                    if (targetReward.toDoTask_Id != null)
                    {
                        parentId = Convert.ToInt32(targetReward.toDoTask_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (targetReward.statusClass_Id != null)
                    {
                        statusClass = targetReward.StatusClass.ClassName;
                        statusClassBackColor = targetReward.StatusClass.backcolor;
                    }
                    if (targetReward.isVoid == true)
                        stage = "Void";
                    else if (targetReward.isReApproved == false)
                        stage = "Under Approval";
                    else if (targetReward.isApproved == true && targetReward.stage == "Closed")
                        stage = "Closed";
                    else if (targetReward.isApproved == true && targetReward.Status.isActive == false && targetReward.PendingForClosing != true)
                        stage = "Closed";
                    else if (targetReward.isApproved == true && targetReward.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (targetReward.isApproved == true)
                        stage = "Approved";
                    else if (targetReward.isApproved == false)
                        stage = "Under Approval";
                    else if (targetReward.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";

                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = targetReward.Id,
                        transactionType = TransactionItemType.TargetReward,
                        //Company = targetReward.company.CompanyName,
                        //Department = targetReward.department.DeptName,
                        CreationDate = (DateTime)targetReward.CreationDate,
                        Currency = targetReward.currency.CurrencyName,
                        SalesReference = targetReward.financeRefNo,
                        Stage = stage,
                        Status = targetReward.Status.Status,
                        AmountOC = targetReward.RewardAmount,
                        GroupId = null,
                        BackColor = targetReward.Status.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (billl != null && billl.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
                {
                    int parentId = 0;
                    if (billl.saleOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(billl.saleOrder_Id);
                    }
                    if (billl.purchaseOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(billl.purchaseOrder_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (billl.statusClass_Id != null)
                    {
                        statusClass = billl.StatusClass.ClassName;
                        statusClassBackColor = billl.StatusClass.backcolor;
                    }
                    if (billl.isVoid == true)
                        stage = "Void";
                    else if (billl.isReApproved == false)
                        stage = "Under Approval";
                    else if (billl.isApproved == true && billl.stage == "Closed")
                        stage = "Closed";
                    else if (billl.isApproved == true && billl.BillStatus.isActive == false && billl.PendingForClosing != true)
                        stage = "Closed";
                    else if (billl.isApproved == true && billl.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (billl.isApproved == true)
                        stage = "Approved";
                    else if (billl.isApproved == false)
                        stage = "Under Approval";
                    else if (billl.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (billl.vendor != null)
                    {
                        vendor = billl.vendor.company.CompanyName;
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = billl.Id,
                        transactionType = TransactionItemType.Bill,
                        Company = billl.company.CompanyName,
                        Department = billl.department.DeptName,
                        CreationDate = (DateTime)billl.CreationDate,
                        Currency = billl.currency.CurrencyName,
                        SalesReference = billl.SalesReferenceNo,
                        Stage = stage,
                        Status = billl.BillStatus.Status,
                        AmountOC = billl.totalCFRValue,
                        GroupId = null,
                        BackColor = billl.BillStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (PInvoice != null && PInvoice.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
                {
                    int parentId = 0;
                    if (PInvoice.purchaseOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(PInvoice.purchaseOrder_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (PInvoice.statusClass_Id != null)
                    {
                        statusClass = PInvoice.StatusClass.ClassName;
                        statusClassBackColor = PInvoice.StatusClass.backcolor;
                    }
                    if (PInvoice.isVoid == true)
                        stage = "Void";
                    else if (PInvoice.isReApproved == false)
                        stage = "Under Approval";
                    else if (PInvoice.isApproved == true && PInvoice.stage == "Closed")
                        stage = "Closed";
                    else if (PInvoice.isApproved == true && PInvoice.PurchaseInvoiceStatus.isActive == false && PInvoice.PendingForClosing != true)
                        stage = "Closed";
                    else if (PInvoice.isApproved == true && PInvoice.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (PInvoice.isApproved == true)
                        stage = "Approved";
                    else if (PInvoice.isApproved == false)
                        stage = "Under Approval";
                    else if (PInvoice.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (PInvoice.Vendor != null)
                    {
                        vendor = PInvoice.Vendor.company.CompanyName;
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = PInvoice.Id,
                        transactionType = TransactionItemType.Purchase_Invoice,
                        Company = PInvoice.company.CompanyName,
                        Department = PInvoice.department.DeptName,
                        CreationDate = (DateTime)PInvoice.CreationDate,
                        Currency = PInvoice.currency.CurrencyName,
                        SalesReference = PInvoice.SalesReferenceNo,
                        Stage = stage,
                        Status = PInvoice.PurchaseInvoiceStatus.Status,
                        AmountOC = PInvoice.totalInvoiceAmount,
                        GroupId = null,
                        BackColor = PInvoice.PurchaseInvoiceStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (inquiry != null && inquiry.Id != 0 && allowedPermissions.Find(x => x.Name == "List Of Inquiries") != null)
                {
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (inquiry.statusClass_Id != null)
                    {
                        statusClass = inquiry.StatusClass.ClassName;
                        statusClassBackColor = inquiry.StatusClass.backcolor;
                    }
                    if (inquiry.isVoid == true)
                        stage = "Void";
                    else if (inquiry.isApproved == true && inquiry.stage == "Closed")
                        stage = "Closed";
                    else if (inquiry.isApproved == true && inquiry.inquiryStatus.isActive == false && inquiry.PendingForClosing != true)
                        stage = "Closed";
                    else if (inquiry.isApproved == true && inquiry.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (inquiry.isApproved == true)
                        stage = "Approved";
                    else if (inquiry.isApproved == false)
                        stage = "Under Approval";
                    else if (inquiry.PendingForClosing == true)
                        stage = "Under Closing";

                    var vendor = "";

                    //if (inquiry.vendors.Count > 0)
                    //{
                    //    inquiry.vendors.First().company.CompanyName.ToString();
                    //}
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = inquiry.Id,
                        transactionType = TransactionItemType.Inquiry,
                        Company = inquiry.company.CompanyName,
                        Department = inquiry.department.DeptName,
                        CreationDate = (DateTime)inquiry.CreationDate,
                        //Currency = inquiry.currency.CurrencyName,
                        SalesReference = inquiry.SalesReferenceNo,
                        Stage = stage,
                        Status = inquiry.inquiryStatus.Status,
                        //AmountOC = inquiry.,
                        GroupId = null,
                        BackColor = inquiry.inquiryStatus.backcolor,
                        ParentId = inquiry.Id,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (offer != null && offer.Id != 0 && allowedPermissions.Find(x => x.Name == "List Of Offers") != null)
                {
                    if (offer.offertype == InquiryType.DistributionBiz || offer.offertype == InquiryType.DistributionBiz_CustomerCredit)
                    {
                        if (offers.Count > 0 && allowedPermissions.Find(x => x.Name == "List Of Offers") != null)
                        {
                            foreach (var off in offers)
                            {
                                string stage = "", statusClass = "", statusClassBackColor = "";
                                if (off.statusClass_Id != null)
                                {
                                    statusClass = off.StatusClass.ClassName;
                                    statusClassBackColor = off.StatusClass.backcolor;
                                }
                                double fob = 0 ;
                                fob = off.BookerStatementItems.Sum(x => x.amount);
                                if (off.salesTax != 0)
                                    fob = fob +off.salesTax;

                                if (off.isVoid == true)
                                    stage = "Void";
                                else if (off.isApproved == true && off.stage == "Closed")
                                    stage = "Closed";
                                else if (off.isApproved == true && off.offerStatus.isActive == false && off.PendingForClosing != true)
                                    stage = "Closed";
                                else if (off.isApproved == true && off.PendingForClosing == true)
                                    stage = "Under Closing";
                                else if (off.isApproved == true)
                                    stage = "Approved";
                                else if (off.isApproved == false)
                                    stage = "Under Approval";
                                else if (off.PendingForClosing == true)
                                    stage = "Under Closing";

                                var vendor = "";

                                if (off.vendors.Count > 0)
                                {
                                    off.vendors.First().company.CompanyName.ToString();
                                }
                                AllOrdersView view = new AllOrdersView()
                                {
                                    Id = off.Id,
                                    transactionType = TransactionItemType.Offer,
                                    Company = off.company.CompanyName,
                                    Department = off.department.DeptName,
                                    CreationDate = (DateTime)off.CreationDate,
                                    Currency = off.currency.CurrencyName,
                                    SalesReference = off.SalesReferenceNo,
                                    Stage = stage,
                                    Status = off.offerStatus.Status,
                                    AmountOC = fob,
                                    GroupId = null,
                                    BackColor = off.offerStatus.backcolor,
                                    ParentId = off.inquiry_Id,
                                    Vendor = vendor,
                                    StatusClass = statusClass,
                                    BackColorStatusClass = statusClassBackColor

                                };
                                allTransactions.Add(view);
                            }
                        }
                    }
                    else
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (offer.statusClass_Id != null)
                        {
                            statusClass = offer.StatusClass.ClassName;
                            statusClassBackColor = offer.StatusClass.backcolor;
                        }
                        if (offer.isVoid == true)
                            stage = "Void";
                        else if (offer.isApproved == true && offer.stage == "Closed")
                            stage = "Closed";
                        else if (offer.isApproved == true && offer.offerStatus.isActive == false && offer.PendingForClosing != true)
                            stage = "Closed";
                        else if (offer.isApproved == true && offer.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (offer.isApproved == true)
                            stage = "Approved";
                        else if (offer.isApproved == false)
                            stage = "Under Approval";
                        else if (offer.PendingForClosing == true)
                            stage = "Under Closing";

                        var vendor = "";

                        if (offer.vendors.Count > 0)
                        {
                            offer.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = offer.Id,
                            transactionType = TransactionItemType.Offer,
                            Company = offer.company.CompanyName,
                            Department = offer.department.DeptName,
                            CreationDate = (DateTime)offer.CreationDate,
                            Currency = offer.currency.CurrencyName,
                            SalesReference = offer.SalesReferenceNo,
                            Stage = stage,
                            Status = offer.offerStatus.Status,
                            AmountOC = offer.totalCFRValue,
                            GroupId = null,
                            BackColor = offer.offerStatus.backcolor,
                            ParentId = offer.inquiry_Id,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (saleOrder != null && saleOrder.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
                {
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (saleOrder.statusClass_Id != null)
                    {
                        statusClass = saleOrder.StatusClass.ClassName;
                        statusClassBackColor = saleOrder.StatusClass.backcolor;
                    }
                    int parentId = 0;
                    if (saleOrder.offer_Id != null)
                    {
                        parentId = Convert.ToInt32(saleOrder.offer_Id);
                    }
                    else
                      if (saleOrder.moduleContract_Id != null)
                    {
                        parentId = Convert.ToInt32(saleOrder.moduleContract_Id);
                    }

                    if (saleOrder.isVoid == true)
                        stage = "Void";
                    else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
                        stage = "Closed";
                    else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
                        stage = "Closed";
                    else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (saleOrder.isApproved == true)
                        stage = "Approved";
                    else if (saleOrder.isApproved == false)
                        stage = "Under Approval";
                    else if (saleOrder.PendingForClosing == true)
                        stage = "Under Closing";

                    var vendor = "";

                    if (saleOrder.vendors.Count > 0)
                    {
                        vendor = saleOrder.vendors.First().company.CompanyName.ToString();
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = saleOrder.Id,
                        transactionType = TransactionItemType.Sale_Order,
                        Company = saleOrder.company.CompanyName,
                        Department = saleOrder.department.DeptName,
                        CreationDate = (DateTime)saleOrder.CreationDate,
                        Currency = saleOrder.currency.CurrencyName,
                        SalesReference = saleOrder.SalesReferenceNo,
                        Stage = stage,
                        Status = saleOrder.saleOrderStatus.Status,
                        AmountOC = saleOrder.totalCFRValue,
                        GroupId = null,
                        BackColor = saleOrder.saleOrderStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor

                    };
                    allTransactions.Add(view);
                }
                if (saleOrders.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
                {
                    foreach (var order in saleOrders)
                    {
                        int parentId = 0;
                        if (order.offer_Id != null)
                        {
                            parentId = Convert.ToInt32(order.offer_Id);
                        }
                        else
                          if (order.moduleContract_Id != null)
                        {
                            parentId = Convert.ToInt32(order.moduleContract_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";

                        if (order.statusClass_Id != null)
                        {
                            statusClass = order.StatusClass.ClassName;
                            statusClassBackColor = order.StatusClass.backcolor;
                        }
                        if (order.isVoid == true)
                            stage = "Void";
                        else if (order.isApproved == true && order.stage == "Closed")
                            stage = "Closed";
                        else if (order.isApproved == true && order.saleOrderStatus.isActive == false && order.PendingForClosing != true)
                            stage = "Closed";
                        else if (order.isApproved == true && order.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (order.isApproved == true)
                            stage = "Approved";
                        else if (order.isApproved == false)
                            stage = "Under Approval";
                        else if (order.PendingForClosing == true)
                            stage = "Under Closing";

                        var vendor = "";

                        if (order.vendors.Count > 0)
                        {
                            order.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = order.Id,
                            transactionType = TransactionItemType.Sale_Order,
                            Company = order.company.CompanyName,
                            Department = order.department.DeptName,
                            CreationDate = (DateTime)order.CreationDate,
                            Currency = order.currency.CurrencyName,
                            SalesReference = order.SalesReferenceNo,
                            Stage = stage,
                            Status = order.saleOrderStatus.Status,
                            AmountOC = order.totalCFRValue,
                            GroupId = null,
                            BackColor = order.saleOrderStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (invoicedPurchaseOrder != null && invoicedPurchaseOrder.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
                {
                    int parentId = 0;
                    if (invoicedPurchaseOrder.saleOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(invoicedPurchaseOrder.saleOrder_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (invoicedPurchaseOrder.statusClass_Id != null)
                    {
                        statusClass = invoicedPurchaseOrder.StatusClass.ClassName;
                        statusClassBackColor = invoicedPurchaseOrder.StatusClass.backcolor;
                    }
                    if (invoicedPurchaseOrder.isVoid == true)
                        stage = "Void";
                    else if (invoicedPurchaseOrder.isReApproved == false)
                        stage = "Under Approval";
                    else if (invoicedPurchaseOrder.isApproved == true && invoicedPurchaseOrder.stage == "Closed")
                        stage = "Closed";
                    else if (invoicedPurchaseOrder.isApproved == true && invoicedPurchaseOrder.PurchaseOrderStatus.isActive == false && invoicedPurchaseOrder.PendingForClosing != true)
                        stage = "Closed";
                    else if (invoicedPurchaseOrder.isApproved == true && invoicedPurchaseOrder.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (invoicedPurchaseOrder.isApproved == true)
                        stage = "Approved";
                    else if (invoicedPurchaseOrder.isApproved == false)
                        stage = "Under Approval";
                    else if (invoicedPurchaseOrder.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (invoicedPurchaseOrder.vendors.Count > 0)
                    {
                        vendor = invoicedPurchaseOrder.vendors.First().company.CompanyName.ToString();
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = invoicedPurchaseOrder.Id,
                        transactionType = TransactionItemType.Purchase_Order,
                        Company = invoicedPurchaseOrder.company.CompanyName,
                        Department = invoicedPurchaseOrder.department.DeptName,
                        CreationDate = (DateTime)invoicedPurchaseOrder.CreationDate,
                        Currency = invoicedPurchaseOrder.currency.CurrencyName,
                        SalesReference = invoicedPurchaseOrder.SalesReferenceNo,
                        Stage = invoicedPurchaseOrder.stage,
                        Status = invoicedPurchaseOrder.PurchaseOrderStatus.Status,
                        AmountOC = invoicedPurchaseOrder.totalCFRValue,
                        GroupId = null,
                        BackColor = invoicedPurchaseOrder.PurchaseOrderStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (saleInvoices != null && allowedPermissions.Find(x => x.Name == "List of Sale Invoices") != null)
                {
                    foreach (var saleInvoice in saleInvoices)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (saleInvoice.statusClass_Id != null)
                        {
                            statusClass = saleInvoice.StatusClass.ClassName;
                            statusClassBackColor = saleInvoice.StatusClass.backcolor;
                        }
                        if (saleInvoice.isVoid == true)
                            stage = "Void";
                        else if (saleInvoice.isApproved == true && saleInvoice.stage == "Closed")
                            stage = "Closed";
                        else if (saleInvoice.isApproved == true && saleInvoice.saleInvoiceStatus.isActive == false && saleInvoice.PendingForClosing != true)
                            stage = "Closed";
                        else if (saleInvoice.isApproved == true && saleInvoice.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (saleInvoice.isApproved == true)
                            stage = "Approved";
                        else if (saleInvoice.isApproved == false)
                            stage = "Under Approval";
                        else if (saleInvoice.PendingForClosing == true)
                            stage = "Under Closing";

                        var vendor = "";

                        if (saleInvoice.vendors.Count > 0)
                        {
                            vendor = saleInvoice.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = saleInvoice.Id,
                            transactionType = TransactionItemType.Sale_Invoice,
                            Company = saleInvoice.company.CompanyName,
                            Department = saleInvoice.department.DeptName,
                            CreationDate = (DateTime)saleInvoice.CreationDate,
                            Currency = saleInvoice.currency.CurrencyName,
                            SalesReference = saleInvoice.SalesReferenceNo,
                            Stage = stage,
                            Status = saleInvoice.saleInvoiceStatus.Status,
                            AmountOC = saleInvoice.totalInvoiceAmount,
                            GroupId = null,
                            BackColor = saleInvoice.saleInvoiceStatus.backcolor,
                            ParentId = saleOrder.Id,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (saleReceipts != null && allowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
                {
                    foreach (var receipt in saleReceipts)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (receipt.statusClass_Id != null)
                        {
                            statusClass = receipt.StatusClass.ClassName;
                            statusClassBackColor = receipt.StatusClass.backcolor;
                        }
                        if (receipt.isVoid == true)
                            stage = "Void";
                        else if (receipt.isReApproved == false)
                            stage = "Under Approval";
                        else if (receipt.isApproved == true && receipt.stage == "Closed")
                            stage = "Closed";
                        else if (receipt.isApproved == true && receipt.saleReceiptStatus.isActive == false && receipt.PendingForClosing != true)
                            stage = "Closed";
                        else if (receipt.isApproved == true && receipt.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (receipt.isApproved == true)
                            stage = "Approved";
                        else if (receipt.isApproved == false)
                            stage = "Under Approval";
                        else if (receipt.PendingForClosing == true)
                            stage = "Under Closing";

                        Department node = new Department()/*receipt.department*/;
                        switch (receipt.receiptType)
                        {
                            case ReceiptType.Customer_Credits:
                                node = receipt.saleInvoice?.department;
                                break;
                            case ReceiptType.Direct_Receipt:
                                if (receipt.payment != null)
                                {
                                    switch (receipt.payment.transactionType)
                                    {
                                        case PaymentTransactionType.Admin_Bills:
                                            node = receipt.payment.adminBill.department;
                                            break;
                                        case PaymentTransactionType.Loans_Advances:
                                            node = receipt.payment.loansAdvance.department;
                                            break;
                                        case PaymentTransactionType.Purchase_Invoice:
                                            node = receipt.payment.purchaseInvoice.department;
                                            break;
                                        case PaymentTransactionType.Target_Reward:
                                            break;
                                        case PaymentTransactionType.Vendor_Bills:
                                            node = receipt.payment.Bill.department;
                                            break;
                                    }
                                }
                                else
                                    node = receipt.department;
                                break;
                            case ReceiptType.Loans_Advances:
                                node = receipt.loansAdvance?.department;
                                break;
                            case ReceiptType.Sales_Customer:
                                node = receipt.saleInvoice?.department;
                                break;
                            case ReceiptType.Sales_Department:
                                node = receipt.saleInvoice?.department;
                                break;
                        }


                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = receipt.Id,
                            transactionType = TransactionItemType.Sale_Receipt,
                            Company = receipt.company.CompanyName,
                            Department = node.DeptName,
                            CreationDate = (DateTime)receipt.CreationDate,
                            Currency = receipt.Currency.CurrencyName,
                            SalesReference = receipt.ReceiptRefNo,
                            Stage = stage,
                            Status = receipt.saleReceiptStatus.Status,
                            AmountOC = receipt.CollectionAmount,
                            GroupId = receipt.transactionGroupId,
                            BackColor = receipt.saleReceiptStatus.backcolor,
                            ParentId = receipt.saleInvoiceId,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (purchaseOrders != null && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
                {
                    foreach (var PO in purchaseOrders)
                    {
                        int parentId = 0;
                        if (PO.saleOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(PO.saleOrder_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (PO.statusClass_Id != null)
                        {
                            statusClass = PO.StatusClass.ClassName;
                            statusClassBackColor = PO.StatusClass.backcolor;
                        }
                        if (PO.isVoid == true)
                            stage = "Void";
                        else if (PO.isReApproved == false)
                            stage = "Under Approval";
                        else if (PO.isApproved == true && PO.stage == "Closed")
                            stage = "Closed";
                        else if (PO.isApproved == true && PO.PurchaseOrderStatus.isActive == false && PO.PendingForClosing != true)
                            stage = "Closed";
                        else if (PO.isApproved == true && PO.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (PO.isApproved == true)
                            stage = "Approved";
                        else if (PO.isApproved == false)
                            stage = "Under Approval";
                        else if (PO.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (PO.vendors.Count > 0)
                        {
                            vendor = PO.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = PO.Id,
                            transactionType = TransactionItemType.Purchase_Order,
                            Company = PO.company.CompanyName,
                            Department = PO.department.DeptName,
                            CreationDate = (DateTime)PO.CreationDate,
                            Currency = PO.currency.CurrencyName,
                            SalesReference = PO.SalesReferenceNo,
                            Stage = stage,
                            Status = PO.PurchaseOrderStatus.Status,
                            AmountOC = PO.totalCFRValue,
                            GroupId = null,
                            BackColor = PO.PurchaseOrderStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (memorandumSales != null && allowedPermissions.Find(x => x.Name == "List of Memorandum Sales") != null)
                {
                    foreach (var memorandumSale in memorandumSales)
                    {
                        //allTransactions.Add(new AllTransactionsView()
                        //{
                        //    Id = memorandumSale.Id.ToString() + "_" + TransactionItemType.Memorandum_Sale.ToString(),
                        //    TransactionType = TransactionItemType.Memorandum_Sale.ToString(),
                        //    Company = memorandumSale.company.CompanyName,
                        //    CreationDate = memorandumSale.CreationDate,
                        //    Currency = memorandumSale.customerCompany.company.currency.CurrencyName,
                        //    Department = (memorandumSale.department.parentDepartment == null) ? memorandumSale.department.DeptName : memorandumSale.department.parentDepartment.DeptName + " " + memorandumSale.department.DeptName,
                        //    Customer = memorandumSale.customerCompany.company.CompanyName,
                        //    SalesReferenceNo = memorandumSale.referenceNo,
                        //    BackColor = memorandumSale.memorandumSaleStatus.backcolor,
                        //    Status = memorandumSale.memorandumSaleStatus.Status,
                        //    totalCFRValue = memorandumSale.totalCFRValue,
                        //    amountSOC = memorandumSale.totalCFRValue,
                        //    amountME = memorandumSale.totalCFRValue,
                        //    Parent_Id = (memorandumSale.SaleOrder_Id != null ? memorandumSale.SaleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : (memorandumSale.Offer_Id != null ? memorandumSale.Offer_Id.ToString() + "_" + TransactionItemType.Offer.ToString() : ""))

                        //});
                    }
                }
                if (journalVouchers1 != null)
                {
                    foreach (var vouche in journalVouchers1)
                    {
                        int parentId = 0;
                        if (vouche.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(vouche.purchaseOrder_Id);
                        }
                        string stage = "";
                        if (vouche.isVoid == true)
                            stage = "Void";
                        else if (vouche.isReApproved == false)
                            stage = "Under Approval";
                        else if (vouche.isApproved == true && vouche.stage == "Closed")
                            stage = "Closed";
                        else if (vouche.isApproved == true && vouche.JournalVoucherStatus.isActive == false && vouche.PendingForClosing != true)
                            stage = "Closed";
                        else if (vouche.isApproved == true && vouche.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (vouche.isApproved == true)
                            stage = "Approved";
                        else if (vouche.isApproved == false)
                            stage = "Under Approval";
                        else if (vouche.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (vouche.Vendor != null)
                        //{
                        //    vendor = vouche.Vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = vouche.Id,
                            transactionType = TransactionItemType.JV,
                            Company = vouche.company.CompanyName,
                            Department = vouche.department.DeptName,
                            CreationDate = (DateTime)vouche.postingDate,
                            Currency = vouche.Currency.CurrencyName,
                            SalesReference = vouche.voucherRefno,
                            Stage = stage,
                            Status = vouche.JournalVoucherStatus.Status,
                            AmountOC = vouche.journalTransactions.Sum(x => x.debit),
                            GroupId = null,
                            BackColor = vouche.JournalVoucherStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (vouchers.Count != 0)
                {

                    foreach (var jv in vouchers)
                    {
                        int parentId = 0;
                        if (jv.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(jv.purchaseOrder_Id);
                        }
                        string stage = "";
                        if (jv.isVoid == true)
                            stage = "Void";
                        else if (jv.isReApproved == false)
                            stage = "Under Approval";
                        else if (jv.isApproved == true && jv.stage == "Closed")
                            stage = "Closed";
                        else if (jv.isApproved == true && jv.JournalVoucherStatus.isActive == false && jv.PendingForClosing != true)
                            stage = "Closed";
                        else if (jv.isApproved == true && jv.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (jv.isApproved == true)
                            stage = "Approved";
                        else if (jv.isApproved == false)
                            stage = "Under Approval";
                        else if (jv.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (jv.Vendor != null)
                        //{
                        //    vendor = jv.Vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = jv.Id,
                            transactionType = TransactionItemType.JV,
                            Company = jv.company.CompanyName,
                            Department = jv.department.DeptName,
                            CreationDate = (DateTime)jv.postingDate,
                            Currency = jv.Currency.CurrencyName,
                            SalesReference = jv.voucherRefno,
                            Stage = stage,
                            Status = jv.JournalVoucherStatus.Status,
                            AmountOC = jv.journalTransactions.Sum(x => x.debit),
                            GroupId = null,
                            BackColor = jv.JournalVoucherStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (bills != null && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
                {
                    foreach (var bill in bills)
                    {
                        double? hasvalue = bill.SoAmountSOC_ER;

                        int parentId = 0;
                        if (bill.saleOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(bill.saleOrder_Id);
                        }
                        if (bill.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(bill.purchaseOrder_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (bill.statusClass_Id != null)
                        {
                            statusClass = bill.StatusClass.ClassName;
                            statusClassBackColor = bill.StatusClass.backcolor;
                        }
                            if (bill.isVoid == true)
                            stage = "Void";
                        else if (bill.isReApproved == false)
                            stage = "Under Approval";
                        else if (bill.isApproved == true && bill.stage == "Closed")
                            stage = "Closed";
                        else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                            stage = "Closed";
                        else if (bill.isApproved == true && bill.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (bill.isApproved == true)
                            stage = "Approved";
                        else if (bill.isApproved == false)
                            stage = "Under Approval";
                        else if (bill.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (bill.vendor != null)
                        {
                            vendor = bill.vendor.company.CompanyName;
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = bill.Id,
                            transactionType = TransactionItemType.Bill,
                            Company = bill.company.CompanyName,
                            Department = bill.department.DeptName,
                            CreationDate = (DateTime)bill.CreationDate,
                            Currency = bill.currency.CurrencyName,
                            SalesReference = bill.SalesReferenceNo,
                            Stage = stage,
                            Status = bill.BillStatus.Status,
                            AmountOC = bill.totalCFRValue,
                            GroupId = null,
                            BackColor = bill.BillStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (purchaseInvoices != null && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
                {
                    foreach (var invoice in purchaseInvoices)
                    {
                        int parentId = 0;
                        if (invoice.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(invoice.purchaseOrder_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (invoice.statusClass_Id != null)
                        {
                            statusClass = invoice.StatusClass.ClassName;
                            statusClassBackColor = invoice.StatusClass.backcolor;
                        }
                        if (invoice.isVoid == true)
                            stage = "Void";
                        else if (invoice.isReApproved == false)
                            stage = "Under Approval";
                        else if (invoice.isApproved == true && invoice.stage == "Closed")
                            stage = "Closed";
                        else if (invoice.isApproved == true && invoice.PurchaseInvoiceStatus.isActive == false && invoice.PendingForClosing != true)
                            stage = "Closed";
                        else if (invoice.isApproved == true && invoice.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (invoice.isApproved == true)
                            stage = "Approved";
                        else if (invoice.isApproved == false)
                            stage = "Under Approval";
                        else if (invoice.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (invoice.vendors.Count > 0)
                        {
                            vendor = invoice.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = invoice.Id,
                            transactionType = TransactionItemType.Purchase_Invoice,
                            Company = invoice.company.CompanyName,
                            Department = invoice.department.DeptName,
                            CreationDate = (DateTime)invoice.CreationDate,
                            Currency = invoice.currency.CurrencyName,
                            SalesReference = invoice.SalesReferenceNo,
                            Stage = stage,
                            Status = invoice.PurchaseInvoiceStatus.Status,
                            AmountOC = invoice.totalInvoiceAmount,
                            GroupId = null,
                            BackColor = invoice.PurchaseInvoiceStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (bankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                {
                    bankTransfers = bankTransfers.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                    stls.Add(bankTransfer.STL);
                    foreach (var transfer in bankTransfers)
                    {
                        int parentId = 0;
                        if (transfer.paymentGroupId != 0)
                        {
                            var transaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.Payments && x.Id == transfer.paymentGroupId);

                            if (transaction != null)
                            {
                                parentId = Convert.ToInt32(transaction.Id);
                            }
                            else
                            {
                                var newTransaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.Payments && x.GroupId == transfer.stlId);
                                if (transaction != null)
                                {
                                    parentId = Convert.ToInt32(transaction.Id);
                                }
                            }
                        }
                        else
                        if (transfer.receiptGroupId != 0)
                        {
                            var transaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.Sale_Receipt && x.Id == transfer.receiptGroupId);

                            if (transaction != null)
                            {
                                parentId = Convert.ToInt32(transaction.Id);
                            }
                            else
                            {
                                var newTransaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.Sale_Receipt && x.GroupId == transfer.stlId);
                                if (transaction != null)
                                {
                                    parentId = Convert.ToInt32(transaction.Id);
                                }
                            }
                        }
                        else
                        if (transfer.stlId != 0)
                        {
                            var transaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.STL && x.Id == transfer.stlId);
                            if(transaction!=null)
                            {
                                parentId = Convert.ToInt32(transaction.Id);
                            }
                            else
                            {
                                var newTransaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.STL && x.GroupId == transfer.stlId);
                                if (transaction != null)
                                {
                                    parentId = Convert.ToInt32(transaction.Id);
                                }
                            }
                        }

                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (transfer.statusClass_Id != null)
                        {
                            statusClass = transfer.StatusClass.ClassName;
                            statusClassBackColor = transfer.StatusClass.backcolor;
                        }
                        if (transfer.isVoid == true)
                            stage = "Void";
                        else if (transfer.isReApproved == false)
                            stage = "Under Approval";
                        else if (transfer.isApproved == true && transfer.stage == "Closed")
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.interBankTransStatus.isActive == false && transfer.PendingForClosing != true)
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (transfer.isApproved == true)
                            stage = "Approved";
                        else if (transfer.isApproved == false)
                            stage = "Under Approval";
                        else if (transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        string currency = "";
                        double amount = 0;
                        if (transfer.transferType == TransferType.IBT_Single_Currency)
                        {
                            currency = transfer.currency.CurrencyName;

                            amount = transfer.AmountOC;
                        }
                        else
                        {
                            if (transfer.currencyFrom != null)
                                currency = transfer.currencyFrom.CurrencyName;
                            amount = transfer.AmountFrom;
                        }
                        string vendor = "";
                        //if (transfer.vendor != null)
                        //{
                        //    vendor = transfer.vendor.company.CompanyName;
                        //}

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = transfer.Id,
                            transactionType = TransactionItemType.InterBank_Transfer,
                            Company = transfer.company.CompanyName,
                            Department = transfer.department.DeptName,
                            CreationDate = (DateTime)transfer.CreationDate,
                            Currency = currency,
                            SalesReference = transfer.FinanceRefNo,
                            Stage = stage,
                            Status = transfer.interBankTransStatus.Status,
                            AmountOC = amount,
                            GroupId = null,
                            ParentId= parentId,
                            BackColor = transfer.interBankTransStatus.backcolor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (companyBankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                {
                    foreach (var transfer in companyBankTransfers)
                    {
                        int parentId = 0;
                        if (transfer.paymentGroupId != 0)
                        {
                            parentId = Convert.ToInt32(transfer.paymentGroupId);
                        }
                        else
                        if (transfer.receiptGroupId != 0)
                        {
                            parentId = Convert.ToInt32(transfer.receiptGroupId);
                        }
                        else
                             if (transfer.stlId != null)
                        {
                            parentId = Convert.ToInt32(transfer.stlId);

                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (transfer.statusClass_Id != null)
                        {
                            statusClass = transfer.StatusClass.ClassName;
                            statusClassBackColor = transfer.StatusClass.backcolor;
                        }
                        if (transfer.isVoid == true)
                            stage = "Void";
                        else if (transfer.isReApproved == false)
                            stage = "Under Approval";
                        else if (transfer.isApproved == true && transfer.stage == "Closed")
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.interBankTransStatus.isActive == false && transfer.PendingForClosing != true)
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (transfer.isApproved == true)
                            stage = "Approved";
                        else if (transfer.isApproved == false)
                            stage = "Under Approval";
                        else if (transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        string currency = "";
                        if (transfer.transferType == TransferType.IBT_Single_Currency)
                        {
                            currency = transfer.currency.CurrencyName;
                        }
                        else
                        if (transfer.transferType == TransferType.IBT_Multiple_Currency)
                        {
                            currency = transfer.currencyFrom.CurrencyName;
                        }

                        string vendor = "";
                        //if (transfer.vendor != null)
                        //{
                        //    vendor = transfer.vendor.company.CompanyName;
                        //}
                        stls.Add(transfer.STL);
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = transfer.Id,
                            transactionType = TransactionItemType.InterCompanyBank_Transfer,
                            Company = transfer.companyTo.CompanyName,
                            Department = transfer.departmentTo.DeptName,
                            CreationDate = (DateTime)transfer.CreationDate,
                            Currency = currency,
                            SalesReference = transfer.FinanceRefNo,
                            Stage = stage,
                            Status = transfer.interBankTransStatus.Status,
                            AmountOC = transfer.AmountFrom,
                            GroupId = null,
                            BackColor = transfer.interBankTransStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (stl != null && stl.Id != 0 && allowedPermissions.Find(x => x.Name == "List of STL") != null)
                {
                    int parentId = 0;
                    //if (sTL.purchaseOrder_Id != null)
                    //{
                    //    parentId = Convert.ToInt32(sTL.purchaseOrder_Id);
                    //}
                    string stage = "";
                    if (stl.isVoid == true)
                        stage = "Void";
                    else if (stl.isReApproved == false)
                        stage = "Under Approval";
                    else if (stl.isApproved == true && stl.stage == "Closed")
                        stage = "Closed";
                    else if (stl.isApproved == true && stl.stlStatus.isActive == false && stl.PendingForClosing != true)
                        stage = "Closed";
                    else if (stl.isApproved == true && stl.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (stl.isApproved == true)
                        stage = "Approved";
                    else if (stl.isApproved == false)
                        stage = "Under Approval";
                    else if (stl.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    //if (voucher.Vendor != null)
                    //{
                    //    vendor = voucher.Vendor.company.CompanyName;
                    //}
                    PaymentRepo paymentRepo = new PaymentRepo();
                    var paym = paymentRepo.GetPaymentByGroupId((int)stl.paymentGroupId, stl.paymentAmountOC);
                    if (payment != null)
                    {
                        parentId = payment.Id;
                    }
                    else
                    {
                        //SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                        //var rcpt =receiptRepo.getReceiptsByGroupId((int)stl.paymentGroupId);
                    }
                    bankTransfers.AddRange(stl.InterBankTransfers);
                    companyBankTransfers.AddRange(stl.InterCompanyBankTransfers);
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = stl.Id,
                        transactionType = TransactionItemType.STL,
                        Company = stl.company.CompanyName,
                        Department = stl.department.DeptName,
                        CreationDate = (DateTime)stl.CreationDate,
                        Currency = stl.paymentCurrency.CurrencyName,
                        //SalesReference = sTL.ref,
                        Stage = stage,
                        Status = stl.stlStatus.Status,
                        AmountOC = stl.paymentAmountOC,
                        GroupId = null,
                        BackColor = stl.stlStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor
                    };

                    allTransactions.Add(view);
                }

                allTransactions = allTransactions
    .GroupBy(x => new { x.Id, x.transactionType }) // Group by both Id and TransactionType
    .Select(y => y.FirstOrDefault()) // Select the first item from each group
    .ToList();
                return allTransactions;
            }
            catch ( Exception ex)
            {
                return null;
            }
        }
        public List<AllOrdersView> getAllOrderTrackingNew(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {
            try
            {
                Inquiry inquiry = new Inquiry();
                Offer offer = new Offer();
                InterBankTransfer bankTransfer = new InterBankTransfer();
                SaleOrder saleOrder = new SaleOrder();
                PurchaseOrder invoicedPurchaseOrder = new PurchaseOrder();
                List<SaleInvoice> saleInvoices = new List<SaleInvoice>();
                List<PurchaseInvoice> purchaseInvoices = new List<PurchaseInvoice>();
                PurchaseInvoice PInvoice = new PurchaseInvoice();
                List<MemorandumSale> memorandumSales = new List<MemorandumSale>();
                List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
                List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
                List<Bill> bills = new List<Bill>();
                Bill billl = new Bill();
                List<JournalVoucher> journalVouchers = new List<JournalVoucher>();
                AdminBill adminBill = new AdminBill();
                LoansAdvance loansAdvance = new LoansAdvance();
                TargetRewards targetReward = new TargetRewards();
                Payment payment = new Payment();
                List<Payment> payments = new List<Payment>();
                List<STL> Stls = new List<STL>();
                List<InterBankTransfer> bankTransfers = new List<InterBankTransfer>();
                List<JournalVoucher> vouchers = new List<JournalVoucher>();
                List<InterCompanyBankTransfer> companyBankTransfers = new List<InterCompanyBankTransfer>();
                List<AllOrdersView> allTransactions = new List<AllOrdersView>();
                InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();
                JournalVoucherRepo voucherRepo = new JournalVoucherRepo();
                switch (type)
                {
                    case Enums.TransactionItemType.Inquiry:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        inquiry = context.inquiries
                            .FirstOrDefault(x => x.Id == Id);
                        if (inquiry != null)
                        {
                            offer = context.offers.FirstOrDefault(x => x.inquiry_Id == Id);
                        }
                        else
                        {
                            offer = null;
                        }
                        if (offer != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.offer_Id == offer.Id);
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        }
                        else
                        {
                            saleInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders

                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count != 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count != 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                           .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)

                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        break;

                    //Offer
                    case Enums.TransactionItemType.Offer:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        offer = context.offers.FirstOrDefault(x => x.Id == Id);

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.offer_Id == offer.Id);
                        }
                        else
                        {
                            saleOrder = null;
                            inquiry = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        }
                        else
                        {
                            saleInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count != 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count != 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                      .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)

                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        break;

                    //SaleOrder
                    case Enums.TransactionItemType.Sale_Order:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == Id);
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            bankTransfer = context.interBankTransfers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }

                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                         .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        break;
                    //Sale Invoices
                    case Enums.TransactionItemType.Sale_Invoice:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var invoice = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
                        if (invoice != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == invoice.SaleOrderId);
                        }
                        else
                        {
                            saleOrder = null;
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }

                        }
                        else
                        {
                            purchaseOrders = null;
                        }


                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        break;
                    //Sale Invoices
                    case Enums.TransactionItemType.Sale_Receipt:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var receipt = context.salesReceipts
                       .FirstOrDefault(x => x.Id == Id);
                        if (receipt != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                        }
                        else
                        {
                            saleOrder = null;
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                        break;

                    case Enums.TransactionItemType.Admin_Bill:

                        adminBill = context.adminBills.FirstOrDefault(x => x.Id == Id);
                        if (adminBill.Payments != null)
                        {
                            var paymentss = adminBill.Payments;

                            PaymentRepo paymentRepo = new PaymentRepo();
                            foreach (var _paymnt in paymentss)
                            {
                                payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                            }
                        }
                        else
                        {
                            payments = null;
                        }

                        payment = null;
                        billl = null;
                        inquiry = null;
                        offer = null;
                        saleOrder = null;
                        saleInvoices = null;
                        purchaseOrders = null;
                        bills = null;
                        purchaseInvoices = null;
                        journalVouchers = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        //if (adminBill != null)
                        //{
                        //    saleOrder = context.saleOrders
                        //    .Include("offer")
                        //    .Include("offer.inquiry")
                        //    .Include("company")
                        //    .Include("employee")
                        //    .Include("department").Include("SaleInvoices").Include("SaleOrderStatus")
                        //    .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                        //}
                        break;

                    case Enums.TransactionItemType.Payments:
                        payment = context.payments.FirstOrDefault(x => x.Id == Id);

                        if(payment.salesReceipts != null && payment.salesReceipts.Count > 0)
                        {
                            saleReceipts = payment.salesReceipts;
                        }

                        if (payment != null && payment.transactionGroupId != 0)
                        {
                            companyBankTransfers = context.interCompanyBankTransfers
                                .Where(x => x.paymentGroupId == payment.transactionGroupId).ToList();
                            vouchers = context.journalVouchers
                                .Where(x => x.paymentGroupId == payment.transactionGroupId).ToList();
                            Stls = context.STLs.Where(x => x.paymentGroupId == payment.transactionGroupId).ToList();
                            foreach(var stl in Stls)
                            {
                                bankTransfers.AddRange(stl.InterBankTransfers);
                                companyBankTransfers.AddRange(stl.InterCompanyBankTransfers);
                            }
                        }
                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Admin_Bills)
                        {
                            if (payment.AdminBill_Id != null)
                            {
                                Employee empUser = new Employee();
                                EmployeeRepo empRepo = new EmployeeRepo();
                                empUser = empRepo.GetEmployeeForPayments(empID);
                                adminBill = context.adminBills.FirstOrDefault(x => x.Id == payment.AdminBill_Id);
                                if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                    adminBill = null;
                            }
                            else
                            {
                                adminBill = null;
                            }
                            if (adminBill != null)
                            {
                                payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                            }
                            bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                            vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                            billl = null;
                            PInvoice = null;
                            saleOrder = null;
                            offer = null;
                            inquiry = null;
                            payment = null;
                            invoicedPurchaseOrder = null;

                        }

                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Vendor_Bills)
                        {
                            Employee empUser = new Employee();
                            EmployeeRepo empRepo = new EmployeeRepo();
                            if (payment.Bill_Id != null)
                            {

                                empUser = empRepo.GetEmployeeForPayments(empID);

                                billl = context.bills
                                .FirstOrDefault(x => x.Id == payment.Bill_Id);

                                if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                    billl = null;
                            }
                            else
                            {
                                billl = null;
                            }

                            if (billl != null)
                            {

                                if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                {
                                    saleOrder = context.saleOrders

                                        .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                    if (saleOrder != null)
                                        if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                            saleOrder = null;
                                }
                                else
                                {
                                    saleOrder = null;
                                }

                                if (saleOrder == null)
                                {
                                    PurchaseOrder purchaseOrder = new PurchaseOrder();
                                    if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                    {
                                        purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                        if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                            purchaseOrder = null;
                                    }
                                    else
                                    {
                                        purchaseOrder = null;
                                    }

                                    if (purchaseOrder != null)
                                    {
                                        saleOrder = context.saleOrders

                                        .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                        purchaseOrders.Add(purchaseOrder);
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                    }
                                }

                                journalVouchers = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                            }
                            else
                            {
                                saleOrder = null;
                            }
                            if (saleOrder != null)
                            {
                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers

                                  .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (saleOrder != null)
                            {
                                purchaseOrders = context.purchaseOrders

                                .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                if (purchaseOrders.Count != 0)
                                {
                                    foreach (var purchaseOrder in purchaseOrders)
                                    {
                                        if (purchaseOrder.PurchaseInvoices.Count != 0)
                                            purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                    }
                                }
                            }
                            if (purchaseInvoices != null)
                            {
                                foreach (var _PI in purchaseInvoices)
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments);
                            }
                            //else
                            //{
                            //    purchaseOrders = null;
                            //}

                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                            }
                            else
                            {

                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            if (saleOrder != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                            }
                            else if (offer != null)
                            {
                                memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                            }
                            else
                            {
                                memorandumSales = null;
                            }
                            bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                            vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                            PInvoice = null;
                            //payments = null;
                            payment = null;
                            invoicedPurchaseOrder = null;
                            adminBill = null;
                            loansAdvance = null;
                        }



                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                        {

                            if (payment.PInvoice_Id != null)
                            {
                                Employee empUser = new Employee();
                                EmployeeRepo empRepo = new EmployeeRepo();
                                empUser = empRepo.GetEmployeeForPayments(empID);
                                PInvoice = context.purchaseInvoices
                                .FirstOrDefault(x => x.Id == payment.PInvoice_Id);
                                if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                    PInvoice = null;
                            }
                            else
                            {
                                PInvoice = null;
                            }
                            if (PInvoice != null)
                            {
                                invoicedPurchaseOrder = context.purchaseOrders

                                .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                            }
                            else
                            {
                                invoicedPurchaseOrder = null;
                            }
                            if (invoicedPurchaseOrder != null)
                            {
                                saleOrder = context.saleOrders

                                .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                    purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                {
                                    purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                }

                                if (purchaseOrders != null)
                                    foreach (var _po in purchaseOrders)
                                        purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                purchaseInvoices.Distinct();

                                if (purchaseInvoices != null)
                                    foreach (var _pi in purchaseInvoices)
                                        payments.AddRange(_pi.Payments);

                                payments.Distinct();
                            }
                            else
                            {
                                saleOrder = null;
                            }
                            if (saleOrder != null)
                            {

                                saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                            }
                            else
                            {
                                saleInvoices = null;
                                offer = null;
                            }
                            if (offer != null)
                            {
                                inquiry = context.inquiries
                               .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                            }
                            else
                            {

                                inquiry = null;
                            }
                            if (saleInvoices != null)
                            {
                                if(saleReceipts != null)
                                {
                                    saleReceipts.AddRange( context.salesReceipts.Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList());
                                }
                                else
                                    saleReceipts = context.salesReceipts.Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                            }
                            else
                            {
                                saleReceipts = null;
                            }
                            bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int) payment.company_Id);


                            adminBill = null;
                            billl = null;
                            bills = null;
                            //purchaseInvoices = null;
                            payment = null;
                            journalVouchers = null;
                        }

                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Loans_Advances)
                        {

                            if (payment.LoansAdvanceId != null)
                            {
                                Employee empUser = new Employee();
                                EmployeeRepo empRepo = new EmployeeRepo();
                                empUser = empRepo.GetEmployeeForPayments(empID);
                                loansAdvance = context.loansAdvances

                                .FirstOrDefault(x => x.Id == payment.LoansAdvanceId);
                                if (empUser.departments.FirstOrDefault(x => x.Id == loansAdvance.department.Id) == null)
                                    loansAdvance = null;
                            }
                            else
                            {
                                loansAdvance = null;
                            }
                            if (loansAdvance != null)
                            {
                                payments = context.payments.Where(x => x.LoansAdvanceId == loansAdvance.Id).ToList();
                            }
                            bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int) payment.company_Id);
                            vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                            billl = null;
                            PInvoice = null;
                            saleOrder = null;
                            offer = null;
                            inquiry = null;
                            payment = null;
                            invoicedPurchaseOrder = null;
                            adminBill = null;

                        }
                        if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Target_Reward)
                        {

                            if (payment.TargetReward_Id != null)
                            {
                                Employee empUser = new Employee();
                                EmployeeRepo empRepo = new EmployeeRepo();
                                empUser = empRepo.GetEmployeeForPayments(empID);
                                targetReward = context.targetRewards

                                .FirstOrDefault(x => x.Id == payment.TargetReward_Id);
                                //if (empUser.departments.FirstOrDefault(x => x.Id == loansAdvance.department.Id) == null)
                                //    loansAdvance = null;
                            }
                            else
                            {
                                targetReward = null;
                            }
                            if (targetReward != null)
                            {
                                payments = context.payments.Where(x => x.TargetReward_Id == targetReward.Id).ToList();
                            }
                            bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(payment.transactionGroupId, empID, (int)payment.company_Id);
                            vouchers = voucherRepo.GetVouchersByGroupId(payment.transactionGroupId);
                            billl = null;
                            PInvoice = null;
                            saleOrder = null;
                            offer = null;
                            inquiry = null;
                            payment = null;
                            invoicedPurchaseOrder = null;
                            adminBill = null;

                        }
                        break;

                    case Enums.TransactionItemType.Purchase_Order:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var PurchaseOrder = context.purchaseOrders
                            .FirstOrDefault(x => x.Id == Id);
                        if (PurchaseOrder != null)
                        {
                            // purchaseInvoices = context.purchaseInvoices.Include("PurchaseInvoiceStatus")
                            //.Include("customerCompany.company").Include("PurchaseOrder")
                            //.Include("company").Include("employee")
                            //.Include("department").Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == PurchaseOrder.saleOrder_Id);
                        }
                        else
                        {
                            saleOrder = null;
                            purchaseInvoices = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else if (PurchaseOrder != null)
                        {
                            bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else if (PurchaseOrder != null)
                        {
                            purchaseOrders.Add(PurchaseOrder);
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                            purchaseInvoices = null;
                        }


                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        journalVouchers = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();

                        break;
                    case Enums.TransactionItemType.Bill:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;

                        var bill = context.bills.FirstOrDefault(x => x.Id == Id);
                        if (bill != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == bill.saleOrder_Id);
                            if (saleOrder == null)
                            {
                                var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == bill.purchaseOrder_Id);
                                if (purchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders

                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                    bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList();
                                    purchaseOrders.Add(purchaseOrder);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                                else
                                {
                                    bills.Add(bill);
                                }
                            }
                            if (saleOrder != null)
                            {
                                bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            }
                            journalVouchers = context.journalVouchers.Where(x => x.bill_Id == bill.Id).ToList();

                            payments = context.payments.Where(x => x.Bill_Id == bill.Id).ToList();

                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers
                                  .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                        }

                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                   .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }










                        break;
                    //Memorandum Sale
                    case Enums.TransactionItemType.Memorandum_Sale:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        invoicedPurchaseOrder = null;
                        var memorandumSale = context.memorandumSales.Where(x => x.Id == Id).FirstOrDefault();
                        if (memorandumSale != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == memorandumSale.SaleOrder_Id);
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices
                           .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = context.offers.FirstOrDefault(x => x.Id == memorandumSale.Offer_Id);
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        //else
                        //{
                        //    purchaseOrders = null;
                        //}
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                         .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else if (memorandumSale != null)
                        {
                            memorandumSales.Add(memorandumSale);
                        }
                        break;


                    case Enums.TransactionItemType.Purchase_Invoice:
                        adminBill = null;
                        payment = null;
                        billl = null;
                        payments = null;
                        PInvoice = null;
                        var purchaseInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == Id);
                        if (purchaseInvoice != null)
                        {
                            invoicedPurchaseOrder = context.purchaseOrders

                            .FirstOrDefault(x => x.Id == purchaseInvoice.purchaseOrder_Id);

                            payments = context.payments

                            .Where(x => x.PInvoice_Id == purchaseInvoice.Id).ToList();
                        }
                        else
                        {
                            invoicedPurchaseOrder = null;
                        }
                        if (invoicedPurchaseOrder != null)
                        {
                            saleOrder = context.saleOrders

                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts

                         .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        purchaseInvoices = context.purchaseInvoices.Where(x => x.isVoid != true && x.purchaseOrder_Id == purchaseInvoice.purchaseOrder_Id).ToList();

                        //purchaseInvoices.Add(purchaseInvoice);

                        break;
                        //Purchase Invoices


                }
                if (payment != null && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
                {
                    int parentId = 0;
                    if (payment.PInvoice_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.PInvoice_Id);
                    }
                    if (payment.Bill_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.Bill_Id);
                    }
                    if (payment.TargetReward_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.TargetReward_Id);
                    }
                    if (payment.AdminBill_Id != null)
                    {
                        parentId = Convert.ToInt32(payment.AdminBill_Id);
                    }
                    if (payment.LoansAdvanceId != null)
                    {
                        parentId = Convert.ToInt32(payment.LoansAdvanceId);
                    }
                    string stage = "";
                    if (payment.isVoid == true)
                        stage = "Void";
                    else if (payment.isReApproved == false)
                        stage = "Under Approval";
                    else if (payment.isApproved == true && payment.stage == "Closed")
                        stage = "Closed";
                    else if (payment.isApproved == true && payment.Status.isActive == false && payment.PendingForClosing != true)
                        stage = "Closed";
                    else if (payment.isApproved == true && payment.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (payment.isApproved == true)
                        stage = "Approved";
                    else if (payment.isApproved == false)
                        stage = "Under Approval";
                    else if (payment.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (payment.vendor != null)
                    {
                        vendor = payment.vendor.company.CompanyName;
                    }
                    Stls.AddRange(context.STLs.Where(x => x.paymentGroupId == payment.transactionGroupId).ToList());
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = payment.Id,
                        transactionType = TransactionItemType.Payments,
                        Company = payment.company.CompanyName,
                        //Department = payment.departments.DeptName,
                        CreationDate = (DateTime)payment.CreationDate,
                        Currency = payment.currency.CurrencyName,
                        SalesReference = payment.BillFinanceRefNo,
                        Stage = stage,
                        Status = payment.Status.Status,
                        StatusClass = payment.StatusClass.ClassName,
                        AmountOC = payment.DebitedAmount,
                        GroupId = payment.transactionGroupId,
                        BackColor = payment.Status.backcolor,
                        ParentId = parentId,
                        Vendor = vendor
                    };
                    allTransactions.Add(view);
                }
                if (payments != null && allowedPermissions.Find(x => x.Name == "List of Payment") != null)
                {
                    foreach (var _payment in payments)
                    {
                        string parentId = "";
                        if (_payment.PInvoice_Id != null)
                        {

                            parentId = (_payment.PInvoice_Id != null ? _payment.PInvoice_Id.ToString() : "");


                            //parentId = (int)_payment.PInvoice_Id;
                        }
                        else
                        if (_payment.AdminBill_Id != null)
                        {
                            parentId = (_payment.AdminBill_Id != null ? _payment.AdminBill_Id.ToString() : "");

                            //parentId = (int)_payment.AdminBill_Id;

                        }
                        else
                        if (_payment.Bill_Id != null)
                        {
                            parentId = (_payment.Bill_Id != null ? _payment.Bill_Id.ToString() : "");

                        }
                        else
                            if (_payment.LoansAdvanceId != null)
                        {
                            parentId = (_payment.LoansAdvanceId != null ? _payment.LoansAdvanceId.ToString() : "");

                        }
                        else
                            if (_payment.TargetReward_Id != null)
                        {
                            parentId = (_payment.TargetReward_Id != null ? _payment.TargetReward_Id.ToString() : "");
                        }

                        string stage = "";
                        if (_payment.isVoid == true)
                            stage = "Void";
                        else if (_payment.isReApproved == false)
                            stage = "Under Approval";
                        else if (_payment.isApproved == true && _payment.stage == "Closed")
                            stage = "Closed";
                        else if (_payment.isApproved == true && _payment.Status.isActive == false && _payment.PendingForClosing != true)
                            stage = "Closed";
                        else if (_payment.isApproved == true && _payment.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (_payment.isApproved == true)
                            stage = "Approved";
                        else if (_payment.isApproved == false)
                            stage = "Under Approval";
                        else if (_payment.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (_payment.vendor != null)
                        {
                            vendor = _payment.vendor.company.CompanyName;
                        }
                        Stls.AddRange(context.STLs.Where(x => x.paymentGroupId == _payment.transactionGroupId).ToList());

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = _payment.Id,
                            transactionType = TransactionItemType.Payments,
                            Company = _payment.company.CompanyName,
                            //Department = payment.departments.DeptName,
                            CreationDate = (DateTime)_payment.CreationDate,
                            Currency = _payment.currency.CurrencyName,
                            SalesReference = _payment.BillFinanceRefNo,
                            Stage = stage,
                            Status = _payment.Status.Status,
                            //StatusClass = _payment.StatusClass.ClassName,
                            AmountOC = _payment.DebitedAmount,
                            GroupId = _payment.transactionGroupId,
                            BackColor = _payment.Status.backcolor,
                            ParentId = Convert.ToInt32(parentId),
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (Stls.Count != 0)
                {
                    foreach (var sTL in Stls)
                    {
                        //allTransactions.Add(new AllTransactionsView()
                        //{
                        //    Id = sTL.Id.ToString() + "_" + TransactionItemType.STL.ToString(),
                        //    TransactionType = TransactionItemType.STL.ToString(),
                        //    Company = sTL.company.CompanyName,
                        //    Employee = sTL.user.employee.person.FName + " " + sTL.user.employee.person.FName,
                        //    CreationDate = sTL.CreationDate,
                        //    Currency = sTL.stlCurrency.CurrencyName,
                        //    Department = (sTL.department.parentDepartment == null) ? sTL.department.DeptName : sTL.department.parentDepartment.DeptName + " " + sTL.department.DeptName,
                        //    //SalesReferenceNo = sTL.voucherRefno,
                        //    BackColor = sTL.stlStatus.backcolor,
                        //    Status = sTL.stlStatus.Status,
                        //    totalCFRValue = sTL.stlPaymentAmountOC
                        //});
                        int parentId = 0;
                        //if (sTL.purchaseOrder_Id != null)
                        //{
                        //    parentId = Convert.ToInt32(sTL.purchaseOrder_Id);
                        //}
                        string stage = "";
                        if (sTL.isVoid == true)
                            stage = "Void";
                        else if (sTL.isReApproved == false)
                            stage = "Under Approval";
                        else if (sTL.isApproved == true && sTL.stage == "Closed")
                            stage = "Closed";
                        else if (sTL.isApproved == true && sTL.stlStatus.isActive == false && sTL.PendingForClosing != true)
                            stage = "Closed";
                        else if (sTL.isApproved == true && sTL.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (sTL.isApproved == true)
                            stage = "Approved";
                        else if (sTL.isApproved == false)
                            stage = "Under Approval";
                        else if (sTL.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (voucher.Vendor != null)
                        //{
                        //    vendor = voucher.Vendor.company.CompanyName;
                        //}
                        PaymentRepo paymentRepo = new PaymentRepo();
                        var paym = paymentRepo.GetPaymentByGroupId((int)sTL.paymentGroupId, sTL.paymentAmountOC);
                        if (paym != null)
                        {
                            parentId = paym.Id;
                        }
                        else
                        {
                            //SalesReceiptRepo receiptRepo = new SalesReceiptRepo();
                            //var rcpt =receiptRepo.getReceiptsByGroupId((int)stl.paymentGroupId);
                        }
                        payments.Add(paym);
                        bankTransfers.AddRange(sTL.InterBankTransfers);
                        companyBankTransfers.AddRange(sTL.InterCompanyBankTransfers);
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = sTL.Id,
                            transactionType = TransactionItemType.STL,
                            Company = sTL.company.CompanyName,
                            Department = sTL.department.DeptName,
                            CreationDate = (DateTime)sTL.CreationDate,
                            Currency = sTL.paymentCurrency.CurrencyName,
                            //SalesReference = sTL.ref,
                            Stage = stage,
                            Status = sTL.stlStatus.Status,
                            AmountOC = sTL.paymentAmountOC,
                            GroupId = null,
                            BackColor = sTL.stlStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);


                    }
                }
                companyBankTransfers = companyBankTransfers.GroupBy(x => x.Id).Select(g => g.First()).ToList();
                bankTransfers = bankTransfers.GroupBy(x => x.Id).Select(g => g.First()).ToList();

                if (companyBankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                {
                    foreach (var transfer in companyBankTransfers)
                    {
                        int parentId = 0;
                        if (transfer.paymentGroupId != 0)
                        {
                            parentId = Convert.ToInt32(transfer.paymentGroupId);
                        }
                        else
                        if (transfer.receiptGroupId != 0)
                        {
                            parentId = Convert.ToInt32(transfer.receiptGroupId);
                        }
                        else
                        if (transfer.stlId != 0)
                        {
                            parentId = Convert.ToInt32(transfer.stlId);
                        }
                        string stage = "";
                        if (transfer.isVoid == true)
                            stage = "Void";
                        else if (transfer.isReApproved == false)
                            stage = "Under Approval";
                        else if (transfer.isApproved == true && transfer.stage == "Closed")
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.interBankTransStatus.isActive == false && transfer.PendingForClosing != true)
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (transfer.isApproved == true)
                            stage = "Approved";
                        else if (transfer.isApproved == false)
                            stage = "Under Approval";
                        else if (transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        string currency = "";
                        if (transfer.transferType == TransferType.IBT_Single_Currency)
                        {
                            currency = transfer.currency.CurrencyName;
                        }
                        else
                        {
                            currency = transfer.currencyFrom?.CurrencyName;
                        }
                        string vendor = "";
                        //if (transfer.vendor != null)
                        //{
                        //    vendor = transfer.vendor.company.CompanyName;
                        //}

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = transfer.Id,
                            transactionType = TransactionItemType.InterCompanyBank_Transfer,
                            Company = transfer.companyTo.CompanyName,
                            Department = transfer.departmentTo.DeptName,
                            CreationDate = (DateTime)transfer.CreationDate,
                            Currency = currency,
                            SalesReference = transfer.FinanceRefNo,
                            Stage = stage,
                            Status = transfer.interBankTransStatus.Status,
                            AmountOC = transfer.AmountFrom,
                            GroupId = null,
                            BackColor = transfer.interBankTransStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (bankTransfers.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Inter-Bank Transfer") != null)
                {
                    foreach (var transfer in bankTransfers)
                    {
                        int parentId = 0;
                        if (transfer.paymentGroupId != 0)
                        {
                             var transaction=allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.Payments && x.GroupId == transfer.paymentGroupId);
                            parentId = Convert.ToInt32(transaction.Id);
                        }
                        else
                        if (transfer.receiptGroupId != 0)
                        {
                            var transaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.Sale_Receipt && x.GroupId == transfer.receiptGroupId);

                            parentId = Convert.ToInt32(transaction.Id);
                        }
                        else
                        if (transfer.stlId != 0)
                        {
                            var transaction = allTransactions.FirstOrDefault(x => x.transactionType == TransactionItemType.STL && x.Id == transfer.stlId);
                            parentId = Convert.ToInt32(transaction.Id);
                        }


                        string stage = "";
                        if (transfer.isVoid == true)
                            stage = "Void";
                        else if (transfer.isReApproved == false)
                            stage = "Under Approval";
                        else if (transfer.isApproved == true && transfer.stage == "Closed")
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.interBankTransStatus.isActive == false && transfer.PendingForClosing != true)
                            stage = "Closed";
                        else if (transfer.isApproved == true && transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (transfer.isApproved == true)
                            stage = "Approved";
                        else if (transfer.isApproved == false)
                            stage = "Under Approval";
                        else if (transfer.PendingForClosing == true)
                            stage = "Under Closing";
                        string currency = "";
                        double amount = 0;
                        if (transfer.transferType == TransferType.IBT_Single_Currency)
                        {
                            currency = transfer.currency.CurrencyName;

                            amount = transfer.AmountOC;
                        }
                        else
                        {
                            currency = transfer.currencyFrom.CurrencyName;
                            amount = transfer.AmountFrom;
                        }
                        string vendor = "";
                        //if (transfer.vendor != null)
                        //{
                        //    vendor = transfer.vendor.company.CompanyName;
                        //}

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = transfer.Id,
                            transactionType = TransactionItemType.InterBank_Transfer,
                            Company = transfer.company.CompanyName,
                            Department = transfer.department.DeptName,
                            CreationDate = (DateTime)transfer.CreationDate,
                            Currency = currency,
                            SalesReference = transfer.FinanceRefNo,
                            Stage = stage,
                            Status = transfer.interBankTransStatus.Status,
                            AmountOC = amount,
                            GroupId = null,
                            BackColor = transfer.interBankTransStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (vouchers.Count != 0)
                {
                    foreach (var voucher in vouchers)
                    {
                        int parentId = 0;
                        if (voucher.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(voucher.purchaseOrder_Id);
                        }
                        string stage = "";
                        if (voucher.isVoid == true)
                            stage = "Void";
                        else if (voucher.isReApproved == false)
                            stage = "Under Approval";
                        else if (voucher.isApproved == true && voucher.stage == "Closed")
                            stage = "Closed";
                        else if (voucher.isApproved == true && voucher.JournalVoucherStatus.isActive == false && voucher.PendingForClosing != true)
                            stage = "Closed";
                        else if (voucher.isApproved == true && voucher.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (voucher.isApproved == true)
                            stage = "Approved";
                        else if (voucher.isApproved == false)
                            stage = "Under Approval";
                        else if (voucher.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (voucher.Vendor != null)
                        //{
                        //    vendor = voucher.Vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = voucher.Id,
                            transactionType = TransactionItemType.JV,
                            Company = voucher.company.CompanyName,
                            Department = voucher.department.DeptName,
                            CreationDate = (DateTime)voucher.postingDate,
                            Currency = voucher.Currency.CurrencyName,
                            SalesReference = voucher.voucherRefno,
                            Stage = stage,
                            Status = voucher.JournalVoucherStatus.Status,
                            AmountOC = voucher.journalTransactions.Sum(x => x.debit),
                            GroupId = null,
                            BackColor = voucher.JournalVoucherStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (loansAdvance != null && loansAdvance.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Loans and Advances") != null)
                {
                    int parentId = 0;
                    string stage = "";
                    if (loansAdvance.isVoid == true)
                        stage = "Void";
                    else if (loansAdvance.isReApproved == false)
                        stage = "Under Approval";
                    else if (loansAdvance.isApproved == true && loansAdvance.stage == "Closed")
                        stage = "Closed";
                    else if (loansAdvance.isApproved == true && loansAdvance.Status.isActive == false && loansAdvance.PendingForClosing != true)
                        stage = "Closed";
                    else if (loansAdvance.isApproved == true && loansAdvance.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (loansAdvance.isApproved == true)
                        stage = "Approved";
                    else if (loansAdvance.isApproved == false)
                        stage = "Under Approval";
                    else if (loansAdvance.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";

                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = loansAdvance.Id,
                        transactionType = TransactionItemType.LoansAdvances,
                        //Company = targetReward.company.CompanyName,
                        //Department = targetReward.department.DeptName,
                        CreationDate = (DateTime)loansAdvance.CreationDate,
                        Currency = loansAdvance.currency.CurrencyName,
                        SalesReference = loansAdvance.SystemRef,
                        Stage = stage,
                        Status = loansAdvance.Status.Status,
                        AmountOC = loansAdvance.LoanAmountOC,
                        GroupId = null,
                        BackColor = loansAdvance.Status.backcolor,
                        ParentId = parentId,
                        Vendor = vendor
                    };
                    allTransactions.Add(view);
                }
                if (targetReward != null && targetReward.Id != 0 && allowedPermissions.Find(x => x.Name == "View List of Target Rewards") != null)
                {
                    int parentId = 0;
                    if (targetReward.toDoTask_Id != null)
                    {
                        parentId = Convert.ToInt32(targetReward.toDoTask_Id);
                    }
                    string stage = "";
                    if (targetReward.isVoid == true)
                        stage = "Void";
                    else if (targetReward.isReApproved == false)
                        stage = "Under Approval";
                    else if (targetReward.isApproved == true && targetReward.stage == "Closed")
                        stage = "Closed";
                    else if (targetReward.isApproved == true && targetReward.Status.isActive == false && targetReward.PendingForClosing != true)
                        stage = "Closed";
                    else if (targetReward.isApproved == true && targetReward.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (targetReward.isApproved == true)
                        stage = "Approved";
                    else if (targetReward.isApproved == false)
                        stage = "Under Approval";
                    else if (targetReward.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";

                    if (targetReward.Status != null)
                    {

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = targetReward.Id,
                            transactionType = TransactionItemType.TargetReward,
                            //Company = targetReward.company.CompanyName,
                            //Department = targetReward.department.DeptName,
                            CreationDate = (DateTime)targetReward.CreationDate,
                            Currency = targetReward.currency.CurrencyName,
                            SalesReference = targetReward.financeRefNo,
                            Stage = stage,
                            Status = targetReward.Status.Status,
                            AmountOC = targetReward.RewardAmount,
                            GroupId = null,
                            BackColor = targetReward.Status.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                // Adding all transactions in same structure
                if (adminBill != null && allowedPermissions.Find(x => x.Name == "List of Admin Bills") != null)
                {

                    int parentId = 0;
                    if (adminBill.LoansAdvanceId != null)
                    {
                        parentId = Convert.ToInt32(adminBill.LoansAdvanceId);
                    }


                    string stage = "";
                    if (adminBill.isVoid == true)
                        stage = "Void";
                    else if (adminBill.isReApproved == false)
                        stage = "Under Approval";
                    else if (adminBill.isApproved == true && adminBill.stage == "Closed")
                        stage = "Closed";
                    else if (adminBill.isApproved == true && adminBill.BillStatus.isActive == false && adminBill.PendingForClosing != true)
                        stage = "Closed";
                    else if (adminBill.isApproved == true && adminBill.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (adminBill.isApproved == true)
                        stage = "Approved";
                    else if (adminBill.isApproved == false)
                        stage = "Under Approval";
                    else if (adminBill.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (adminBill.vendor != null)
                    {
                        vendor = adminBill.vendor.company.CompanyName;
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = adminBill.Id,
                        transactionType = TransactionItemType.Admin_Bill,
                        Company = adminBill.company.CompanyName,
                        Department = adminBill.department.DeptName,
                        CreationDate = (DateTime)adminBill.CreationDate,
                        Currency = adminBill.currency.CurrencyName,
                        SalesReference = adminBill.FinanceRefNo2,
                        Stage = stage,
                        Status = adminBill.BillStatus.Status,
                        AmountOC = adminBill.AmountOC,
                        GroupId = null,
                        BackColor = adminBill.BillStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor
                    };
                    allTransactions.Add(view);


                }
                if (billl != null && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
                {
                    int parentId = 0;
                    if (billl.saleOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(billl.saleOrder_Id);
                    }
                    if (billl.purchaseOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(billl.purchaseOrder_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (billl.statusClass_Id != null)
                    {
                        statusClass = billl.StatusClass.ClassName;
                        statusClassBackColor = billl.StatusClass.backcolor;
                    }
                    if (billl.isVoid == true)
                        stage = "Void";
                    else if (billl.isReApproved == false)
                        stage = "Under Approval";
                    else if (billl.isApproved == true && billl.stage == "Closed")
                        stage = "Closed";
                    else if (billl.isApproved == true && billl.BillStatus.isActive == false && billl.PendingForClosing != true)
                        stage = "Closed";
                    else if (billl.isApproved == true && billl.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (billl.isApproved == true)
                        stage = "Approved";
                    else if (billl.isApproved == false)
                        stage = "Under Approval";
                    else if (billl.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (billl.vendor != null)
                    {
                        vendor = billl.vendor.company.CompanyName;
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = billl.Id,
                        transactionType = TransactionItemType.Bill,
                        Company = billl.company.CompanyName,
                        Department = billl.department.DeptName,
                        CreationDate = (DateTime)billl.CreationDate,
                        Currency = billl.currency.CurrencyName,
                        SalesReference = billl.SalesReferenceNo,
                        Stage = stage,
                        Status = billl.BillStatus.Status,
                        AmountOC = billl.totalCFRValue,
                        GroupId = null,
                        BackColor = billl.BillStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (PInvoice != null && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
                {

                    int parentId = 0;
                    if (PInvoice.purchaseOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(PInvoice.purchaseOrder_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (PInvoice.statusClass_Id != null)
                    {
                        statusClass = PInvoice.StatusClass.ClassName;
                        statusClassBackColor = PInvoice.StatusClass.backcolor;
                    }
                    if (PInvoice.isVoid == true)
                        stage = "Void";
                    else if (PInvoice.isReApproved == false)
                        stage = "Under Approval";
                    else if (PInvoice.isApproved == true && PInvoice.stage == "Closed")
                        stage = "Closed";
                    else if (PInvoice.isApproved == true && PInvoice.PurchaseInvoiceStatus.isActive == false && PInvoice.PendingForClosing != true)
                        stage = "Closed";
                    else if (PInvoice.isApproved == true && PInvoice.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (PInvoice.isApproved == true)
                        stage = "Approved";
                    else if (PInvoice.isApproved == false)
                        stage = "Under Approval";
                    else if (PInvoice.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (PInvoice.Vendor != null)
                    {
                        vendor = PInvoice.Vendor.company.CompanyName;
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = PInvoice.Id,
                        transactionType = TransactionItemType.Purchase_Invoice,
                        Company = PInvoice.company.CompanyName,
                        Department = PInvoice.department.DeptName,
                        CreationDate = (DateTime)PInvoice.CreationDate,
                        Currency = PInvoice.currency.CurrencyName,
                        SalesReference = PInvoice.SalesReferenceNo,
                        Stage = stage,
                        Status = PInvoice.PurchaseInvoiceStatus.Status,
                        AmountOC = PInvoice.totalInvoiceAmount,
                        GroupId = null,
                        BackColor = PInvoice.PurchaseInvoiceStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (vouchers.Count != 0 && allowedPermissions.Find(x => x.Name == "View JV") != null)
                {
                    foreach (var jv in vouchers)
                    {
                        int parentId = 0;
                        if (jv.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(jv.purchaseOrder_Id);
                        }
                        string stage = "";
                        if (jv.isVoid == true)
                            stage = "Void";
                        else if (jv.isReApproved == false)
                            stage = "Under Approval";
                        else if (jv.isApproved == true && jv.stage == "Closed")
                            stage = "Closed";
                        else if (jv.isApproved == true && jv.JournalVoucherStatus.isActive == false && jv.PendingForClosing != true)
                            stage = "Closed";
                        else if (jv.isApproved == true && jv.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (jv.isApproved == true)
                            stage = "Approved";
                        else if (jv.isApproved == false)
                            stage = "Under Approval";
                        else if (jv.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (jv.Vendor != null)
                        //{
                        //    vendor = jv.Vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = jv.Id,
                            transactionType = TransactionItemType.JV,
                            Company = jv.company.CompanyName,
                            Department = jv.department.DeptName,
                            CreationDate = (DateTime)jv.postingDate,
                            Currency = jv.Currency.CurrencyName,
                            SalesReference = jv.voucherRefno,
                            Stage = stage,
                            Status = jv.JournalVoucherStatus.Status,
                            AmountOC = jv.journalTransactions.Sum(x => x.debit),
                            GroupId = null,
                            BackColor = jv.JournalVoucherStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (inquiry != null && allowedPermissions.Find(x => x.Name == "List Of Inquiries") != null)
                {
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (inquiry.statusClass_Id != null)
                    {
                        statusClass = inquiry.StatusClass.ClassName;
                        statusClassBackColor = inquiry.StatusClass.backcolor;
                    }
                    if (inquiry.isVoid == true)
                        stage = "Void";
                    else if (inquiry.isApproved == true && inquiry.stage == "Closed")
                        stage = "Closed";
                    else if (inquiry.isApproved == true && inquiry.inquiryStatus.isActive == false && inquiry.PendingForClosing != true)
                        stage = "Closed";
                    else if (inquiry.isApproved == true && inquiry.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (inquiry.isApproved == true)
                        stage = "Approved";
                    else if (inquiry.isApproved == false)
                        stage = "Under Approval";
                    else if (inquiry.PendingForClosing == true)
                        stage = "Under Closing";

                    var vendor = "";

                    //if (inquiry.vendors.Count > 0)
                    //{
                    //    inquiry.vendors.First().company.CompanyName.ToString();
                    //}
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = inquiry.Id,
                        transactionType = TransactionItemType.Inquiry,
                        Company = inquiry.company.CompanyName,
                        Department = inquiry.department.DeptName,
                        CreationDate = (DateTime)inquiry.CreationDate,
                        //Currency = inquiry.currency.CurrencyName,
                        SalesReference = inquiry.SalesReferenceNo,
                        Stage = stage,
                        Status = inquiry.inquiryStatus.Status,
                        //AmountOC = inquiry.,
                        GroupId = null,
                        BackColor = inquiry.inquiryStatus.backcolor,
                        ParentId = inquiry.Id,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (offer != null && allowedPermissions.Find(x => x.Name == "List Of Offers") != null)
                {
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (offer.statusClass_Id != null)
                    {
                        statusClass = offer.StatusClass.ClassName;
                        statusClassBackColor = offer.StatusClass.backcolor;
                    }
                    double fob = 0;
                    fob = offer.BookerStatementItems.Sum(x => x.amount);
                    if (offer.salesTax != 0)
                        fob = fob + offer.salesTax;

                    if (offer.isVoid == true)
                        stage = "Void";
                    else if (offer.isApproved == true && offer.stage == "Closed")
                        stage = "Closed";
                    else if (offer.isApproved == true && offer.offerStatus.isActive == false && offer.PendingForClosing != true)
                        stage = "Closed";
                    else if (offer.isApproved == true && offer.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (offer.isApproved == true)
                        stage = "Approved";
                    else if (offer.isApproved == false)
                        stage = "Under Approval";
                    else if (offer.PendingForClosing == true)
                        stage = "Under Closing";

                    var vendor = "";

                    if (offer.vendors.Count > 0)
                    {
                        offer.vendors.First().company.CompanyName.ToString();
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = offer.Id,
                        transactionType = TransactionItemType.Offer,
                        Company = offer.company.CompanyName,
                        Department = offer.department.DeptName,
                        CreationDate = (DateTime)offer.CreationDate,
                        Currency = offer.currency.CurrencyName,
                        SalesReference = offer.SalesReferenceNo,
                        Stage = stage,
                        Status = offer.offerStatus.Status,
                        AmountOC = fob,
                        GroupId = null,
                        BackColor = offer.offerStatus.backcolor,
                        ParentId = offer.inquiry_Id,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor

                    };
                    allTransactions.Add(view);
                }
                if (saleOrder != null && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
                {
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (saleOrder.statusClass_Id != null)
                    {
                        statusClass = saleOrder.StatusClass.ClassName;
                        statusClassBackColor = saleOrder.StatusClass.backcolor;
                    }
                    int parentId = 0;
                    if (saleOrder.offer_Id != null)
                    {
                        parentId = Convert.ToInt32(saleOrder.offer_Id);
                    }
                    else
                      if (saleOrder.moduleContract_Id != null)
                    {
                        parentId = Convert.ToInt32(saleOrder.moduleContract_Id);
                    }

                    if (saleOrder.isVoid == true)
                        stage = "Void";
                    else if (saleOrder.isApproved == true && saleOrder.stage == "Closed")
                        stage = "Closed";
                    else if (saleOrder.isApproved == true && saleOrder.saleOrderStatus.isActive == false && saleOrder.PendingForClosing != true)
                        stage = "Closed";
                    else if (saleOrder.isApproved == true && saleOrder.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (saleOrder.isApproved == true)
                        stage = "Approved";
                    else if (saleOrder.isApproved == false)
                        stage = "Under Approval";
                    else if (saleOrder.PendingForClosing == true)
                        stage = "Under Closing";

                    var vendor = "";

                    if (saleOrder.vendors.Count > 0)
                    {
                        vendor = saleOrder.vendors.First().company.CompanyName.ToString();
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = saleOrder.Id,
                        transactionType = TransactionItemType.Sale_Order,
                        Company = saleOrder.company.CompanyName,
                        Department = saleOrder.department.DeptName,
                        CreationDate = (DateTime)saleOrder.CreationDate,
                        Currency = saleOrder.currency.CurrencyName,
                        SalesReference = saleOrder.SalesReferenceNo,
                        Stage = stage,
                        Status = saleOrder.saleOrderStatus.Status,
                        AmountOC = saleOrder.totalCFRValue,
                        GroupId = null,
                        BackColor = saleOrder.saleOrderStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor

                    };
                    allTransactions.Add(view);
                }
                if (invoicedPurchaseOrder != null && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
                {
                    int parentId = 0;
                    if (invoicedPurchaseOrder.saleOrder_Id != null)
                    {
                        parentId = Convert.ToInt32(invoicedPurchaseOrder.saleOrder_Id);
                    }
                    string stage = "", statusClass = "", statusClassBackColor = "";
                    if (invoicedPurchaseOrder.statusClass_Id != null)
                    {
                        statusClass = invoicedPurchaseOrder.StatusClass.ClassName;
                        statusClassBackColor = invoicedPurchaseOrder.StatusClass.backcolor;
                    }
                    if (invoicedPurchaseOrder.isVoid == true)
                        stage = "Void";
                    else if (invoicedPurchaseOrder.isReApproved == false)
                        stage = "Under Approval";
                    else if (invoicedPurchaseOrder.isApproved == true && invoicedPurchaseOrder.stage == "Closed")
                        stage = "Closed";
                    else if (invoicedPurchaseOrder.isApproved == true && invoicedPurchaseOrder.PurchaseOrderStatus.isActive == false && invoicedPurchaseOrder.PendingForClosing != true)
                        stage = "Closed";
                    else if (invoicedPurchaseOrder.isApproved == true && invoicedPurchaseOrder.PendingForClosing == true)
                        stage = "Under Closing";
                    else if (invoicedPurchaseOrder.isApproved == true)
                        stage = "Approved";
                    else if (invoicedPurchaseOrder.isApproved == false)
                        stage = "Under Approval";
                    else if (invoicedPurchaseOrder.PendingForClosing == true)
                        stage = "Under Closing";
                    string vendor = "";
                    if (invoicedPurchaseOrder.vendors.Count > 0)
                    {
                        vendor = invoicedPurchaseOrder.vendors.First().company.CompanyName.ToString();
                    }
                    AllOrdersView view = new AllOrdersView()
                    {
                        Id = invoicedPurchaseOrder.Id,
                        transactionType = TransactionItemType.Purchase_Order,
                        Company = invoicedPurchaseOrder.company.CompanyName,
                        Department = invoicedPurchaseOrder.department.DeptName,
                        CreationDate = (DateTime)invoicedPurchaseOrder.CreationDate,
                        Currency = invoicedPurchaseOrder.currency.CurrencyName,
                        SalesReference = invoicedPurchaseOrder.SalesReferenceNo,
                        Stage = invoicedPurchaseOrder.stage,
                        Status = invoicedPurchaseOrder.PurchaseOrderStatus.Status,
                        AmountOC = invoicedPurchaseOrder.totalCFRValue,
                        GroupId = null,
                        BackColor = invoicedPurchaseOrder.PurchaseOrderStatus.backcolor,
                        ParentId = parentId,
                        Vendor = vendor,
                        StatusClass = statusClass,
                        BackColorStatusClass = statusClassBackColor
                    };
                    allTransactions.Add(view);
                }
                if (saleInvoices != null && allowedPermissions.Find(x => x.Name == "List of Sale Invoices") != null)
                {
                    foreach (var saleInvoice in saleInvoices)
                    {
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (saleInvoice.statusClass_Id != null)
                        {
                            statusClass = saleInvoice.StatusClass.ClassName;
                            statusClassBackColor = saleInvoice.StatusClass.backcolor;
                        }
                        if (saleInvoice.isVoid == true)
                            stage = "Void";
                        else if (saleInvoice.isApproved == true && saleInvoice.stage == "Closed")
                            stage = "Closed";
                        else if (saleInvoice.isApproved == true && saleInvoice.saleInvoiceStatus.isActive == false && saleInvoice.PendingForClosing != true)
                            stage = "Closed";
                        else if (saleInvoice.isApproved == true && saleInvoice.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (saleInvoice.isApproved == true)
                            stage = "Approved";
                        else if (saleInvoice.isApproved == false)
                            stage = "Under Approval";
                        else if (saleInvoice.PendingForClosing == true)
                            stage = "Under Closing";

                        var vendor = "";

                        if (saleInvoice.vendors.Count > 0)
                        {
                            vendor = saleInvoice.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = saleInvoice.Id,
                            transactionType = TransactionItemType.Sale_Invoice,
                            Company = saleInvoice.company.CompanyName,
                            Department = saleInvoice.department.DeptName,
                            CreationDate = (DateTime)saleInvoice.CreationDate,
                            Currency = saleInvoice.currency.CurrencyName,
                            SalesReference = saleInvoice.SalesReferenceNo,
                            Stage = stage,
                            Status = saleInvoice.saleInvoiceStatus.Status,
                            AmountOC = saleInvoice.totalInvoiceAmount,
                            GroupId = null,
                            BackColor = saleInvoice.saleInvoiceStatus.backcolor,
                            ParentId = saleOrder.Id,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (saleReceipts != null && allowedPermissions.Find(x => x.Name == "List of Sale Receipts") != null)
                {
                    foreach (var receipt in saleReceipts)
                    {
                        string stage = "";
                        if (receipt.isVoid == true)
                            stage = "Void";
                        else if (receipt.isReApproved == false)
                            stage = "Under Approval";
                        else if (receipt.isApproved == true && receipt.stage == "Closed")
                            stage = "Closed";
                        else if (receipt.isApproved == true && receipt.saleReceiptStatus.isActive == false && receipt.PendingForClosing != true)
                            stage = "Closed";
                        else if (receipt.isApproved == true && receipt.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (receipt.isApproved == true)
                            stage = "Approved";
                        else if (receipt.isApproved == false)
                            stage = "Under Approval";
                        else if (receipt.PendingForClosing == true)
                            stage = "Under Closing";

                        Department dept = new Department()/*receipt.department*/;
                        switch (receipt.receiptType)
                        {
                            case ReceiptType.Customer_Credits:
                                dept = receipt.saleInvoice?.department;
                                break;
                            case ReceiptType.Direct_Receipt:
                                if (receipt.payment != null)
                                {
                                    switch (receipt.payment.transactionType)
                                    {
                                        case PaymentTransactionType.Admin_Bills:
                                            dept = receipt.payment.adminBill.department;
                                            break;
                                        case PaymentTransactionType.Loans_Advances:
                                            dept = receipt.payment.loansAdvance.department;
                                            break;
                                        case PaymentTransactionType.Purchase_Invoice:
                                            dept = receipt.payment.purchaseInvoice.department;
                                            break;
                                        case PaymentTransactionType.Target_Reward:
                                            break;
                                        case PaymentTransactionType.Vendor_Bills:
                                            dept = receipt.payment.Bill.department;
                                            break;
                                    }
                                }
                                else
                                    dept = receipt.department;
                                break;
                            case ReceiptType.Loans_Advances:
                                dept = receipt.loansAdvance?.department;
                                break;
                            case ReceiptType.Sales_Customer:
                                dept = receipt.saleInvoice?.department;
                                break;
                            case ReceiptType.Sales_Department:
                                dept = receipt.saleInvoice?.department;
                                break;
                        }

                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = receipt.Id,
                            transactionType = TransactionItemType.Sale_Receipt,
                            Company = receipt.company.CompanyName,
                            Department = dept == null? null :  dept.DeptName,
                            CreationDate = (DateTime)receipt.CreationDate,
                            Currency = receipt.Currency.CurrencyName,
                            SalesReference = receipt.ReceiptRefNo,
                            Stage = stage,
                            Status = receipt.saleReceiptStatus.Status,
                            AmountOC = receipt.CollectionAmount,
                            GroupId = receipt.transactionGroupId,
                            BackColor = receipt.saleReceiptStatus.backcolor,
                            ParentId = receipt.saleInvoiceId
                        };
                        allTransactions.Add(view);
                    }
                }
                if (purchaseOrders != null && allowedPermissions.Find(x => x.Name == "List of Purchase Orders") != null)
                {
                    foreach (var PO in purchaseOrders)
                    {
                        int parentId = 0;
                        if (PO.saleOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(PO.saleOrder_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (PO.statusClass_Id != null)
                        {
                            statusClass = PO.StatusClass.ClassName;
                            statusClassBackColor = PO.StatusClass.backcolor;
                        }
                        if (PO.isVoid == true)
                            stage = "Void";
                        else if (PO.isReApproved == false)
                            stage = "Under Approval";
                        else if (PO.isApproved == true && PO.stage == "Closed")
                            stage = "Closed";
                        else if (PO.isApproved == true && PO.PurchaseOrderStatus.isActive == false && PO.PendingForClosing != true)
                            stage = "Closed";
                        else if (PO.isApproved == true && PO.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (PO.isApproved == true)
                            stage = "Approved";
                        else if (PO.isApproved == false)
                            stage = "Under Approval";
                        else if (PO.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (PO.vendors.Count > 0)
                        {
                            vendor = PO.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = PO.Id,
                            transactionType = TransactionItemType.Purchase_Order,
                            Company = PO.company.CompanyName,
                            Department = PO.department.DeptName,
                            CreationDate = (DateTime)PO.CreationDate,
                            Currency = PO.currency.CurrencyName,
                            SalesReference = PO.SalesReferenceNo,
                            Stage = stage,
                            Status = PO.PurchaseOrderStatus.Status,
                            AmountOC = PO.totalCFRValue,
                            GroupId = null,
                            BackColor = PO.PurchaseOrderStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }

                }
                if (journalVouchers != null)
                {
                    foreach (var jv in vouchers)
                    {
                        int parentId = 0;
                        if (jv.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(jv.purchaseOrder_Id);
                        }
                        string stage = "";
                        if (jv.isVoid == true)
                            stage = "Void";
                        else if (jv.isReApproved == false)
                            stage = "Under Approval";
                        else if (jv.isApproved == true && jv.stage == "Closed")
                            stage = "Closed";
                        else if (jv.isApproved == true && jv.JournalVoucherStatus.isActive == false && jv.PendingForClosing != true)
                            stage = "Closed";
                        else if (jv.isApproved == true && jv.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (jv.isApproved == true)
                            stage = "Approved";
                        else if (jv.isApproved == false)
                            stage = "Under Approval";
                        else if (jv.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        //if (jv.Vendor != null)
                        //{
                        //    vendor = jv.Vendor.company.CompanyName;
                        //}
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = jv.Id,
                            transactionType = TransactionItemType.JV,
                            Company = jv.company.CompanyName,
                            Department = jv.department.DeptName,
                            CreationDate = (DateTime)jv.postingDate,
                            Currency = jv.Currency.CurrencyName,
                            SalesReference = jv.voucherRefno,
                            Stage = stage,
                            Status = jv.JournalVoucherStatus.Status,
                            AmountOC = jv.journalTransactions.Sum(x => x.debit),
                            GroupId = null,
                            BackColor = jv.JournalVoucherStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (bills != null && allowedPermissions.Find(x => x.Name == "List of Bills") != null)
                {
                    foreach (var bill in bills)
                    {
                        double? hasvalue = bill.SoAmountSOC_ER;

                        int parentId = 0;
                        if (bill.saleOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(bill.saleOrder_Id);
                        }
                        if (bill.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(bill.purchaseOrder_Id);
                        }
                        string stage = "";
                        if (bill.isVoid == true)
                            stage = "Void";
                        else if (bill.isReApproved == false)
                            stage = "Under Approval";
                        else if (bill.isApproved == true && bill.stage == "Closed")
                            stage = "Closed";
                        else if (bill.isApproved == true && bill.BillStatus.isActive == false && bill.PendingForClosing != true)
                            stage = "Closed";
                        else if (bill.isApproved == true && bill.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (bill.isApproved == true)
                            stage = "Approved";
                        else if (bill.isApproved == false)
                            stage = "Under Approval";
                        else if (bill.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (bill.vendor != null)
                        {
                            vendor = bill.vendor.company.CompanyName;
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = bill.Id,
                            transactionType = TransactionItemType.Bill,
                            Company = bill.company.CompanyName,
                            Department = bill.department.DeptName,
                            CreationDate = (DateTime)bill.CreationDate,
                            Currency = bill.currency.CurrencyName,
                            SalesReference = bill.SalesReferenceNo,
                            Stage = stage,
                            Status = bill.BillStatus.Status,
                            AmountOC = bill.totalCFRValue,
                            GroupId = null,
                            BackColor = bill.BillStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (purchaseInvoices != null && allowedPermissions.Find(x => x.Name == "List of Purchase Invoices") != null)
                {
                    foreach (var invoice in purchaseInvoices)
                    {
                        int parentId = 0;
                        if (invoice.purchaseOrder_Id != null)
                        {
                            parentId = Convert.ToInt32(invoice.purchaseOrder_Id);
                        }
                        string stage = "", statusClass = "", statusClassBackColor = "";
                        if (invoice.statusClass_Id != null)
                        {
                            statusClass = invoice.StatusClass.ClassName;
                            statusClassBackColor = invoice.StatusClass.backcolor;
                        }
                        if (invoice.isVoid == true)
                            stage = "Void";
                        else if (invoice.isReApproved == false)
                            stage = "Under Approval";
                        else if (invoice.isApproved == true && invoice.stage == "Closed")
                            stage = "Closed";
                        else if (invoice.isApproved == true && invoice.PurchaseInvoiceStatus.isActive == false && invoice.PendingForClosing != true)
                            stage = "Closed";
                        else if (invoice.isApproved == true && invoice.PendingForClosing == true)
                            stage = "Under Closing";
                        else if (invoice.isApproved == true)
                            stage = "Approved";
                        else if (invoice.isApproved == false)
                            stage = "Under Approval";
                        else if (invoice.PendingForClosing == true)
                            stage = "Under Closing";
                        string vendor = "";
                        if (invoice.vendors.Count > 0)
                        {
                            vendor = invoice.vendors.First().company.CompanyName.ToString();
                        }
                        AllOrdersView view = new AllOrdersView()
                        {
                            Id = invoice.Id,
                            transactionType = TransactionItemType.Purchase_Invoice,
                            Company = invoice.company.CompanyName,
                            Department = invoice.department.DeptName,
                            CreationDate = (DateTime)invoice.CreationDate,
                            Currency = invoice.currency.CurrencyName,
                            SalesReference = invoice.SalesReferenceNo,
                            Stage = stage,
                            Status = invoice.PurchaseInvoiceStatus.Status,
                            AmountOC = invoice.totalInvoiceAmount,
                            GroupId = null,
                            BackColor = invoice.PurchaseInvoiceStatus.backcolor,
                            ParentId = parentId,
                            Vendor = vendor,
                            StatusClass = statusClass,
                            BackColorStatusClass = statusClassBackColor
                        };
                        allTransactions.Add(view);
                    }
                }
                if (memorandumSales != null && allowedPermissions.Find(x => x.Name == "List of Memorandum Sales") != null)
                {
                    foreach (var memorandumSale in memorandumSales)
                    {
                        //allTransactions.Add(new AllTransactionsView()
                        //{
                        //    Id = memorandumSale.Id.ToString() + "_" + TransactionItemType.Memorandum_Sale.ToString(),
                        //    TransactionType = TransactionItemType.Memorandum_Sale.ToString(),
                        //    Company = memorandumSale.company.CompanyName,
                        //    CreationDate = memorandumSale.CreationDate,
                        //    Currency = memorandumSale.customerCompany.company.currency.CurrencyName,
                        //    Department = (memorandumSale.department.parentDepartment == null) ? memorandumSale.department.DeptName : memorandumSale.department.parentDepartment.DeptName + " " + memorandumSale.department.DeptName,
                        //    Customer = memorandumSale.customerCompany.company.CompanyName,
                        //    SalesReferenceNo = memorandumSale.referenceNo,
                        //    BackColor = memorandumSale.memorandumSaleStatus.backcolor,
                        //    Status = memorandumSale.memorandumSaleStatus.Status,
                        //    totalCFRValue = memorandumSale.totalCFRValue,
                        //    amountSOC = memorandumSale.totalCFRValue,
                        //    amountME = memorandumSale.totalCFRValue,
                        //    Parent_Id = (memorandumSale.SaleOrder_Id != null ? memorandumSale.SaleOrder_Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString() : (memorandumSale.Offer_Id != null ? memorandumSale.Offer_Id.ToString() + "_" + TransactionItemType.Offer.ToString() : ""))

                        //});
                    }
                }
                allTransactions = allTransactions.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                return allTransactions;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// Get inquiry based on Inquiry ID.
        /// </summary>
        /// <param name="inquiryId"></param>
        /// <returns></returns>
        /// 

        public List<AllTransactionsView> getLinkedSO(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {
            SaleOrder saleOrder = new SaleOrder();
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();

            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            EmployeeRepo empRepo = new EmployeeRepo();

            switch (type)
            {
                case Enums.TransactionItemType.Sale_Order:
                 
                    saleOrder = context.saleOrders
            
                        .FirstOrDefault(x => x.Id == Id);

                    saleOrders.AddRange(context.saleOrders
                   .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());

                    if (saleOrder.ParentSO_Id != null)
                        saleOrders.Add(context.saleOrders
                        .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));

                    break;
                //Sale Invoices
               
            };
            if (saleOrder != null && saleOrder.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = saleOrder.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                    TransactionType = TransactionItemType.Sale_Order.ToString(),
                    Company = saleOrder.company.CompanyName,
                    Employee = saleOrder.employee.person.FName + " " + saleOrder.employee.person.FName,
                    CreationDate = saleOrder.CreationDate,
                    Currency = saleOrder.customerCompany.company.currency.CurrencyName,
                    Department = (saleOrder.department.parentDepartment == null) ? saleOrder.department.DeptName : saleOrder.department.parentDepartment.DeptName + " " + saleOrder.department.DeptName,
                    Customer = saleOrder.customerCompany.company.CompanyName,
                    SalesReferenceNo = saleOrder.SalesReferenceNo,
                    BackColor = saleOrder.saleOrderStatus.backcolor,
                    Status = saleOrder.saleOrderStatus.Status,
                    totalCFRValue = saleOrder.totalCFRValue,
                    amountSOC = saleOrder.totalCFRValue,
                    amountME = saleOrder.totalBaseCFRValue

                });
            }
            if (saleOrders.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                List<int> compIds= new List<int>();
                List<Company> userCompanies = new List<Company>();
                List<int> deptIds = new List<int>();
                var employee = context.Employees.FirstOrDefault(x => x.EmpId == empID);

                userCompanies.AddRange(employee.Companies);
                userCompanies.AddRange(employee.AdminBillCompanies);
                userCompanies.AddRange(employee.TaskCompanies);
                var finalCompanies = userCompanies.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                compIds= finalCompanies.Select(x=>x.Id).ToList();
                deptIds= employee.departments.Select(x=>x.Id).ToList();

                saleOrders = saleOrders.Where(x => compIds.Contains((int)x.company_Id) && deptIds.Contains(x.dept_Id)).ToList();

                foreach (var order in saleOrders)
                {
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = order.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                        TransactionType = TransactionItemType.Sale_Order.ToString(),
                        Company = order.company.CompanyName,
                        Employee = order.employee.person.FName + " " + order.employee.person.FName,
                        CreationDate = order.CreationDate,
                        Currency = order.customerCompany.company.currency.CurrencyName,
                        Department = (order.department.parentDepartment == null) ? order.department.DeptName : order.department.parentDepartment.DeptName + " " + order.department.DeptName,
                        Customer = order.customerCompany.company.CompanyName,
                        SalesReferenceNo = order.SalesReferenceNo,
                        BackColor = order.saleOrderStatus.backcolor,
                        Status = order.saleOrderStatus.Status,
                        totalCFRValue = order.totalCFRValue,
                        amountSOC = order.totalCFRValue,
                        amountME = order.totalBaseCFRValue,
                        Parent_Id = order.ParentSO_Id.ToString()


                    });
                }
            }
            return allTransactions;
        }
        public List<AllTransactionsView> getLinkedSOs(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {
            SaleOrder saleOrder = new SaleOrder();
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();

            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            EmployeeRepo empRepo = new EmployeeRepo();

            switch (type)
            {
                case Enums.TransactionItemType.Sale_Order:
                 
                    saleOrder = context.saleOrders
            
                        .FirstOrDefault(x => x.Id == Id);


                    if (saleOrder.ParentSO_Id != null)
                    {
                        saleOrders = context.saleOrders.Where(x => x.ParentSO_Id == saleOrder.ParentSO_Id).ToList();
                        saleOrders.Add(saleOrder.ParentSaleOrder);
                    }
                    else
                    {
                        saleOrders = context.saleOrders.Where(x => x.ParentSO_Id == saleOrder.Id).ToList();
                        saleOrders.Add(saleOrder);

                    }

                    break;
                //Sale Invoices
               
            };
            if (saleOrders.Count != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                List<int> compIds= new List<int>();
                List<Company> userCompanies = new List<Company>();
                List<int> deptIds = new List<int>();
                var employee = context.Employees.FirstOrDefault(x => x.EmpId == empID);

                userCompanies.AddRange(employee.Companies);
                userCompanies.AddRange(employee.AdminBillCompanies);
                userCompanies.AddRange(employee.TaskCompanies);
                var finalCompanies = userCompanies.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
                compIds= finalCompanies.Select(x=>x.Id).ToList();
                deptIds= employee.departments.Select(x=>x.Id).ToList();

                saleOrders = saleOrders.Where(x => compIds.Contains((int)x.company_Id) && deptIds.Contains(x.dept_Id)).ToList();

                foreach (var order in saleOrders)
                {
                    bool parent = false;
                    if (order.ParentSO_Id == null)
                    {
                        parent = true;
                    }
                    else
                        parent = false;
                    allTransactions.Add(new AllTransactionsView()
                    {
                        Id = order.Id.ToString() + "_" + TransactionItemType.Sale_Order.ToString(),
                        TransactionType = TransactionItemType.Sale_Order.ToString(),
                        Company = order.company.CompanyName,
                        Employee = order.employee.person.FName + " " + order.employee.person.FName,
                        CreationDate = order.CreationDate,
                        Currency = order.customerCompany.company.currency.CurrencyName,
                        Department = (order.department.parentDepartment == null) ? order.department.DeptName : order.department.parentDepartment.DeptName + " " + order.department.DeptName,
                        Customer = order.customerCompany.company.CompanyName,
                        SalesReferenceNo = order.SalesReferenceNo,
                        BackColor = order.saleOrderStatus.backcolor,
                        Status = order.saleOrderStatus.Status,
                        totalCFRValue = order.totalCFRValue,
                        amountSOC = order.totalCFRValue,
                        amountME = order.totalBaseCFRValue,
                        Parent_Id = order.ParentSO_Id.ToString(),
                        Parent=parent
                    });
                }
            }
            return allTransactions;
        }
        public List<SaleOrder> getLinkedSOSForCostSheets(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {

            SaleOrder saleOrder = new SaleOrder();
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            EmployeeRepo empRepo = new EmployeeRepo();

            switch (type)
            {
                case Enums.TransactionItemType.Sale_Order:

                    saleOrder = context.saleOrders

                        .FirstOrDefault(x => x.Id == Id);

                    if (saleOrder.ParentSO_Id != null)
                    {
                        saleOrders = context.saleOrders.Where(x => x.ParentSO_Id == saleOrder.ParentSO_Id).ToList();
                        saleOrders.Add(saleOrder.ParentSaleOrder);
                    }
                    else
                    {
                        saleOrders = context.saleOrders.Where(x => x.ParentSO_Id == saleOrder.Id).ToList();
                        saleOrders.Add(saleOrder);

                    }

                    //saleOrders.AddRange(context.saleOrders.Where(x => x.ParentSO_Id == saleOrder.Id).ToList());

                    //if (saleOrder.ParentSO_Id != null)
                    //    saleOrders.Add(context.saleOrders.FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));

                    break;
                    //Sale Invoices

            };
            List<int> compIds = new List<int>();
            List<Company> userCompanies = new List<Company>();
            List<int> deptIds = new List<int>();
            var employee = context.Employees.FirstOrDefault(x => x.EmpId == empID);

            userCompanies.AddRange(employee.Companies);
            userCompanies.AddRange(employee.AdminBillCompanies);
            userCompanies.AddRange(employee.TaskCompanies);
            var finalCompanies = userCompanies.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            compIds = finalCompanies.Select(x => x.Id).ToList();
            deptIds = employee.departments.Select(x => x.Id).ToList();

            saleOrders = saleOrders.Where(x => compIds.Contains((int)x.company_Id) && deptIds.Contains(x.dept_Id)).ToList();
            return saleOrders;
        }
        public List<SaleOrder> getLinkedSOS(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {

            SaleOrder saleOrder = new SaleOrder();
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            EmployeeRepo empRepo = new EmployeeRepo();

            switch (type)
            {
                case Enums.TransactionItemType.Sale_Order:

                    saleOrder = context.saleOrders
            
                        .FirstOrDefault(x => x.Id == Id);

                    saleOrders.AddRange(context.saleOrders.Where(x => x.ParentSO_Id == saleOrder.Id).ToList());

                    if (saleOrder.ParentSO_Id != null)
                        saleOrders.Add(context.saleOrders.FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));

                    break;
                    //Sale Invoices

            };
            if (saleOrder != null && saleOrder.Id != 0 && allowedPermissions.Find(x => x.Name == "List of Sale Orders") != null)
            {
                saleOrders.Add(saleOrder);
            }
            List<int> compIds = new List<int>();
            List<Company> userCompanies = new List<Company>();
            List<int> deptIds = new List<int>();
            var employee = context.Employees.FirstOrDefault(x => x.EmpId == empID);

            userCompanies.AddRange(employee.Companies);
            userCompanies.AddRange(employee.AdminBillCompanies);
            userCompanies.AddRange(employee.TaskCompanies);
            var finalCompanies = userCompanies.GroupBy(x => x.Id).Select(y => y.FirstOrDefault()).ToList();
            compIds = finalCompanies.Select(x => x.Id).ToList();
            deptIds = employee.departments.Select(x => x.Id).ToList();

            saleOrders = saleOrders.Where(x => compIds.Contains((int)x.company_Id) && deptIds.Contains(x.dept_Id)).ToList();
            return saleOrders;
        }

        public List<SaleOrder> GetLInkedSaleOrders(int Id, ERP_BL.Enums.TransactionItemType type, List<Permission> allowedPermissions, int empID)
        {

            Inquiry inquiry = new Inquiry();
            Offer offer = new Offer();
            InterBankTransfer bankTransfer = new InterBankTransfer();
            JournalVoucher voucher = new JournalVoucher();
            SaleOrder saleOrder = new SaleOrder();
            PurchaseOrder invoicedPurchaseOrder = new PurchaseOrder();

            List<SaleInvoice> saleInvoices = new List<SaleInvoice>();
            List<PurchaseInvoice> purchaseInvoices = new List<PurchaseInvoice>();
            PurchaseInvoice PInvoice = new PurchaseInvoice();
            List<MemorandumSale> memorandumSales = new List<MemorandumSale>();
            List<SalesReceipt> saleReceipts = new List<SalesReceipt>();
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            List<Bill> bills = new List<Bill>();
            Bill billl = new Bill();
            List<JournalVoucher> journalVouchers1 = new List<JournalVoucher>();
            List<JournalVoucher> vouchers = new List<JournalVoucher>();
            AdminBill adminBill = new AdminBill();
            List<AdminBill> adminBills = new List<AdminBill>();
            LoansAdvance loansAdvance = new LoansAdvance();
            List<LoansAdvance> loansAdvances = new List<LoansAdvance>();

            Payment payment = new Payment();
            List<Payment> payments = new List<Payment>();
            List<SaleOrder> saleOrders = new List<SaleOrder>();
            List<InterBankTransfer> bankTransfers = new List<InterBankTransfer>();

            InterCompanyBankTransferRepo bankTransferRepo = new InterCompanyBankTransferRepo();
            JournalVoucherRepo voucherRepo = new JournalVoucherRepo();

            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();

            EmployeeRepo empRepo = new EmployeeRepo();
            //Employee currEmp = new Employee();
            //currEmp = empRepo.GetEmployeeForPayments(empID);

            switch (type)
            {
                case Enums.TransactionItemType.InterBank_Transfer:
                    //bankTransfer = bankTransferRepo.GetInterBankTransfer(Id);

                    bankTransfer = context.interBankTransfers
 
                .FirstOrDefault(x => x.Id == Id);

                    if (bankTransfer.paymentGroupId != 0)
                    {
                        bankTransfers = bankTransferRepo.GetPaymentInterBankTransfers(bankTransfer.paymentGroupId, empID, (int)bankTransfer.company_Id);

                    }
                    else
                    {
                        if (bankTransfer.receiptGroupId != 0)
                            bankTransfers = bankTransferRepo.GetReceiptInterBankTransfers(bankTransfer.receiptGroupId, empID, (int)bankTransfer.company_Id);

                    }
                    if (bankTransfers.Count > 0)
                    {

                        foreach (var transfer in bankTransfers)
                        {
                            if (transfer.paymentGroupId != 0)
                            {
                                payments = context.payments.Where(x => x.transactionGroupId == transfer.paymentGroupId).ToList();
                                foreach (var paym in payments)
                                {

                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                                    {

                                        if (paym.AdminBill_Id != null)
                                        {
                                            Employee empUser = new Employee();
                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                            adminBill = context.adminBills
                                            .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                            if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                                adminBill = null;
                                        }
                                        else
                                        {
                                            adminBill = null;
                                        }
                                        if (adminBill != null)
                                        {
                                            payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                        }
                                        billl = null;
                                        PInvoice = null;
                                        saleOrder = null;
                                        offer = null;
                                        inquiry = null;
                                        payment = null;
                                        invoicedPurchaseOrder = null;

                                    }

                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                                    {
                                        Employee empUser = new Employee();
                                        if (paym.Bill_Id != null)
                                        {

                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                            billl = context.bills
                                            .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                            if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                                billl = null;
                                        }
                                        else
                                        {
                                            billl = null;
                                        }

                                        if (billl != null)
                                        {

                                            if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                            {
                                                saleOrder = context.saleOrders
                                                
                                                    .FirstOrDefault(x => x.Id == billl.saleOrder_Id);

                                                if (saleOrder != null)
                                                    if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                        saleOrder = null;
                                            }
                                            else
                                            {
                                                saleOrder = null;
                                            }

                                            if (saleOrder == null)
                                            {
                                                PurchaseOrder purchaseOrder = new PurchaseOrder();
                                                if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                                {
                                                    purchaseOrder = context.purchaseOrders
                                     .FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                                    if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                        purchaseOrder = null;
                                                }
                                                else
                                                {
                                                    purchaseOrder = null;
                                                }

                                                if (purchaseOrder != null)
                                                {
                                                    saleOrder = context.saleOrders
                                              
                                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                           
                                                    purchaseOrders.Add(purchaseOrder);
                                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                
                                                }
                                            }

                                            journalVouchers1 = context.journalVouchers.Include("JournalVoucherStatus").Include("department").Where(x => x.bill_Id == billl.Id).ToList();

                                            payments = context.payments.Include("Status").Include("company").Where(x => x.Bill_Id == billl.Id).ToList();

                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                        }
                                        else
                                        {
                                            saleInvoices = null;
                                            offer = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            purchaseOrders = context.purchaseOrders
                                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                            if (purchaseOrders.Count != 0)
                                            {
                                                foreach (var purchaseOrder in purchaseOrders)
                                                {
                                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                                }
                                            }
                                        }
                                        if (purchaseInvoices != null)
                                        {
                                            foreach (var _PI in purchaseInvoices)
                                                if (_PI.Payments.Count > 0)
                                                    payments.AddRange(_PI.Payments);
                                        }
                                        //else
                                        //{
                                        //    purchaseOrders = null;
                                        //}

                                        if (offer != null)
                                        {
                                            inquiry = context.inquiries
                                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                        }
                                        else
                                        {

                                            inquiry = null;
                                        }
                                        if (saleInvoices != null)
                                        {
                                            saleReceipts = context.salesReceipts
                                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                        }
                                        else
                                        {
                                            saleReceipts = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                        }
                                        else if (offer != null)
                                        {
                                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                        }
                                        else
                                        {
                                            memorandumSales = null;
                                        }

                                        PInvoice = null;
                                        //payments = null;
                                        payment = null;
                                        invoicedPurchaseOrder = null;
                                        adminBill = null;
                                    }



                                    if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                                    {

                                        if (paym.PInvoice_Id != null)
                                        {
                                            Employee empUser = new Employee();
                                            empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                            PInvoice = context.purchaseInvoices
                                         
                                            .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                            if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                                PInvoice = null;
                                        }
                                        else
                                        {
                                            PInvoice = null;
                                        }
                                        if (PInvoice != null)
                                        {
                                            invoicedPurchaseOrder = context.purchaseOrders
                                        
                                            .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                            payments = context.payments.Include("Status").Include("company").Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                        }
                                        else
                                        {
                                            invoicedPurchaseOrder = null;
                                        }
                                        if (invoicedPurchaseOrder != null)
                                        {
                                            saleOrder = context.saleOrders
                                
                                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);

                                            if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                                purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                            if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                            {
                                                purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                            }

                                            if (purchaseOrders != null)
                                                foreach (var _po in purchaseOrders)
                                                    purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                            purchaseInvoices.Distinct();

                                            if (purchaseInvoices != null)
                                                foreach (var _pi in purchaseInvoices)
                                                    payments.AddRange(_pi.Payments);

                                            payments.Distinct();
                                        }
                                        else
                                        {
                                            saleOrder = null;
                                        }
                                        if (saleOrder != null)
                                        {
                                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                        }
                                        else
                                        {
                                            saleInvoices = null;
                                            offer = null;
                                        }
                                        if (offer != null)
                                        {
                                            inquiry = context.inquiries
                                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                        }
                                        else
                                        {

                                            inquiry = null;
                                        }
                                        if (saleInvoices != null)
                                        {
                                            saleReceipts = context.salesReceipts
                                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                        }
                                        else
                                        {
                                            saleReceipts = null;
                                        }



                                        adminBill = null;
                                        billl = null;
                                        bills = null;
                                        //purchaseInvoices = null;
                                        payment = null;
                                        journalVouchers1 = null;
                                    }

                                }
                            }
                            else
                            {
                                saleReceipts = context.salesReceipts
                           .Where(x => x.transactionGroupId == transfer.receiptGroupId).ToList();


                            }
                        }
                    }
                    if (bankTransfer.vendorBillId != null)
                    {
                        billl = bankTransfer.Bill;
                    }
                    break;



                case Enums.TransactionItemType.JV:
                    voucher = voucherRepo.GetJournalVoucherById(Id);
                    if (voucher.paymentGroupId != 0)
                    {
                        vouchers = voucherRepo.GetVouchersByGroupId(voucher.paymentGroupId);
                        foreach (var paym in payments)
                        {

                            if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Admin_Bills)
                            {

                                if (paym.AdminBill_Id != null)
                                {
                                    Employee empUser = new Employee();
                                    empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                    adminBill = context.adminBills
                                    .FirstOrDefault(x => x.Id == paym.AdminBill_Id);
                                    if (empUser.departments.FirstOrDefault(x => x.Id == adminBill.department.Id) == null)
                                        adminBill = null;
                                }
                                else
                                {
                                    adminBill = null;
                                }
                                if (adminBill != null)
                                {
                                    payments = context.payments.Where(x => x.AdminBill_Id == adminBill.Id).ToList();
                                }
                                billl = null;
                                PInvoice = null;
                                saleOrder = null;
                                offer = null;
                                inquiry = null;
                                payment = null;
                                invoicedPurchaseOrder = null;

                            }

                            if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Vendor_Bills)
                            {
                                Employee empUser = new Employee();
                                if (paym.Bill_Id != null)
                                {

                                    empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);

                                    billl = context.bills
                                    .FirstOrDefault(x => x.Id == paym.Bill_Id);

                                    if (empUser.departments.FirstOrDefault(x => x.Id == billl.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == billl.InterDepartment_Id) == null)
                                        billl = null;
                                }
                                else
                                {
                                    billl = null;
                                }

                                if (billl != null)
                                {

                                    if (billl.saleOrder_Id != null && billl.saleOrder_Id > 0)
                                    {
                                        saleOrder = context.saleOrders
                                             
                                            .FirstOrDefault(x => x.Id == billl.saleOrder_Id);
                                        saleOrders.AddRange(context.saleOrders
                                           .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                                        if (saleOrder.ParentSO_Id != null)
                                            saleOrders.Add(context.saleOrders
                                              .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                                        if (saleOrder != null)
                                            if (empUser.departments.FirstOrDefault(x => x.Id == saleOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == saleOrder.InterDepartment_Id) == null)
                                                saleOrder = null;
                                    }
                                    else
                                    {
                                        saleOrder = null;
                                    }

                                    if (saleOrder == null)
                                    {
                                        PurchaseOrder purchaseOrder = new PurchaseOrder();
                                        if (billl.purchaseOrder_Id != null && billl.purchaseOrder_Id > 0)
                                        {
                                            purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);

                                            if (empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == purchaseOrder.InterDepartment_Id) == null)
                                                purchaseOrder = null;
                                        }
                                        else
                                        {
                                            purchaseOrder = null;
                                        }

                                        if (purchaseOrder != null)
                                        {
                                            saleOrder = context.saleOrders
                                  
                                            .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                            saleOrders.AddRange(context.saleOrders
                                            
                                                .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                                            if (saleOrder.ParentSO_Id != null)
                                                saleOrders.Add(context.saleOrders
                                             .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                                            purchaseOrders.Add(purchaseOrder);
                                            if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());

                                        }
                                    }

                                    journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                                    payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                                }
                                else
                                {
                                    saleOrder = null;
                                }
                                if (saleOrder != null)
                                {
                                    saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                    offer = context.offers
                                           .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                                }
                                else
                                {
                                    saleInvoices = null;
                                    offer = null;
                                }
                                if (saleOrder != null)
                                {
                                    purchaseOrders = context.purchaseOrders
                                    .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                                    if (purchaseOrders.Count != 0)
                                    {
                                        foreach (var purchaseOrder in purchaseOrders)
                                        {
                                            if (purchaseOrder.PurchaseInvoices.Count != 0)
                                                purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                        }
                                    }
                                }
                                if (purchaseInvoices != null)
                                {
                                    foreach (var _PI in purchaseInvoices)
                                        if (_PI.Payments.Count > 0)
                                            payments.AddRange(_PI.Payments);
                                }
                                //else
                                //{
                                //    purchaseOrders = null;
                                //}

                                if (offer != null)
                                {
                                    inquiry = context.inquiries
                                   .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                                }
                                else
                                {

                                    inquiry = null;
                                }
                                if (saleInvoices != null)
                                {
                                    saleReceipts = context.salesReceipts
                                .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                }
                                else
                                {
                                    saleReceipts = null;
                                }
                                if (saleOrder != null)
                                {
                                    memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                                }
                                else if (offer != null)
                                {
                                    memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                                }
                                else
                                {
                                    memorandumSales = null;
                                }

                                PInvoice = null;
                                //payments = null;
                                payment = null;
                                invoicedPurchaseOrder = null;
                                adminBill = null;
                            }



                            if (paym != null && paym.Id != 0 && paym.transactionType == PaymentTransactionType.Purchase_Invoice)
                            {

                                if (paym.PInvoice_Id != null)
                                {
                                    Employee empUser = new Employee();
                                    empUser = empRepo.GetEmployeeForPayments(paym.user.employeeId);
                                    PInvoice = context.purchaseInvoices
                                    .FirstOrDefault(x => x.Id == paym.PInvoice_Id);
                                    if (empUser.departments.FirstOrDefault(x => x.Id == PInvoice.department.Id) == null && empUser.departments.FirstOrDefault(x => x.Id == PInvoice.InterDepartment_Id) == null)
                                        PInvoice = null;
                                }
                                else
                                {
                                    PInvoice = null;
                                }
                                if (PInvoice != null)
                                {
                                    invoicedPurchaseOrder = context.purchaseOrders
                                       
                                    .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);

                                    payments = context.payments.Where(x => x.PInvoice_Id == PInvoice.Id).ToList();
                                }
                                else
                                {
                                    invoicedPurchaseOrder = null;
                                }
                                if (invoicedPurchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders
                                  
                                    .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                                    saleOrders.AddRange(context.saleOrders
                                       .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                                    if (saleOrder.ParentSO_Id != null)
                                        saleOrders.Add(context.saleOrders
                                              .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                                    if (invoicedPurchaseOrder.PurchaseInvoices != null && invoicedPurchaseOrder.PurchaseInvoices.Count > 0)
                                        purchaseInvoices.AddRange(invoicedPurchaseOrder.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                    if (saleOrder != null && saleOrder.PurchaseOrders != null && saleOrder.PurchaseOrders.Count > 0)
                                    {
                                        purchaseOrders.AddRange(saleOrder.PurchaseOrders.Where(x => x.Id != invoicedPurchaseOrder.Id).ToList());
                                    }

                                    if (purchaseOrders != null)
                                        foreach (var _po in purchaseOrders)
                                            purchaseInvoices.AddRange(_po.PurchaseInvoices.Where(x => x.Id != PInvoice.Id).ToList());

                                    purchaseInvoices.Distinct();

                                    if (purchaseInvoices != null)
                                        foreach (var _pi in purchaseInvoices)
                                            payments.AddRange(_pi.Payments);

                                    payments.Distinct();
                                }
                                else
                                {
                                    saleOrder = null;
                                }
                                if (saleOrder != null)
                                {

                                    saleInvoices = context.saleInvoices
                                  
                                    .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                                    offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);

                                }
                                else
                                {
                                    saleInvoices = null;
                                    offer = null;
                                }
                                if (offer != null)
                                {
                                    inquiry = context.inquiries
                                   .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                                }
                                else
                                {

                                    inquiry = null;
                                }
                                if (saleInvoices != null)
                                {
                                    saleReceipts = context.salesReceipts
                                  .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                                }
                                else
                                {
                                    saleReceipts = null;
                                }



                                adminBill = null;
                                billl = null;
                                bills = null;
                                //purchaseInvoices = null;
                                payment = null;
                                journalVouchers1 = null;
                            }

                        }

                    }
                    else
                    if (voucher.receiptGroupId != 0)
                    {
                        vouchers = voucherRepo.GetVouchersByReceiptGroupId(voucher.receiptGroupId);
                        saleReceipts = context.salesReceipts
                      .Where(x => x.transactionGroupId == voucher.receiptGroupId).ToList();

                    }


                    break;


                case Enums.TransactionItemType.Inquiry:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    inquiry = context.inquiries
                        .FirstOrDefault(x => x.Id == Id);
                    if (inquiry != null)
                    {
                        offer = context.offers.FirstOrDefault(x => x.inquiry_Id == Id);
                    }
                    else
                    {
                        offer = null;
                    }
                    if (offer != null)
                    {
                        saleOrder = context.saleOrders
                       
                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                        if (saleOrder != null && saleOrder.Id != 0)
                        {
                            saleOrders.AddRange(context.saleOrders
                         .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                            if (saleOrder.ParentSO_Id != null)
                                saleOrders.Add(context.saleOrders
                                      .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                        }
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                    }
                    else
                    {
                        saleInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders

                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count != 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count != 0)
                                    payments.AddRange(_PI.Payments.Distinct());

                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }

                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }

                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)

                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;

                //Offer
                case Enums.TransactionItemType.Offer:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    offer = context.offers.FirstOrDefault(x => x.Id == Id);

                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);
                        saleOrder = context.saleOrders
                    
                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                    }
                    else
                    {
                        saleOrder = null;
                        inquiry = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                    }
                    else
                    {
                        saleInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }

                        }

                        if (purchaseInvoices.Count != 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count != 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);

                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)

                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;

                //SaleOrder
                case Enums.TransactionItemType.Sale_Order:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    saleOrder = context.saleOrders
                                                  
                        .FirstOrDefault(x => x.Id == Id);
                    saleOrders.AddRange(context.saleOrders
                                                  .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                    if (saleOrder.ParentSO_Id != null)
                        saleOrders.Add(context.saleOrders
                                             .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                  
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        //bankTransfer = context.interBankTransfers.Include("interBankTransStatus").Include("company")

                        //.Include("department").Include("employee").FirstOrDefault(x => x.Id == saleOrder.interBankTransfer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                    }

                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }

                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    if (bills != null && bills.Count > 0)
                    {
                        if (payments == null)
                            payments = new List<Payment>();
                        foreach (var __bill in bills)
                        {
                            if (__bill.Payments != null && __bill.Payments.Count > 0)
                            {
                                payments.AddRange(__bill.Payments);

                                //foreach (Payment _pymnnt in __bill.Payments)
                                //{
                                //    if(_pymnnt!=null)
                                //    payments.Add(payment);
                                //}
                            }
                        }
                    }
                    if (payments != null && payments.Count > 0)
                        payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Invoice:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var invoice = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
                    if (invoice != null)
                    {
                        saleOrder = context.saleOrders
 
                        .FirstOrDefault(x => x.Id == invoice.SaleOrderId);
                        saleOrders.AddRange(context.saleOrders
                            .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                        if (saleOrder.ParentSO_Id != null)
                            saleOrders.Add(context.saleOrders
                          .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                    }
                    else
                    {
                        saleOrder = null;
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }

                    }
                    else
                    {
                        purchaseOrders = null;
                    }


                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Receipt:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var receipt = context.salesReceipts
                        .FirstOrDefault(x => x.Id == Id);


                    if (receipt.receiptType == ReceiptType.Loans_Advances)
                    {
                        loansAdvances.Add(receipt.loansAdvance);
                        PaymentRepo paymentRepo = new PaymentRepo();
                        payments = new List<Payment>();
                        payments.AddRange(paymentRepo.getPaymentsByLAid((int)receipt.LoansAdvanceId, empID));
                        foreach (var pymnt in payments.ToList())
                        {
                            if (pymnt.loansAdvance != null)
                            {
                                loansAdvances.Add(pymnt.loansAdvance);
                                if (pymnt.loansAdvance.SalesReceipts != null && pymnt.loansAdvance.SalesReceipts.Count > 0)
                                    saleReceipts.AddRange(pymnt.loansAdvance.SalesReceipts);
                                if (pymnt.loansAdvance.Payments != null && pymnt.loansAdvance.Payments.Count > 0)
                                    payments.AddRange(pymnt.loansAdvance.Payments);
                            }
                        }
                        if (saleReceipts.Find(x => x.Id == receipt.Id) != null)
                            saleReceipts.Remove(receipt);
                        saleReceipts = saleReceipts.Distinct().ToList();
                        foreach (var _receipt in saleReceipts)
                        {
                            if (_receipt.loansAdvance != null)
                                loansAdvances.Add(_receipt.loansAdvance);
                        }
                        payments = payments.Distinct().ToList();
                        loansAdvances = loansAdvances.Distinct().ToList();

                    }
                    else if (receipt.receiptType != ReceiptType.Direct_Receipt)
                    {
                        bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(receipt.transactionGroupId, empID, (int)receipt.companyId));
                        vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(receipt.transactionGroupId));
                        if (receipt != null)
                        {
                            saleOrder = context.saleOrders
                         
                            .FirstOrDefault(x => x.Id == receipt.saleInvoice.SaleOrderId);
                            saleOrders.AddRange(context.saleOrders
                         .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                            if (saleOrder.ParentSO_Id != null)
                                saleOrders.Add(context.saleOrders
                                       .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));


                        }
                        else
                        {
                            saleOrder = null;
                        }

                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }

                            if (purchaseInvoices.Count > 0)
                            {
                                payments = new List<Payment>();
                                foreach (var _PI in purchaseInvoices)
                                {
                                    if (_PI.Payments.Count > 0)
                                        payments.AddRange(_PI.Payments.Distinct());
                                }
                                foreach (var paym in payments)
                                {
                                    bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                                }
                                foreach (var paym in payments)
                                {
                                    vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                                }
                            }
                        }
                        else
                        {
                            purchaseOrders = null;
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            bills = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }
                    }

                    break;

                case Enums.TransactionItemType.Admin_Bill:

                    adminBill = context.adminBills
                        .FirstOrDefault(x => x.Id == Id);
                    AdvanceRepo loansAdvanceRepo = new AdvanceRepo();
                    if (adminBill.Payments != null)
                    {
                        var paymentss = adminBill.Payments;

                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _paymnt in paymentss)
                        {
                            payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                        }
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            if (paym.LoansAdvanceId != null)
                                loansAdvances.Add(loansAdvanceRepo.GetLoansAdvance(paym.LoansAdvanceId.Value));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        payments = null;
                    }
                    if (adminBill.LoansAdvanceId != null)
                    {
                        loansAdvances.Add(loansAdvanceRepo.GetLoansAdvance(adminBill.LoansAdvanceId.Value));
                    }
                    if (loansAdvances != null && loansAdvances.Count > 0)
                    {
                        foreach (var _LA in loansAdvances)
                        {
                            if (_LA.AdminBills != null && _LA.AdminBills.Count > 0)
                                adminBills.AddRange(_LA.AdminBills);
                        }
                    }
                    if (adminBills != null && adminBills.Count > 0)
                    {
                        foreach (var _adminBill in adminBills)
                        {
                            if (_adminBill.Payments != null && _adminBill.Payments.Count > 0)
                                payments.AddRange(_adminBill.Payments);
                        }
                    }
                    if (adminBills.FirstOrDefault(x => x.Id == adminBill.Id) != null)
                        adminBills.Remove(adminBills.FirstOrDefault(x => x.Id == adminBill.Id));

                    adminBills = adminBills.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    loansAdvances = loansAdvances.GroupBy(x => x.Id).Select(y => y.First()).ToList();
                    payment = null;
                    billl = null;
                    inquiry = null;
                    offer = null;
                    saleOrder = null;
                    saleInvoices = null;
                    purchaseOrders = null;
                    bills = null;
                    purchaseInvoices = null;
                    journalVouchers1 = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;

                    break;

                case Enums.TransactionItemType.LoansAdvances:

                    loansAdvance = context.loansAdvances
                        .Include("company")
                        .Include("department")
                        .FirstOrDefault(x => x.Id == Id);
                    if (loansAdvance.Payments != null)
                    {
                        var paymentss = loansAdvance.Payments;

                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _paymnt in paymentss)
                        {
                            payments.Add(paymentRepo.GetPayment(_paymnt.Id));
                        }
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        payments = null;
                    }
                    if (loansAdvance.SalesReceipts != null)
                    {
                        var saleReceiptss = loansAdvance.SalesReceipts;

                        SalesReceiptRepo saleReceiptRepo = new SalesReceiptRepo();
                        foreach (var _receipt in saleReceiptss)
                        {
                            saleReceipts.Add(saleReceiptRepo.GetSalesReceipt(_receipt.Id));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (loansAdvance.AdminBills != null)
                    {
                        adminBills = loansAdvance.AdminBills;
                        PaymentRepo paymentRepo = new PaymentRepo();
                        foreach (var _adminBill in adminBills)
                        {
                            if (_adminBill.Payments != null && _adminBill.Payments.Count > 0)
                                payments.AddRange(_adminBill.Payments);
                        }
                    }
                    payments = payments.GroupBy(x => x.Id).Select(y => y.First()).ToList();


                    adminBill = null;
                    payment = null;
                    billl = null;
                    inquiry = null;
                    offer = null;
                    saleOrder = null;
                    saleInvoices = null;
                    purchaseOrders = null;
                    bills = null;
                    purchaseInvoices = null;
                    journalVouchers1 = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                
                    break;

                case Enums.TransactionItemType.Payments:

                    payment = context.payments.FirstOrDefault(x => x.Id == Id);
                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Admin_Bills)
                    {

                        if (payment.AdminBill_Id != null)
                        {
                            adminBill = context.adminBills.FirstOrDefault(x => x.Id == payment.AdminBill_Id);
                        }
                        else
                        {
                            adminBill = null;
                        }
                        billl = null;
                        PInvoice = null;
                        saleOrder = null;
                        offer = null;
                        inquiry = null;
                        invoicedPurchaseOrder = null;

                    }

                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Vendor_Bills)
                    {

                        if (payment.Bill_Id != null)
                        {
                            billl = context.bills.FirstOrDefault(x => x.Id == payment.Bill_Id);
                        }
                        else
                        {
                            billl = null;
                        }

                        if (billl != null)
                        {
                            saleOrder = context.saleOrders
                          
                            .FirstOrDefault(x => x.Id == billl.saleOrder_Id);
                            saleOrders.AddRange(context.saleOrders
                          .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                            if (saleOrder.ParentSO_Id != null)
                                saleOrders.Add(context.saleOrders
                                     .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));


                            if (saleOrder == null)
                            {
                                var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == billl.purchaseOrder_Id);
                                if (purchaseOrder != null)
                                {
                                    saleOrder = context.saleOrders
                                    
                                    .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                    saleOrders.AddRange(context.saleOrders
                            .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                                    if (saleOrder.ParentSO_Id != null)
                                        saleOrders.Add(context.saleOrders
                                           .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));

                                    purchaseOrders.Add(purchaseOrder);
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }

                            }
                            if (saleOrder != null)
                            {

                            }
                            journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == billl.Id).ToList();

                            payments = context.payments.Where(x => x.Bill_Id == billl.Id).ToList();

                        }
                        if (saleOrder != null)
                        {
                            saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (saleOrder != null)
                        {
                            purchaseOrders = context.purchaseOrders
                            .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                            if (purchaseOrders.Count != 0)
                            {
                                foreach (var purchaseOrder in purchaseOrders)
                                {
                                    if (purchaseOrder.PurchaseInvoices.Count != 0)
                                        purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                                }
                            }
                        }


                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                          
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }
                        if (saleOrder != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                        }
                        else if (offer != null)
                        {
                            memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                        }
                        else
                        {
                            memorandumSales = null;
                        }

                        PInvoice = null;
                        payments = null;
                        invoicedPurchaseOrder = null;
                        adminBill = null;
                    }

                    if (payment != null && payment.Id != 0 && payment.transactionType == PaymentTransactionType.Purchase_Invoice)
                    {

                        if (payment.PInvoice_Id != null)
                        {
                            PInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == payment.PInvoice_Id);
                        }
                        else
                        {
                            PInvoice = null;
                        }
                        if (PInvoice != null)
                        {
                            invoicedPurchaseOrder = context.purchaseOrders
                    
                            .FirstOrDefault(x => x.Id == PInvoice.purchaseOrder_Id);
                        }
                        else
                        {
                            invoicedPurchaseOrder = null;
                        }
                        if (invoicedPurchaseOrder != null)
                        {
                            saleOrder = context.saleOrders
              
                            .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                            saleOrders.AddRange(context.saleOrders
      
                                                .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                            if (saleOrder.ParentSO_Id != null)
                                saleOrders.Add(context.saleOrders
                                 .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                        }
                        else
                        {
                            saleOrder = null;
                        }
                        if (saleOrder != null)
                        {
                            //purchaseOrders = context.purchaseOrders
                            //.Include("SaleOrder")
                            //.Include("company")
                            //.Include("AllocateTo")
                            //.Include("department").Include("purchaseInvoices").Include("PurchaseOrderStatus")
                            //.Where(x => x.saleOrder_Id == saleOrder.Id).ToList();


                            saleInvoices = context.saleInvoices
                       .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                            offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                        }
                        else
                        {
                            saleInvoices = null;
                            offer = null;
                        }
                        if (offer != null)
                        {
                            inquiry = context.inquiries
                           .FirstOrDefault(x => x.Id == offer.inquiry_Id);


                        }
                        else
                        {

                            inquiry = null;
                        }
                        if (saleInvoices != null)
                        {
                            saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                            foreach (var recpt in saleReceipts)
                            {
                                var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (trans == null)
                                    bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                            }
                            foreach (var recpt in saleReceipts)
                            {
                                var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                                if (vouch == null)
                                    vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                            }
                        }
                        else
                        {
                            saleReceipts = null;
                        }



                        adminBill = null;
                        billl = null;
                        bills = null;
                        purchaseInvoices = null;
                        journalVouchers1 = null;
                    }
                    break;

                case Enums.TransactionItemType.Purchase_Order:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var PurchaseOrder = context.purchaseOrders
                        .FirstOrDefault(x => x.Id == Id);
                    if (PurchaseOrder != null)
                    {

                        saleOrder = context.saleOrders
                      
                        .FirstOrDefault(x => x.Id == PurchaseOrder.saleOrder_Id);
                        if (saleOrder != null)
                        {
                            saleOrders.AddRange(context.saleOrders
                          .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                            if (saleOrder.ParentSO_Id != null)
                                saleOrders.Add(context.saleOrders
                                     .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                        }
                    }
                    else
                    {
                        saleOrder = null;
                        purchaseInvoices = null;
                    }
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();

                        if (bills.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _bill in bills)
                            {
                                if (_bill.Payments.Count > 0)
                                {
                                    payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());

                                }
                                //payments.AddRange(_bill.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else if (PurchaseOrder != null)
                    {
                        bills = context.bills.Where(x => x.purchaseOrder_Id == (int?)(PurchaseOrder.Id)).ToList();

                        if (bills.Count > 0)
                        {
                            payments = new List<Payment>();
                            foreach (var _bill in bills)
                            {
                                if (_bill.Payments.Count > 0)
                                    payments.AddRange(context.payments.Where(x => x.Bill_Id == _bill.Id).ToList());
                                //payments.AddRange(_bill.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else
                    {
                        bills = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }

                        if (purchaseInvoices.Count > 0)
                        {
                            foreach (var _PI in purchaseInvoices)
                            {
                                if (payments == null)
                                    payments = new List<Payment>();

                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }

                        }
                    }
                    else if (PurchaseOrder != null)
                    {
                        purchaseOrders.Add(PurchaseOrder);
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                        if (purchaseInvoices.Count > 0)
                        {
                            if (payments == null)
                                payments = new List<Payment>();

                            foreach (var _PI in purchaseInvoices)
                            {
                                if (_PI.Payments.Count > 0)
                                    payments.AddRange(_PI.Payments.Distinct());
                            }
                            foreach (var paym in payments)
                            {
                                bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                            }
                            foreach (var paym in payments)
                            {
                                vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                            }
                        }
                    }
                    else
                    {
                        purchaseOrders = null;
                        purchaseInvoices = null;
                    }

                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                    
                     .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }

                    journalVouchers1 = context.journalVouchers.Where(x => x.purchaseOrder_Id == PurchaseOrder.Id).ToList();

                    break;
                case Enums.TransactionItemType.Bill:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;

                    var bill = context.bills.FirstOrDefault(x => x.Id == Id);
                    if (bill != null)
                    {
                        saleOrder = context.saleOrders
                  
                        .FirstOrDefault(x => x.Id == bill.saleOrder_Id);
                        if (saleOrder != null)
                        {
                            saleOrders.AddRange(context.saleOrders
                          .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                            if (saleOrder.ParentSO_Id != null)
                                saleOrders.Add(context.saleOrders
                                   .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                        }
                        if(bill.interBankTransfers.Count>0)
                        bankTransfers = bill.interBankTransfers;
                        if (saleOrder == null)
                        {
                            var purchaseOrder = context.purchaseOrders.FirstOrDefault(x => x.Id == bill.purchaseOrder_Id);
                            if (purchaseOrder != null)
                            {
                                saleOrder = context.saleOrders
                     
                                .FirstOrDefault(x => x.Id == purchaseOrder.saleOrder_Id);
                                bills = context.bills
                         .Where(x => x.purchaseOrder_Id == (int?)(purchaseOrder.Id)).ToList();
                                purchaseOrders.Add(purchaseOrder);
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                            else
                            {
                                bills.Add(bill);
                            }
                        }
                        if (saleOrder != null)
                        {
                            bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        }
                        journalVouchers1 = context.journalVouchers.Where(x => x.bill_Id == bill.Id).ToList();

                        payments = context.payments.Where(x => x.Bill_Id == bill.Id).ToList();
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }

                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders
                        .Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                        if (purchaseOrders.Count != 0)
                        {
                            foreach (var purchaseOrder in purchaseOrders)
                            {
                                if (purchaseOrder.PurchaseInvoices.Count != 0)
                                    purchaseInvoices.AddRange(purchaseOrder.PurchaseInvoices.Distinct());
                            }
                        }
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();

                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else
                    {
                        memorandumSales = null;
                    }
                    break;
                //Memorandum Sale
                case Enums.TransactionItemType.Memorandum_Sale:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    invoicedPurchaseOrder = null;
                    var memorandumSale = context.memorandumSales.Where(x => x.Id == Id).FirstOrDefault();
                    if (memorandumSale != null)
                    {
                        saleOrder = context.saleOrders
                    
                        .FirstOrDefault(x => x.Id == memorandumSale.SaleOrder_Id);
                        saleOrders.AddRange(context.saleOrders
                        .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                        if (saleOrder.ParentSO_Id != null)
                            saleOrders.Add(context.saleOrders
                             .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                    }

                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices
                   .Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers
                               .FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = context.offers.FirstOrDefault(x => x.Id == memorandumSale.Offer_Id);
                    }
                    if (saleOrder != null)
                    {
                        purchaseOrders = context.purchaseOrders.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
              
                    if (saleOrder != null)
                    {
                        bills = context.bills.Where(x => x.saleOrder_Id == (int?)(saleOrder.Id)).ToList();
                    }
                    else
                    {
                        bills = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                       .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    if (saleOrder != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.SaleOrder_Id == saleOrder.Id).ToList();
                    }
                    else if (offer != null)
                    {
                        memorandumSales = context.memorandumSales.Where(x => x.Offer_Id == offer.Id).ToList();
                    }
                    else if (memorandumSale != null)
                    {
                        memorandumSales.Add(memorandumSale);
                    }
                    break;


                case Enums.TransactionItemType.Purchase_Invoice:
                    adminBill = null;
                    payment = null;
                    billl = null;
                    payments = null;
                    PInvoice = null;
                    var purchaseInvoice = context.purchaseInvoices.FirstOrDefault(x => x.Id == Id);
                    if (purchaseInvoice != null)
                    {
                        invoicedPurchaseOrder = context.purchaseOrders
                     
                        .FirstOrDefault(x => x.Id == purchaseInvoice.purchaseOrder_Id);

                        payments = context.payments
                   
                        .Where(x => x.PInvoice_Id == purchaseInvoice.Id).ToList();
                        foreach (var paym in payments)
                        {
                            bankTransfers.AddRange(bankTransferRepo.GetPaymentInterBankTransfers(paym.transactionGroupId, empID, (int)paym.company_Id));
                        }
                        foreach (var paym in payments)
                        {
                            vouchers.AddRange(voucherRepo.GetVouchersByGroupId(paym.transactionGroupId));
                        }
                    }
                    else
                    {
                        invoicedPurchaseOrder = null;
                    }
                    if (invoicedPurchaseOrder != null)
                    {
                        saleOrder = context.saleOrders
                    
                        .FirstOrDefault(x => x.Id == invoicedPurchaseOrder.saleOrder_Id);
                        saleOrders.AddRange(context.saleOrders
                          .Where(x => x.ParentSO_Id == saleOrder.Id).ToList());
                        if (saleOrder.ParentSO_Id != null)
                            saleOrders.Add(context.saleOrders
                                .FirstOrDefault(x => x.Id == saleOrder.ParentSO_Id));
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    if (saleOrder != null)
                    {
                        saleInvoices = context.saleInvoices.Where(x => x.SaleOrderId == saleOrder.Id).ToList();
                        offer = context.offers.FirstOrDefault(x => x.Id == saleOrder.offer_Id);
                    }
                    else
                    {
                        saleInvoices = null;
                        offer = null;
                    }
                    if (offer != null)
                    {
                        inquiry = context.inquiries
                       .FirstOrDefault(x => x.Id == offer.inquiry_Id);

                    }
                    else
                    {

                        inquiry = null;
                    }
                    if (saleInvoices != null)
                    {
                        saleReceipts = context.salesReceipts
                        .Where(x => x.saleInvoice.SaleOrderId == (int?)(saleOrder.Id)).ToList();
                        foreach (var recpt in saleReceipts)
                        {
                            var trans = bankTransfers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (trans == null)
                                bankTransfers.AddRange(bankTransferRepo.GetReceiptInterBankTransfers(saleReceipts[0].transactionGroupId, empID, (int)saleReceipts[0].companyId));
                        }
                        foreach (var recpt in saleReceipts)
                        {
                            var vouch = vouchers.FirstOrDefault(x => x.receiptGroupId == recpt.transactionGroupId);
                            if (vouch == null)
                                vouchers.AddRange(voucherRepo.GetVouchersByReceiptGroupId(recpt.transactionGroupId));
                        }
                    }
                    else
                    {
                        saleReceipts = null;
                    }
                    purchaseInvoices = context.purchaseInvoices.Where(x => x.isVoid != true && x.purchaseOrder_Id == purchaseInvoice.purchaseOrder_Id).ToList();

                    if (purchaseInvoices != null)
                        foreach (var _pi in purchaseInvoices)
                            payments.AddRange(_pi.Payments.Except(payments));
                    payments.Distinct();
                    break;
               
            }
            saleOrders.Add(saleOrder);
            return saleOrders;
        }

        public Inquiry get(int inquiryId)
        {
            return context.inquiries

                .FirstOrDefault(x => x.Id == inquiryId);
        }
        ///<summary>
        ///Get SO from any transaction ID
        /// </summary>
        /// <param name="Id" >TransactionID</param>
        /// 
        public List<SaleOrder> getSoFromTransaction(int Id, ERP_BL.Enums.TransactionItemType type)
        {
            Inquiry inquiry = new Inquiry();
            Offer offer = new Offer();
            SaleOrder saleOrder = new SaleOrder();
            List<SaleInvoice> saleInvoices = new List<SaleInvoice>();
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();

            List<AllTransactionsView> allTransactions = new List<AllTransactionsView>();
            switch (type)
            {
                case Enums.TransactionItemType.Inquiry:
                    offer = context.offers
                             .FirstOrDefault(x => x.inquiry_Id == Id);

                    if (offer != null)
                    {
                        saleOrder = context.saleOrders
                
                        .FirstOrDefault(x => x.offer_Id == offer.Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }

                    break;
                //Offer
                case Enums.TransactionItemType.Offer:

                    saleOrder = context.saleOrders
          
                        .FirstOrDefault(x => x.offer_Id == Id);
                    break;

                //SaleOrder
                case Enums.TransactionItemType.Sale_Order:

                    saleOrder = context.saleOrders
                   
                        .FirstOrDefault(x => x.Id == Id);


                    break;
                //Sale Invoices
                case Enums.TransactionItemType.Sale_Invoice:

                    var invoice = context.saleInvoices.FirstOrDefault(x => x.Id == Id);
                    if (invoice != null)
                    {
                        saleOrder = context.saleOrders
                      
                        .FirstOrDefault(x => x.Id == invoice.SaleOrderId);
                    }
                    else
                    {
                        saleOrder = null;
                    }



                    break;
                case Enums.TransactionItemType.Memorandum_Sale:

                    var memorandumSale = context.memorandumSales.FirstOrDefault(x => x.Id == Id);
                    if (memorandumSale != null)
                    {
                        saleOrder = context.saleOrders
              
                        .FirstOrDefault(x => x.Id == memorandumSale.SaleOrder_Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }



                    break;
                case Enums.TransactionItemType.Purchase_Order:

                    var Po = context.purchaseOrders.FirstOrDefault(x => x.Id == Id);
                    if (Po != null)
                    {
                        saleOrder = context.saleOrders

                        .FirstOrDefault(x => x.Id == Po.saleOrder_Id);
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    break;
                case Enums.TransactionItemType.Bill:

                    var bill = context.bills.FirstOrDefault(x => x.Id == Id);
                    if (bill != null)
                    {
                        if (bill.saleOrder_Id != null)
                        {
                            saleOrder = context.saleOrders
 
                        .FirstOrDefault(x => x.Id == bill.saleOrder_Id);
                        }
                        else if (bill.purchaseOrder_Id != null)
                        {
                            var PO = context.purchaseOrders.FirstOrDefault(x => x.Id == Id);
                            if (PO != null)
                            {
                                saleOrder = context.saleOrders
                  
                                .FirstOrDefault(x => x.Id == PO.saleOrder_Id);
                            }
                            else
                            {
                                saleOrder = null;
                            }
                        }
                        
                    }
                    else
                    {
                        saleOrder = null;
                    }
                    
                    break;

            }

            // Adding all transactions in same structure

            if (saleOrder != null && saleOrder.Id != 0)
            {
                allTransactions.Add(new AllTransactionsView()
                {
                    Id = saleOrder.Id.ToString(),
                    TransactionType = TransactionItemType.Sale_Order.ToString(),
                    Company = saleOrder.company.CompanyName,
                    Employee = saleOrder.employee.person.FName + " " + saleOrder.employee.person.FName,
                    CreationDate = saleOrder.CreationDate,
                    Currency = saleOrder.customerCompany.company.currency.CurrencyName,
                    Department = (saleOrder.department.parentDepartment == null) ? saleOrder.department.DeptName : saleOrder.department.parentDepartment.DeptName + " " + saleOrder.department.DeptName,
                    Customer = saleOrder.customerCompany.company.CompanyName,
                    SalesReferenceNo = saleOrder.SalesReferenceNo,
                    BackColor = saleOrder.saleOrderStatus.backcolor,
                    Status = saleOrder.saleOrderStatus.Status,
                    totalCFRValue = saleOrder.totalCFRValue

                });
            }

            List<SaleOrder> sales = new List<SaleOrder>();
            sales.Add(saleOrder);
            return sales;
        }

        /// <summary>
        /// Get list of all CommentCategories in DB
        /// </summary>
        /// <returns>List of CommentCategories Objects</returns>
        public List<CommentCategory> getallCommentCategory()
        {
            return context.commentCategories
                .ToList();

        }
        /// <summary>
        /// Get All Active CommentCategory
        /// </summary>
        /// <returns></returns>
        public List<CommentCategory> getCurrentTransactionCommentCategories(TransactionItemType type)
        {
            return context.commentCategories.Where(x => x.isActive == true && x.TransactionTypes.FirstOrDefault(y => y.TransactionType == type) != null).ToList();

        }
        /// <summary>
        /// Get All Active CommentCategory
        /// </summary>
        /// <returns></returns>
        public List<CommentCategory> getActiveCommentCategories()
        {
            return context.commentCategories.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive CommentCategories
        /// </summary>
        /// <returns></returns>
        public List<CommentCategory> getinActiveCommentCategories()
        {
            return context.commentCategories.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new CommentCategory
        /// </summary>
        /// <param name="commentCategory">CommentCategory Object</param>
        public void AddCommentCategory(CommentCategory commentCategory)
        {
            context.commentCategories.Add(commentCategory);
            context.SaveChanges();
        }
        /// <summary>
        /// get CommentCategory by ID
        /// </summary>
        /// <param name="commentCategoryid">CommentCategory ID</param>
        /// <returns></returns>
        public CommentCategory getCommentCategory(int commentCategoryId)
        {
            return context.commentCategories
                .FirstOrDefault(x => x.Id == commentCategoryId);
        }
        /// <summarCommentNature
        /// </summary>
        /// <param name="CommentCategory">CommentCategory  Object</param>
        public void UpdateCommentCategory(CommentCategory commentCategory)
        {
            CommentCategory prod = context.commentCategories.FirstOrDefault(x => x.Id == commentCategory.Id);
            prod = commentCategory;
            context.SaveChanges();
        }
        public List<CostSheetBillField> GetSystemBillCosts(int id)
        {
            return context.costSheetBillFields.Where(x => x.Bill_Id == id).ToList();
        }
        public List<CostSheetSIField> GetSystemSICosts(int id)
        {
            return context.costSheetSIFields.Where(x => x.SI_Id == id).ToList();
        }
        public List<CostSheetPOField> GetSystemPOCosts(int id)
        {
            return context.costSheetPOFields.Where(x => x.PO_Id == id).ToList();
        }
        public List<CostSheetPaymentField> GetSystemPaymentCosts(int id)
        {
            return context.costSheetPaymentFields.Where(x => x.Payment_Id == id).ToList();
        }
        public List<CostSheetSaleReceiptField> GetSystemReceiptCosts(int id)
        {
            return context.costSheetSaleReceiptFields.Where(x => x.Receipt_Id == id).ToList();
        }
        public List<CostSheetBillField> GetSystemBillCostsByFieldId(int id, int costSheetId)
        {
            return context.costSheetBillFields.Where(x => x.FieldId == id && x.Value!=0 && x.CostSheetId== costSheetId).ToList();
        }

        public List<CostSheetPOField> GetSystemPOCostsByFieldId(int id, int costSheetId)
        {
            return context.costSheetPOFields.Where(x => x.FieldId == id && x.Value != 0 && x.CostSheetId == costSheetId).ToList();
        }
        public List<CostSheetPaymentField> GetSystemPaymentCostsByFieldId(int id,int costSheetId)
        {
            return context.costSheetPaymentFields.Where(x => x.FieldId == id && x.Value != 0 && x.CostSheetId == costSheetId).ToList();
        }
        public List<CostSheetSaleReceiptField> GetSystemReceiptCostsByFieldId(int id,int costSheetId)
        {
            return context.costSheetSaleReceiptFields.Where(x => x.FieldId == id && x.Value != 0 && x.CostSheetId == costSheetId).ToList();
        }
        public List<CostSheetSIField> GetSystemSaleInvoiceCostsByFieldId(int id, int costSheetId)
        {
            return context.costSheetSIFields.Where(x => x.FieldId == id && x.Value != 0 && x.CostSheetId == costSheetId).ToList();
        }
        public List<CostSheetField> GetAllCostSheetFields()
        {
            return context.costSheetFields.ToList();
        }
        public CostSheetField GetSheetField(int id)
        {
            return context.costSheetFields.FirstOrDefault(x=>x.Id==id);
        }
        public Offer GetComparativeOffer(int OfferId)
        {
            return context.offers.FirstOrDefault(x => x.Id == OfferId);

        }  
        public List<ERP_BL.Procurements.StatusClass.StatusClass> GetActiveStatusClasses()
        {
            return context.statusClasses.Where(x => x.isActive == true).ToList();
        } 
        public ERP_BL.Procurements.StatusClass.StatusClass GetStatusClass(int statusClassId)
        {
            return context.statusClasses.FirstOrDefault(x => x.Id == statusClassId);

        } 
        public void AddStatusClass(ERP_BL.Procurements.StatusClass.StatusClass statusClass)
        {
            context.statusClasses.Add(statusClass);
            context.SaveChanges();

        } 
        public void UpdateStatusClass(ERP_BL.Procurements.StatusClass.StatusClass statusClass)
        {
            var dbStatusClass= context.statusClasses.FirstOrDefault(x=>x.Id==statusClass.Id);
            dbStatusClass = statusClass;
            context.SaveChanges();

        }
        public CommentLog getcommentlogAsc(int TransactionId, TransactionItemType transactiontype)
        {
            Console.WriteLine(transactiontype);
            List<CommentLog> comments = new List<CommentLog>();
            DateTime _creationDate = DateTime.Today;
            if (transactiontype == TransactionItemType.ToDo_Task)
                _creationDate = context.toDoTasks.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.toDoTasks.FirstOrDefault(x => x.Id == TransactionId).creationDate;
            if (transactiontype == TransactionItemType.Loans)
                _creationDate = context.loans.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.loans.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            if (transactiontype == TransactionItemType.Payments)
                _creationDate = context.payments.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.payments.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            if (transactiontype == TransactionItemType.Admin_Bill)
                _creationDate = context.adminBills.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.adminBills.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Bill)
                _creationDate = context.bills.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.bills.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            //else if (transactiontype == TransactionItemType.CostCenter)
            //_creationDate = context.costSheets.FirstOrDefault(x => x.Id == TransactionId) == null? _creationDate : (DateTime)context.costSheets.FirstOrDefault(x => x.Id == TransactionId).Timestamp; 
            else if (transactiontype == TransactionItemType.Inquiry)
            {
                //_creationDate = context.inquiries.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.inquiries.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
                comments= context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();
                var comment = comments.FirstOrDefault(p => p.Id == comments.Max(x => x.Id));
                return comment;
            }
            else if (transactiontype == TransactionItemType.InterBank_Transfer)
                _creationDate = context.interBankTransfers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.interBankTransfers.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.InterCompanyBank_Transfer)
                _creationDate = context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.interCompanyBankTransfers.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.JV)
                _creationDate = context.journalVouchers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.journalVouchers.FirstOrDefault(x => x.Id == TransactionId).postingDate;
            else if (transactiontype == TransactionItemType.Leave)
                _creationDate = context.leaveApplications.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.leaveApplications.FirstOrDefault(x => x.Id == TransactionId).ApplyDate;
            else if (transactiontype == TransactionItemType.Memorandum_Sale)
                _creationDate = context.memorandumSales.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.memorandumSales.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Offer)
            {
                //_creationDate = context.offers.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.offers.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
                comments = context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();
                var comment = comments.FirstOrDefault(p => p.Id == comments.Max(x => x.Id));
                return comment;
            }
            else if (transactiontype == TransactionItemType.Purchase_Invoice)
                _creationDate = context.purchaseInvoices.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.purchaseInvoices.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Purchase_Order)
                _creationDate = context.purchaseOrders.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.purchaseOrders.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Sale_Invoice)
                _creationDate = context.saleInvoices.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.saleInvoices.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Sale_Order)
                _creationDate = context.saleOrders.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.saleOrders.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.Sale_Receipt)
                _creationDate = context.salesReceipts.FirstOrDefault(x => x.transactionGroupId == TransactionId) == null ? _creationDate : (DateTime)context.salesReceipts.FirstOrDefault(x => x.transactionGroupId == TransactionId).CreationDate;
            else if (transactiontype == TransactionItemType.SummarySheet)
                _creationDate = context.summarySheetFields.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.summarySheetFields.FirstOrDefault(x => x.Id == TransactionId).Timestamp;
            else if (transactiontype == TransactionItemType.Budget)
                _creationDate = context.budgetCostSheets.FirstOrDefault(x => x.Id == TransactionId) == null ? _creationDate : (DateTime)context.budgetCostSheets.FirstOrDefault(x => x.Id == TransactionId).CreationDate;
            if (transactiontype == TransactionItemType.CostCenter)
            {
                comments = context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype).ToList();
                var comment = comments.FirstOrDefault(p => p.Id == comments.Max(x => x.Id));
                return comment;
            }
            else
            {
                comments = context.CommentLogs.OrderBy(x => x.Timestamp).Where(x => x.TransactionId == TransactionId && x.TransactionType == transactiontype /*&& x.Timestamp >= _creationDate*/).ToList();
                var comment= comments.FirstOrDefault(p => p.Id == comments.Max(x => x.Id));
                return comment;
            }

        }

    }
}
