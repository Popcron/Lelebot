using System;
using System.Linq;
using System.Reflection;

namespace Lelebot
{
    public static class Library
    {
        /// <summary>
        /// Array of all command templates registered.
        /// </summary>
        public static ICommand[] Commands { get; private set; }

        static Library()
        {
            FindAllCommands();
        }

        private static void FindAllCommands()
        {
            Type baseType = typeof(ICommand);
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes().Where(x => baseType.IsAssignableFrom(x) && x != baseType).ToArray();
            Commands = new ICommand[types.Length];
            for (int i = 0; i < types.Length; i++)
            {
                Commands[i] = Activator.CreateInstance(types[i]) as ICommand;
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