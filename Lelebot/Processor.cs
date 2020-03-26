﻿using Discord.WebSocket;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Processor
    {
        /// <summary>
        /// The bot that is using this processor.
        /// </summary>
        public Bot Bot { get; set; }

        public virtual async Task OnChannelUpdated(SocketChannel oldChannel, SocketChannel newChannel)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnUserLeftServer(SocketGuildUser user)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnUserJoinedServer(SocketGuildUser user)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnMessage(SocketMessage message)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnUserVoiceUpdated(SocketUser user, SocketVoiceState oldState, SocketVoiceState newState)
        {
            await Task.CompletedTask;
        }
    }
}
