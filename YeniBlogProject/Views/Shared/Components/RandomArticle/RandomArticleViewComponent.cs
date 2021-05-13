using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Views.Shared.Components.RandomArticle
{
    public class RandomArticleViewComponent:ViewComponent
    {
        private readonly YeniBlogDbContext _context;
        ArticleRep articleRep;
        public RandomArticleViewComponent(YeniBlogDbContext context)
        {
            _context = context;
            articleRep = new ArticleRep(context);
        }

        public IViewComponentResult Invoke()
        {

            List<Article> articles = articleRep.GetRandomArticle();
            return View(articles);
        }
    }
}
