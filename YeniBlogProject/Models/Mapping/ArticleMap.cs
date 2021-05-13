using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Mapping
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {

        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.ArticleID);
            builder.Property(a => a.ArticleID).ValueGeneratedOnAdd();
            builder.HasIndex(a => a.Tittle)
                   .IsUnique();
            builder.Property(a => a.Tittle)
                   .HasMaxLength(50)
                   .IsRequired();
            builder.Property(a => a.Content)
                   .IsRequired();
        }

    }
}
