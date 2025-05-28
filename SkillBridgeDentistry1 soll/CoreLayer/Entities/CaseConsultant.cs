using CoreLayer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class CaseConsultant
    {
        public int CaseConsultantId { get; set; }

        public int CaseRequestId { get; set; }
        public CaseRequest CaseRequest { get; set; }

        public string ConsultantId { get; set; }
        public Consultant Consultant { get; set; }

        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public DateTime? ReplyDate { get; set; }

        public int? Rating { get; set; } // تقييم من 1 لـ 5

        public CaseConsultantStatus Status { get; set; } = CaseConsultantStatus.Pending;
    }
}
