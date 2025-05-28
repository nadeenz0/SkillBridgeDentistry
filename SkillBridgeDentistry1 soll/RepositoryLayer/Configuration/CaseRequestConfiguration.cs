using CoreLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Configuration
{
   public class CaseRequestConfiguration : IEntityTypeConfiguration<CaseRequest>
    {
        public void Configure(EntityTypeBuilder<CaseRequest> builder)
        {
            builder.HasKey(c => c.CaseRequestId);

            
            builder.Property(c => c.Diagnoses);
            builder.Property(c => c.ImagePath);
            
            builder.Property(c => c.SpecialityId).IsRequired();

            builder.HasOne(c => c.freashGrad)
                   .WithMany(d => d.CaseRequests)
                   .HasForeignKey(c => c.freashGradId);

            builder.HasOne(c => c.Speciality)
                   .WithMany()
                   .HasForeignKey(c => c.SpecialityId);
        }
    }

}
