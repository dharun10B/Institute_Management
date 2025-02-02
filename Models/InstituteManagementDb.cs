using Microsoft.EntityFrameworkCore;

namespace Institute_Management.Models
{
    public class InstituteManagementDb : DbContext
    {
        public InstituteManagementDb(DbContextOptions<InstituteManagementDb> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Batch> Batches { get; set; }
    }
}
