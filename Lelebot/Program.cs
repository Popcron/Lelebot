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
            Info info = new Info();
            if (args.Length == 1)
            {
                info = await Info.LoadAtPath(args[0]);
            }

            Bot bot = new(info);
            while (true)
            {
                string command = Console.ReadLine();
                await bot.RunTask(command);
            }
        }
    }
}
