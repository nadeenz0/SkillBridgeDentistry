using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities.Identity
{
    public class Consultant
    {
        
        public string ConsultantId { get; set; } = Guid.NewGuid().ToString();
        
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }

        public string ResumeLink { get; set; }
        public int YearsOfExperience { get; set; }
        public string ShortBiography { get; set; }
        public decimal? Rating { get; set; } = 0;
        public string ProfilePicturePath { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<CaseConsultant> CaseConsultants { get; set; }
    }
}
