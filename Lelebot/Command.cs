using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Lelebot
{
    public abstract class Command
    {
        private static List<Command> all = null;

        public static List<Command> All
        {
            get
            {
                if (all == null)
                {
                    all = new List<Command>();
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    for (int a = 0; a < assemblies.Length; a++)
                    {
                        Type[] types = assemblies[a].GetTypes();
                        for (int t = 0; t < types.Length; t++)
                        {
                            if (types[t].IsSubclassOf(typeof(Command)))
                            {
                                Command command = (Command)Activator.CreateInstance(types[t]);
                                all.Add(command);
                            }
                        }
                    }
                }

                return all;
            }
        }

        /// <summary>
        /// Returns a brand new instance of a command with this name if it exists.
        /// </summary>
        public static bool TryGet(Context context, out Command command)
        {
            List<Command> commands = All;
            for (int i = 0; i < commands.Count; i++)
            {
                bool match = commands[i].Match(context);
                if (match)
                {
                    command = (Command)Activator.CreateInstance(commands[i].GetType());
                    return true;
                }
            }

            command = null;
            return false;
        }

        public abstract bool TriggerTyping { get; }

        /// <summary>
        /// Should return true if this context is appropriate for this command.
        /// </summary>
        public abstract bool Match(Context context);

        /// <summary>
        /// The many names of this command.
        /// </summary>
        public abstract string[] Names { get; }

        /// <summary>
        /// What this commands does
        /// </summary>
        public virtual string Description => "";

        /// <summary>
        /// How to use this command, like an example.
        /// </summary>
        public virtual string Usage => "";

        /// <summary>
        /// The bot that called this command.
        /// </summary>
        public Bot Bot { get; set; }

        public virtual void Run(Context context)
        {

        }

        /// <summary>
        /// Prints a message to the context origin.
        /// This is either the console window or the text channel.
        /// </summary>
        protected void SendText(Context context, object text)
        {
            if (context.Channel != null)
            {
                context.Channel.SendMessageAsync(text?.ToString());
            }
            else
            {
                Console.WriteLine(text);
            }
        }

        protected void SendAttachment(Context context, byte[] data, string fileName, object text = null)
        {
            if (context.Channel != null)
            {
                Stream stream = new MemoryStream(data);
                context.Channel.SendFileAsync(stream, fileName, text?.ToString());
            }
            else
            {
                Console.WriteLine($"byte data {data.Length}, {text?.ToString()}");
            }
        }
    }
}
