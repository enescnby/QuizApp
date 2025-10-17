using System.Text.Json.Serialization;

namespace QuizApp.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Category
    {
        General,
        History,
        Movies,
        Books
    }
}