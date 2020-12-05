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

        private async Task OnMessageReceived(SocketMessage discordMessage)
        {
            Call call = Parser.Build(discordMessage);
            await RunTask(call);
        }

        public async Task RunTask(string text)
        {
            Call call = Parser.Build(text);
            await RunTask(call);
        }

        private async Task RunTask(Call call)
        {
            if (call is not null)
            {
                ICommand command = Library.Get(call.BaseCommand);
                if (command is not null)
                {
                    Message message = await command.Run(call);
                    if (message is not null)
                    {
                        if (call.Origin == Origin.Console)
                        {
                            Console.WriteLine(message.Text);
                        }
                        else
                        {
                            await call.DiscordMessage.Channel.SendMessageAsync(message.Text);
                        }
                    }
                }
            }
        }
    }
}