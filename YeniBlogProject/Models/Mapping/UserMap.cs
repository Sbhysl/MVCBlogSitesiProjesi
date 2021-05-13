using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {

        public void Configure(EntityTypeBuilder<User> builder)
        {
          
            builder.HasKey(a => a.UserID);
            builder.Property(a => a.UserID).ValueGeneratedOnAdd();
            builder.HasMany(a => a.Articles)
                   .WithOne(a => a.User)
                   .HasForeignKey(a => a.UserID);
            builder.HasIndex(a => a.Mail).IsUnique();
            builder.Property(a => a.FirstName).IsRequired();
            builder.Property(a => a.LastName).IsRequired();
            builder.Property(a => a.Mail).IsRequired();

        }

    }
}
