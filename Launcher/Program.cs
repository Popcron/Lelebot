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
                    processInfo.RedirectStandardOutput = true;
                    processInfo.RedirectStandardInput = true;
                    processInfo.RedirectStandardError = true;
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
