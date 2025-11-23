namespace QuizApp.Models
{
    public class TestQuestion
    {
        public int TestQuestionId { get; set; }
        public int QuestionCategoryId { get; set; }
        public QuestionCategory Category { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;
        public string CorrectOption { get; set; } = null!;
        public ICollection<QuestionReport> Reports { get; set; } = new List<QuestionReport>();

    }
}
