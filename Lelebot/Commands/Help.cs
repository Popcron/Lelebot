using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class Help : IBasicCommand
    {
        string IBasicCommand.Name => "help";

        async Task<Message> ICommand.Run(Call call)
        {
            Message message = new();
            List<ICommand> commands = Library.GetCommands(call);
            List<(IHelp help, IBasicCommand basic)> validCommands = new List<(IHelp help, IBasicCommand basic)>();
            foreach (ICommand command in commands)
            {
                if (command is IHelp help && command is IBasicCommand basic)
                {
                    validCommands.Add((help, basic));
                }
            }

            for (int i = 0; i < validCommands.Count; i++)
            {
                (IHelp help, IBasicCommand basic) = validCommands[i];
                message.Append(basic.Name);
                message.Append(" = ");
                message.Append(help.Help);

                if (i < validCommands.Count - 1)
                {
                    message.AppendLine();
                }
            }

            await Task.CompletedTask;
            return message;
        }
    }
}
