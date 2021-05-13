using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Views.Shared.Components.GetFilteredArticles
{
    public class GetFilteredArticlesViewComponent:ViewComponent
    {
        private readonly YeniBlogDbContext _context;
        ArticleRep articleRep;
        public GetFilteredArticlesViewComponent(YeniBlogDbContext context)
        {
            _context = context;
            articleRep = new ArticleRep(context);
        }

        public IViewComponentResult Invoke(int id)
        {
            List<Article> articles = articleRep.GetArticlesByTopicId(id);
            return View(articles);
        }
    }
}
