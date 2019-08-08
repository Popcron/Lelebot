namespace Lelebot.Commands
{
    public class Echo : Command
    {
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
