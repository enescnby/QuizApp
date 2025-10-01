namespace QuizApp.Models
{
    public class UserStats
    {
        public Guid UserStatsId { get; set; }
        public Guid UserId { get; set; }
        public int TotalScore { get; set; } = 0;
        public int SoloScore { get; set; } = 0;
        public int SoloCorrectAnswers { get; set; } = 0;
        public int SoloWrongAnswers { get; set; } = 0;
        public int DuelScore { get; set; } = 0;
        public int DuelCorrectAnswers { get; set; } = 0;
        public int DuelWrongAnswes { get; set; } = 0;
        public int TotalQuizzesPlayed { get; set; } = 0;
        public int TotalCorrectAnswers { get; set; } = 0;


        public User User { get; set; } = null!;
    }
}