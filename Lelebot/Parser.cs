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

        public static Call Build(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                string baseCommand = command;
                string[] args = new string[] { };
                if (command.IndexOf(' ') != -1)
                {
                    args = command.Split(' ');
                }

                Call call = new(baseCommand, args);
                call.Origin = Origin.Console;
                return call;
            }

            return default;
        }
    }
}