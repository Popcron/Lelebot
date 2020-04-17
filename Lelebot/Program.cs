using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Program
    {
        public static Info ProgramInfo { get; private set; } = null;

        private static Bot bot;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("[main] starting");

            //load the info
            ProgramInfo = Load();
            if (ProgramInfo == null)
            {
                Console.WriteLine("[main] no program info found, press any key to close");
                Console.Read();
                return;
            }

            //check for updates first
            bool updateAvailable = await Updater.IsUpdateAvailable();
            if (updateAvailable)
            {
                Console.WriteLine("[updater] an update is available, update? say `ok` to update");
                string response = Console.ReadLine();
                if (response.Equals("ok", StringComparison.OrdinalIgnoreCase))
                {
                    Updater.Update();
                    return;
                }
            }
            else
            {
                Console.WriteLine("[updater] supposedly up to date");
            }

            //start the bot process
            bot = new Bot(ProgramInfo);
            while (true)
            {
                string command = Console.ReadLine();
                bot.Run(command);
            }
        }

        private static Info Load()
        {
            string pathToTokenFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "info.json");
            if (File.Exists(pathToTokenFile))
            {
                string contents = File.ReadAllText(pathToTokenFile);
                try
                {
                    Info info = JsonConvert.DeserializeObject<Info>(contents);
                    return info;
                }
                catch
                {
                    Console.WriteLine("[program] couldnt parse info.json correctly");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("[program] info.json file not present beside the executable");
                return null;
            }
        }
    }
}