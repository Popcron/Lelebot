using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class FairDiceRoll : ICommand
    {
        bool ICommand.ShouldRun(Call call)
        {
            return call.RawText.Equals("fair dice roll", StringComparison.OrdinalIgnoreCase);
        }

        async Task<Message> ICommand.Run(Call call)
        {
            if (call.DiscordMessage is not null)
            {
                await call.DiscordMessage.Channel.TriggerTypingAsync();
                await Task.Delay(1400);
            }

            Message message = new();
            message.Append("4");
            return message;
        }
    }
}
