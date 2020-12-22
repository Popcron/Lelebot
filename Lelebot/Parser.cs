using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

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
                    call.Origin = MessageOrigin.Server;
                }
                else
                {
                    call.Origin = MessageOrigin.PrivateDM;
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
                    List<string> listArgs = text.Split(' ').ToList();
                    if (listArgs.Count > 1)
                    {
                        baseCommand = listArgs[0];
                        listArgs.RemoveAt(0);
                        args = listArgs.ToArray();
                    }
                }

                Call call = new(text, baseCommand, args);
                call.Origin = MessageOrigin.Console;
                return call;
            }

            return default;
        }
    }
}