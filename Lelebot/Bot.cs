using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;
using Discord.WebSocket;

namespace Lelebot
{
    public class Bot
    {
        public bool CancellationRequested { get; private set; }
        public static IAudioClient AudioClient { get; set; }

        /// <summary>
        /// The info that this bot was loaded with.
        /// </summary>
        public Info Info { get; }

        private DiscordSocketClient client;
        private List<Processor> processors = new List<Processor>();

        public Bot(Info info)
        {
            Info = info;
            Console.WriteLine("[bot] initializing bot");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LoadProcessors();
            Initialize();
            stopwatch.Stop();

            Console.WriteLine($"[bot] initialized bot in {stopwatch.ElapsedMilliseconds}ms");
        }

        private void LoadProcessors()
        {
            //create all processors
            Assembly assembly = typeof(Program).Assembly;
            Type[] types = assembly.GetTypes();
            for (int t = 0; t < types.Length; t++)
            {
                if (types[t].IsSubclassOf(typeof(Processor)))
                {
                    Processor processor = (Processor)Activator.CreateInstance(types[t]);
                    processor.Bot = this;
                    processors.Add(processor);
                    Console.WriteLine($"[bot] created processor {processor.GetType().Name}");
                }
            }
        }

        private async void Initialize()
        {
            //create the discord client
            Console.WriteLine($"[bot] creating socket client");
            DiscordSocketConfig config = new DiscordSocketConfig();
            config.LogLevel = LogSeverity.Verbose;
            client = new DiscordSocketClient(config);
            client.Connected += OnConnected;
            client.Disconnected += OnDisconnected;
            client.LoggedIn += OnLoggedIn;
            client.LoggedOut += OnLoggedOut;
            client.Ready += OnReady;
            client.MessageReceived += OnMessage;
            client.UserJoined += OnUserJoinedServer;
            client.UserLeft += OnUserLeftServer;
            client.ChannelUpdated += OnChannelUpdated;
            client.UserVoiceStateUpdated += OnUserVoiceUpdated;

            await Start(Info.token);
        }

        private async Task Start(string token)
        {
            try
            {
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[error] {exception.Message.ToLower()}");
            }
        }

        private async Task OnChannelUpdated(SocketChannel oldChannel, SocketChannel newChannel)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnChannelUpdated(oldChannel, newChannel);
            }
        }

        private async Task OnUserLeftServer(SocketGuildUser user)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnUserLeftServer(user);
            }
        }

        private async Task OnUserJoinedServer(SocketGuildUser user)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnUserJoinedServer(user);
            }
        }

        private async Task OnUserVoiceUpdated(SocketUser user, SocketVoiceState oldChannel, SocketVoiceState newChannel)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnUserVoiceUpdated(user, oldChannel, newChannel);
            }
        }

        private async Task OnReady()
        {
            Console.WriteLine("[bot] ready");
            await Task.CompletedTask;
        }

        private async Task OnLoggedOut()
        {
            Console.WriteLine("[bot] logged out");
            await Task.CompletedTask;
        }

        private async Task OnLoggedIn()
        {
            Console.WriteLine("[bot] logged in");
            await Task.CompletedTask;
        }

        private async Task OnDisconnected(Exception arg)
        {
            Console.WriteLine("[bot] disconnected");
            await Task.CompletedTask;
        }

        private async Task OnConnected()
        {
            Console.WriteLine("[bot] connected");
            await Task.CompletedTask;
        }

        /// <summary>
        /// Says a message in a voice channel.
        /// </summary>
        public async Task Say(Context ctx, string text)
        {
            SocketGuildChannel guildChannel = ctx.Message.Channel as SocketGuildChannel;
            IGuild guild = guildChannel.Guild as IGuild;
            SocketVoiceChannel voiceChannel = null;
            foreach (SocketGuildChannel channel in await guild.GetVoiceChannelsAsync())
            {
                foreach (SocketGuildUser user in channel.Users)
                {
                    if (user.Id == Info.botUserId)
                    {
                        voiceChannel = channel as SocketVoiceChannel;
                        break;
                    }
                }

                if (voiceChannel != null)
                {
                    break;
                }
            }

            if (AudioClient == null)
            {
                AudioClient = await voiceChannel.ConnectAsync();
            }

            if (voiceChannel == null)
            {
                return;
            }

            Console.WriteLine($"[say] {text}");
            Stream ret = new MemoryStream();
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                MethodInfo mi = synth.GetType().GetMethod("SetOutputStream", BindingFlags.Instance | BindingFlags.NonPublic);
                SpeechAudioFormatInfo fmt = new SpeechAudioFormatInfo(8000, AudioBitsPerSample.Sixteen, AudioChannel.Mono);
                mi.Invoke(synth, new object[] { ret, fmt, true, true });
                //synth.SelectVoice(voiceName);
                synth.Speak(text);

                await Task.Delay(500);
                AudioOutStream discord = AudioClient.CreateOpusStream();
                await ret.CopyToAsync(discord);
                ret.Flush();
            }
        }

        private async Task OnMessage(SocketMessage message)
        {
            //dont run on self
            if (message.Author.Id == client.CurrentUser.Id)
            {
                return;
            }

            //invoke the raw processors
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnMessage(message);
            }

            //try and invoke a command
            if (TryGetContext(message.Content, out Context context))
            {
                if (Command.TryGet(context, out Command command))
                {
                    if (command.TriggerTyping)
                    {
                        await message.Channel.TriggerTypingAsync();
                    }

                    SocketGuildChannel textChannel = message.Channel as SocketGuildChannel;
                    SocketGuild guild = textChannel?.Guild;

                    context.Author = message.Author;
                    context.Channel = message.Channel;
                    context.Guild = guild;
                    context.Message = message;
                    command.Bot = this;
                    command.Run(context);
                }
            }
        }

        /// <summary>
        /// Runs a command.
        /// </summary>
        public void Run(string text)
        {
            if (TryGetContext(text, out Context context))
            {
                if (Command.TryGet(context, out Command command))
                {
                    command.Bot = this;
                    command.Run(context);
                }
            }
        }

        /// <summary>
        /// Tries to create a context element from a message.
        /// </summary>
        private bool TryGetContext(string text, out Context context)
        {
            string originalText = text;
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
                        Text = originalText,
                        Args = Parser.CommandLineToArgs(text)
                    };
                    return true;
                }
                else if (index == -1)
                {
                    context = new Context
                    {
                        Command = text,
                        Text = originalText
                    };
                    return true;
                }
            }

            context = null;
            return false;
        }
    }
}