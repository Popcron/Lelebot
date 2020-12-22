using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lelebot
{
    public static class Library
    {
        /// <summary>
        /// Array of all command templates registered.
        /// </summary>
        public static List<ICommand> AllCommands { get; private set; }

        static Library()
        {
            FindAllCommands();
        }

        private static void FindAllCommands()
        {
            Type baseType = typeof(ICommand);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes().Where(x => baseType.IsAssignableFrom(x)).ToArray();
            AllCommands = new List<ICommand>();
            foreach (Type type in types)
            {
                if (!type.IsAbstract && !type.IsInterface)
                {
                    ICommand command = Activator.CreateInstance(type) as ICommand;
                    AllCommands.Add(command);
                }
            }
        }

        public static List<ICommand> GetCommands(Call call)
        {
            List<ICommand> commands = new List<ICommand>();
            foreach (ICommand template in AllCommands)
            {
                Type type = template.GetType();
                bool allowed = true;
                List<ExecutionFilterAttribute> attributes = type.GetCustomAttributes<ExecutionFilterAttribute>().ToList();
                if (attributes.Count > 0)
                {
                    allowed = false;
                    foreach (Attribute attribute in attributes)
                    {
                        if (attribute is ConsoleOnlyAttribute)
                        {
                            if (call.Origin == MessageOrigin.Console)
                            {
                                allowed = true;
                                break;
                            }
                        }
                        else if (attribute is BotOwnerOnlyAttribute)
                        {
                            if (call.DiscordMessage?.Author?.Id == Info.BotOwner)
                            {
                                allowed = true;
                                break;
                            }
                        }
                        else if (attribute is ServerOwnerOnlyAttribute)
                        {
                            if (call.DiscordMessage?.Channel is ITextChannel textChannel)
                            {
                                if (textChannel.Guild.OwnerId == call.DiscordMessage.Author.Id)
                                {
                                    allowed = true;
                                    break;
                                }
                            }
                        }
                        else if (attribute is PrivateDMOnlyAttribute)
                        {
                            if (call.DiscordMessage?.Channel is IPrivateChannel)
                            {
                                allowed = true;
                                break;
                            }
                        }
                    }
                }
                
                if (allowed)
                {
                    commands.Add(template);
                }
            }

            return commands;
        }

        public static ICommand Get(Call call)
        {
            List<ICommand> commands = GetCommands(call);
            foreach (ICommand template in commands)
            {
                if (template.ShouldRun(call))
                {
                    Type type = template.GetType();
                    return Activator.CreateInstance(type) as ICommand;
                }
            }

            return default;
        }
    }
}