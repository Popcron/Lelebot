using Discord;
using Discord.WebSocket;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class ViewPermissions : Command
    {
        private const string CommandName = "view perms";

        public override bool TriggerTyping => false;
        public override string[] Names => new string[] { CommandName };

        public override bool Match(Context context)
        {
            if (context.Text.StartsWith(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public override async void Run(Context context)
        {
            if (context.Message.MentionedUsers.Count > 0)
            {
                foreach (SocketUser u in context.Message.MentionedUsers)
                {
                    SocketGuildUser user = context.Guild.GetUser(u.Id);
                    GuildPermissions userPermissions = user.GuildPermissions;
                    await ShowPermissions(userPermissions, context);
                }
            }
            else
            {
                SocketGuildUser botUser = context.Guild.GetUser(Bot.Client.CurrentUser.Id);
                GuildPermissions botPermissions = botUser.GuildPermissions;
                await ShowPermissions(botPermissions, context);
            }
        }

        private async Task ShowPermissions(GuildPermissions perms, Context context)
        {
            StringBuilder builder = new StringBuilder();
            if (perms.AddReactions)
            {
                builder.AppendLine("can add reactions");
            }

            if (perms.Administrator)
            {
                builder.AppendLine("is admin");
            }

            if (perms.AttachFiles)
            {
                builder.AppendLine("can attach files");
            }

            if (perms.BanMembers)
            {
                builder.AppendLine("can ban people");
            }

            if (perms.ChangeNickname)
            {
                builder.AppendLine("is able to change their own nickname");
            }

            if (perms.Connect)
            {
                builder.AppendLine("can connect to voice channels");
            }

            if (perms.CreateInstantInvite)
            {
                builder.AppendLine("is able to create invite links");
            }

            if (perms.DeafenMembers)
            {
                builder.AppendLine("can deafen other people in voice channels");
            }

            if (perms.EmbedLinks)
            {
                builder.AppendLine("will have their links embedded");
            }

            if (perms.KickMembers)
            {
                builder.AppendLine("is able to kick people");
            }

            if (perms.ManageChannels)
            {
                builder.AppendLine("can modify voice and text channels");
            }

            if (perms.ManageEmojis)
            {
                builder.AppendLine("can add/remove emojis");
            }

            if (perms.ManageGuild)
            {
                builder.AppendLine("can adjust server settings");
            }

            if (perms.ManageMessages)
            {
                builder.AppendLine("can delete messages");
            }

            if (perms.ManageNicknames)
            {
                builder.AppendLine("allowed to manage other people's nicknames");
            }

            if (perms.ManageRoles)
            {
                builder.AppendLine("allowed to manage server roles");
            }

            if (perms.ManageWebhooks)
            {
                builder.AppendLine("is able to manage webhooks");
            }

            if (perms.MentionEveryone)
            {
                builder.AppendLine("can @everyone");
            }
            
            if (perms.MoveMembers)
            {
                builder.AppendLine("can move people from and to voice channels");
            }
            
            if (perms.MuteMembers)
            {
                builder.AppendLine("is able to mute people in voice channels");
            }

            if (perms.PrioritySpeaker)
            {
                builder.AppendLine("can have priority speaker status");
            }

            if (perms.ReadMessageHistory)
            {
                builder.AppendLine("can read the entire history");
            }

            if (perms.SendMessages)
            {
                builder.AppendLine("is able to send messages");
            }

            if (perms.SendTTSMessages)
            {
                builder.AppendLine("is able to sent text to speech messages");
            }

            if (perms.Speak)
            {
                builder.AppendLine("can speak");
            }

            if (perms.Stream)
            {
                builder.AppendLine("can stream video in voice channels");
            }

            if (perms.UseExternalEmojis)
            {
                builder.AppendLine("can use external emojis");
            }

            if (perms.UseVAD)
            {
                builder.AppendLine("isnt forced to use push to talk");
            }

            if (perms.ViewAuditLog)
            {
                builder.AppendLine("can see the server audit log");
            }

            if (perms.ViewChannel)
            {
                builder.AppendLine("is allowed to read messages");
            }

            if (builder.Length == 0)
            {
                await context.Channel.SendMessageAsync("no permissions");
            }
            else
            {
                builder.Insert(0, "```\n");
                builder.Append("```");
                await context.Channel.SendMessageAsync(builder.ToString());
            }
        }
    }
}
