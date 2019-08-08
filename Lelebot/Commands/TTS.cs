using System;

namespace Lelebot.Commands
{
    public class TTS : Command
    {
        public override bool TriggerTyping => false;

        public override bool Match(Context ctx)
        {
            if (ctx.Command.ToLower() == "say" && ctx.Text.Length >= 5)
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
