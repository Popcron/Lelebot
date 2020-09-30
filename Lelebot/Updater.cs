using Octokit;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
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
        /// Get live version.
        /// </summary>
        public static async Task<int> GetLiveVersion()
        {
            byte[] infoContent = await client.Repository.Content.GetRawContent("popcron", "lelebot", "Lelebot/Info.cs");
            string[] infoLines = Encoding.UTF8.GetString(infoContent).Split('\n');
            string targetLine = "public const uint Version = ";
            int versionNumber = -1;
            foreach (string line in infoLines)
            {
                int versionConstantIndex = line.IndexOf(targetLine);
                if (versionConstantIndex != -1)
                {
                    string versionText = line.Substring(versionConstantIndex + targetLine.Length);
                    versionText = versionText.TrimStart(' ');
                    versionText = versionText.TrimEnd(' ');
                    versionText = versionText.TrimEnd(';');

                    versionNumber = int.Parse(versionText);
                    break;
                }
            }

            return versionNumber;
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
                Release latestRelease = await client.Repository.Release.GetLatest("popcron", "lelebot");
                if (latestRelease != null)
                {
                    string releaseName = latestRelease.Name;
                    if (releaseName.IndexOf(" ") != -1)
                    {
                        string versionNumberText = releaseName.Split(' ')[1];
                        if (int.TryParse(versionNumberText, out int versionNumber))
                        {
                            Console.WriteLine($"[updater] latest available version is {versionNumber}");
                            return versionNumber > Info.Version;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("[updater] repository does not exist");
            }

            return false;
        }

        public static void CleanArtifacts()
        {
            string pathToExe = Assembly.GetEntryAssembly().Location;
            string oldPath = pathToExe.Replace(".exe", ".exe.old");
            if (File.Exists(oldPath))
            {
                File.Delete(oldPath);
            }
        }

        /// <summary>
        /// Updates the bot, duh.
        /// </summary>
        public static async Task Update()
        {
            Console.WriteLine("[updater] thats it, im gonna update");

            //rename the currently running exe
            string pathToExe = Assembly.GetEntryAssembly().Location;
            string newPath = pathToExe.Replace(".exe", ".exe.old");
            File.Move(pathToExe, newPath);

            //download the exe from the repo
            Release latestRelease = await client.Repository.Release.GetLatest("popcron", "lelebot");
            if (latestRelease != null)
            {
                WebClient downloadClient = new WebClient();
                foreach (ReleaseAsset asset in latestRelease.Assets)
                {
                    if (asset.Name == "Lelebot.exe")
                    {
                        await downloadClient.DownloadFileTaskAsync(asset.BrowserDownloadUrl, pathToExe);
                        break;
                    }
                }
            }

            //start the newly updated bot
            ProcessStartInfo startInfo = new ProcessStartInfo(pathToExe);
            Process.Start(startInfo);
        }
    }
}
