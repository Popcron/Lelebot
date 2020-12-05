using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class Help : ICommand
    {
        string ICommand.BaseCommand => "help";

        Task ICommand.Run(params string[] args)
        {
            foreach (ICommand command in Library.Commands)
            {
                if (command is IHelp help)
                {
                    Console.WriteLine($"{command.BaseCommand} = {help.Help}");
                }
            }

            return Task.CompletedTask;
        }
    }
}
