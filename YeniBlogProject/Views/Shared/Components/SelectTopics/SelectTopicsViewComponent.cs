using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Views.Shared.Components.SelectTopics
{
    public class SelectTopicsViewComponent:ViewComponent
    {
        private readonly YeniBlogDbContext _context;
        TopicRep topicRep;
        public SelectTopicsViewComponent(YeniBlogDbContext context)
        {
            _context = context;
            topicRep = new TopicRep(context);

        }
        public IViewComponentResult Invoke()
        {
            List<Topic> topics = topicRep.GetTopics();
            return View(topics);
        }
    }
}
