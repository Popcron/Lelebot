using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class DiceRoll : ICommand, IHelp
    {
        string IHelp.Help => "Rolls a dice with the amount of faces provided";

        bool ICommand.ShouldRun(Call call)
        {
            if (call.BaseCommand.Equals("roll", StringComparison.OrdinalIgnoreCase))
            {
                if (call.Args.Length == 1)
                {
                    if (int.TryParse(call.Args[0], out int max))
                    {
                        return max > 1;
                    }
                }
            }

            return false;
        }

        async Task<Message> ICommand.Run(Call call)
        {
            if (call.DiscordMessage is not null)
            {
                await call.DiscordMessage.Channel.TriggerTypingAsync();
                await Task.Delay(1400);
            }

            Message message = new();
            int max = int.Parse(call.Args[0]);
            int result = new Random().Next(max) + 1;
            message.Append(result);
            return message;
        }
    }
}
