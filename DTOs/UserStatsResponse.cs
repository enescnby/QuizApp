namespace QuizApp.DTOs
{
    public sealed class UserStatsResponse
    {
        public Guid UserId { get; init; }
        public int TotalScore { get; init; }
        public int SoloScore { get; init; }
        public int SoloCorrectAnswers { get; init; }
        public int SoloWrongAnswers { get; init; }
        public int DuelScore { get; init; }
        public int DuelCorrectAnswers { get; init; }
        public int DuelWrongAnswers { get; init; }
        public int TotalQuizzesPlayed { get; init; }
        public int TotalCorrectAnswers { get; init; }
    }
}
