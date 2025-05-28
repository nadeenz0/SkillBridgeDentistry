using CoreLayer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime SentAt { get; set; }

        public int? CaseRequestId { get; set; }
        public CaseRequest CaseRequest { get; set; }

        public int? CaseConsultantId { get; set; }
        public CaseConsultant CaseConsultant { get; set; }
    }
}
