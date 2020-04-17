using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class SayCreeper : Command
    {
        public override string[] Names => new string[] { "creeper" };
        public override string Description => "awwwwww man";
        public override bool TriggerTyping => true;

        public override bool Match(Context context)
        {
            //purposelly check the entire message regardless of context
            //to be as annoying as possible
            if (context.Text.IndexOf("creeper", StringComparison.OrdinalIgnoreCase) != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override async void Run()
        {
            //after 1 second, run this
            await Task.Delay(1000);
            Print("awwwww man");
        }
    }
}
