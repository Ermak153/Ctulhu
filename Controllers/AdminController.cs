using Ctulhu.BaseContext;
using Ctulhu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ctulhu.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        ApplicationContext _context;
        public AdminController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Admin()
        {
            var posts = _context._posts.Where(p => !p.IsApproved).ToList();
            return View(posts);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            var post = _context._posts.FirstOrDefault(p => p.ID == id);
            if (post != null)
            {
                post.IsApproved = true;
                _context._posts.Update(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            var post = _context._posts.FirstOrDefault(p => p.ID == id);
            if (post != null)
            {
                _context._posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Admin");
        }

        [HttpGet]
        public IActionResult Users()
        {
            var users = _context._users.ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult GetUser(int userId)
        {
            var user = _context._users.Find(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Json(user);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context._users.FirstOrDefault(u => u.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id, Users model)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var user = await _context._users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    user.Password = model.Password;
                }

                user.Login = model.Login;
                user.Email = model.Email;
                user.Role = model.Role;

                _context._users.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Users");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult EditPost(int id)
        {
            var post = _context._posts.FirstOrDefault(p => p.ID == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> EditPost(Posts model)
        {
            if (ModelState.IsValid)
            {
                var post = _context._posts.FirstOrDefault(p => p.ID == model.ID);
                if (post != null)
                {
                    post.Title = model.Title;
                    post.Description = model.Description;
                    post.Author = model.Author;

                    _context._posts.Update(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Admin");
                }
            }
            return View(model);
        }
    }
}