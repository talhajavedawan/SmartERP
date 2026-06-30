using ERP_BL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Databases
{
    public class NotificationsRepo
    {
        DBContextERP context = new DBContextERP();

        public NotificationsRepo()
        {
            SystemLog.LogInfo(this.GetType(), "Instance Created");

        }
        /// <summary>
        /// Add new Notification 
        /// </summary>
        /// <param name="notification">Notification  Object</param>
        public void Add(Notification notification)
        {
            context.Notifications.Add(notification);
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Added Notification with Name= " + notification.Title + " Id= " + notification.Id);

        }


        /// <summary>
        /// return all Notification Company list.
        /// </summary>
        /// <returns></returns>
        public List<Notification> getAll()
        {
            SystemLog.LogInfo(this.GetType(), "Retrived List of All Notifications");
            return context.Notifications
                .Where(x => x.TransactionType != TransactionItemType.Memo)
                .ToList();
        }

        /// <summary>
        /// get Notification matching to ID
        /// </summary>
        /// <param name="notificationID">Notification ID</param>
        /// <returns></returns>
        public Notification get(int notificationID)
        {
            SystemLog.LogInfo(this.GetType(), "Retrive Notification with  Id= " + notificationID);
            return context.Notifications

                .FirstOrDefault(x => x.Id == notificationID);
        }
        /// <summary>
        /// get Notifications matching to userID
        /// </summary>
        /// <param name="userID">userID</param>
        /// <returns></returns>
        public List<Notification> getAllForUser(int userId)
        {
            SystemLog.LogInfo(this.GetType(), "Retrive Notification for  userId= " + userId);
            return context.Notifications

                .Where(x => x.Id == userId && x.TransactionType != TransactionItemType.Memo).ToList();
        }


        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void Update(Notification notification)
        {
            Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == notification.Id);
            notificationToUpdate = notification;
            context.SaveChanges();
            SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notification.Title + " Id= " + notification.Id);
        }

        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void MarkasRead(int id)
        {
            try
            {
                Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == id);
                if (notificationToUpdate != null)
                {
                    notificationToUpdate.ReadTimestamp = System.DateTime.Now;
                    notificationToUpdate.isRead = true;
                    context.SaveChanges();
                    SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notificationToUpdate.Title + " Id= " + notificationToUpdate.Id);
                }
            }
            catch { }
        }

        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void MarkasReadNotificationList(List<Notification> notifications)
        {
            try
            {
                foreach(var _notification in notifications)
                {
                    Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == _notification.Id);
                    if (notificationToUpdate != null)
                    {
                        notificationToUpdate.ReadTimestamp = System.DateTime.Now;
                        notificationToUpdate.isRead = true;
                        SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notificationToUpdate.Title + " Id= " + notificationToUpdate.Id);
                    }
                }
                

                context.SaveChanges();
            }
            catch { }
        }

        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void MarkasUnRead(int id)
        {
            try
            {
                Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == id);
                if (notificationToUpdate != null)
                {
                    notificationToUpdate.ReadTimestamp = System.DateTime.Now;
                    notificationToUpdate.isRead = false;
                    context.SaveChanges();
                    SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notificationToUpdate.Title + " Id= " + notificationToUpdate.Id);
                }
            }
            catch { }
        }


        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void MarkasUnReadNotificationList(List<Notification> notifications)
        {
            try
            {
                foreach (var _notification in notifications)
                {
                    Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == _notification.Id);
                    if (notificationToUpdate != null)
                    {
                        notificationToUpdate.ReadTimestamp = System.DateTime.Now;
                        notificationToUpdate.isRead = false;
                        SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notificationToUpdate.Title + " Id= " + notificationToUpdate.Id);
                    }
                }
                context.SaveChanges();
            }
            catch { }
        }


        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void MarkasPendingNotificationList(List<Notification> notifications)
        {
            try
            {
                foreach (var _notification in notifications)
                {
                    Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == _notification.Id);
                    if (notificationToUpdate != null)
                    {
                        notificationToUpdate.isPending = true;
                        SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notificationToUpdate.Title + " Id= " + notificationToUpdate.Id);
                    }
                }


                context.SaveChanges();
            }
            catch { }
        }

        /// <summary>
        /// update Notification  object details
        /// </summary>
        /// <param name="notification">Notification Object</param>
        public void UnMarkasPendingNotificationList(List<Notification> notifications)
        {
            try
            {
                foreach (var _notification in notifications)
                {
                    Notification notificationToUpdate = context.Notifications.FirstOrDefault(x => x.Id == _notification.Id);
                    if (notificationToUpdate != null)
                    {
                        notificationToUpdate.isPending = false;
                        SystemLog.LogInfo(this.GetType(), "Updated Notification with title= " + notificationToUpdate.Title + " Id= " + notificationToUpdate.Id);
                    }
                }


                context.SaveChanges();
            }
            catch { }
        }

        /// <summary>
        /// Add new notification for transaction with CC User
        /// </summary>
        /// <param name="notification">notification  Object for Inquiry</param>
        public void Add(string Title, int Transactionid, TransactionItemType Transactiontype, string description, int userid, int CCuserid, string info, int? flagId)
        {
            try
            {
                bool flagGlow = false;

                if (flagId != null && flagId != 0)
                {
                    NotificationFlag notificationFlag = context.notificationFlags.FirstOrDefault(x => x.Id == flagId);
                    flagGlow = notificationFlag.canGlow;
                }
                Notification notification = new Notification();
                if (SystemLog.CurrentUserId != 0)
                {

                    
                    notification.CcUserId = CCuserid;
                    notification.SendingUserId = SystemLog.CurrentUserId;
                    notification.TransactionId = Transactionid;
                    notification.TransactionType = Transactiontype;
                    notification.Timestamp = System.DateTime.Now;
                    notification.Info = info;
                    notification.Title = Title;
                    notification.ReadTimestamp = System.DateTime.Now;
                    notification.Description = description;
                    notification.FlagId = flagId;
                    notification.Glow = flagGlow;
                    context.Notifications.Add(notification);

                    context.SaveChanges();


                    SystemLog.LogInfo(this.GetType(), "Added notification  Id= " + notification.Id);
                }
            }
            catch (Exception ex) { }
        }


        public void AddNotificationForCC(string Title, int Transactionid, TransactionItemType Transactiontype, string description, int userid, int CCuserid, string info, int? flagId, int commentId)
        {
            try
            {
                bool flagGlow = false;

                if (flagId != null && flagId != 0)
                {
                    NotificationFlag notificationFlag = context.notificationFlags.FirstOrDefault(x => x.Id == flagId);
                    flagGlow = notificationFlag.canGlow;
                }
                Notification notification = new Notification();
                if (SystemLog.CurrentUserId != 0)
                {


                    notification.CcUserId = CCuserid;
                    notification.SendingUserId = SystemLog.CurrentUserId;
                    notification.TransactionId = Transactionid;
                    notification.TransactionType = Transactiontype;
                    notification.Timestamp = System.DateTime.Now;
                    notification.Info = info;
                    notification.Title = Title;
                    notification.ReadTimestamp = System.DateTime.Now;
                    notification.Description = description;
                    notification.FlagId = flagId;
                    notification.Glow = flagGlow;
                    notification.commentLogId = commentId;
                    context.Notifications.Add(notification);

                    context.SaveChanges();


                    SystemLog.LogInfo(this.GetType(), "Added notification  Id= " + notification.Id);
                }
            }
            catch (Exception ex) { }
        }

        public void AddNotificationForTag(string Title, int Transactionid, TransactionItemType Transactiontype, string description, int userid, string info, int? flagId,int commentId)
        {
            try
            {
                bool flagGlow = false;

                if (flagId != null && flagId != 0)
                {
                    NotificationFlag notificationFlag = context.notificationFlags.FirstOrDefault(x => x.Id == flagId);
                    flagGlow = notificationFlag.canGlow;
                }

                Notification notification = new Notification();
                if (SystemLog.CurrentUserId != 0)
                {

                    notification.UserId = userid;
                    notification.SendingUserId = SystemLog.CurrentUserId;
                    notification.TransactionId = Transactionid;
                    notification.TransactionType = Transactiontype;
                    notification.Timestamp = System.DateTime.Now;
                    notification.Info = info;
                    notification.Title = Title;
                    notification.ReadTimestamp = System.DateTime.Now;
                    notification.Description = description;
                    notification.FlagId = flagId;
                    notification.Glow = flagGlow;
                    notification.commentLogId = commentId;
                    context.Notifications.Add(notification);

                    context.SaveChanges();


                    SystemLog.LogInfo(this.GetType(), "Added notification  Id= " + notification.Id);
                }
            }
            catch (Exception ex) { }
        }


        /// <summary>
        /// Add new notification for transaction
        /// </summary>
        /// <param name="notification">notification  Object for Inquiry</param>
        public void Add(string Title, int Transactionid, TransactionItemType Transactiontype, string description, int userid, string info, int? flagId)
        {
            try
            {
                bool flagGlow = false;

                if(flagId != null && flagId != 0)
                {
                    NotificationFlag notificationFlag = context.notificationFlags.FirstOrDefault(x=>x.Id == flagId);
                    flagGlow = notificationFlag.canGlow;
                }

                Notification notification = new Notification();
                if (SystemLog.CurrentUserId != 0)
                {
                   
                        notification.UserId = userid;
                        notification.SendingUserId = SystemLog.CurrentUserId;
                        notification.TransactionId = Transactionid;
                        notification.TransactionType = Transactiontype;
                        notification.Timestamp = System.DateTime.Now;
                        notification.Info = info;
                        notification.Title = Title;
                        notification.ReadTimestamp = System.DateTime.Now;
                        notification.Description = description;
                    notification.FlagId = flagId;
                    notification.Glow = flagGlow;
                        context.Notifications.Add(notification);

                        context.SaveChanges();

                    
                    SystemLog.LogInfo(this.GetType(), "Added notification  Id= " + notification.Id);
                }
            }
            catch (Exception ex) { }
        }
        public void Add(string Title, int Transactionid, TransactionItemType Transactiontype, string description, int? userid, int? CCuserId, string info, string billReference, string poReference)
        {
            try
            {
                Notification notification = new Notification();
                if (SystemLog.CurrentUserId != 0)
                {
                    notification.UserId = userid;
                    notification.CcUserId = CCuserId;
                    notification.SendingUserId = SystemLog.CurrentUserId;
                    notification.TransactionId = Transactionid;
                    notification.TransactionType = Transactiontype;
                    notification.Timestamp = System.DateTime.Now;
                    notification.Info = info;
                    notification.Title = Title;
                    notification.ReadTimestamp = System.DateTime.Now;
                    notification.Description = description;
                    if (billReference != null)
                    {
                        notification.BillReferenceNo = billReference;
                    }
                    if (poReference != null)
                    {
                        notification.PoReferenceNo = poReference;
                    }
                    context.Notifications.Add(notification);


                    context.SaveChanges();
                    SystemLog.LogInfo(this.GetType(), "Added notification  Id= " + notification.Id);
                }
            }
            catch { }
        }

        /// <summary>
        /// Get List of Notification for user
        /// </summary>
        /// 
        public List<Notification> getUsersNotificationOrderAsc(int UserId)
        {
            return context.Notifications.OrderBy(x => x.Timestamp).Where(x => x.UserId == UserId && x.TransactionType != TransactionItemType.Memo).ToList();

        }
        /// <summary>
        /// Get List of Notification for user

        /// </summary>
        /// 
        public List<Notification> getUsersNotificationOrderDsc(int UserId)
        {
            try
            {
                DateTime yesterday = DateTime.Now.AddDays(-2);
                //return await Task.Run(() => context.Notifications.Include("User.employee.person").OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId && x.Timestamp > yesterday).ToList());

                var notifications = context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => (x.UserId == UserId || x.CcUserId == UserId) && x.Timestamp > yesterday && x.TransactionType != TransactionItemType.Memo).Take(20).ToList();
                return notifications;
            }
            catch(Exception ex)
            {
                return null;
            }

            
        }

        /// <summary>
        /// Get List of Notification for user

        /// </summary>
        /// 
        public List<Notification> getMoreUsersNotificationOrderDsc(int skipCount, int UserId)
        {

            //return await Task.Run(() => context.Notifications.Include("User.employee.person").OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId && x.Timestamp > yesterday).ToList());

            var notifications = context.Notifications
                .OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId || x.CcUserId == UserId && x.TransactionType != TransactionItemType.Memo).Skip(skipCount).Take(20).ToList();
            return notifications;
        }

        /// <summary>
        /// Get List of Notification for user

        /// </summary>
        /// 
        public List<Notification> getMoreUsersNotificationOrderByDate(DateTime dateFrom, DateTime dateTo, int UserId)
        {

            //return await Task.Run(() => context.Notifications.Include("User.employee.person").OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId && x.Timestamp > yesterday).ToList());
            dateTo = dateTo.AddDays(1);
            var notifications = context.Notifications
            
                .OrderByDescending(x => x.Timestamp)
                .Where(x => x.Timestamp >= dateFrom && x.Timestamp <= dateTo  &&( x.UserId == UserId || x.CcUserId == UserId) && x.TransactionType != TransactionItemType.Memo).ToList();
            return notifications;
        }


        /// <summary>
        /// Get List of Notification for user

        /// </summary>
        /// 
        public List<Notification> getAllInboxNotifications( int UserId)
        {

            //return await Task.Run(() => context.Notifications.Include("User.employee.person").OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId && x.Timestamp > yesterday).ToList());

            var notifications = context.Notifications
                .OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId || x.CcUserId == UserId && x.TransactionType != TransactionItemType.Memo).ToList();
            return notifications;
        }


        /// <summary>
        /// Get  Notification for user

        /// </summary>
        /// 
        public int getNotification(int UserId)
        {
            var date = DateTime.Now;
            var datFrom = date.AddDays(-3);
            var datTo = date.AddDays(1);
            //return await Task.Run(() => context.Notifications.Include("User.employee.person").OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId && x.Timestamp > yesterday).ToList());

            var notifications = context.Notifications.Where(x => /*x.Timestamp >= datFrom && x.Timestamp <= datTo &&*/ x.UserId == UserId && x.notificationFlag.canGlow == true && x.Glow == true && x.TransactionType != TransactionItemType.Memo).Count();
            return notifications;
        }

        /// <summary>
        /// Get List of Notification for user

        /// </summary>
        /// 
        public List<Notification> RefreshUsersNotificationOrderDsc(int UserId, int count)
        {
            return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId && x.TransactionType != TransactionItemType.Memo).Take(count).ToList();
        }
        /// <summary>
        /// Get List of Notification for user

        /// </summary>
        /// 
        public List<Notification> getLoadMoreUsersNotificationOrderDsc(int UserId, int skipcount)
        {
            return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => x.UserId == UserId || x.CcUserId == UserId && x.TransactionType != TransactionItemType.Memo).Skip(skipcount).Take(20).ToList();

        }

        public object SentNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => x.SendingUserId == currentUserid && x.TransactionType != TransactionItemType.Memo).Take(20).ToList();
            }
            catch
            { }
            return null;

        }


        public List<Notification> getMoreSentNotifications(int skipCount, int currentUserid)
        {
            try
            {
                return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => x.SendingUserId == currentUserid && x.TransactionType != TransactionItemType.Memo).Skip(skipCount).Take(20).ToList();
            }
            catch
            { }
            return null;

        }

        public List<Notification> getMoreSentNotificationsByDate(DateTime dateFrom, DateTime dateTo, int UserId)
        {
            try
            {
                dateTo = dateTo.AddDays(1);
                var notifications = context.Notifications
                    .OrderByDescending(x => x.Timestamp)
                    .Where(x => x.Timestamp >= dateFrom && x.Timestamp <= dateTo && x.SendingUserId == UserId && x.TransactionType != TransactionItemType.Memo).ToList();

                return notifications;
            }
            catch
            { }
            return null;

        }

       


        public List<Notification> getAllSentNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => x.SendingUserId == currentUserid && x.TransactionType != TransactionItemType.Memo).ToList();
            }
            catch
            { }
            return null;

        }

        ////
        ///All Unread
        ///
        public object AllUnreadNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => (x.UserId == currentUserid || x.CcUserId == currentUserid) && x.isRead == false && x.TransactionType != TransactionItemType.Memo)/*.Take(50)*/.ToList();
            }
            catch
            { }
            return null;

        }


        ////
        ///All Pending
        ///
        public object AllPendingNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.OrderByDescending(x => x.Timestamp).Where(x => (x.UserId == currentUserid || x.CcUserId == currentUserid) && x.isPending == true && x.TransactionType != TransactionItemType.Memo)/*.Take(50)*/.ToList();
            }
            catch
            { }
            return null;

        }


        ////
        ///All Unread
        ///
        public object AllUrgentNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.Where(x => x.UserId == currentUserid && x.notificationFlag.canGlow == true && x.Glow == true && x.TransactionType != TransactionItemType.Memo)/*.Take(50)*/.ToList();
            }
            catch
            { }
            return null;

        }

        ////
        ///All Unread
        ///
        public int CountAllUnreadNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.Count(x => (x.UserId == currentUserid || x.CcUserId == currentUserid ) && x.isRead == false && x.TransactionType != TransactionItemType.Memo);
            }
            catch
            { }
            return 0;

        }


        ////
        ///All Pending
        ///
        public int CountAllPendingNotifications(int currentUserid)
        {
            try
            {
                return context.Notifications.Count(x => (x.UserId == currentUserid || x.CcUserId == currentUserid ) && x.isPending == true && x.TransactionType != TransactionItemType.Memo);
            }
            catch
            { }
            return 0;

        }


        public void AddNotificationFlag(NotificationFlag flag)
        {
            context.notificationFlags.Add(flag);
            context.SaveChanges();

        }

        public void UpdateNotificationFlag(NotificationFlag flag)
        {
            if (flag == null)
                throw new NullReferenceException("Object can not be null");
            var _flag = context.notificationFlags.FirstOrDefault(x => x.Id == flag.Id);

            _flag.Flag = flag.Flag;
            _flag.isActive = flag.isActive;
            _flag.backcolor = flag.backcolor;
            _flag.canGlow = flag.canGlow;

            context.SaveChanges();

        }

        public NotificationFlag GetNotificationFlag(int flagId)
        {
            var flag = context.notificationFlags.FirstOrDefault(x => x.Id == flagId);
            return flag;

        }
        public Notification GteLastTagNotification(int UserID)
        {
            var LastTag = context.Notifications.ToList().LastOrDefault(x => x.UserId == UserID && x.TransactionType != TransactionItemType.Memo); 
            return LastTag;

        }

        public List<NotificationFlag> GetAllNotificationFlags()
        {
            var flagList = context.notificationFlags.ToList();
            return flagList;

        }

        public List<NotificationFlag> GetAllCloseNotificationFlags()
        {
            var flagList = context.notificationFlags.Where(x => x.isActive == false)
                .ToList();
            return flagList;

        }

        public List<NotificationFlag> GetAllOpenFlags()
        {
            var flagList = context.notificationFlags.Where(x => x.isActive == true)
                .ToList();
            return flagList;

        }
    }
}
