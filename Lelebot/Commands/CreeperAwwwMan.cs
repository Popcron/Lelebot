using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class CreeperAwwwMan : ICommand
    {
        bool ICommand.ShouldRun(Call call) => call.RawText.Contains("creeper");

        async Task<Message> ICommand.Run(Call call)
        {
            await Task.Delay(2000);
            Message message = new();
            message.Append("aawwww maannnn");
            return message;
        }
    }
}
