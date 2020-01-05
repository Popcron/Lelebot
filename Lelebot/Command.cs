using System;
using System.Collections.Generic;
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
                    command.Context = context;
                    return true;
                }
            }

            command = null;
            return false;
        }

        public Context Context { get; set; }
        public virtual bool TriggerTyping => true;

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

        public virtual void Run()
        {

        }

        /// <summary>
        /// Prints a message to the context origin.
        /// </summary>
        protected void Print(object text)
        {
            if (Context.Channel != null)
            {
                Context.Channel.SendMessageAsync(text.ToString());
            }
            else
            {
                Console.WriteLine(text);
            }
        }
    }
}
