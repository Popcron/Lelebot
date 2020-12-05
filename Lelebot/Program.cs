using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Program
    {
        public static string PathToExecutable => Assembly.GetEntryAssembly().Location;
        public static string PathToDirectory => AppDomain.CurrentDomain.BaseDirectory;

        private static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {
                string pathToInfo = args[0];
                Console.WriteLine($"Info = {pathToInfo}");

                Info info = await Info.LoadAtPath(pathToInfo);
                Bot bot = new(info);
                while (true)
                {
                    string command = Console.ReadLine();
                    await bot.RunTask(command);
                }
            }
            else
            {
                Console.WriteLine("Path to info.json is expected as an argument");
            }
        }
    }
}
