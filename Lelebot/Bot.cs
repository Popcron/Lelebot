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
                Log.Error(e.Message);
            }
        }

        private Task OnReady()
        {
            Log.DiscordEvent("Ready");
            return Task.CompletedTask;
        }

        private Task OnLoggedOut()
        {
            Log.DiscordEvent("Logged Out");
            return Task.CompletedTask;
        }

        private Task OnDisconnected(Exception arg)
        {
            Log.DiscordEvent("Disconnected");
            return Task.CompletedTask;
        }

        private Task OnConnected()
        {
            Log.DiscordEvent("Connected");
            return Task.CompletedTask;
        }

        private Task OnLoggedIn()
        {
            Log.DiscordEvent("Logged In");
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
                ICommand command = Library.Get(call);
                if (command is not null)
                {
                    Message message = await command.Run(call);
                    if (message is not null)
                    {
                        Console.WriteLine($"[debug] {call.Origin}");
                        if (call.Origin == Origin.Console)
                        {
                            Log.User(message.Text);
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