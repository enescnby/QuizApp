using Microsoft.EntityFrameworkCore;
using QuizApp.Models;

namespace QuizApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserStats> UserStats { get; set; } = null!;
        public DbSet<Attempt> Attempts { get; set; } = null!;
        public DbSet<TestQuestion> TestQuestions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Stats)
                .WithOne(s => s.User)
                .HasForeignKey<UserStats>(s => s.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Attempts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .IsRequired();
        }
    }
}