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
   public class CaseConsultantConfiguration
  : IEntityTypeConfiguration<CaseConsultant>
    {
        public void Configure(EntityTypeBuilder<CaseConsultant> builder)
        {
            builder.HasKey(cc => cc.CaseConsultantId);

            builder.Property(cc => cc.Diagnosis).HasMaxLength(1000);
            builder.Property(cc => cc.Treatment).HasMaxLength(1000);
            builder.Property(cc => cc.Status).IsRequired();
            builder.Property(cc => cc.Rating).HasDefaultValue(0);

            builder.HasOne(cc => cc.CaseRequest)
                   .WithMany(cr => cr.CaseConsultants)
                   .HasForeignKey(cc => cc.CaseRequestId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Consultant)
                   .WithMany(c => c.CaseConsultants)
                   .HasForeignKey(cc => cc.ConsultantId)
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
