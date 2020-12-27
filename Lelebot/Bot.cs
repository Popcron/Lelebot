using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Bot
    {
        private DiscordSocketClient client;

        public Bot()
        {
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
                await client.LoginAsync(TokenType.Bot, Info.Token);
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
            if (discordMessage.Author.Id != Info.ClientID)
            {
                Call call = Parser.Build(discordMessage);
                Log.User(discordMessage.Author.Username, discordMessage.Content);
                await RunTask(call);
            }
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
                    try
                    {
                        Message message = await command.Run(call);
                        if (message is not null)
                        {
                            if (call.Origin == MessageOrigin.Console)
                            {
                                Log.CommandResult(message.Text);
                            }
                            else
                            {
                                await call.DiscordMessage.Channel.SendMessageAsync(message.Text);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                    }
                }
            }
        }
    }
}