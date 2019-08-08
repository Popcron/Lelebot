using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.IO;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class UserJoinedTheChannel : Processor
    {
        private SpeechSynthesizer synth;

        public UserJoinedTheChannel()
        {
            synth = new SpeechSynthesizer();
        }

        public override void OnUserVoiceUpdated(SocketUser user, SocketVoiceState oldState, SocketVoiceState newState)
        {
            if (oldState.VoiceChannel != null && newState.VoiceChannel != null)
            {
                if (newState.VoiceChannel.GetUser(Bot.MyID) != null)
                {
                    Console.WriteLine(user.Username + " has joined");
                }
                else if (oldState.VoiceChannel.GetUser(Bot.MyID) != null)
                {
                    Console.WriteLine(user.Username + " has left");
                }
            }
            else if (oldState.VoiceChannel == null)
            {
                if (newState.VoiceChannel.GetUser(Bot.MyID) != null)
                {
                    Console.WriteLine(user.Username + " has joined");
                }
            }
            else if (newState.VoiceChannel == null)
            {
                if (oldState.VoiceChannel.GetUser(Bot.MyID) != null)
                {
                    Console.WriteLine(user.Username + " has left");
                }
            }
        }

        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }

        private async Task SendAsync(IAudioClient client, string path)
        {
            // Create FFmpeg using the previous example
            using (Process ffmpeg = CreateStream(path))
            {
                using (Stream output = ffmpeg.StandardOutput.BaseStream)
                {
                    using (AudioOutStream discord = client.CreatePCMStream(AudioApplication.Mixed))
                    {
                        try
                        {
                            await output.CopyToAsync(discord);
                        }
                        finally
                        {
                            await discord.FlushAsync();
                        }
                    }
                }
            }
        }
    }
}
