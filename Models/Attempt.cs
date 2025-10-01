namespace QuizApp.Models
{
    public class Attempt
    {
        public Guid AttemptId { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public User User { get; set; } = null!;
    }
}