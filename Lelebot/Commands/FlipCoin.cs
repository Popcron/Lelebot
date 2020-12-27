using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class FlipCoin : IBasicCommand, IHelp
    {
        string IBasicCommand.Name => "flipcoin";
        string IHelp.Help => "Flips a coin";

        async Task<Message> ICommand.Run(Call call)
        {
            if (call.DiscordMessage is not null)
            {
                await call.DiscordMessage.Channel.TriggerTypingAsync();
                await Task.Delay(1400);
            }

            bool tails = new Random().Next(100) >= 50;
            Message message = new();
            message.Append(tails ? "tails" : "heads");
            return message;
        }
    }
}
