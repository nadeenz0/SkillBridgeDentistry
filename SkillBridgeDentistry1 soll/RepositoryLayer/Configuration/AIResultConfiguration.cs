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
   public class AIResultConfiguration : IEntityTypeConfiguration<AIResult>
    {
        public void Configure(EntityTypeBuilder<AIResult> builder)
        {
            builder.HasKey(a => a.AIResultId);
            builder.Property(a => a.DiseaseName).HasMaxLength(500);
            builder.Property(a => a.Treatment).HasMaxLength(1000);
            builder.Property(a => a.Confidence).IsRequired();

            builder.HasOne(a => a.CaseRequest)
                   .WithOne(c => c.AIResults)
                   .HasForeignKey<AIResult>(a => a.CaseRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
