using CoreLayer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        public ICollection<Consultant> Consultants { get; set; }
        public ICollection<Speciality> Specialities { get; set; }

        }
}
