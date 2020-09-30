using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Processor
    {
        public virtual void OnCreated(Bot bot)
        {

        }

        public string GetLocalFilePath(string filePath)
        {
            string processorsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Processors", GetType().Name);
            return Path.Combine(processorsFolder, filePath);
        }

        public virtual async Task OnChannelUpdated(SocketChannel oldChannel, SocketChannel newChannel)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnUserBanned(SocketUser user, SocketGuild guild, bool isBanned)
        {
            await Task.CompletedTask;
        }

        public virtual async Task OnGuildUpdated(SocketGuild before, SocketGuild after)
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
