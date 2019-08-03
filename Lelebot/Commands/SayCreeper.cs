using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class SayCreeper : Command
    {
        public override bool Match(Context context)
        {
            if (context.Command == "creeper")
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
            await Task.Delay(1000);
            Print("awwwww man");
        }
    }
}
