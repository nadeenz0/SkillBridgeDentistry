using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities.Identity
{
   public class freashGrad
    {
        
        public string freashGradId { get; set; }
        [ForeignKey(nameof(freashGradId))]
        public ApplicationUser User { get; set; }

        public string University { get; set; }
        public string Department { get; set; }
        public int GraduationDate { get; set; }

        public ICollection<CaseRequest> CaseRequests { get; set; }

        public static implicit operator string(freashGrad v)
        {
            throw new NotImplementedException();
        }
    }
}
