using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models
{
    public class User:BaseEntity
    {
        public User()
        {
            Articles = new HashSet<Article>();
        }

        
        public int UserID { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Please enter your mail adresses")]
        public string Mail { get; set; }

        [Display(Name = "Descripe Yourself")]
        [DataType(DataType.MultilineText)]
        [MaxLength(250)]
        public string UserDescription { get; set; }

        [DataType(DataType.ImageUrl)]
        public string ProfilePicture { get; set; }
        public UserRole UserRole { get; set; }
        public ICollection<Article> Articles { get; set; }
        public ICollection<UserTopic> UserTopics { get; set; }
    }
}
