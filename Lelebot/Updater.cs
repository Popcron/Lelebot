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

        public static async Task<bool> DoesRepositoryExist()
        {
            //check if repo still exists
            try
            {
                Repository repo = await client.Repository.Get("popcron", "lelebot");
                return repo != null;
            }
            catch (Exception exception)
            {
            }

            return false;
        }

        /// <summary>
        /// Checks for an update by polling the github repo.
        /// </summary>
        public static async Task<bool> IsUpdateAvailable()
        {
            Console.WriteLine("[updater] checking for updates");

            bool exists = await DoesRepositoryExist();
            if (exists)
            {
                IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("popcron", "lelebot");
                Release latest = releases.OrderBy(x => x.CreatedAt).First();
                Console.WriteLine($"[debug] latest release is {latest.Name}");
            }

            Console.WriteLine("[updater] up to date yo");
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
