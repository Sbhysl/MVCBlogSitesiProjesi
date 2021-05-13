using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Mapping
{
    public class TopicMap : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.HasKey(a => a.TopicID);
            builder.Property(a => a.TopicID).ValueGeneratedOnAdd();
            builder.HasIndex(a => a.TopicName)
                   .IsUnique();

        }

    }
}
