using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class Help : ICommand
    {
        string ICommand.BaseCommand => "help";

        async Task<Message> ICommand.Run(Call call)
        {
            Message message = new();
            foreach (ICommand command in Library.Commands)
            {
                if (command is IHelp help)
                {
                    message.Append(command.BaseCommand);
                    message.Append(" = ");
                    message.Append(help.Help);
                    message.AppendLine();
                }
            }

            await Task.CompletedTask;
            return message;
        }
    }
}
