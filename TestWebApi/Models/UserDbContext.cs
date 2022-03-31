using Microsoft.EntityFrameworkCore;

namespace TestWebApi.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Person> Persons { get; set; } = null!;
        public DbSet<Kid> Kids { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Person>()
                .HasMany(b => b.Kids)
                .WithOne();
        }
    }
}