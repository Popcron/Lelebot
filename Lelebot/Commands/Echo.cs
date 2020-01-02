namespace Lelebot.Commands
{
    public class Echo : Command
    {
        public override string[] Names => new string[] { "echo" };
        public override string Description => "Repeats whatever is said after the command.";
        public override string Usage => "`echo lorem ipsum` will print out `lorem ipsum`";

        public override bool Match(Context context)
        {
            if (context.Command == "echo" && context.Text.Length >= 6)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Run()
        {
            Print(Context.Text.Substring(5));
        }
    }
}
