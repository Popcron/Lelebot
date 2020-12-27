using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Lelebot
{
    public class Bot
    {
        public DiscordSocketClient Client { get; }

        public Bot()
        {
            Client = new DiscordSocketClient();
            Client.Connected += OnConnected;
            Client.Disconnected += OnDisconnected;
            Client.LoggedIn += OnLoggedIn;
            Client.LoggedOut += OnLoggedOut;
            Client.Ready += OnReady;
            Client.MessageReceived += OnMessageReceived;

            Initialize();
        }

        private async void Initialize()
        {
            Log.DiscordEvent("Initialize");

            foreach (IProcessor processor in Library.AllProcessors)
            {
                processor.OnInitialized(this);
            }

            try
            {
                await Client.LoginAsync(TokenType.Bot, Info.Token);
                await Client.StartAsync();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
        }

        private Task OnReady()
        {
            Log.DiscordEvent("Ready");

            foreach (IProcessor processor in Library.AllProcessors)
            {
                processor.OnReady();
            }

            return Task.CompletedTask;
        }

        private Task OnLoggedOut()
        {
            Log.DiscordEvent("Logged Out");

            foreach (IProcessor processor in Library.AllProcessors)
            {
                processor.OnLoggedOut();
            }

            return Task.CompletedTask;
        }

        private Task OnDisconnected(Exception exception)
        {
            Log.DiscordEvent("Disconnected");

            foreach (IProcessor processor in Library.AllProcessors)
            {
                processor.OnDisconnected(exception);
            }

            return Task.CompletedTask;
        }

        private Task OnConnected()
        {
            Log.DiscordEvent("Connected");

            foreach (IProcessor processor in Library.AllProcessors)
            {
                processor.OnConnected();
            }

            return Task.CompletedTask;
        }

        private Task OnLoggedIn()
        {
            Log.DiscordEvent("Logged In");

            foreach (IProcessor processor in Library.AllProcessors)
            {
                processor.OnLoggedIn();
            }

            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(SocketMessage discordMessage)
        {
            if (discordMessage.Author.Id != Info.ClientID)
            {
                Call call = Parser.Build(discordMessage);
                Log.User(discordMessage.Author.Username, discordMessage.Content);

                foreach (IProcessor processor in Library.AllProcessors)
                {
                    processor.OnMessageReceived(discordMessage, call);
                }

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