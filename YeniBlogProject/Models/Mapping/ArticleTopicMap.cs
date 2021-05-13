using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Mapping
{
    public class ArticleTopicMap : IEntityTypeConfiguration<ArticleTopic>
    {
        public void Configure(EntityTypeBuilder<ArticleTopic> builder)
        {
            builder.HasKey(a => new { a.ArticleID, a.TopicID });
            builder.HasOne(a => a.Article)
                .WithMany(b => b.ArticleTopics)
                .HasForeignKey(a => a.ArticleID);
            builder.HasOne(a => a.Topic)
                .WithMany(b => b.ArticleTopics)
                .HasForeignKey(a => a.TopicID);
        }
    }
}
