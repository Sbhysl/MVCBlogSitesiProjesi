using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Mapping
{
    public class UserTopicMap : IEntityTypeConfiguration<UserTopic>
    {
        public void Configure(EntityTypeBuilder<UserTopic> builder)
        {
           
            builder.HasKey(a => new { a.UserID, a.TopicID });
            builder.HasOne(a => a.User).WithMany(b => b.UserTopics).HasForeignKey(a => a.UserID);
            builder.HasOne(a => a.Topic).WithMany(b => b.UserTopics).HasForeignKey(a => a.TopicID);
        }
    }
}
