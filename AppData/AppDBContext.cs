using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Model;

namespace Todo.AppData
{
    public class AppDBContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
