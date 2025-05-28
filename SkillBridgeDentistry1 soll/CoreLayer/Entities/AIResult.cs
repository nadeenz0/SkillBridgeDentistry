using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class AIResult
    {
        public int AIResultId { get; set; }
        public string DiseaseName { get; set; }
        public double Confidence { get; set; }
        public string Treatment { get; set; }
        public int? SpecialityId { get; set; }
        public Speciality Speciality { get; set; }

        public int CaseRequestId { get; set; }
        public CaseRequest CaseRequest { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
