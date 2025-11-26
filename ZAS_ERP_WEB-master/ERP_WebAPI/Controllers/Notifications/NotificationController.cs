using ERP_BL.Entities.Notifications;
using ERP_REPO.Repo.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ERP_API.Controllers.Notifications
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepo _notificationRepo;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationRepo notificationRepo, ILogger<NotificationController> logger)
        {
            _notificationRepo = notificationRepo;
            _logger = logger;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        // GET: /Notification
        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            try
            {
                var notifications = await _notificationRepo.GetAllNotificationsAsync();
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                var notification = await _notificationRepo.GetNotificationByIdAsync(id);
                if (notification == null)
                    return NotFound(new { message = $"Notification with ID {id} not found." });

                return Ok(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving notification {id}");
                return StatusCode(500, new { message = "An error occurred while retrieving the notification." });
            }
        }

        // GET: /Notification/user
        [HttpGet("user")]
        public async Task<IActionResult> GetUserNotifications([FromQuery] int take = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetUserNotificationsAsync(userId, take);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/user/more
        [HttpGet("user/more")]
        public async Task<IActionResult> GetMoreUserNotifications([FromQuery] int skip, [FromQuery] int take = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetMoreUserNotificationsAsync(userId, skip, take);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving more user notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/user/bydate
        [HttpGet("user/bydate")]
        public async Task<IActionResult> GetUserNotificationsByDate([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetUserNotificationsByDateAsync(userId, dateFrom, dateTo);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user notifications by date");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/sent
        [HttpGet("sent")]
        public async Task<IActionResult> GetSentNotifications([FromQuery] int take = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetSentNotificationsAsync(userId, take);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sent notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/sent/more
        [HttpGet("sent/more")]
        public async Task<IActionResult> GetMoreSentNotifications([FromQuery] int skip, [FromQuery] int take = 20)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetMoreSentNotificationsAsync(userId, skip, take);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving more sent notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/sent/bydate
        [HttpGet("sent/bydate")]
        public async Task<IActionResult> GetSentNotificationsByDate([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetSentNotificationsByDateAsync(userId, dateFrom, dateTo);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sent notifications by date");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/unread
        [HttpGet("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetAllUnreadNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/unread/count
        [HttpGet("unread/count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var count = await _notificationRepo.GetUnreadCountAsync(userId);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unread count");
                return StatusCode(500, new { message = "An error occurred while retrieving unread count." });
            }
        }

        // GET: /Notification/pending
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetAllPendingNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/pending/count
        [HttpGet("pending/count")]
        public async Task<IActionResult> GetPendingCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var count = await _notificationRepo.GetPendingCountAsync(userId);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pending count");
                return StatusCode(500, new { message = "An error occurred while retrieving pending count." });
            }
        }

        // GET: /Notification/urgent
        [HttpGet("urgent")]
        public async Task<IActionResult> GetUrgentNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var notifications = await _notificationRepo.GetAllUrgentNotificationsAsync(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving urgent notifications");
                return StatusCode(500, new { message = "An error occurred while retrieving notifications." });
            }
        }

        // GET: /Notification/urgent/count
        [HttpGet("urgent/count")]
        public async Task<IActionResult> GetUrgentCount()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == 0)
                    return Unauthorized(new { message = "User not authenticated." });

                var count = await _notificationRepo.GetUrgentCountAsync(userId);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving urgent count");
                return StatusCode(500, new { message = "An error occurred while retrieving urgent count." });
            }
        }

        // POST: /Notification
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] Notification notification)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _notificationRepo.CreateNotificationAsync(notification);
                if (created == null)
                    return StatusCode(500, new { message = "Failed to create notification." });

                return CreatedAtAction(nameof(GetNotificationById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification");
                return StatusCode(500, new { message = "An error occurred while creating the notification." });
            }
        }

        // PUT: /Notification/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] Notification notification)
        {
            try
            {
                if (id != notification.Id)
                    return BadRequest(new { message = "ID mismatch." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _notificationRepo.UpdateNotificationAsync(notification);
                if (!success)
                    return NotFound(new { message = $"Notification with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating notification {id}");
                return StatusCode(500, new { message = "An error occurred while updating the notification." });
            }
        }

        // DELETE: /Notification/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var success = await _notificationRepo.DeleteNotificationAsync(id);
                if (!success)
                    return NotFound(new { message = $"Notification with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting notification {id}");
                return StatusCode(500, new { message = "An error occurred while deleting the notification." });
            }
        }

        // PUT: /Notification/{id}/mark-read
        [HttpPut("{id}/mark-read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            try
            {
                var success = await _notificationRepo.MarkAsReadAsync(id);
                if (!success)
                    return NotFound(new { message = $"Notification with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {id} as read");
                return StatusCode(500, new { message = "An error occurred while marking the notification as read." });
            }
        }

        // PUT: /Notification/{id}/mark-unread
        [HttpPut("{id}/mark-unread")]
        public async Task<IActionResult> MarkAsUnread(int id)
        {
            try
            {
                var success = await _notificationRepo.MarkAsUnreadAsync(id);
                if (!success)
                    return NotFound(new { message = $"Notification with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {id} as unread");
                return StatusCode(500, new { message = "An error occurred while marking the notification as unread." });
            }
        }

        // PUT: /Notification/mark-read
        [HttpPut("mark-read")]
        public async Task<IActionResult> MarkMultipleAsRead([FromBody] IEnumerable<int> notificationIds)
        {
            try
            {
                var success = await _notificationRepo.MarkMultipleAsReadAsync(notificationIds);
                if (!success)
                    return StatusCode(500, new { message = "Failed to mark notifications as read." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking multiple notifications as read");
                return StatusCode(500, new { message = "An error occurred while marking notifications as read." });
            }
        }

        // PUT: /Notification/mark-unread
        [HttpPut("mark-unread")]
        public async Task<IActionResult> MarkMultipleAsUnread([FromBody] IEnumerable<int> notificationIds)
        {
            try
            {
                var success = await _notificationRepo.MarkMultipleAsUnreadAsync(notificationIds);
                if (!success)
                    return StatusCode(500, new { message = "Failed to mark notifications as unread." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking multiple notifications as unread");
                return StatusCode(500, new { message = "An error occurred while marking notifications as unread." });
            }
        }

        // PUT: /Notification/mark-pending
        [HttpPut("mark-pending")]
        public async Task<IActionResult> MarkAsPending([FromBody] IEnumerable<int> notificationIds)
        {
            try
            {
                var success = await _notificationRepo.MarkAsPendingAsync(notificationIds);
                if (!success)
                    return StatusCode(500, new { message = "Failed to mark notifications as pending." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking notifications as pending");
                return StatusCode(500, new { message = "An error occurred while marking notifications as pending." });
            }
        }

        // PUT: /Notification/unmark-pending
        [HttpPut("unmark-pending")]
        public async Task<IActionResult> UnmarkAsPending([FromBody] IEnumerable<int> notificationIds)
        {
            try
            {
                var success = await _notificationRepo.UnmarkAsPendingAsync(notificationIds);
                if (!success)
                    return StatusCode(500, new { message = "Failed to unmark notifications as pending." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unmarking notifications as pending");
                return StatusCode(500, new { message = "An error occurred while unmarking notifications as pending." });
            }
        }

        // Notification Flags Endpoints

        // GET: /Notification/flags
        [HttpGet("flags")]
        public async Task<IActionResult> GetAllNotificationFlags()
        {
            try
            {
                var flags = await _notificationRepo.GetAllNotificationFlagsAsync();
                return Ok(flags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving notification flags");
                return StatusCode(500, new { message = "An error occurred while retrieving notification flags." });
            }
        }

        // GET: /Notification/flags/active
        [HttpGet("flags/active")]
        public async Task<IActionResult> GetActiveNotificationFlags()
        {
            try
            {
                var flags = await _notificationRepo.GetActiveNotificationFlagsAsync();
                return Ok(flags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active notification flags");
                return StatusCode(500, new { message = "An error occurred while retrieving notification flags." });
            }
        }

        // GET: /Notification/flags/{id}
        [HttpGet("flags/{id}")]
        public async Task<IActionResult> GetNotificationFlagById(int id)
        {
            try
            {
                var flag = await _notificationRepo.GetNotificationFlagByIdAsync(id);
                if (flag == null)
                    return NotFound(new { message = $"Notification flag with ID {id} not found." });

                return Ok(flag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving notification flag {id}");
                return StatusCode(500, new { message = "An error occurred while retrieving the notification flag." });
            }
        }

        // POST: /Notification/flags
        [HttpPost("flags")]
        public async Task<IActionResult> CreateNotificationFlag([FromBody] NotificationFlag flag)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var created = await _notificationRepo.CreateNotificationFlagAsync(flag);
                if (created == null)
                    return StatusCode(500, new { message = "Failed to create notification flag." });

                return CreatedAtAction(nameof(GetNotificationFlagById), new { id = created.Id }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification flag");
                return StatusCode(500, new { message = "An error occurred while creating the notification flag." });
            }
        }

        // PUT: /Notification/flags/{id}
        [HttpPut("flags/{id}")]
        public async Task<IActionResult> UpdateNotificationFlag(int id, [FromBody] NotificationFlag flag)
        {
            try
            {
                if (id != flag.Id)
                    return BadRequest(new { message = "ID mismatch." });

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _notificationRepo.UpdateNotificationFlagAsync(flag);
                if (!success)
                    return NotFound(new { message = $"Notification flag with ID {id} not found." });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating notification flag {id}");
                return StatusCode(500, new { message = "An error occurred while updating the notification flag." });
            }
        }
    }
}
