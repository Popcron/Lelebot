using System;
using System.IO;

namespace Lelebot
{
    public class Program
    {
        private static Bot bot;

        public static void Main(string[] args)
        {
            string token = GetToken();
            bot = new Bot(token);

            while (true)
            {
                string command = Console.ReadLine();
                bot.Run(command);
            }
        }

        private static string GetToken()
        {
            string pathToTokenFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "token.txt");
            if (File.Exists(pathToTokenFile))
            {
                return File.ReadAllText(pathToTokenFile);
            }
            else
            {
                Console.WriteLine("[program] token.txt file not present beside the executable");
                return null;
            }
        }
    }
}