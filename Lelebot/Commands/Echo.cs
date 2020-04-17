using System;

namespace Lelebot.Commands
{
    public class Echo : Command
    {
        public override string[] Names => new string[] { "echo" };
        public override string Description => "Repeats whatever is said after the command.";
        public override string Usage => "`echo lorem ipsum` will print out `lorem ipsum`";
        public override bool TriggerTyping => true;

        public override bool Match(Context context)
        {
            if (context.Command.Equals("echo", StringComparison.OrdinalIgnoreCase) && context.Text?.Length >= 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Run(Context context)
        {
            string message = context.Text.Substring(5);
            SendText(context, message);
        }
    }
}
