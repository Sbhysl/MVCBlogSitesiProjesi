using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly YeniBlogDbContext _context;
        ArticleRep articleRep;
        UserRep userRep;
        public ArticlesController(YeniBlogDbContext context)
        {
            _context = context;
            articleRep = new ArticleRep(context);
            userRep = new UserRep(context);
        }

        public IActionResult Index()
        {
            return View(articleRep.GetActiveArticlesByID(userRep.GetUserByMail(Request.Cookies["EMail"]).UserID));

        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        public IActionResult Create()
        {
            TempData["UserMail"] = userRep.GetUserByMail(Request.Cookies["EMail"]).Mail;

            return View();
        }

        
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article,int[] checkedId)
        {
           
            if (checkedId.Length > 0)
            {

                string content = HttpContext.Request.Form["Content"];
                if (articleRep.IsArticleRegister(article.Tittle) == true)
                {
                    return Content("This title has been used before. You cannot use this title.");
                }

                else if (ModelState.IsValid)
                {
                    article.UserID = userRep.GetUserByMail(Request.Cookies["EMail"]).UserID;
                    article.Content = HtmlToPlainText(content);
                    articleRep.AddArticle(article);

                    await _context.SaveChangesAsync();

                    ArticleTopic articleTopic = new ArticleTopic();

                    foreach (var topicID in checkedId)
                    {
                        articleTopic.ArticleID = article.ArticleID;
                        articleTopic.TopicID = topicID;
                        articleRep.AddArticleTopic(articleTopic);
                     
                    }
                  
                    return RedirectToAction(nameof(Index));

                }
            }
            else {
                ViewBag.Message = "Please choose a topic name.";
            }
            return View(article);
        }
        private static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", article.UserID);
            return View(article);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
                 
        public async Task<IActionResult> Edit(int id, [Bind("ArticleID,Tittle,Content,ReadingTime,NumberOfClick,UserID,IsActive,CreatedDate,ModifiedDate")] Article article, int[] checkedId)
        {
            if (id != article.ArticleID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    articleRep.UpdateArticle(article);
                    await _context.SaveChangesAsync();
                   
                    ArticleTopic articleTopic = new ArticleTopic();

                    foreach (var topicID in checkedId)
                    {
                        articleTopic.ArticleID = article.ArticleID;
                        articleTopic.TopicID = topicID;
                        articleRep.AddArticleTopic(articleTopic);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleID))
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
            ViewData["UserID"] = new SelectList(_context.Users, "UserID", "UserID", article.UserID);
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ArticleID == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleID == id);
        }

        public IActionResult ArticleApproval()
        {
            return View();
        }
        public IActionResult Approve(int id)
        {
            articleRep.ActivateArticle(id);
            return RedirectToAction("ArticleApproval");
        }
        public IActionResult Disapprove(int id)
        {
            articleRep.DeActivateArticle(id);
            return RedirectToAction("ArticleApproval");

        }
        public IActionResult ReadingTime()
        {
            return View();
        }

        public IActionResult FilteredArticles(int topicId)
        {
            List<Article> articles = articleRep.GetArticlesByTopicId(topicId);
            return View(articles);
        }
    }
}
