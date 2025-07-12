using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure TodoItem entity
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.Priority).IsRequired();
                
                // Add index for better query performance
                entity.HasIndex(e => e.IsCompleted);
                entity.HasIndex(e => e.CreatedAt);
                entity.HasIndex(e => e.Priority);
            });

            // Seed initial data
            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem
                {
                    Id = 1,
                    Title = "Sample Task 1",
                    Description = "This is a sample todo item to get you started",
                    IsCompleted = false,
                    Priority = Priority.High,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new TodoItem
                {
                    Id = 2,
                    Title = "Sample Task 2",
                    Description = "Another sample task",
                    IsCompleted = true,
                    Priority = Priority.Medium,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    CompletedAt = DateTime.UtcNow.AddHours(-2)
                }
            );
        }
    }
}
