using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Model;

namespace Todo.AppData
{
    public class AppDBContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ToDo> ToDoItems { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //var admin = new IdentityRole("admin");
            //admin.NormalizedName = "admin";

            var user = new IdentityRole("user");
            user.NormalizedName = "user";

            modelBuilder.Entity<IdentityRole>().HasData( user);

            modelBuilder.Entity<User>()
                .HasMany(item => item.ToDoItems)
                .WithOne(u => u.User)
                .HasForeignKey(ui => ui.UserId);



        }
    }
}
