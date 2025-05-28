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
    public class NotificationScheduler
    {
        private readonly INotificationService _notificationService;
        private readonly SkillBridgeDbContext _context;

        public NotificationScheduler(INotificationService notificationService, SkillBridgeDbContext context)
        {
            _notificationService = notificationService;
            _context = context;
        }

        public async Task SendRateNotifications(int caseRequestId)
        {
            var caseConsultants = await _context.CaseConsultants
                .Include(cc => cc.Consultant)
                .ThenInclude(c => c.User)
                .Where(cc => cc.CaseRequestId == caseRequestId && cc.Diagnosis != null&&cc.Treatment!=null)
                .ToListAsync();

            var caseRequest = await _context.CaseRequests.FindAsync(caseRequestId);
            if (caseRequest == null) return;

            foreach (var consultant in caseConsultants)
            {
                
                await _notificationService.CreateNotificationAsync(
                caseRequest.freashGradId,
                "تقييم تجربتك مع الاستشاري",
                $"يرجى تقييم تجربتك مع الاستشاري {consultant.Consultant.User.FullName} الذي ساعدك في حالتك",
                caseRequestId,
                consultant.CaseConsultantId
                );

            }
        }
    }

}
