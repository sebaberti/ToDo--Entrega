
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoList_API.Models;

namespace ToDoList_API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }


        public DbSet<Tasks> tasks { get; set; }

        public DbSet<Categories> categories { get; set; }

        public DbSet<CategoryTask> categoryTasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryTask>()
                .HasKey(ct => new { ct.TaskId, ct.CategoryId });
        }




    }
}
