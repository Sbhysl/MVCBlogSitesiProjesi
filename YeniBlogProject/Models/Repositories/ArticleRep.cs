using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YeniBlogProject.Models.Repositories
{
    public class ArticleRep
    {
        YeniBlogDbContext ctx;
        public ArticleRep(YeniBlogDbContext context)
        {
            ctx = context;
        }
        public bool AddArticle(Article newArticle)
        {
            newArticle.IsActive = false;
            newArticle.CreatedDate = DateTime.Now;
            newArticle.ReadingTime = CalculateReadingTime(newArticle.Content);
            ctx.Articles.Add(newArticle);
            return ctx.SaveChanges() > 0;
        }

        public List<Article> GetArticlesByTopicId(int id)
        {
            List<Article> articles = ctx.Articles
                        .Include(i => i.ArticleTopics)
                        .ThenInclude(i => i.Topic)
                        .Where(i => i.ArticleTopics
                            .Any(a => a.Topic.TopicID == id)).ToList();

            return articles;

        }
      
        public void ActivateArticle(int id)
        {
            var article=GetArticleById(id);
            article.IsActive = true;
            ctx.SaveChanges();
        }
        public void DeActivateArticle(int id)
        {
            var article = GetArticleById(id);
            article.IsActive = false;
            ctx.SaveChanges();
        }

        public bool AddArticleTopic(ArticleTopic articleTopic)
        {
            articleTopic.IsActive = true;
            ctx.ArticleTopics.Add(articleTopic);
            return ctx.SaveChanges()>0;
        }
        public Article GetArticleByID(int id)
        {
            return ctx.Articles.Where(a => a.ArticleID == id).FirstOrDefault();
        }


        public bool IsArticleRegister(string title)
        {
            if (ctx.Articles.Where(a=>a.Tittle==title).FirstOrDefault()!=null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Article> GetArticles()
        {
            return ctx.Articles.ToList();
        }
        public List<Article> GetActiveArticles()
        {
            return ctx.Articles.Where(a => a.IsActive).ToList();
        }
        public List<Article> GetActiveArticlesByID(int id)
        {
            return ctx.Articles.Where(a=>a.IsActive && a.UserID==id).ToList();
        }
        public List<Article> GetArticlesByUserID(int id)//yazar detay sayfası için yazaraın makalelerini yeniden eskiye sıralama
        {
            return ctx.Articles.Where(a => a.UserID == id && a.IsActive).OrderBy(a => a.CreatedDate).ToList();
        }

        void DeleteArticle(int id)
        {
            Article article = ctx.Articles.FirstOrDefault(a => a.ArticleID == id && a.IsActive);
            article.IsActive = false;
            ctx.SaveChanges();
        }
        public bool UpdateArticle(Article article)
        {
            Article selected = ctx.Articles.Where(a=>a.ArticleID==article.ArticleID).FirstOrDefault();
            selected.Tittle = article.Tittle;
            selected.Content = article.Content;
            selected.ReadingTime = CalculateReadingTime(article.Content);
            selected.ModifiedDate = DateTime.Now;
            return ctx.SaveChanges() > 0;
        }

        //public List<Article> GetArticleByTopicName(string name)
        //{
           
        //    List<ArticleTopic> deneme = ctx.ArticleTopics.Include(a => a.Topic).Include(a => a.Article).Where(a => a.Topic.TopicName == name && a.Topic.IsActive && a.Article.IsActive).ToList();
        //    List<Article> articles = new List<Article>();
        //    foreach (var item in deneme)
        //    {
        //        Article article = new Article();
        //        article.ArticleID = item.ArticleID;
        //        article.Tittle = item.Article.Tittle;
        //        article.Content = item.Article.Content;
        //        article.ReadingTime = item.Article.ReadingTime;

        //    }
        //    return articles;
        //}

        private Article GetArticleById(int articleId)
        {
            return ctx.Articles.Where(a => a.ArticleID == articleId).FirstOrDefault();
        }
        public bool ActivateArticle(Article article)
        {
            Article activatedArticle = GetArticleById(article.ArticleID);
            activatedArticle.IsActive = true;
            return ctx.SaveChanges() > 0;
        }

        public List<Article> GetArticleByUserMail(string mail)
        {
            return ctx.Articles.Where(a => a.User.Mail == mail && a.IsActive).ToList();
        }

        public List<Article> GetTrendArticles()
        {
            return ctx.Articles.Where(a => a.IsActive).OrderByDescending(a => a.NumberOfClick).ToList();
        }
        public decimal CalculateReadingTime(string content)//Dünyada dakikada ortalama 300 kelime okunuyor. 
        {
            decimal readingTime;
            decimal numberOfContentChar = content.Length;
            if (numberOfContentChar>300)
            {
               readingTime = numberOfContentChar / 300;
                return readingTime;
            }
            else
            {

               return 1;
            }
        }
       
        public List<Article> GetNotActiveArticles()
        {
            return ctx.Articles.Where(a => a.IsActive == false).ToList();
        }
        public List<Article> GetRandomThreeArticles() //kayıtlı fazla makalem bulunmadığından dolayı 3 yaptım görebilmek adına
        {
            return ctx.Articles.Where(a=>a.IsActive).OrderBy(a => Guid.NewGuid()).Take(3).ToList();
        }
        public List<Article> GetRandomArticle() //kayıtlı fazla makalem bulunmadığından dolayı 3 yaptım görebilmek adına
        {
            return ctx.Articles.Where(a => a.IsActive).OrderBy(a => Guid.NewGuid()).Take(1).ToList();
        }

        public List<Article> GetArticlesSortedByReadingTime()
        {
            return ctx.Articles.Where(a=>a.IsActive).OrderBy(a => a.ReadingTime).ToList();
        }


    }
}
