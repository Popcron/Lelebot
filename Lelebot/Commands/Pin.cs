using Discord;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class PinCommand : Command
    {
        public const string DiscordMessageExtension = "DISCORDMESSAGE";

        public static string PinsDirectory
        {
            get
            {
                string location = AppDomain.CurrentDomain.BaseDirectory;
                return Path.Combine(location, "Pins");
            }
        }

        public override string[] Names => new string[] { "pin" };
        public override string Description => "Pins the message with a custom key.";
        public override string Usage => "`*<key>` to save the message with this key, typing this without anything else with print out the saved key if any is present. For example, `*fish`";
        public override bool TriggerTyping => false;

        public override bool Match(Context context)
        {
            if (context.Text.Length >= 2 && context.Text.StartsWith("*"))
            {
                //first argument is the pin key,
                //everything else is the message
                return true;
            }

            return false;
        }

        public override async void Run(Context context)
        {
            int spaceIndex = context.Text.IndexOf(' ');
            if (spaceIndex != -1)
            {
                //save this message to this key
                string key = context.Text.Substring(1, spaceIndex - 1);
                await Save(key, context);
            }
            else
            {
                //find a pin with this key and print it out
                string key = context.Text.TrimStart('*');
                Pin pin = Get(key);
                if (pin != null)
                {
                    if (context.Channel != null)
                    {
                        await context.Channel.TriggerTypingAsync();
                    }

                    if (pin.attachment?.Length > 0)
                    {
                        SendAttachment(context, pin.attachment, pin.fileName, pin.text);
                    }
                    else if (!string.IsNullOrEmpty(pin.text))
                    {
                        SendText(context, pin.text);
                    }
                }
            }
        }

        private Pin Get(string key)
        {
            //ensure dir exists
            if (!Directory.Exists(PinsDirectory))
            {
                Directory.CreateDirectory(PinsDirectory);
                return null;
            }

            string[] files = Directory.GetFiles(PinsDirectory, "*.*", SearchOption.TopDirectoryOnly);
            Pin pin = new Pin();
            for (int i = 0; i < files.Length; i++)
            {
                string keyName = Path.GetFileNameWithoutExtension(files[i]);
                if (keyName.Equals(key))
                {
                    if (files[i].EndsWith($".{DiscordMessageExtension}"))
                    {
                        pin.text = File.ReadAllText(files[i]);
                    }
                    else
                    {
                        pin.attachment = File.ReadAllBytes(files[i]);
                        pin.fileName = Path.GetFileName(files[i]);
                    }
                }
            }

            return pin;
        }

        private async Task Save(string key, Context context)
        {
            //ensure dir exists
            string pinDir = PinsDirectory;
            if (!Directory.Exists(pinDir))
            {
                Directory.CreateDirectory(pinDir);
            }

            string message = context.Text.Replace($"*{key} ", "");
            Console.WriteLine($"{key} = {message}");

            //save message if not empty
            if (message != null && !string.IsNullOrEmpty(message))
            {
                string pathToFile = Path.Combine(pinDir, Path.ChangeExtension(key, DiscordMessageExtension));
                Console.WriteLine($"saving '{message}' to {pathToFile}");
                File.WriteAllText(pathToFile, message);
            }

            //save the first attachment
            if (context.Message?.Attachments.Count > 0)
            {
                Attachment attachment = context.Message.Attachments.First();
                string extension = Path.GetExtension(attachment.Filename);
                string pathToFile = Path.Combine(pinDir, Path.ChangeExtension(key, extension));
                Console.WriteLine($"saving '{attachment.Filename}' to {pathToFile} from {attachment.Url}");

                HttpClient client = new HttpClient();
                byte[] data = await client.GetByteArrayAsync(attachment.Url);
                File.WriteAllBytes(pathToFile, data);
                Console.WriteLine("saved");
            }
        }

        public class Pin
        {
            public string text;
            public byte[] attachment;
            public string fileName;
        }
    }
}
