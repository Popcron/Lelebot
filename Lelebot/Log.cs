using System;

namespace Lelebot
{
    public class Log
    {
        public static void Write(object obj)
        {
            Console.Write(obj);
        }

        public static void WriteLine(object obj = null)
        {
            Console.WriteLine(obj);
        }

        public static void Error(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;
        }

        public static void DiscordEvent(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;
        }

        public static void User(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"> {obj}");
            Console.ForegroundColor = color;
        }

        public static void CommandResult(object obj)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(obj);
            Console.ForegroundColor = color;
        }
    }
}
