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
        public static List<ICommand> Commands { get; private set; }

        static Library()
        {
            FindAllCommands();
        }

        private static void FindAllCommands()
        {
            Type baseType = typeof(ICommand);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes().Where(x => baseType.IsAssignableFrom(x)).ToArray();
            Commands = new List<ICommand>();
            foreach (Type type in types)
            {
                if (!type.IsAbstract && !type.IsInterface)
                {
                    ICommand command = Activator.CreateInstance(type) as ICommand;
                    Commands.Add(command);
                }
            }
        }

        public static ICommand Get(Call call)
        {
            foreach (ICommand template in Commands)
            {
                if (template.ShouldRun(call))
                {
                    return Activator.CreateInstance(template.GetType()) as ICommand;
                }
            }

            return default;
        }
    }
}