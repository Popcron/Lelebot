using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Bot
    {
        private Info info;
        private DiscordSocketClient client;

        public Bot(Info info)
        {
            this.info = info;
            Initialize();
        }

        private async void Initialize()
        {
            DiscordSocketConfig config = new()
            {
                LogLevel = LogSeverity.Verbose
            };

            client = new DiscordSocketClient(config);
            client.Connected += Client_Connected;
            client.Disconnected += Client_Disconnected;
            client.LoggedIn += Client_LoggedIn;
            client.LoggedOut += Client_LoggedOut;
            client.Ready += Client_Ready;
            client.MessageReceived += OnMessageReceived;

            await client.LoginAsync(TokenType.Bot, info.Token);
            await client.StartAsync();
        }

        private Task Client_Ready()
        {
            return Task.CompletedTask;
        }

        private Task Client_LoggedOut()
        {
            return Task.CompletedTask;
        }

        private Task Client_Disconnected(System.Exception arg)
        {
            return Task.CompletedTask;
        }

        private Task Client_Connected()
        {
            return Task.CompletedTask;
        }

        private Task Client_LoggedIn()
        {
            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(SocketMessage arg)
        {
            await RunTask(arg.Content);
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
                    await command.Run(call.Value.Args);
                }
            }
        }
    }
}