using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models
{
    public class UserTopic:BaseEntity
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int TopicID { get; set; }
        public Topic Topic { get; set; }
    }
}
