using System;

namespace QuizApp.DTOs
{
    public sealed class QuestionReportResponse
    {
        public int QuestionReportId { get; init; }
        public int TestQuestionId { get; init; }
        public string QuestionText { get; init; } = null!;
        public string CategoryName { get; init; } = null!;
        public string CategorySlug { get; init; } = null!;
        public string OptionA { get; init; } = null!;
        public string OptionB { get; init; } = null!;
        public string OptionC { get; init; } = null!;
        public string OptionD { get; init; } = null!;
        public string CorrectOption { get; init; } = null!;
        public string? Reason { get; init; }
        public Guid? ReporterUserId { get; init; }
        public string? ReporterUsername { get; init; }
        public DateTime ReportedAt { get; init; }
    }
}
