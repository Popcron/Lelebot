using System;

namespace Lelebot.Commands
{
    public class AskToUpdate : Command
    {
        private const string CommandName = "update";

        public override bool TriggerTyping => false;
        public override string[] Names => new string[] { CommandName };

        public override bool Match(Context context)
        {
            if (context.Text.StartsWith(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                if (context.Text.Length > CommandName.Length + 1)
                {
                    return true;
                }
            }

            return false;
        }

        public override async void Run(Context context)
        {
            bool updateAvailable = await Updater.IsUpdateAvailable();
            if (updateAvailable)
            {
                await Updater.Update();
                return;
            }
            else
            {
                Console.WriteLine("[updater] supposedly up to date");
            }
        }
    }
}
