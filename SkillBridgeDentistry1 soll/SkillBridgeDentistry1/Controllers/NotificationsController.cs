using CoreLayer.Entities;
using CoreLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using ServiceLayer;
using SkillBridgeDentistry1.Dtos;
using System;
using System.Security.Claims;

namespace SkillBridgeDentistry1.Controllers
{

    

    [Authorize]
   
    public class NotificationsController : ApiBaseController
    {
        private readonly SkillBridgeDbContext _context;

        public NotificationsController(SkillBridgeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.SentAt)
                .Select(n => new NotificationDto
                {
                    NotificationId = n.NotificationId,
                    Title = n.Title,
                    Body = n.Body,
                    IsRead = n.IsRead,
                    ReadAt = n.ReadAt,
                    SentAt = n.SentAt,
                    CaseRequestId = n.CaseRequestId,
                    CaseConsultantId = n.CaseConsultantId
                })
                .ToListAsync();

            return Ok(notifications);
        }


        // ✅ Get unread count
        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _context.Notifications.CountAsync(n => n.UserId == userId && !n.IsRead);
            return Ok(new { unreadCount = count });
        }

        // ✅ Mark as read
        [HttpPost("mark-as-read/{id}")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null || notification.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Marked as read" });
        }

        // ✅ Mark all as read
        [HttpPost("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "All marked as read" });
        }

        // ✅ Delete notification
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null || notification.UserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
                return NotFound();

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Notification deleted" });
        }
    }


}
        
    

