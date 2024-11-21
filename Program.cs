using QuizApp.Core;
using QuizApp.Models;

namespace QuizApp.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var quizService = new QuizService();
            Console.WriteLine("Welcome to Quiz App!");

            while (true)
            {
                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. View Leaderboard");
                Console.WriteLine("4. Exit");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegisterUser(quizService);
                        break;
                    case "2":
                        var user = LoginUser(quizService);
                        if (user != null) UserMenu(quizService, user);
                        break;
                    case "3":
                        ShowLeaderboard(quizService);
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void RegisterUser(QuizService quizService)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            if (quizService.RegisterUser(username, password))
                Console.WriteLine("Registration successful!");
            else
                Console.WriteLine("Username already exists.");
        }

        static User LoginUser(QuizService quizService)
        {
            Console.Write("Enter username: ");
            var username = Console.ReadLine();
            Console.Write("Enter password: ");
            var password = Console.ReadLine();

            var user = quizService.AuthenticateUser(username, password);
            if (user == null)
                Console.WriteLine("Invalid credentials.");
            return user;
        }

        static void UserMenu(QuizService quizService, User user)
        {
            while (true)
            {
                Console.WriteLine($"\nWelcome, {user.Username}!");
                Console.WriteLine("1. Create a Quiz");
                Console.WriteLine("2. Solve a Quiz");
                Console.WriteLine("3. View My High Score");
                Console.WriteLine("4. Logout");
                Console.Write("Choose an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateQuiz(quizService, user.Username);
                        break;
                    case "2":
                        SolveQuiz(quizService, user);
                        break;
                    case "3":
                        Console.WriteLine($"Your High Score: {user.HighScore}");
                        break;
                    case "4":
                        return;
                }
            }
        }

        static void CreateQuiz(QuizService quizService, string creatorUsername)
        {
            Console.Write("Enter a title for your quiz: ");
            var title = Console.ReadLine();

            var questions = new List<Question>();

            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"\nQuestion {i}:");
                Console.Write("Enter question text: ");
                var text = Console.ReadLine();

                var options = new List<string>();
                for (int j = 1; j <= 4; j++)
                {
                    Console.Write($"Option {j}: ");
                    options.Add(Console.ReadLine());
                }

                Console.Write("Enter the number of the correct option (1-4): ");
                int correctOptionIndex;
                while (!int.TryParse(Console.ReadLine(), out correctOptionIndex) || correctOptionIndex < 1 || correctOptionIndex > 4)
                {
                    Console.Write("Invalid input. Please enter a number between 1 and 4: ");
                }

                questions.Add(new Question
                {
                    Text = text,
                    Options = options,
                    CorrectOptionIndex = correctOptionIndex - 1
                });
            }

            quizService.CreateQuiz(creatorUsername, title, questions);
            Console.WriteLine("Quiz created successfully!");
        }

        static void SolveQuiz(QuizService quizService, User user)
        {
            var availableQuizzes = quizService.GetAvailableQuizzes(user.Username);

            if (!availableQuizzes.Any())
            {
                Console.WriteLine("No quizzes available to solve.");
                return;
            }

            Console.WriteLine("\nAvailable Quizzes:");
            for (int i = 0; i < availableQuizzes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {availableQuizzes[i].Title}");
            }

            Console.Write("Select a quiz to solve: ");
            int quizChoice;
            while (!int.TryParse(Console.ReadLine(), out quizChoice) || quizChoice < 1 || quizChoice > availableQuizzes.Count)
            {
                Console.Write("Invalid choice. Try again: ");
            }

            var selectedQuiz = availableQuizzes[quizChoice - 1];
            int score = 0;

            Console.WriteLine($"\nYou have 2 minutes to complete this quiz.");
            var startTime = DateTime.Now;

            foreach (var question in selectedQuiz.Questions)
            {
                if ((DateTime.Now - startTime).TotalMinutes > 2)
                {
                    Console.WriteLine("\nTime is up! You failed to complete the quiz.");
                    return;
                }

                Console.WriteLine($"\n{question.Text}");
                for (int i = 0; i < question.Options.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.Options[i]}");
                }

                Console.Write("Your answer (1-4): ");
                int userAnswer;
                while (!int.TryParse(Console.ReadLine(), out userAnswer) || userAnswer < 1 || userAnswer > 4)
                {
                    Console.Write("Invalid input. Please enter a number between 1 and 4: ");
                }

                if (userAnswer - 1 == question.CorrectOptionIndex)
                {
                    Console.WriteLine("Correct!");
                    score += 20;
                }
                else
                {
                    Console.WriteLine("Incorrect.");
                    score -= 20;
                }
            }

            Console.WriteLine($"\nQuiz completed! Your score: {score}");
            quizService.UpdateHighScore(user.Username, score);
        }

        static void ShowLeaderboard(QuizService quizService)
        {
            var leaderboard = quizService.GetLeaderboard();
            Console.WriteLine("Top 10 Players:");
            foreach (var entry in leaderboard)
            {
                Console.WriteLine($"{entry.Username}: {entry.HighScore} points");
            }
        }
    }
}
