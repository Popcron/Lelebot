namespace Lelebot.Commands
{
    public class Echo : Command
    {
        public override bool Match(Context context)
        {
            if (context.Command == "echo")
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
            Print(Context.Text);
        }
    }
}
