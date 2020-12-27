using System;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class RemindMe : IBasicCommand, IHelp
    {
        string IBasicCommand.Name => "remindme";
        string IHelp.Help => "Reminds the author of someone after an amount of time";

        bool ICommand.ShouldRun(Call call)
        {
            if (call.BaseCommand == "remindme")
            {
                if (call.Args.Length >= 2)
                {
                    if (GetTime(call.Args[0]) is TimeSpan time)
                    {
                        return time.TotalMilliseconds > 0;
                    }
                }
            }

            return false;
        }

        private TimeSpan? GetTime(string text)
        {
            try
            {
                int seconds = 0;
                int minutes = 0;
                int hours = 0;
                int days = 0;
                string digits = string.Empty;
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (char.IsDigit(c))
                    {
                        digits += c;
                    }
                    else
                    {
                        if (c == 's')
                        {
                            seconds = int.Parse(digits);
                        }
                        else if (c == 'm')
                        {
                            minutes = int.Parse(digits);
                        }
                        else if (c == 'h')
                        {
                            hours = int.Parse(digits);
                        }
                        else if (c == 'd')
                        {
                            days = int.Parse(digits);
                        }

                        digits = string.Empty;
                    }
                }

                return new TimeSpan(days, hours, minutes, seconds);
            }
            catch { }

            return null;
        }

        async Task<Message> ICommand.Run(Call call)
        {
            Message message = new();
            TimeSpan time = GetTime(call.Args[0]).Value;
            DateTime then = DateTime.UtcNow + time;
            string text = string.Join(" ", call.Args[1..^0]);

            message.Append(text);
            await Task.Delay((int)time.TotalMilliseconds);
            return message;
        }
    }
}
