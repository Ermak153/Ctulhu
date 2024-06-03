using Microsoft.EntityFrameworkCore;
using Ctulhu.Models;

namespace Ctulhu.BaseContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Users> _users { get; set; }
        public DbSet<Posts> _posts { get; set; }
        public DbSet<Tag> _tag { get; set; }
        public DbSet<Comment> _comments { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
    }
}
