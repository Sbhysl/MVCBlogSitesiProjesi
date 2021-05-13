using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models
{
    public class Topic:BaseEntity
    {
        public Topic()
        {
            ArticleTopics = new HashSet<ArticleTopic>();
        }
       
        public int TopicID { get; set; }

        [Required]
        public string TopicName { get; set; }

        public string Description { get; set; }

        public ICollection<UserTopic> UserTopics { get; set; }
        public ICollection<ArticleTopic> ArticleTopics { get; set; }
    }
}
