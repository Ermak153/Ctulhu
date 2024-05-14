using Microsoft.EntityFrameworkCore;
using Ctulhu.Models;

namespace Ctulhu.BaseContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> _users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
