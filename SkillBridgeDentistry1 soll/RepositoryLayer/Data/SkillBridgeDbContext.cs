using CoreLayer.Entities;
using CoreLayer.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RepositoryLayer.Data
{
    public class SkillBridgeDbContext : IdentityDbContext<ApplicationUser>
    {
        public SkillBridgeDbContext(DbContextOptions<SkillBridgeDbContext> options)
        : base(options)
        {
        }

        // DbSets
        public DbSet<freashGrad> freashGrad { get; set; }
        public DbSet<Consultant> Consultants { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<CaseRequest> CaseRequests { get; set; }
        public DbSet<CaseConsultant> CaseConsultants { get; set; }
        public DbSet<AIResult> AIResults { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Case> Case { get; set; }
        public DbSet<Department>Department { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new ConsultantConfiguration());
            modelBuilder.ApplyConfiguration(new FreshGradConfiguration());
            modelBuilder.ApplyConfiguration(new SpecialityConfiguration());
            modelBuilder.ApplyConfiguration(new CaseRequestConfiguration());
            modelBuilder.ApplyConfiguration(new CaseConsultantConfiguration());

            modelBuilder.Entity<Department>().HasData(
            new Department { DepartmentId = 1, Name = "Periodontics" },
            new Department { DepartmentId = 2, Name = "Operative Dentistry" },
            new Department { DepartmentId = 3, Name = "Pathology Dentistry" }
            
            );

            modelBuilder.Entity<Speciality>().HasData(
                new Speciality { SpecialityId = 7, Name = "Gingivitis", DepartmentId = 1 },
                new Speciality { SpecialityId = 1, Name = "Mouth Ulcer", DepartmentId = 1 },
                new Speciality { SpecialityId = 2, Name = "Dental Caries", DepartmentId = 2 },
                new Speciality { SpecialityId = 4, Name = "Tooth Discoloration", DepartmentId = 2 },
                new Speciality { SpecialityId = 3, Name = "Hypodontia", DepartmentId = 2 },
                new Speciality { SpecialityId = 5, Name = "Dental Malignant tumors", DepartmentId = 3 },
                new Speciality { SpecialityId = 6, Name = "Dental benign tumors", DepartmentId = 3 }

 );
            modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId);

           
            
        }
    }
    }
