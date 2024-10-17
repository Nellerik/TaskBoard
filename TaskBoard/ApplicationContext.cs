using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskBoard.Model;

namespace TaskBoard
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Model.Task> Tasks { get; set; }
        public DbSet<Board> Boards { get; set; }
        public DbSet<TaskState> TaskStates { get; set; }
        public ApplicationContext()
        {
            Database.EnsureCreated();
            if (TaskStates.Count() == 0)
            {
                TaskStates.Add(new TaskState() { Id = 1, Name = "Backlog" });
                TaskStates.Add(new TaskState() { Id = 2, Name = "To Do" });
                TaskStates.Add(new TaskState() { Id = 3, Name = "In Progress" });
                TaskStates.Add(new TaskState() { Id = 4, Name = "Done" });
                SaveChanges();
            }
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=taskboardapp.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.Task>()
                .HasOne(t => t.Board)
                .WithMany(b => b.Tasks)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}