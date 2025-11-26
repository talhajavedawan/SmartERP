using ERP_BL.Data;
using ERP_BL.Entities.Notifications;
using ERP_BL.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ERP_REPO.Repo.Notifications
{
    public interface INotificationRepo
    {
        // Basic CRUD operations
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(int id);
        Task<Notification?> CreateNotificationAsync(Notification notification);
        Task<bool> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int id);

        // User-specific notifications
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, int take = 20);
        Task<IEnumerable<Notification>> GetMoreUserNotificationsAsync(int userId, int skip, int take = 20);
        Task<IEnumerable<Notification>> GetUserNotificationsByDateAsync(int userId, DateTime dateFrom, DateTime dateTo);

        // Sent notifications
        Task<IEnumerable<Notification>> GetSentNotificationsAsync(int sendingUserId, int take = 20);
        Task<IEnumerable<Notification>> GetMoreSentNotificationsAsync(int sendingUserId, int skip, int take = 20);
        Task<IEnumerable<Notification>> GetSentNotificationsByDateAsync(int sendingUserId, DateTime dateFrom, DateTime dateTo);

        // Unread notifications
        Task<IEnumerable<Notification>> GetAllUnreadNotificationsAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);

        // Pending notifications
        Task<IEnumerable<Notification>> GetAllPendingNotificationsAsync(int userId);
        Task<int> GetPendingCountAsync(int userId);

        // Urgent notifications
        Task<IEnumerable<Notification>> GetAllUrgentNotificationsAsync(int userId);
        Task<int> GetUrgentCountAsync(int userId);

        // Mark operations
        Task<bool> MarkAsReadAsync(int notificationId);
        Task<bool> MarkAsUnreadAsync(int notificationId);
        Task<bool> MarkMultipleAsReadAsync(IEnumerable<int> notificationIds);
        Task<bool> MarkMultipleAsUnreadAsync(IEnumerable<int> notificationIds);
        Task<bool> MarkAsPendingAsync(IEnumerable<int> notificationIds);
        Task<bool> UnmarkAsPendingAsync(IEnumerable<int> notificationIds);

        // Notification Flags
        Task<IEnumerable<NotificationFlag>> GetAllNotificationFlagsAsync();
        Task<IEnumerable<NotificationFlag>> GetActiveNotificationFlagsAsync();
        Task<NotificationFlag?> GetNotificationFlagByIdAsync(int flagId);
        Task<NotificationFlag?> CreateNotificationFlagAsync(NotificationFlag flag);
        Task<bool> UpdateNotificationFlagAsync(NotificationFlag flag);
    }

    public class NotificationService : INotificationRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(ApplicationDbContext context, ILogger<NotificationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Basic CRUD operations
        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .OrderByDescending(n => n.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all notifications");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<Notification?> GetNotificationByIdAsync(int id)
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .FirstOrDefaultAsync(n => n.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching notification with ID {id}");
                return null;
            }
        }

        public async Task<Notification?> CreateNotificationAsync(Notification notification)
        {
            try
            {
                notification.Timestamp = DateTime.UtcNow;
                _context.Notifications.Add(notification);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification created with ID {notification.Id}");
                return notification;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification");
                return null;
            }
        }

        public async Task<bool> UpdateNotificationAsync(Notification notification)
        {
            try
            {
                var existing = await _context.Notifications.FindAsync(notification.Id);
                if (existing == null)
                {
                    _logger.LogWarning($"Notification with ID {notification.Id} not found");
                    return false;
                }

                _context.Entry(existing).CurrentValues.SetValues(notification);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification with ID {notification.Id} updated");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating notification with ID {notification.Id}");
                return false;
            }
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(id);
                if (notification == null)
                {
                    _logger.LogWarning($"Notification with ID {id} not found");
                    return false;
                }

                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification with ID {id} deleted");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting notification with ID {id}");
                return false;
            }
        }

        // User-specific notifications
        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, int take = 20)
        {
            try
            {
                var twoDaysAgo = DateTime.UtcNow.AddDays(-2);
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && n.Timestamp > twoDaysAgo
                          && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching notifications for user {userId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<IEnumerable<Notification>> GetMoreUserNotificationsAsync(int userId, int skip, int take = 20)
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching more notifications for user {userId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsByDateAsync(int userId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                dateTo = dateTo.AddDays(1); // Include the entire last day
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && n.Timestamp >= dateFrom && n.Timestamp <= dateTo
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching notifications by date for user {userId}");
                return Enumerable.Empty<Notification>();
            }
        }

        // Sent notifications
        public async Task<IEnumerable<Notification>> GetSentNotificationsAsync(int sendingUserId, int take = 20)
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => n.SendingUserId == sendingUserId
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching sent notifications for user {sendingUserId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<IEnumerable<Notification>> GetMoreSentNotificationsAsync(int sendingUserId, int skip, int take = 20)
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => n.SendingUserId == sendingUserId
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .Skip(skip)
                    .Take(take)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching more sent notifications for user {sendingUserId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<IEnumerable<Notification>> GetSentNotificationsByDateAsync(int sendingUserId, DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                dateTo = dateTo.AddDays(1);
                return await _context.Notifications
                   .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => n.SendingUserId == sendingUserId
                           && n.Timestamp >= dateFrom && n.Timestamp <= dateTo
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching sent notifications by date for user {sendingUserId}");
                return Enumerable.Empty<Notification>();
            }
        }

        // Unread notifications
        public async Task<IEnumerable<Notification>> GetAllUnreadNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && !n.IsRead
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching unread notifications for user {userId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && !n.IsRead
                           && n.TransactionType != TransactionItemType.Undefined)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting unread count for user {userId}");
                return 0;
            }
        }

        // Pending notifications
        public async Task<IEnumerable<Notification>> GetAllPendingNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                   .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && n.IsPending
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching pending notifications for user {userId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<int> GetPendingCountAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => (n.UserId == userId || n.CcUserId == userId)
                           && n.IsPending
                           && n.TransactionType != TransactionItemType.Undefined)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting pending count for user {userId}");
                return 0;
            }
        }

        // Urgent notifications
        public async Task<IEnumerable<Notification>> GetAllUrgentNotificationsAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Include(n => n.User)
                    .Include(n => n.SendingUser)
                    .Include(n => n.CcUser)
                    .Include(n => n.NotificationFlag)
                    .Include(n => n.NotificationFlag)
                    .Where(n => n.UserId == userId
                           && n.NotificationFlag != null
                           && n.NotificationFlag.CanGlow
                           && n.Glow
                           && n.TransactionType != TransactionItemType.Undefined)
                    .OrderByDescending(n => n.Timestamp)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching urgent notifications for user {userId}");
                return Enumerable.Empty<Notification>();
            }
        }

        public async Task<int> GetUrgentCountAsync(int userId)
        {
            try
            {
                return await _context.Notifications
                    .Where(n => n.UserId == userId
                           && n.NotificationFlag != null
                           && n.NotificationFlag.CanGlow
                           && n.Glow
                           && n.TransactionType != TransactionItemType.Undefined)
                    .CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting urgent count for user {userId}");
                return 0;
            }
        }

        // Mark operations
        public async Task<bool> MarkAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    _logger.LogWarning($"Notification with ID {notificationId} not found");
                    return false;
                }

                notification.IsRead = true;
                notification.ReadTimestamp = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification {notificationId} marked as read");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {notificationId} as read");
                return false;
            }
        }

        public async Task<bool> MarkAsUnreadAsync(int notificationId)
        {
            try
            {
                var notification = await _context.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    _logger.LogWarning($"Notification with ID {notificationId} not found");
                    return false;
                }

                notification.IsRead = false;
                notification.ReadTimestamp = null;
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification {notificationId} marked as unread");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {notificationId} as unread");
                return false;
            }
        }

        public async Task<bool> MarkMultipleAsReadAsync(IEnumerable<int> notificationIds)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => notificationIds.Contains(n.Id))
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = true;
                    notification.ReadTimestamp = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Marked {notifications.Count} notifications as read");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking multiple notifications as read");
                return false;
            }
        }

        public async Task<bool> MarkMultipleAsUnreadAsync(IEnumerable<int> notificationIds)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => notificationIds.Contains(n.Id))
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsRead = false;
                    notification.ReadTimestamp = null;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Marked {notifications.Count} notifications as unread");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking multiple notifications as unread");
                return false;
            }
        }

        public async Task<bool> MarkAsPendingAsync(IEnumerable<int> notificationIds)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => notificationIds.Contains(n.Id))
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsPending = true;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Marked {notifications.Count} notifications as pending");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notifications as pending");
                return false;
            }
        }

        public async Task<bool> UnmarkAsPendingAsync(IEnumerable<int> notificationIds)
        {
            try
            {
                var notifications = await _context.Notifications
                    .Where(n => notificationIds.Contains(n.Id))
                    .ToListAsync();

                foreach (var notification in notifications)
                {
                    notification.IsPending = false;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Unmarked {notifications.Count} notifications as pending");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unmarking notifications as pending");
                return false;
            }
        }

        // Notification Flags
        public async Task<IEnumerable<NotificationFlag>> GetAllNotificationFlagsAsync()
        {
            try
            {
                return await _context.NotificationFlags
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all notification flags");
                return Enumerable.Empty<NotificationFlag>();
            }
        }

        public async Task<IEnumerable<NotificationFlag>> GetActiveNotificationFlagsAsync()
        {
            try
            {
                return await _context.NotificationFlags
                    .Where(f => f.IsActive)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active notification flags");
                return Enumerable.Empty<NotificationFlag>();
            }
        }

        public async Task<NotificationFlag?> GetNotificationFlagByIdAsync(int flagId)
        {
            try
            {
                return await _context.NotificationFlags
                    .FirstOrDefaultAsync(f => f.Id == flagId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching notification flag with ID {flagId}");
                return null;
            }
        }

        public async Task<NotificationFlag?> CreateNotificationFlagAsync(NotificationFlag flag)
        {
            try
            {
                flag.CreatedDate = DateTime.UtcNow;
                _context.NotificationFlags.Add(flag);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification flag created with ID {flag.Id}");
                return flag;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification flag");
                return null;
            }
        }

        public async Task<bool> UpdateNotificationFlagAsync(NotificationFlag flag)
        {
            try
            {
                var existing = await _context.NotificationFlags.FindAsync(flag.Id);
                if (existing == null)
                {
                    _logger.LogWarning($"Notification flag with ID {flag.Id} not found");
                    return false;
                }

                existing.Flag = flag.Flag;
                existing.BackColor = flag.BackColor;
                existing.IsActive = flag.IsActive;
                existing.CanGlow = flag.CanGlow;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Notification flag with ID {flag.Id} updated");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating notification flag with ID {flag.Id}");
                return false;
            }
        }
    }
}
