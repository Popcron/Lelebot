using System;

namespace Lelebot.Commands
{
    public class TTS : Command
    {
        public override string[] Names => new string[] { "say" };
        public override string Description => "Makes the bot say something in the channel that you're in.";
        public override string Usage => "`say creeper` will make the bot say creeper in a voice channel.";
        public override bool TriggerTyping => false;

        public override bool Match(Context ctx)
        {
            if (ctx.Command.Equals("say", StringComparison.OrdinalIgnoreCase) && ctx.Text?.Length >= 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override async void Run()
        {
            string text = Context.Text.Substring(4);
            await Bot.Say(Context, text);
        }
    }
}
