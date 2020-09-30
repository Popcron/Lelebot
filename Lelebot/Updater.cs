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
            try
            {
                //check if repo still exists
                Repository repo = await client.Repository.Get("popcron", "lelebot");
                return repo != null;
            }
            catch (Exception exception)
            {
                if (exception is NotFoundException)
                {
                    Console.WriteLine("[updater] repo not found");
                }
                else
                {
                    Console.WriteLine(exception.ToString());
                }
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
                Branch branch = await client.Repository.Branch.Get("popcron", "lelebot", "master");

                IReadOnlyList<Release> releases = await client.Repository.Release.GetAll("popcron", "lelebot");
                Release latest = releases.OrderBy(x => x.CreatedAt).First();
                Console.WriteLine($"[debug] latest release is {latest.Name}");
            }

            return false;
        }

        /// <summary>
        /// Updates the bot, duh.
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
