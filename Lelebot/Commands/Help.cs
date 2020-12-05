using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class Help : IBaseCommand
    {
        string IBaseCommand.BaseCommand => "help";

        async Task<Message> ICommand.Run(Call call)
        {
            Message message = new();
            foreach (ICommand command in Library.Commands)
            {
                if (command is IHelp help && command is IBaseCommand baseCommand)
                {
                    message.Append(baseCommand.BaseCommand);
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
