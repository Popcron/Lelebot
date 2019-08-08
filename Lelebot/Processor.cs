using Discord.WebSocket;

namespace Lelebot
{
    public class Processor
    {
        public virtual void OnChannelUpdated(SocketChannel oldChannel, SocketChannel newChannel)
        {
        }

        public virtual void OnUserLeftServer(SocketGuildUser user)
        {
        }

        public virtual void OnUserJoinedServer(SocketGuildUser user)
        {
        }

        public virtual void OnMessage(SocketMessage message)
        {
        }

        public virtual void OnUserVoiceUpdated(SocketUser user, SocketVoiceState oldState, SocketVoiceState newState)
        {
        }
    }
}
