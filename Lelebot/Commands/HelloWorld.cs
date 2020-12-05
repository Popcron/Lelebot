using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class HelloWorld : ICommand, IHelp
    {
        string ICommand.BaseCommand => "test";
        string IHelp.Help => "Just prints Hello World as a test";

        async Task<Message> ICommand.Run(Call call)
        {
            Message message = new();
            message.Append($"Hello World from {call.Origin}");

            await Task.CompletedTask;
            return message;
        }
    }
}
