using Azure;
using Ctulhu.BaseContext;
using Ctulhu.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
            //Tag tag12 = new Tag { Name = "Травы" };
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
            //_context._tag.Add(tag12);
            //_context._users.Add(user1);
            //_context.SaveChanges();
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context._users.ToListAsync();
            var posts = await _context._posts.Where(p => p.IsApproved).ToListAsync();
            var tags = await _context._tag.ToListAsync();
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

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(description) || image == null || string.IsNullOrEmpty(tag))
            {
                return RedirectToAction("Index");
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
            var comments = await _context._comments.Where(c => c.PostID == id).ToListAsync();
            var post = await _context._posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var model = new PostComment
            {
                Posts = new List<Posts> { post },
                Comment = comments
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetPost(int id)
        {
            var post = await _context._posts.Where(p => p.ID == id).Select(p => new
            {
                p.CreatedAt
            }).FirstOrDefaultAsync();

            if (post == null)
            {
                return NotFound();
            }

            return Json(post);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(int postId, string text)
        {
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user == null)
            {
                return Unauthorized();
            }

            var comment = new Comment
            {
                PostID = postId,
                Author = user.Login,
                Text = text,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context._comments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Post", new { id = postId });
        }

        [HttpPost]
        public IActionResult DeleteComment(int postId, int commentId)
        {
            var commentToDelete = _context._comments.FirstOrDefault(c => c.ID == commentId);

            if (commentToDelete == null)
            {
                return NotFound();
            }
            var user = _context._users.FirstOrDefault(u => u.Login == User.Identity.Name);

            if (commentToDelete != null && (User.IsInRole("admin") || commentToDelete.Author == user.Login))
            {
                _context._comments.Remove(commentToDelete);
                _context.SaveChanges();
                return RedirectToAction("Post", new { id = postId });
            }
            return Unauthorized(new { message = "У вас недостаточно прав, чтобы удалить этот комментарий" });
        }

        public async Task<IActionResult> Recipes()
        {
            var users = await _context._users.ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var tag = await _context._tag.FirstOrDefaultAsync(t => t.Name == "Рецепты");
            var posts = await _context._posts
                .Where(p => p.Tag == tag.Name && p.IsApproved)
                .ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags };
            ViewBag.CurrentTagName = tag.Name;
            return View(model);
        }
        public async Task<IActionResult> Tinctures()
        {
            var users = await _context._users.ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var tag = await _context._tag.FirstOrDefaultAsync(t => t.Name == "Настойки");
            var posts = await _context._posts
                .Where(p => p.Tag == tag.Name && p.IsApproved)
                .ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags };
            ViewBag.CurrentTagName = tag.Name;
            return View(model);
        }
        public async Task<IActionResult> Potions()
        {
            var users = await _context._users.ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var tag = await _context._tag.FirstOrDefaultAsync(t => t.Name == "Зелья");
            var posts = await _context._posts
                .Where(p => p.Tag == tag.Name && p.IsApproved)
                .ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags };
            ViewBag.CurrentTagName = tag.Name;
            return View(model);
        }
        public async Task<IActionResult> Medicines()
        {
            var users = await _context._users.ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var tag = await _context._tag.FirstOrDefaultAsync(t => t.Name == "Зелья");
            var posts = await _context._posts
                .Where(p => p.Tag == tag.Name && p.IsApproved)
                .ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags };
            ViewBag.CurrentTagName = tag.Name;
            return View(model);
        }
        public async Task<IActionResult> Articles()
        {
            var users = await _context._users.ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var tag = await _context._tag.FirstOrDefaultAsync(t => t.Name == "Полезные статьи");
            var posts = await _context._posts
                .Where(p => p.Tag == tag.Name && p.IsApproved)
                .ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags };
            ViewBag.CurrentTagName = tag.Name;
            return View(model);
        }
        public async Task<IActionResult> Blog()
        {
            var users = await _context._users.ToListAsync();
            var tags = await _context._tag.ToListAsync();
            var tag = await _context._tag.FirstOrDefaultAsync(t => t.Name == "Блог");
            var posts = await _context._posts
                .Where(p => p.Tag == tag.Name && p.IsApproved)
                .ToListAsync();
            var model = new UserPosts { Users = users, Posts = posts, Tags = tags };
            ViewBag.CurrentTagName = tag.Name;
            return View(model);
        }
    }
}
