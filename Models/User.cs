using QuizApp.Models.Enums;

namespace QuizApp.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public UserStats Stats { get; set; } = null!;
        public ICollection<Attempt> Attempts { get; set; } = new List<Attempt>();

    }
}