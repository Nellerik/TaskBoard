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
        public ApplicationContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=taskboardapp.db");
        }
    }
}