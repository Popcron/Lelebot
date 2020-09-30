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
        public DiscordSocketClient Client => client;

        /// <summary>
        /// The info that this bot was loaded with.
        /// </summary>
        public Info Info { get; }

        private DiscordSocketClient client;
        private List<Processor> processors = null;

        public Bot(Info info)
        {
            Info = info;
            Console.WriteLine("[bot] initializing bot");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Initialize();
            stopwatch.Stop();

            Console.WriteLine($"[bot] initialized bot in {stopwatch.ElapsedMilliseconds}ms");
        }

        private void LoadProcessors()
        {
            //have we aready loaded before?
            if (processors != null)
            {
                return;
            }

            processors = new List<Processor>();
            string processorsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Processors");
            if (!Directory.Exists(processorsFolder))
            {
                Directory.CreateDirectory(processorsFolder);
            }

            //create all processors
            Assembly assembly = typeof(Program).Assembly;
            Type[] types = assembly.GetTypes();
            for (int t = 0; t < types.Length; t++)
            {
                Type type = types[t];
                if (type.IsSubclassOf(typeof(Processor)))
                {
                    string processorFolder = Path.Combine(processorsFolder, type.Name);
                    if (!Directory.Exists(processorFolder))
                    {
                        Directory.CreateDirectory(processorFolder);
                    }

                    Processor processor = (Processor)Activator.CreateInstance(type);
                    processor.OnCreated(this);

                    processors.Add(processor);
                    Console.WriteLine($"[bot] created processor {type.Name}");
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
            client.UserBanned += (SocketUser user, SocketGuild guild) => OnUserBanned(user, guild, true);
            client.UserUnbanned += (SocketUser user, SocketGuild guild) => OnUserBanned(user, guild, false);
            client.GuildUpdated += OnGuildUpdated;
            client.UserJoined += OnUserJoinedServer;
            client.UserLeft += OnUserLeftServer;
            client.ChannelUpdated += OnChannelUpdated;
            client.UserVoiceStateUpdated += OnUserVoiceUpdated;

            await Start();
        }

        private async Task Start()
        {
            try
            {
                Console.WriteLine($"[bot] attempting to login and start the bot");
                await client.LoginAsync(TokenType.Bot, Info.token);
                await client.StartAsync();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"[exception] {exception.Message}");
                Console.WriteLine($"stack: {exception.StackTrace}");
            }
        }

        private async Task OnChannelUpdated(SocketChannel oldChannel, SocketChannel newChannel)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnChannelUpdated(oldChannel, newChannel);
            }
        }

        private async Task OnUserBanned(SocketUser user, SocketGuild guild, bool isBanned)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnUserBanned(user, guild, isBanned);
            }
        }

        private async Task OnGuildUpdated(SocketGuild before, SocketGuild after)
        {
            for (int i = 0; i < processors.Count; i++)
            {
                await processors[i].OnGuildUpdated(before, after);
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
            //waiting another extra second on purpose
            await Task.Delay(1000);

            Console.WriteLine("[bot] ready");
            LoadProcessors();
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
        public async Task Speak(Context ctx, string text)
        {
            SocketGuildChannel guildChannel = ctx.Message.Channel as SocketGuildChannel;
            IGuild guild = guildChannel.Guild as IGuild;
            SocketVoiceChannel voiceChannel = null;
            foreach (SocketGuildChannel channel in await guild.GetVoiceChannelsAsync())
            {
                foreach (SocketGuildUser user in channel.Users)
                {
                    if (user.Id == Info.clientId)
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
            Console.WriteLine(message);
            if (TryGetContext(message.Content, out Context context))
            {
                SocketGuildChannel textChannel = message.Channel as SocketGuildChannel;
                SocketGuild guild = textChannel?.Guild;

                context.Message = message;
                context.Author = message.Author;
                context.Channel = message.Channel;
                context.Guild = guild;

                if (Command.TryGet(context, out Command command))
                {
                    command.Bot = this;

                    if (command.TriggerTyping)
                    {
                        await message.Channel.TriggerTypingAsync();
                    }

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