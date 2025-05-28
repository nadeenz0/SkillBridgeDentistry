using CoreLayer.Entities;
using CoreLayer.Services;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class NotificationService : INotificationService
    {
        private readonly SkillBridgeDbContext _context;

        public NotificationService(SkillBridgeDbContext context)
        {
            _context = context;
        }

        public async Task CreateNotificationAsync(string userId, string title, string body, int? caseRequestId = null, int? caseConsultantId = null)
        {
            var notification = new Notification
            {
                Title = title,
                Body = body,
                UserId = userId,
                SentAt = DateTime.UtcNow,
                CaseRequestId = caseRequestId,
                CaseConsultantId = caseConsultantId
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

       
    }

}


