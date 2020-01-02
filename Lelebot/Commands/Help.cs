using System;
using System.Text;

namespace Lelebot.Commands
{
    public class Help : Command
    {
        public override string[] Names => new string[] { "help" };
        public override string Description => "Prints out a list of the commands registered.";

        public override bool Match(Context context)
        {
            if (context.Command == "help")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Run()
        {
            //build the text that contains all the info the show
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("```");
            foreach (Command command in All)
            {
                if (command.Names != null && command.Names.Length > 0)
                {
                    builder.Append(string.Join(", ", command.Names));
                }
                else
                {
                    builder.Append(command.GetType().FullName);
                }

                //add description text
                if (!string.IsNullOrEmpty(command.Description))
                {
                    builder.Append(" = ");
                    builder.Append(command.Description);
                }

                //add usage text
                if (!string.IsNullOrEmpty(command.Usage))
                {
                    builder.AppendLine();
                    builder.Append("    Usage: ");

                    string[] lines = command.Usage.Split('\n');
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i != 0)
                        {
                            builder.Append("    ");
                        }

                        builder.Append(lines[i]);
                        builder.AppendLine();
                    }
                }

                builder.AppendLine();
            }
            builder.Append("```");

            //if the string builder is too long, then split it
            int limit = 1000;
            string text = builder.ToString();
            int splits = (int)Math.Ceiling(text.Length / (float)limit);
            for (int i = 0; i < splits; i++)
            {
                int start = i * limit;
                if (i == splits - 1)
                {
                    Print(text.Substring(start));
                }
                else
                {
                    Print(text.Substring(start, limit));
                }
            }
        }
    }
}
