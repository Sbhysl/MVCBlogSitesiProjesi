using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YeniBlogProject.Models;
using YeniBlogProject.Models.Repositories;

namespace YeniBlogProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly YeniBlogDbContext _context;
        UserRep userRep;

        public UsersController(YeniBlogDbContext context)
        {
            _context = context;
            userRep = new UserRep(context);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (userRep.IsUserRegistered(user.Mail) == false )
            {
                if (ModelState.IsValid)
                {
                    await _context.SaveChangesAsync();
                    userRep.AddUser(user);
                    SendActivationMail(user.UserID);
                    return RedirectToAction("Activation");
                }
                return View(user);
            }
            else {
                ViewBag.Message = "This mail is used. Please try another mail.";
                return View(user);
              
            }
           
        }
       
        public ActionResult Activation()
        {
            return View();
        }
        private void SendActivationMail(int id)
        {

            User user = new User();
            user = userRep.GetUserByID(id);
            string controller = user.UserID.ToString();
            string url = string.Format("{0}://{1}", HttpContext.Request.Scheme, HttpContext.Request.Host) + "/Users/Verify?Id=" + controller;
            string message = string.Format("Click on the link to activate your membership.");
            message += url;
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("DenemeBaBoost@gmail.com");
            mail.To.Add(user.Mail);
            mail.Subject = "";
            mail.Body = message;
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("DenemeBaBoost@gmail.com", "DenemeBaBoost123");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }
        public ActionResult Verify(string id)//doğrulama
        {
            User user = userRep.GetUserByID(int.Parse(id));
            userRep.UpdateUserInformations(user);
            return RedirectToAction("Success");
        }
        public ActionResult Success()
        {
            return View();
        }


        public IActionResult Login()
        {

            return View();
        }
        public IActionResult LoginPage()
        {
            return View();
        }

        public IActionResult UsLog()//unsuccessedlogin
        {
            return View();
        }
        public IActionResult Logout()
        {
            DeleteCookie();
            return RedirectToAction("index", "home");
        }
        public IActionResult LoginControl(string login)
        {
            if (userRep.IsAdmin(login)==true)
            {
                return RedirectToAction("AdminPage");
            }
            else if (userRep.UserIsActive(login))
            {
                AddCookie(login);
                return RedirectToAction("LoginPage", "Users");

            }
            return RedirectToAction("USLog");
        }
        private void AddCookie(string login)
        {
            Response.Cookies.Append("Email", login);
        }
        private void DeleteCookie()
        {
            Response.Cookies.Delete("Email");
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,FirstName,LastName,UserName,BirthDate,Mail,UserDescription,ProfilePicture,UserRole,IsActive,CreatedDate,ModifiedDate")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string mail)
        {
            if (mail== userRep.GetUserByMail(Request.Cookies["EMail"]).Mail)
            {
                var user = userRep.GetUserByMail(mail);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                DeleteCookie();
                return RedirectToAction("DeleteInfo");
            }

            return Content("You can delete the account you are logged in to.");
        }
        public IActionResult DeleteInfo()
        {
            return View();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }

        public IActionResult AdminPage()
        {
            return View();
        }

        public IActionResult DeletePage()//burayı tekrar kontrol et sildirmek için mail bilgisi almam gerekiyor sanırım.
        {
            return View();
        }

    }
}
