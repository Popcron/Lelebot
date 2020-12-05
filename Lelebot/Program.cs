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
            Info info = await Info.LoadFromFile();
            Bot bot = new Bot(info);
            while (true)
            {
                string command = Console.ReadLine();
                await bot.RunTask(command);
            }
        }
    }
}
