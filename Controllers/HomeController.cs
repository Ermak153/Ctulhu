using Ctulhu.BaseContext;
using Ctulhu.Models;
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
        }
        public async Task<IActionResult> Print()
        {
            List<Users> users = await _context._users.ToListAsync();
            return View("Index", users);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
