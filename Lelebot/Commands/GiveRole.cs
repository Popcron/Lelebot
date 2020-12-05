using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;

namespace Lelebot.Commands
{
    public class GiveRole : Command
    {
        private const string CommandName = "get role";

        public override bool TriggerTyping => false;
        public override string[] Names => new string[] { CommandName };

        public override bool Match(Context context)
        {
            if (context.Text.StartsWith(CommandName, StringComparison.OrdinalIgnoreCase))
            {
                if (context.Text.Length > CommandName.Length + 1)
                {
                    return true;
                }
            }

            return false;
        }

        public override async void Run(Context context)
        {
            //check if bot has permissions to modify roles
            SocketGuildUser botUser = context.Guild.GetUser(Bot.Client.CurrentUser.Id);
            GuildPermissions permissions = botUser.GuildPermissions;
            if (!permissions.ManageRoles)
            {
                IEmote emote = Emote.Parse(":no_entry_sign:");
                await context.Message.AddReactionAsync(emote);
                return;
            }

            string roleName = context.Text.Substring(CommandName.Length + 1);
            SocketGuild guild = context.Guild;
            IReadOnlyCollection<SocketRole> roles = guild.Roles;
            foreach (SocketRole role in roles)
            {
                if (role.Name.Equals(roleName))
                {
                    SocketGuildUser guildUser = guild.GetUser(context.Author.Id);
                    await guildUser.AddRolesAsync(new IRole[] { role });

                    //role was found, give a checkmark to signify that
                    IEmote checkmark = Emote.Parse(":white_check_mark:");
                    await context.Message.AddReactionAsync(checkmark);
                    return;
                }
            }

            //role was not found, add an X reaction to signify that
            IEmote bad = Emote.Parse(":x:");
            await context.Message.AddReactionAsync(bad);
        }
    }
}
