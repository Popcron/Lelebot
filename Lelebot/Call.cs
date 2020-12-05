using Discord;

namespace Lelebot
{
    public class Call
    {
        public string BaseCommand { get; }
        public string[] Args { get; }
        public Origin Origin { get; set; }
        public IMessage DiscordMessage { get; set; }

        public Call(string baseCommand, params string[] args)
        {
            BaseCommand = baseCommand;
            Args = args ?? new string[] { };
        }
    }
}