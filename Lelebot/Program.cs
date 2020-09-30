using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Program
    {
        /// <summary>
        /// The metadata info that this bot started with.
        /// </summary>
        public static Info ProgramInfo { get; private set; } = null;

        private static Bot bot;

        public static async Task Main(string[] args)
        {
            Console.WriteLine("[main] starting");
            Console.WriteLine($"[main] version: {Info.Version}");

            //load the info
            ProgramInfo = Load();
            if (ProgramInfo == null)
            {
                Console.WriteLine("[main] no program info found, press any key to close");
                Console.ReadKey();
                return;
            }

            //check for updates first
            bool updateAvailable = await Updater.IsUpdateAvailable();
            if (updateAvailable)
            {
                await Updater.Update();
                return;
            }
            else
            {
                Console.WriteLine("[updater] supposedly up to date");
            }

            //start the bot process
            Console.WriteLine("[main] ok go");
            bot = new Bot(ProgramInfo);
            while (true)
            {
                string command = Console.ReadLine();
                bot.Run(command);
            }
        }

        /// <summary>
        /// Ensures that the info json template exists.
        /// </summary>
        private static void EnsureTemplateExists()
        {
            Info template = new Info();
            string fileName = "info.json.template";
            string pathToTemplate = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            string json = JsonConvert.SerializeObject(template, Formatting.Indented);
            File.WriteAllText(pathToTemplate, json);
        }

        /// <summary>
        /// Loads the info.json file into the bot.
        /// </summary>
        private static Info Load()
        {
            EnsureTemplateExists();

            string fileName = "info.json";
            string pathToTokenFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
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
                    Console.WriteLine($"[main] couldnt parse {fileName} correctly");
                    return null;
                }
            }
            else
            {
                Console.WriteLine($"[main] {fileName} file not present beside the executable");
                return null;
            }
        }
    }
}