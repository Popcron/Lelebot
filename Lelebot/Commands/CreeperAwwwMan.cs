﻿using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class CreeperAwwwMan : ICommand
    {
        bool ICommand.ShouldRun(Call call) => call.RawText.Contains("creeper", StringComparison.OrdinalIgnoreCase);

        async Task<Message> ICommand.Run(Call call)
        {
            if (call.DiscordMessage is not null)
            {
                await call.DiscordMessage.Channel.TriggerTypingAsync();
                await Task.Delay(1400);
            }

            Message message = new();
            message.Append("aawwww maannnn");
            return message;
        }
    }
}
