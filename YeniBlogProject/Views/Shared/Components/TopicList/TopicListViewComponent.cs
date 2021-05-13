using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Views.Shared.Components.TopicList
{
    public class TopicListViewComponent:ViewComponent
    {
        private readonly YeniBlogDbContext _context;
        TopicRep topicRep;
        public TopicListViewComponent(YeniBlogDbContext context)
        {
            _context = context;
            topicRep = new TopicRep(context);

        }
     
        public IViewComponentResult Invoke()
        {
           List<Topic> topics= topicRep.GetTopics();
            return View(topics);
        }
    }
}
