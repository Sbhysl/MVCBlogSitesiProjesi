using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Controllers
{
    public class TopicsController : Controller
    {
        private readonly YeniBlogDbContext _context;

        TopicRep topicRepository;
        UserRep userRep;
        ArticleRep articleRep;
        public TopicsController(YeniBlogDbContext context)
        {
            _context = context;
            topicRepository = new TopicRep(context);
            articleRep = new ArticleRep(context);
            userRep = new UserRep(context);
        }

        public IActionResult Topic()
        {
            List<Topic> topics = topicRepository.GetTopics();
            return View(topics);
        }

        // GET: Topics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Topics.ToListAsync());
        }

        // GET: Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicID == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // GET: Topics/Create
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Topic topic)
        {
            if (topicRepository.IsTopicRegistered(topic.TopicName)==false)
            {
                if (ModelState.IsValid)
                {
                    await _context.SaveChangesAsync();
                    topicRepository.AddTopic(topic);
                }
                return RedirectToAction("Topic","Topics");
            }
            else { return Content("This topic name is used.Registration is not possible."); }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            return View(topic);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicID,TopicName,Description,IsActive,CreatedDate,ModifiedDate")] Topic topic)
        {
            if (id != topic.TopicID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    topicRepository.UpdateTopic(topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(topic);
        }

        // GET: Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topics
                .FirstOrDefaultAsync(m => m.TopicID == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topics.FindAsync(id);
            _context.Topics.Remove(topic);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _context.Topics.Any(e => e.TopicID == id);
        }

        public IActionResult ArticlesByTopics(int id)
        {
            var articles=articleRep.GetArticlesByTopicId(id);
            return View(articles);
        }
    }
}
