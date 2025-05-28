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
  public  class NotificationConfiguration
     : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.NotificationId);
            builder.Property(n => n.Title).IsRequired();
            builder.Property(n => n.Body).IsRequired();
            builder.Property(n => n.User).HasMaxLength(50);
            builder.Property(n => n.SentAt).IsRequired();

            builder.HasOne(n => n.User)
                   .WithMany()
                   .HasForeignKey(n => n.UserId);
        }
    }

}
