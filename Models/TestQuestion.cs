using QuizApp.Models.Enums;


namespace QuizApp.Models
{
    public class TestQuestion
    {
        public Guid TestQuestionId { get; set; }
        public Category Category { get; set; }
        public string Text { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;
        public string CorrectOption { get; set; } = null!;

    }
}