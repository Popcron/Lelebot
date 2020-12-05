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
                    using (Process process = Process.Start(processInfo))
                    {
                        do
                        {
                            int output = process.StandardOutput.Read();
                            char key = Convert.ToChar(output);
                            if (key == Convert.ToChar(ConsoleKey.Enter))
                            {
                                Console.WriteLine();
                                process.StandardInput.Write('\n');
                            }
                            else
                            {
                                process.StandardInput.Write(key);
                            }
                        }
                        while (!process.HasExited);
                    }
                }
            }
            else
            {
                Console.WriteLine("Missing argument for the bot executable path");
            }
        }
    }
}
