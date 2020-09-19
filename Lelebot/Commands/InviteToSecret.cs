using Discord;

namespace Lelebot.Commands
{
    public class InviteToSecret : Command
    {
        public override bool TriggerTyping => false;
        public override string[] Names => new string[] { "inv" };

        public override bool Match(Context context)
        {
            return context.Command == "inv";
        }

        public override async void Run(Context context)
        {
            IGuild secretServer = Bot.Client.GetGuild(754790983027130398);
            ITextChannel generalChannel = await secretServer.GetChannelAsync(754790983027130401) as ITextChannel;
            IInviteMetadata invite = await generalChannel.CreateInviteAsync(5, 1, true);
            SendText(context, invite.Url);
        }
    }
}
