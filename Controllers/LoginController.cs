using Ctulhu.BaseContext;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ctulhu.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Win32;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ctulhu.Filters;

namespace Ctulhu.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationContext _context;
        public LoginController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        [RedirectAuthenticatedUser]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Registration model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context._users.FirstOrDefault(u => u.Login == model.Login || u.Email == model.Email);
                if (existingUser == null)
                {
                    var user = new Users
                    {
                        Login = model.Login,
                        Email = model.Email,
                        Password = model.Password,
                        Role = "user"
                    };

                    _context._users.Add(user);
                    await _context.SaveChangesAsync();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Profile", "Login");
                }

                ModelState.AddModelError("", "Пользователь с таким логином или email уже существует");
            }

            return View(model);
        }
        [HttpGet]
        [RedirectAuthenticatedUser]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = _context._users.FirstOrDefault(u => u.Login == model.Username && u.Password == model.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Login),
                        new Claim(ClaimTypes.Role, user.Role)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    
                    if (user.Role == "user")
                    {
                        return RedirectToAction("Profile", "Login");
                    }
                    else if (user.Role == "admin")
                    {
                        return RedirectToAction("Profile", "Login");
                    }
                }

                ModelState.AddModelError("", "Неверный логин или пароль");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userLogin = User.Identity.Name;
            var user = await _context._users.FirstOrDefaultAsync(u => u.Login == userLogin);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new UserProfile
            {
                ID = user.ID,
                Login = user.Login,
                Email = user.Email
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfile model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context._users.FirstOrDefaultAsync(u => u.ID == model.ID);

                if (user != null)
                {
                    if (!string.IsNullOrEmpty(model.OldPassword) && model.OldPassword == user.Password)
                    {
                        if (!string.IsNullOrEmpty(model.Password) && model.Password == model.ConfirmPassword)
                        {
                            user.Password = model.Password;
                        }
                        user.Email = model.Email;
                        _context._users.Update(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Profile");
                    }
                    ModelState.AddModelError("", "Старый пароль неверен или новые пароли не совпадают");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
