using System.Text.Json;
using QuizApp.Models;

namespace QuizApp.Data
{
    public static class DataStore
    {
        private const string UsersFile = "C:\\Users\\kikia\\source\\repos\\QuizzAppp\\UsersFile.json";
        private const string QuizzesFile = "C:\\Users\\kikia\\source\\repos\\QuizzAppp\\QuizzesFile.json";
        public static List<User> LoadUsers()
        {
            if (!File.Exists(UsersFile)) return new List<User>();
            var json = File.ReadAllText(UsersFile);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        public static void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(UsersFile, json);
        }

        public static List<Quiz> LoadQuizzes()
        {
            if (!File.Exists(QuizzesFile)) return new List<Quiz>();
            var json = File.ReadAllText(QuizzesFile);
            return JsonSerializer.Deserialize<List<Quiz>>(json) ?? new List<Quiz>();
        }

        public static void SaveQuizzes(List<Quiz> quizzes)
        {
            var json = JsonSerializer.Serialize(quizzes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(QuizzesFile, json);
        }
    }
}