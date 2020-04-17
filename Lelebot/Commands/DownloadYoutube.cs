using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class DownloadYoutube : Command
    {
        public override string[] Names => new string[] { "mp3" };
        public override string Description => "Returns download links for audio and video sources of a youtube video.";
        public override string Usage => "Write the word `mp3` then the youtube url separated by a space, like so: `mp3 youtu.be/v=s1ty18h31`";
        public override bool TriggerTyping => true;

        public override bool Match(Context context)
        {
            if (context.Command.Equals("mp3", StringComparison.OrdinalIgnoreCase))
            {
                if (context.Args?.Length == 1)
                {
                    if (context.Args[0].IndexOf("youtu", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override async void Run(Context context)
        {
            await Task.CompletedTask;
        }
    }
}
