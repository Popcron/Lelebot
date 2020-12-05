using Discord;
using Discord.WebSocket;

namespace Lelebot
{
    public class Parser
    {
        public static Call Build(IMessage message)
        {
            Call call = Build(message.Content);
            if (message is SocketMessage socketMessage)
            {
                call.DiscordMessage = message;
                if (socketMessage.Channel is IGuildChannel)
                {
                    call.Origin = Origin.Server;
                }
                else
                {
                    call.Origin = Origin.PrivateDM;
                }
            }

            return call;
        }

        public static Call Build(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                string baseCommand = text;
                string[] args = new string[] { };
                if (text.IndexOf(' ') != -1)
                {
                    args = text.Split(' ');
                }

                Call call = new(text, baseCommand, args);
                call.Origin = Origin.Console;
                return call;
            }

            return default;
        }
    }
}