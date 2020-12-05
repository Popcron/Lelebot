using System;
using System.IO;

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
                if (File.Exists(pathToExecutable))
                {
                    Console.WriteLine("Exists ya");
                }
                else
                {
                    Console.WriteLine("Non existente");
                }
            }
            else
            {
                Console.WriteLine("Missing argument for the bot executable path");
            }
        }
    }
}
