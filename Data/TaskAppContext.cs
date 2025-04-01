using Microsoft.EntityFrameworkCore;
using TaskApp.Models;

namespace TaskApp.Data
{
    public class TaskAppContext : DbContext
    {
        public TaskAppContext(DbContextOptions<TaskAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
    }
}
