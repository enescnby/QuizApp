namespace QuizApp.DTOs
{
    public sealed record LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}