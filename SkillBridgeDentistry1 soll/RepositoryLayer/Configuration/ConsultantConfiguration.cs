using CoreLayer.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Configuration
{
    public class ConsultantConfiguration : IEntityTypeConfiguration<Consultant>
    {
        public void Configure(EntityTypeBuilder<Consultant> builder)
        {
            builder.HasKey(c => c.ConsultantId);

            builder.Property(c => c.ResumeLink).HasMaxLength(500);
            builder.Property(c => c.ProfilePicturePath).HasMaxLength(500);
            builder.Property(c => c.YearsOfExperience).IsRequired();

            //builder.HasOne(c => c.User)
            //   .WithOne(u => u.Consultant)
            //   .HasForeignKey<Consultant>(c => c.ConsultantId)
            //  .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.User)
           .WithOne(u => u.Consultant)
           .HasForeignKey<Consultant>(c => c.UserId) // 👈 هذا هو التصحيح
           .OnDelete(DeleteBehavior.Restrict);


        }
    }
    
    }

