using Discord.WebSocket;
using System;

namespace Lelebot
{
    public interface IProcessor
    {
        public virtual void OnReady()
        {

        }

        public virtual void OnLoggedOut()
        {

        }

        public virtual void OnDisconnected(Exception exception)
        {

        }

        public virtual void OnConnected()
        {

        }

        public virtual void OnLoggedIn()
        {

        }

        public virtual void OnMessageReceived(SocketMessage discordMessage, Call call)
        {

        }

        public virtual void OnInitialized(Bot bot)
        {

        }
    }
}