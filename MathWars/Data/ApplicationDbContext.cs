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
    }
}
