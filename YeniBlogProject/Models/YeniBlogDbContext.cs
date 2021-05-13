using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YeniBlogProject.Models.Mapping;

namespace YeniBlogProject.Models
{
    public class YeniBlogDbContext:DbContext
    {
        //public YeniBlogDbContext()
        //{
        //}

        public YeniBlogDbContext(DbContextOptions<YeniBlogDbContext>options):base(options)
        {

        }
        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ArticleTopic> ArticleTopics { get; set; }
        public DbSet<UserTopic> UserTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleTopicMap());
            modelBuilder.ApplyConfiguration(new UserTopicMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new TopicMap());
            modelBuilder.ApplyConfiguration(new ArticleMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}
