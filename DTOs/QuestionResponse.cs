namespace QuizApp.DTOs
{
    public sealed class QuestionResponse
    {
        public int TestQuestionId { get; init; }
        public int QuestionCategoryId { get; init; }
        public string Text { get; init; } = null!;
        public string OptionA { get; init; } = null!;
        public string OptionB { get; init; } = null!;
        public string OptionC { get; init; } = null!;
        public string OptionD { get; init; } = null!;
    }
}
