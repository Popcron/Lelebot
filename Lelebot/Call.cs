using Discord;

namespace Lelebot
{
    public class Call
    {
        public string BaseCommand { get; }
        public string[] Args { get; }
        public string RawText { get; }
        public Origin Origin { get; set; }
        public IMessage DiscordMessage { get; set; }

        public Call(string rawText, string baseCommand, params string[] args)
        {
            BaseCommand = baseCommand;
            RawText = rawText;
            Args = args ?? new string[] { };
        }
    }
}