using System;
using System.Diagnostics;
using System.IO;

namespace Launcher
{
    public class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                string pathToExecutable = Path.GetFullPath(args[0]);
                string pathToInfo = Path.GetFullPath(args[1]);
                if (File.Exists(pathToExecutable))
                {
                    Console.WriteLine($"Executable = {pathToExecutable}");

                    Process process = new();
                    process.StartInfo = new(pathToExecutable)
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        RedirectStandardError = true,
                        Arguments = pathToInfo
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
                Console.WriteLine("Missing arguments for the bot executable path and info.json file");
            }
        }

        private static void ErrorData(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void OutputData(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
