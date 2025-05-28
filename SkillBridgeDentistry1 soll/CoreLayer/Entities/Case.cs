using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class Case
    {
        public int CaseId { get; set; }
        public string Disease { get; set; }
        public string ImageUrl { get; set; } // For storing the image URL of the case
        public string Treatment { get; set; }
        public int SpecialityId { get; set; }
        public Speciality Speciality { get; set; }

        public ICollection<CaseRequest> CaseRequests { get; set; }
    }
}
