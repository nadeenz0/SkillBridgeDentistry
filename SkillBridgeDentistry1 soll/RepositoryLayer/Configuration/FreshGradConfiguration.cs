using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreLayer.Entities.Identity;

namespace RepositoryLayer.Configuration
{
    public class FreshGradConfiguration : IEntityTypeConfiguration<freashGrad>
    {
        public void Configure(EntityTypeBuilder<freashGrad> builder)
        {
            builder.HasKey(d => d.freashGradId);

            builder.Property(d => d.University).HasMaxLength(255);
            builder.Property(d => d.Department).HasMaxLength(255);
            builder.Property(d => d.GraduationDate).IsRequired();

            builder.HasOne(n => n.User)
               .WithOne(u => u.freashGrad)
               .HasForeignKey<freashGrad>(n => n.freashGradId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
 
