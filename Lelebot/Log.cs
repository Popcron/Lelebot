using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Lelebot
{
    public static class Log
    {
        private static DateTime startTime;
        private static StringBuilder logWriter;
        private static string pathToLogFile;
        private static bool dirty;

        static Log()
        {
            startTime = DateTime.UtcNow;
            logWriter = new StringBuilder();
            string timeStamp = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
            string logFolder = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }

            pathToLogFile = Path.Combine(logFolder, $"{timeStamp}.txt");

            PeriodicFlush();
            AppDomain.CurrentDomain.ProcessExit += OnExit;
        }

        private static void OnExit(object sender, EventArgs e)
        {
            WriteLine("Closing");
            Flush();
        }

        private static async void PeriodicFlush()
        {
            while (true)
            {
                await Task.Delay(500);
                if (dirty)
                {
                    dirty = false;
                    Flush();
                }
            }
        }

        private static void Flush()
        {
            File.AppendAllText(pathToLogFile, logWriter.ToString());
        }

        private static void LogToFile(string category, object obj)
        {
            TimeSpan time = DateTime.UtcNow - startTime;
            string stringTime = ((int)time.TotalSeconds).ToString();
            string prefix = $"{stringTime,-7} {category,4}";
            string line = $"[{prefix}] {obj}";
            logWriter.AppendLine(line);
            dirty = true;
        }

        public static void WriteLine(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;

            LogToFile("Log", obj);
        }

        public static void Error(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;

            LogToFile("Error", obj);
        }

        public static void DiscordEvent(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;

            LogToFile("Discord", obj);
        }

        public static void ConsoleUser(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"> {obj}");
            Console.ForegroundColor = color;

            LogToFile("User", $"> {obj}");
        }

        public static void User(string author, object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{author} > {obj}");
            Console.ForegroundColor = color;

            LogToFile("User", $"{author} > {obj}");
        }

        public static void CommandResult(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;

            LogToFile("Result", obj);
        }
    }
}
