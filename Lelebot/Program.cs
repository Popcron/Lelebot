using System;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length == 1)
            {
                await Info.LoadAtPath(args[0]);
                Log.WriteLine("Loaded info from file");
            }
            else
            {
                await Info.LoadAtPath(null);
                Log.WriteLine("No info file given, loading defaults");
            }

            Bot bot = new();
            while (true)
            {
                string command = Console.ReadLine();
                Log.ConsoleUser(command);

                await bot.RunTask(command);
            }
        }
    }
}
