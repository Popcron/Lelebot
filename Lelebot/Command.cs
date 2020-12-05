using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Lelebot
{
    public abstract class Command
    {
        private static List<Command> all = null;

        static Command()
        {
            all = new List<Command>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (!type.IsAbstract && typeof(Command).IsSubclassOf(type))
                    {
                        Command command = (Command)Activator.CreateInstance(type);
                        all.Add(command);
                    }
                }
            }
        }

        public static IEnumerable<Command> GetCommands(Context context)
        {
            bool fromServerOwner = context?.Author.Id == 0;
            bool fromConsoleWindow = context.Author == null;
            bool fromBotOwner = Bot.IsOwnerOfThisBot(context.Author);

            foreach (Command command in all)
            {
                if (command is IServerOwnerOnly && !fromServerOwner)
                {
                    continue;
                }

                if (command is IConsoleOnly && !fromConsoleWindow)
                {
                    continue;
                }

                if (command is IBotOwnerOnly && !fromBotOwner)
                {
                    continue;
                }

                yield return command;
            }
        }

        /// <summary>
        /// Returns a brand new instance of a command with this name if it exists.
        /// </summary>
        public static Command Create(Context context)
        {
            foreach (Command command in GetCommands(context))
            {
                if (command.Match(context))
                {
                    return (Command)Activator.CreateInstance(command.GetType());
                }
            }

            return default;
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
        public virtual string Description => default;

        /// <summary>
        /// How to use this command, like an example.
        /// </summary>
        public virtual string Usage => default;

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
