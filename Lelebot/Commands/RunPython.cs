using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class RunPython : IBaseCommand, IHelp
    {
        string IHelp.Help => "Executes a line of python code";
        string IBaseCommand.BaseCommand => "py";

        async Task<Message> ICommand.Run(Call call)
        {
            await Task.CompletedTask;
            if (call.RawText.Length > 4)
            {
                string text = call.RawText.Substring(3);
                Message message = new();
                try
                {
                    MemoryStream memoryStream = new();
                    ScriptEngine pythonEngine = Python.CreateEngine();
                    pythonEngine.Runtime.IO.RedirectToConsole();

                    ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromString(text);
                    pythonScript.Execute();

                    StreamReader reader = new(memoryStream);
                    message.Append(reader.ReadToEnd());
                }
                catch (Exception e)
                {
                    message.Append(e.Message);
                }

                return message;
            }

            return null;
        }
    }
}
