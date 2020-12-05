using System.Threading.Tasks;

namespace Lelebot
{
    public class Bot
    {
        private Info info;

        public Bot(Info info)
        {
            this.info = info;
        }

        public async void Run(string text) => await RunTask(text);

        public async Task RunTask(string text)
        {
            Call? call = Parser.Build(text);
            if (call != null)
            {
                ICommand command = Library.Get(call.Value.BaseCommand);
                if (command != null)
                {
                    await command.Run();
                }
            }
        }
    }
}