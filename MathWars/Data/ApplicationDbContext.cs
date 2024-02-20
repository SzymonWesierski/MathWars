using MathWars.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection.Metadata;

namespace MathWars.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<TasksCategory> TasksCategory { get; set; }
        public DbSet<AnswerTypes> AnswerTypes { get; set; }
        public DbSet<TasksAndCategories> TasksAndCategories { get; set; }
        public DbSet<UsersReports> UsersReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Answers>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Answers>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Answers)
                .HasForeignKey(a => a.TaskId);

			modelBuilder.Entity<Tasks>()
		        .HasMany(e => e.Category)
		        .WithMany(e => e.Tasks)
		        .UsingEntity<TasksAndCategories>();

			modelBuilder.Entity<Tasks>()
                .HasOne(e => e.AnswerType)
                .WithMany(e => e.Tasks)
                .HasForeignKey(e => e.AnswerTypeId)
                .IsRequired();
        }   
    }
}
