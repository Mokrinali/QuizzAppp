using QuizApp.Models;
using QuizApp.Data;
using System.Linq;

namespace QuizApp.Core
{
    public class QuizService
    {
        private List<User> _users;
        private List<Quiz> _quizzes;

        public QuizService()
        {
            _users = DataStore.LoadUsers();
            _quizzes = DataStore.LoadQuizzes();
        }

        public bool RegisterUser(string username, string password)
        {
            if (_users.Any(u => u.Username == username)) return false;
            _users.Add(new User { Username = username, Password = password });
            DataStore.SaveUsers(_users);
            return true;
        }

        public User AuthenticateUser(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public void CreateQuiz(string creatorUsername, string title, List<Question> questions)
        {
            var quiz = new Quiz
            {
                QuizId = _quizzes.Any() ? _quizzes.Max(q => q.QuizId) + 1 : 1,
                Title = title,
                CreatorUsername = creatorUsername,
                Questions = questions
            };
            _quizzes.Add(quiz);
            DataStore.SaveQuizzes(_quizzes);
        }

        public List<Quiz> GetAvailableQuizzes(string username)
        {
            return _quizzes.Where(q => q.CreatorUsername != username).ToList();
        }

        public void UpdateHighScore(string username, int score)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null && score > user.HighScore)
            {
                user.HighScore = score;
                DataStore.SaveUsers(_users);
            }
        }

        public List<LeaderboardEntry> GetLeaderboard()
        {
            return _users
                .OrderByDescending(u => u.HighScore)
                .Take(10)
                .Select(u => new LeaderboardEntry { Username = u.Username, HighScore = u.HighScore })
                .ToList();
        }
    }
}