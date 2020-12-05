namespace Lelebot
{
    public class Parser
    {
        public static Call? Build(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                string baseCommand = command;
                string[] args = new string[] { };
                if (command.IndexOf(' ') != -1)
                {
                    args = command.Split(' ');
                }

                return new Call(baseCommand, args);
            }

            return default;
        }
    }
}