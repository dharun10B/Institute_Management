using System.Data;
using Institute_Management.Handler;
using Microsoft.EntityFrameworkCore;

namespace Institute_Management.Models
{
    public class InstituteManagement : DbContext
    {
        public InstituteManagement(DbContextOptions<InstituteManagement> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Batch> Batches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _= modelBuilder.Entity<User>().HasData([
                new User
                {
                    Id =1,
                    Username ="admin",
                    Email = "admin123@gmail.com",
                    PasswordHash = PasswordHashHandler.HashPassword("admin123"),
                    Role ="admin"
                }
                ]);
        }
    }
}
