using Discord;
using System;
using System.Linq;

namespace Lelebot.Commands
{
    public class ChangeNickname : Command
    {
        public override bool TriggerTyping => false;
        public override string[] Names => new string[] { "nick" };
        public override string Description => "Changes someone elses nickname on the server";
        public override string Usage => "nick @kuku = bobunga";

        public override bool Match(Context context)
        {
            if (context.Message == null)
            {
                return false;
            }

            if (context.Message.MentionedUsers.Count != 1)
            {
                return false;
            }

            if (context.Text.IndexOf(" = ") == -1)
            {
                return false;
            }

            return context.Text.StartsWith("nick ", StringComparison.OrdinalIgnoreCase);
        }

        public override async void Run(Context context)
        {
            if (context.Message != null)
            {
                if (context.Message.MentionedUsers.Count > 0)
                {
                    IGuildUser user = context.Message.MentionedUsers.ToArray()[0] as IGuildUser;
                    if (user != null)
                    {
                        try
                        {
                            int index = context.Text.IndexOf(" = ");
                            string desiredNickname = context.Text.Substring(index + 2);
                            await user.ModifyAsync(x => x.Nickname = desiredNickname);
                        }
                        catch
                        {
                            SendText(context, "couldnt change the nick");
                        }
                    }
                }
            }
        }
    }
}
