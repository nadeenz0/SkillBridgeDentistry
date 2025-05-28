using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities.Identity
{
   public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public RoleType RoleType { get; set; } // 0=NewDentist, 1=Consultant
        public ICollection<Notification> Notifications { get; set; }

        public Consultant Consultant { get; set; }
        public freashGrad freashGrad { get; set; }
    }
}
