using System;
using System.Diagnostics;
using System.IO;

namespace Launcher
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string pathToExecutable = args[0];
                if (File.Exists(pathToExecutable))
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo(pathToExecutable);
                    Console.WriteLine(processInfo.UseShellExecute);
                    Console.WriteLine(processInfo.RedirectStandardOutput);
                    Console.WriteLine(processInfo.RedirectStandardInput);
                    Console.WriteLine(processInfo.RedirectStandardError);
                    Console.WriteLine(processInfo.CreateNoWindow);
                    Process.Start(processInfo);
                }
            }
            else
            {
                Console.WriteLine("Missing argument for the bot executable path");
            }
        }
    }
}
