using Discord;
using Discord.WebSocket;
using System;
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
            client = new DiscordSocketClient();
            client.Connected += OnConnected;
            client.Disconnected += OnDisconnected;
            client.LoggedIn += OnLoggedIn;
            client.LoggedOut += OnLoggedOut;
            client.Ready += OnReady;
            client.MessageReceived += OnMessageReceived;

            try
            {
                await client.LoginAsync(TokenType.Bot, info.Token);
                await client.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private Task OnReady()
        {
            Console.WriteLine("Ready");
            return Task.CompletedTask;
        }

        private Task OnLoggedOut()
        {
            Console.WriteLine("Logged Out");
            return Task.CompletedTask;
        }

        private Task OnDisconnected(Exception arg)
        {
            Console.WriteLine("Disconnected");
            return Task.CompletedTask;
        }

        private Task OnConnected()
        {
            Console.WriteLine("Connected");
            return Task.CompletedTask;
        }

        private Task OnLoggedIn()
        {
            Console.WriteLine("Logged In");
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