using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Views.Shared.Components.GetRandomArticles
{
    public class GetRandomArticlesViewComponent:ViewComponent
    {
        private readonly YeniBlogDbContext _context;
        ArticleRep articleRep;
        public GetRandomArticlesViewComponent(YeniBlogDbContext context)
        {
            _context = context;
            articleRep = new ArticleRep(context);
        }

        public IViewComponentResult Invoke()
        {

            List<Article> articles = articleRep.GetRandomThreeArticles();
            return View(articles);
        }
    }
}
