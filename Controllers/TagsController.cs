using Ctulhu.BaseContext;
using Ctulhu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ctulhu.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private readonly ApplicationContext _context;

        public TagsController(ApplicationContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> SearchTags(string query)
        {
            List<Tag> tags;

            if (string.IsNullOrEmpty(query))
            {
                tags = await _context._tag.ToListAsync();
            }
            else
            {
                tags = await _context._tag.Where(t => t.Name.Contains(query)).ToListAsync();
            }

            return Json(tags.Select(t => t.Name));
        }
    }
}
