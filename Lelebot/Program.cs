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
                await Info.LoadAtPath(args[0]);
            }
            else
            {
                await Info.LoadAtPath(null);
            }

            Bot bot = new();
            while (true)
            {
                string command = Console.ReadLine();
                Log.User(command);

                await bot.RunTask(command);
            }
        }
    }
}
