using CoreLayer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class CaseRequest
    {
        public int CaseRequestId { get; set; }
        public string? Diagnoses { get; set; }
        public string? ImagePath { get; set; }

        public int? SpecialityId { get; set; }
        public Speciality Speciality { get; set; }

        public string freashGradId { get; set; }
        public freashGrad freashGrad { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<CaseConsultant> CaseConsultants { get; set; }
        public AIResult AIResults { get; set; }
    }
}
