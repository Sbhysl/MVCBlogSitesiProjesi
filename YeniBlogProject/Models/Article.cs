using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models
{
    public class Article:BaseEntity
    {
        public Article()
        {
            ArticleTopics = new HashSet<ArticleTopic>();
        }

        public int ArticleID { get; set; }

        [Required(ErrorMessage = "This area is neccessery")]
        public string Tittle { get; set; }

        [MinLength(100,ErrorMessage = "You must enter at least 100 characters.")]
        public string Content { get; set; }

        public decimal? ReadingTime { get; set; }
        public int? NumberOfClick { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
        public ICollection<ArticleTopic> ArticleTopics { get; set; }
    }
}
