using Ctulhu.BaseContext;
using Ctulhu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ctulhu.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _context;
        IWebHostEnvironment _appEnvironment;
        public HomeController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            //Users user1 = new Users { Login = "Neko", Email = "vorkunovae@mer.ci.nsu.ru", Password = "qwerty", Role = "admin" };
            //Users user2 = new Users { Login = "Tamik17", Email = "syratts@mer.ci.nsu.ru", Password = "12345", Role = "user" };
            //Posts post1 = new Posts { Title = "Example Title", Description = "Hello world", Author = "Neko" };
            //Tag tag1 = new Tag { Name = "Рецепты" };
            //Tag tag2 = new Tag { Name = "Настойки" };
            //Tag tag3 = new Tag { Name = "Зелья" };
            //Tag tag4 = new Tag { Name = "Травы" };
            //_context._tags.Add(tag1);
            //_context._tags.Add(tag2);
            //_context._tags.Add(tag3);
            //_context._tags.Add(tag4);
            //_context._posts.Add(post1);
            //_context._users.Add(user1);
            //_context._users.Add(user2);
            //_context.SaveChanges();
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context._users.ToListAsync();
            var posts = await _context._posts.Where(p => p.IsApproved).ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var model = new CreatePost
            {
                Posts = posts,
                AvailableTags = tags
            };
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePost()
        {
            var tags = await _context._tag.ToListAsync();
            ViewBag.Tags = tags;
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost(CreatePost model)
        {
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            string imagePath = null;
            if (model.Image != null)
            {
                imagePath = "/uploads/" + model.Image.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await model.Image.CopyToAsync(fileStream);
                }
            }

            var tags = await _context._tag.Where(tag => model.SelectedTagIds.Contains(tag.ID)).ToListAsync();

            var post = new Posts
            {
                Title = model.Title,
                Description = model.Description,
                Author = user.Login,
                ImageUrl = imagePath,
                Tags = tags,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
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
                post.UpdatedAt = DateTime.Now;
                _context._posts.Update(post);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return Unauthorized();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeletePost([FromBody] int id)
        {
            var post = await _context._posts.FirstOrDefaultAsync(p => p.ID == id);
            var user = await _context._users.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);

            if (post == null)
            {
                return NotFound(new { message = "Post not found" });
            }

            if (User.IsInRole("admin") || post.Author == user.Login)
            {
                _context._posts.Remove(post);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Post deleted successfully" });
            }

            return Unauthorized(new { message = "You do not have permission to delete this post" });
        }

        public async Task<IActionResult> Post(int id)
        {
            var post = await _context._posts.FirstOrDefaultAsync(p => p.ID == id && p.IsApproved);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }
    }
}
