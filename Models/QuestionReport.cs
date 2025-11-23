namespace QuizApp.Models
{
    public class QuestionReport
    {
        public int QuestionReportId { get; set; }
        public int TestQuestionId { get; set; }
        public Guid? ReporterUserId { get; set; }
        public string? Reason { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public TestQuestion Question { get; set; } = null!;
        public User? Reporter { get; set; }
    }
}
