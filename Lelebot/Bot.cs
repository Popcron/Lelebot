using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Lelebot
{
    public class Bot
    {
        public bool CancellationRequested { get; private set; }

        private DiscordSocketClient client;

        public Bot(string token)
        {
            client = new DiscordSocketClient();
            client.Connected += OnConnected;
            client.Disconnected += OnDisconnected;
            client.LoggedIn += OnLoggedIn;
            client.LoggedOut += OnLoggedOut;
            client.Ready += OnReady;
            client.MessageReceived += OnMessage;

            Start(token);
        }

        private async void Start(string token)
        {
            try
            {
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine("[error] " + exception.Message.ToLower());
            }
        }

        private async Task OnReady()
        {
            Console.WriteLine("[bot] ready");
        }

        private async Task OnLoggedOut()
        {
            Console.WriteLine("[bot] logged out");
        }

        private async Task OnLoggedIn()
        {
            Console.WriteLine("[bot] logged in");
        }

        private async Task OnDisconnected(Exception arg)
        {
            Console.WriteLine("[bot] disconnected");
        }

        private async Task OnConnected()
        {
            Console.WriteLine("[bot] connected");
        }

        private async Task OnMessage(SocketMessage message)
        {
            if (TryGetContext(message.Content, out Context context))
            {
                if (Command.TryGet(context, out Command command))
                {
                    await message.Channel.TriggerTypingAsync();

                    context.Author = message.Author;
                    context.Channel = message.Channel;

                    command.Context = context;
                    command.Run();
                }
            }
        }

        public void Run(string text)
        {
            if (TryGetContext(text, out Context context))
            {
                if (Command.TryGet(context, out Command command))
                {
                    command.Context = context;
                    command.Run();
                }
            }
        }

        private bool TryGetContext(string text, out Context context)
        {
            if (!string.IsNullOrEmpty(text))
            {
                int index = text.IndexOf(' ');
                if (index != -1 && text.Length >= index + 2)
                {
                    string name = text.Substring(0, index);
                    text = text.Substring(index + 1);

                    context = new Context
                    {
                        Command = name,
                        Text = text,
                        Args = Parser.CommandLineToArgs(text)
                    };
                    return true;
                }
                else if (index == -1)
                {
                    context = new Context
                    {
                        Command = text,
                        Text = text
                    };
                    return true;
                }
            }

            context = null;
            return false;
        }
    }
}