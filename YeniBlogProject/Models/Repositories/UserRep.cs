using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Repositories
{
    public class UserRep
    {
        YeniBlogDbContext ctx;
        public UserRep(YeniBlogDbContext context)
        {
            ctx = context;
        }

        public bool IsAdmin(string mail)
        {
            string adminMail = "sabihauysal93@gmail.com";
            if (mail==adminMail)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsUserRegistered(string mail)
        {
            if (ctx.Users.Where(a=>a.Mail==mail).FirstOrDefault()!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UserIsActive(string mail)
        {

            if (ctx.Users.Where(a => a.Mail == mail && a.IsActive).FirstOrDefault()!=null)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
 
        public bool AddUser(User newUser)
        {
            newUser.UserRole = UserRole.Member;
            newUser.IsActive = false;
            newUser.CreatedDate = DateTime.Now;
            ctx.Users.Add(newUser);
            return ctx.SaveChanges() > 0;
        }

        public void ActivateUser(User user)//maille aktive etmek için bunu yazdım ama inş doğrudur.
        {
            User activeUser = GetUserByMail(user.Mail);
            user.IsActive = true;
            ctx.SaveChanges();
        }
        public User GetUserByID(int id)
        {
            return ctx.Users.Where(a => a.UserID == id).FirstOrDefault();
        }
        public User GetUserByMail(string email)
        {
            User user = ctx.Users.Where(a => a.Mail == email && a.IsActive).SingleOrDefault();
            return user;
        }

        public User GetWriterInformationByID(int id)
        {
            return ctx.Users.Where(a => a.UserID == id && a.IsActive).SingleOrDefault();
        }

        public bool UpdateUserInformations(User user)
        {
            User selected = ctx.Users.Where(a => a.UserID == user.UserID).FirstOrDefault();
            selected.FirstName = user.FirstName;
            selected.LastName = user.LastName;
            selected.Mail = user.Mail;
            selected.UserDescription = user.UserDescription;
            selected.ProfilePicture = user.ProfilePicture;
            selected.BirthDate = user.BirthDate;
            selected.IsActive = true;
            selected.ModifiedDate = DateTime.Now;
            return ctx.SaveChanges() > 0;
        }

        public bool DeleteAccount(string mail)
        {
            User user = ctx.Users.Where(a => a.Mail == mail).FirstOrDefault();
            user.IsActive = false;
            return ctx.SaveChanges() > 0;
        }

        //aşağıdaki metodu asla yazamadım
        public List<Article> GetArticlesByUserSelectedTopics()
        {
            List<Article> articles = new List<Article>();
            List<ArticleTopic> userArticles = ctx.ArticleTopics.Include(a => a.Article).Include(a => a.Topic).Include(a => a.Article.Tittle).ToList();
            Article article = new Article();

            foreach (var item in userArticles.GroupBy(a => a.Article))
            {

            }

            return articles;
        }
       
    }
}
