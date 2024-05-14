using Microsoft.AspNetCore.Mvc;

namespace Ctulhu.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
