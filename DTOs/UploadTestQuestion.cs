namespace QuizApp.DTOs
{
    public sealed record UploadTestQuestion
    {
        public string CategorySlug { get; set; } = null!;
        public string? CategoryName { get; set; }
        public string Text { get; set; } = null!;
        public string OptionA { get; set; } = null!;
        public string OptionB { get; set; } = null!;
        public string OptionC { get; set; } = null!;
        public string OptionD { get; set; } = null!;
        public string CorrectOption { get; set; } = null!;
    }
}
