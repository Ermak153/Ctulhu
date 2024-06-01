using Azure;
using Ctulhu.BaseContext;
using Ctulhu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;

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
            //Tag tag4 = new Tag { Name = "Лекарства" };
            //Tag tag5 = new Tag { Name = "Полезные статьи" };
            //Tag tag6 = new Tag { Name = "Блог" };
            //Tag tag7 = new Tag { Name = "Травы" };
            //Tag tag8 = new Tag { Name = "Цветы" };
            //Tag tag9 = new Tag { Name = "Фрукты" };
            //Tag tag10 = new Tag { Name = "Ягоды" };
            //Tag tag11 = new Tag { Name = "Овощи" };
            //_context._tag.Add(tag1);
            //_context._tag.Add(tag2);
            //_context._tag.Add(tag3);
            //_context._tag.Add(tag4);
            //_context._tag.Add(tag5);
            //_context._tag.Add(tag6);
            //_context._tag.Add(tag8);
            //_context._tag.Add(tag9);
            //_context._tag.Add(tag10);
            //_context._tag.Add(tag11);
            //_context._posts.Add(post1);
            //_context._users.Add(user1);
            //_context._users.Add(user2);
            //_context.SaveChanges();
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context._users.ToListAsync();
            var posts = await _context._posts.Where(p => p.IsApproved).ToListAsync();
            var tags = await _context._tag.ToListAsync(); // Получаем список тегов
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags};
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
        [Authorize]
        public async Task<IActionResult> CreatePost(string title, string description, IFormFile image, string tag)
        {
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            string imagePath = null;

            if (image != null)
            {
                imagePath = "/uploads/" + image.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
            }

            var post = new Posts
            {
                Title = title,
                Description = description,
                Author = user.Login,
                ImageUrl = imagePath,
                Tag = tag,
                IsApproved = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
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
