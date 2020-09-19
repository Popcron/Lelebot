using Discord.Rest;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class UserJoinLeaveEvents : Processor
    {
        public class Strings
        {
            public string[] messages = { };
        }

        private Random random;
        private Strings greetingMessages;
        private Strings departureMessages;

        public UserJoinLeaveEvents()
        {
            random = new Random();

            EnsureTemplateExists(GetLocalFilePath("greetings.json.template"), "{username} joined");
            EnsureTemplateExists(GetLocalFilePath("departures.json.template"), "{username} left");

            greetingMessages = LoadStrings(GetLocalFilePath("greetings.json"));
            departureMessages = LoadStrings(GetLocalFilePath("departures.json"));
        }

        private static void EnsureTemplateExists(string pathToFile, params string[] messages)
        {
            Strings strings = new Strings
            {
                messages = messages
            };

            string json = JsonConvert.SerializeObject(strings, Formatting.Indented);
            File.WriteAllText(pathToFile, json);
        }

        private Strings LoadStrings(string pathToFile)
        {
            if (File.Exists(pathToFile))
            {
                try
                {
                    string json = File.ReadAllText(pathToFile);
                    return JsonConvert.DeserializeObject<Strings>(json);
                }
                catch
                {
                    return new Strings();
                }
            }
            else
            {
                return new Strings();
            }
        }

        public override async Task OnGuildUpdated(SocketGuild before, SocketGuild after)
        {
            IAsyncEnumerable<IReadOnlyCollection<RestAuditLogEntry>> auditLog = after.GetAuditLogsAsync(1);
            await foreach (IReadOnlyCollection<RestAuditLogEntry> element in auditLog)
            {
                foreach (RestAuditLogEntry entry in element)
                {
                    Console.WriteLine(entry.Action);
                }
            }
        }

        public override async Task OnUserJoinedServer(SocketGuildUser user)
        {
            SocketGuild guild = user.Guild;
            SocketTextChannel channel = guild.SystemChannel;
            if (channel != null)
            {
                string text = GetGreetingMessage(user);
                if (!string.IsNullOrEmpty(text))
                {
                    await channel.SendMessageAsync(text);
                }
            }

            await Task.CompletedTask;
        }

        public override async Task OnUserLeftServer(SocketGuildUser user)
        {
            SocketGuild guild = user.Guild;
            SocketTextChannel channel = guild.SystemChannel;
            if (channel != null)
            {
                string text = GetDepartureMessage(user);
                if (!string.IsNullOrEmpty(text))
                {
                    await channel.SendMessageAsync(text);
                }
            }

            await Task.CompletedTask;
        }

        private string GetGreetingMessage(SocketGuildUser user)
        {
            if (greetingMessages.messages.Length > 0)
            {
                int index = random.Next(departureMessages.messages.Length);
                string message = greetingMessages.messages[index];
                return ParseMessage(message, user);
            }
            else
            {
                return null;
            }
        }

        private string GetDepartureMessage(SocketGuildUser user)
        {
            if (departureMessages.messages.Length > 0)
            {
                int index = random.Next(departureMessages.messages.Length);
                string message = departureMessages.messages[index];
                return ParseMessage(message, user);
            }
            else
            {
                return null;
            }
        }

        private string ParseMessage(string message, SocketGuildUser user)
        {
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = message.Replace("{username}", user.Username);
            return message;
        }
    }
}
