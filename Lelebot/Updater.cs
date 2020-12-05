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

        private bool shouldUpdateSelf;

        public Updater(bool shouldUpdateSelf)
        {
            this.shouldUpdateSelf = shouldUpdateSelf;
            ProductHeaderValue productInformation = new ProductHeaderValue("discord-lelebot");
            client = new GitHubClient(productInformation);
            client.Credentials = new Credentials(Program.Info.githubToken, AuthenticationType.Bearer);
            CleanArtifacts();
        }

        public async Task<bool> DoesRepositoryExist()
        {
            try
            {
                //check if repo still exists
                string owner = Program.Info.repoOwner;
                string repo = Program.Info.repoName;
                Console.WriteLine($"[updater] checking at https://github.com/{owner}/{repo}");
                return (await client.Repository.Get(owner, repo)) != null;
            }
            catch (Exception exception)
            {
                if (exception is NotFoundException)
                {
                    Console.WriteLine("[updater] repo not found");
                }
                else if (exception is AuthorizationException)
                {
                    Console.WriteLine("[updater] bad credentials, check your token");
                }
                else
                {
                    Console.WriteLine(exception.ToString());
                }
            }

            return false;
        }

        /// <summary>
        /// Returns the current version of the bot.
        /// </summary>
        public int GetLocalVersion()
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string versionFile = Path.Combine(dir, Program.Info.pathToVersionFile);
            if (File.Exists(versionFile))
            {
                string text = File.ReadAllText(versionFile);
                return int.Parse(text);
            }

            return -1;
        }

        /// <summary>
        /// Gets the version that is on the remote website.
        /// </summary>
        public async Task<int> GetLiveVersion()
        {
            string owner = Program.Info.repoOwner;
            string repo = Program.Info.repoName;
            string pathToVersionFile = Program.Info.pathToVersionFile;
            byte[] infoContent = await client.Repository.Content.GetRawContent(owner, repo, pathToVersionFile);
            string[] infoLines = Encoding.UTF8.GetString(infoContent).Split('\n');
            return int.Parse(infoLines[0]);
        }

        /// <summary>
        /// Checks for an update by polling the github repo.
        /// </summary>
        public async Task<bool> IsUpdateAvailable()
        {
            if (!shouldUpdateSelf)
            {
                return false;
            }

            Console.WriteLine("[updater] checking for updates");

            bool exists = await DoesRepositoryExist();
            if (exists)
            {
                int localVersion = GetLocalVersion();
                int liveVersion = await GetLiveVersion();
                if (localVersion < liveVersion)
                {
                    return true;
                }
            }
            else
            {
                Console.WriteLine("[updater] repository does not exist");
            }

            return false;
        }

        /// <summary>
        /// Clears old update files.
        /// </summary>
        public void CleanArtifacts()
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
        public async Task Update()
        {
            CleanArtifacts();
            Console.WriteLine("[updater] thats it, im gonna update");

            //rename the currently running exe
            string pathToExe = Assembly.GetEntryAssembly().Location;
            string newPath = pathToExe.Replace(".exe", ".exe.old");
            File.Move(pathToExe, newPath);

            //download the exe from the repo
            string owner = Program.Info.repoOwner;
            string repo = Program.Info.repoName;
            Release latestRelease = await client.Repository.Release.GetLatest(owner, repo);
            if (latestRelease != null)
            {
                WebClient downloadClient = new WebClient();
                foreach (ReleaseAsset asset in latestRelease.Assets)
                {
                    if (asset.Name.Equals("lelebot.exe", StringComparison.OrdinalIgnoreCase))
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
