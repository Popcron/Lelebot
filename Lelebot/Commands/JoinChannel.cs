using Discord.WebSocket;
using System;
using System.Collections.Generic;

namespace Lelebot.Commands
{
    public class JoinChannel : Command
    {
        public override string[] Names => new string[] { "join" };
        public override string Description => "Asks the bot to join a voice channel.";
        public override string Usage => "`join me` will make the bot join a voice channel that the message author is in.\nTyping `join <channelId>` also works";
        public override bool TriggerTyping => false;

        public override bool Match(Context context)
        {
            if (context.Command.Equals("join", StringComparison.OrdinalIgnoreCase) && context.Args?.Length == 1)
            {
                return context.Args[0].Equals("me", StringComparison.OrdinalIgnoreCase) || ulong.TryParse(context.Args[0], out _);
            }
            else
            {
                return false;
            }
        }

        public override async void Run()
        {
            if (Context.Channel != null)
            {
                SocketGuildChannel textChannel = Context.Channel as SocketGuildChannel;
                SocketGuild guild = textChannel.Guild;

                Context.DeleteMessage();

                ulong id = 0;
                if (Context.Args[0] == "me")
                {
                    foreach (SocketVoiceChannel channel in guild.VoiceChannels)
                    {
                        foreach (SocketGuildUser user in channel.Users)
                        {
                            if (user == Context.Author)
                            {
                                id = channel.Id;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    id = ulong.Parse(Context.Args[0]);
                }

                if (id != 0)
                {
                    SocketVoiceChannel voiceChannel = guild.GetVoiceChannel(id);
                    Bot.AudioClient = await voiceChannel.ConnectAsync();
                }
            }
        }
    }
}
