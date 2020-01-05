using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Updater
    {
        private static GitHubClient client;

        static Updater()
        {
            ProductHeaderValue productInformation = new ProductHeaderValue("Lelebot");
            client = new GitHubClient(productInformation);
        }

        /// <summary>
        /// Checks for an update by polling the github repo.
        /// </summary>
        public static async Task<bool> IsUpdateAvailable()
        {
            Console.WriteLine("[updater] checking for updates");

            try
            {
                IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("popcron", "lelebot");
                Release latest = releases.OrderBy(x => x.CreatedAt).First();
                Console.WriteLine($"[debug] latest release is {latest.Name}");
            }
            catch
            {

            }

            return false;
        }

        /// <summary>
        /// Downloads latest build.
        /// </summary>
        public static void Update()
        {
            Console.WriteLine("[updater] thats it, gonna update");

            //rename the original running exe
            string pathToExe = Assembly.GetEntryAssembly().Location;
            string newPath = pathToExe.Replace(".exe", ".exe.old");
            File.Move(pathToExe, newPath);
        }
    }
}
