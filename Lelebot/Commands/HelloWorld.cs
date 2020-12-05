using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class HelloWorld : ICommand, IHelp
    {
        string ICommand.BaseCommand => "test";
        string IHelp.Help => "Just prints Hello World as a test";

        Task ICommand.Run()
        {
            Console.WriteLine("Hello World");
            return Task.CompletedTask;
        }
    }
}
