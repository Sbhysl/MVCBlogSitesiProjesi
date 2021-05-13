using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models
{
    public class ArticleTopic:BaseEntity
    {
        public int ArticleID { get; set; }
        public Article Article { get; set; }
        
        public int TopicID { get; set; }
        public Topic Topic { get; set; }
    }
}
