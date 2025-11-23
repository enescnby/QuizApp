namespace QuizApp.DTOs
{
    public sealed class GuessQuestionResponse
    {
        public bool IsCorrect { get; init; }
        public string CorrectOption { get; init; } = null!;
    }
}
