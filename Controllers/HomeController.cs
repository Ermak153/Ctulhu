using Ctulhu.BaseContext;
using Ctulhu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ctulhu.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _context;
        public HomeController(ApplicationContext context)
        {
            _context = context;
            //Users user1 = new Users { Login = "Neko", Email = "vorkunovae@mer.ci.nsu.ru", Password = "qwerty", Role = "admin" };
            //Users user2 = new Users { Login = "Tamik17", Email = "syratts@mer.ci.nsu.ru", Password = "12345", Role = "user" };
            //Posts post1 = new Posts { Title = "Example Title", Description = "Hello world", Author = "Neko" };
            //_context._posts.Add(post1);
            //_context._users.Add(user1);
            //_context._users.Add(user2);
            //_context.SaveChanges();
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context._users.ToListAsync();
            var posts = await _context._posts.ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string Title, string Description)
        {
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            var post = new Posts
            {
                Title = Title,
                Description = Description,
                Author = user.Login // Assuming you have an Author property in Posts model
            };

            _context._posts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
