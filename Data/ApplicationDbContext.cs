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
        public DbSet<QuestionCategory> QuestionCategories { get; set; } = null!;
        public DbSet<QuestionReport> QuestionReports { get; set; } = null!;

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
                .HasIndex(u => u.Auth0Id)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Auth0Id)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<QuestionCategory>()
                .HasIndex(c => c.Slug)
                .IsUnique();

            modelBuilder.Entity<QuestionCategory>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<QuestionCategory>()
                .Property(c => c.Slug)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<TestQuestion>()
                .HasOne(tq => tq.Category)
                .WithMany(c => c.Questions)
                .HasForeignKey(tq => tq.QuestionCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuestionReport>()
                .HasOne(qr => qr.Question)
                .WithMany(q => q.Reports)
                .HasForeignKey(qr => qr.TestQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionReport>()
                .HasOne(qr => qr.Reporter)
                .WithMany(u => u.QuestionReports)
                .HasForeignKey(qr => qr.ReporterUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<QuestionReport>()
                .Property(qr => qr.Reason)
                .HasMaxLength(500);
        }
    }
}
