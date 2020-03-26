using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class DownloadYoutube : Command
    {
        public override string[] Names => new string[] { "get" };
        public override string Description => "Returns download links for audio and video sources of a youtube video.";
        public override string Usage => "Write the word `get` then the youtube url separated by a space, like so: `get youtu.be/v=s1ty18h31`";

        public override bool Match(Context context)
        {
            if (context.Command == "get")
            {
                if (context.Args.Length == 1)
                {
                    if (context.Args[0].Contains("youtu"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override async void Run()
        {
            await Task.CompletedTask;
        }
    }
}
