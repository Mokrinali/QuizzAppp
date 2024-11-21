namespace QuizApp.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int HighScore { get; set; }
        public List<int> AttemptedQuizIds { get; set; } = new List<int>();
    }

    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string CreatorUsername { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }

    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; } = new List<string>();
        public int CorrectOptionIndex { get; set; }
    }

    public class LeaderboardEntry
    {
        public string Username { get; set; }
        public int HighScore { get; set; }
    }
}