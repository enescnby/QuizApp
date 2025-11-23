using System.Text.Json.Serialization;

namespace QuizApp.Models
{
    public class QuestionCategory
    {
        public int QuestionCategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;

        [JsonIgnore]
        public ICollection<TestQuestion> Questions { get; set; } = new List<TestQuestion>();
    }
}
