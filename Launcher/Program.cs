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
                    Process process = new Process();
                    process.StartInfo = new(pathToExecutable)
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        RedirectStandardError = true
                    };

                    process.EnableRaisingEvents = true;
                    process.OutputDataReceived += OutputData;
                    process.ErrorDataReceived += ErrorData;
                    process.Start();
                    process.BeginErrorReadLine();
                    process.BeginOutputReadLine();

                    while (!process.HasExited)
                    {
                        process.StandardInput.WriteLine(Console.ReadLine());
                    }

                    Console.WriteLine("Started");
                }
            }
            else
            {
                Console.WriteLine("Missing argument for the bot executable path");
            }
        }

        private static void ErrorData(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void OutputData(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data + "\n");
        }
    }
}
