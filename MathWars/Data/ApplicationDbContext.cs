using MathWars.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskRating> TaskRating { get; set; }
        public DbSet<AnswersToTask> AnswersToTasks {  get; set; }
        public DbSet<UserAnswer> Answers { get; set; }
        public DbSet<TasksCategory> TasksCategory { get; set; }
        public DbSet<TasksAndCategories> TasksAndCategories { get; set; }
        public DbSet<UsersReports> UsersReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(a => a.User)
                .WithMany(u => u.Answers)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<UserAnswer>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Answers)
                .HasForeignKey(a => a.TaskId);

            modelBuilder.Entity<Tasks>()
                .HasMany(e => e.Category)
                .WithMany(e => e.Tasks)
                .UsingEntity<TasksAndCategories>();

			modelBuilder.Entity<AnswersToTask>()
				.HasOne(a => a.Task)
				.WithMany(u => u.AnswersToTask)
				.HasForeignKey(a => a.TaskId);
                //.OnDelete(Cascade);

            modelBuilder.Entity<TaskRating>()
				.HasOne(r => r.Task)
                .WithMany(r => r.TaskRatings)
                .HasForeignKey(r => r.TaskId);

			modelBuilder.Entity<TaskRating>()
				.HasOne(r => r.User)
				.WithMany(r => r.TaskRatings)
				.HasForeignKey(r => r.UserId);
		}   
    }
}
