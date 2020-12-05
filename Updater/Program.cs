using System;

namespace Updater
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string pathToExecutable = args[0];
                Console.WriteLine($"Path = {pathToExecutable}");
            }
            else
            {
                Console.WriteLine("Missing argument for the bot executable path");
            }
        }
    }
}
