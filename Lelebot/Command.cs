using System;
using System.Collections.Generic;
using System.Reflection;

namespace Lelebot
{
    public abstract class Command
    {
        private static List<Command> commands = null;

        private static List<Command> Commands
        {
            get
            {
                if (commands == null)
                {
                    commands = new List<Command>();
                    Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    for (int a = 0; a < assemblies.Length; a++)
                    {
                        Type[] types = assemblies[a].GetTypes();
                        for (int t = 0; t < types.Length; t++)
                        {
                            if (types[t].IsSubclassOf(typeof(Command)))
                            {
                                Command command = (Command)Activator.CreateInstance(types[t]);
                                commands.Add(command);
                            }
                        }
                    }
                }

                return commands;
            }
        }

        /// <summary>
        /// Returns a brand new instance of a command with this name if it exists.
        /// </summary>
        public static bool TryGet(Context context, out Command command)
        {
            List<Command> commands = Commands;
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

        public abstract bool Match(Context ctx);

        public virtual void Run()
        {

        }

        protected void Print(string text)
        {
            if (Context.Channel != null)
            {
                Context.Channel.SendMessageAsync(text);
            }
            else
            {
                Console.WriteLine(text);
            }
        }
    }
}
