using Discord.WebSocket;
using System.Collections.Generic;
using System.Text;

namespace Lelebot.Commands
{
    public class ListJoinedServers : Command
    {
        public override bool TriggerTyping => false;
        public override string[] Names => new string[] { "ls servers" };

        public override bool Match(Context context)
        {
            return context.Text == "ls servers";
        }

        public override void Run(Context context)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("```");

            IReadOnlyCollection<SocketGuild> guilds = Bot.Client.Guilds;
            if (guilds != null)
            {
                foreach (SocketGuild guild in guilds)
                {
                    builder.Append(guild.Id);
                    builder.Append(" = ");
                    builder.AppendLine(guild.Name);
                }
            }

            builder.Append("```");
            SendText(context, builder.ToString());
        }
    }
}
