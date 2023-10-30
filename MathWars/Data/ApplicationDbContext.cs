using MathWars.Models;
using Microsoft.EntityFrameworkCore;

namespace MathWars.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answers>()
            .HasOne(a => a.User)
            .WithMany(u => u.Answers)
            .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Answers>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Answers)
                .HasForeignKey(a => a.TaskId);


        }
    }
}
