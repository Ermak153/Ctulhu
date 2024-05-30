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
            var posts = await _context._posts.Where(p => p.IsApproved).ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts };
            return View(model);
        }
        public async Task<IActionResult> About()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            return View();
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
                Author = user.Login,
                IsApproved = false
            };

            _context._posts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditPost([FromBody] PostEdit model)
        {
            var post = _context._posts.FirstOrDefault(p => p.ID == model.id);
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);

            if (post != null && (User.IsInRole("admin") || post.Author == user.Login))
            {
                post.Title = model.title;
                post.Description = model.description;
                _context._posts.Update(post);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = _context._posts.FirstOrDefault(p => p.ID == id);
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);

            if (post != null && (User.IsInRole("admin") || post.Author == user.Login))
            {
                _context._posts.Remove(post);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Unauthorized();
        }
    }
}
