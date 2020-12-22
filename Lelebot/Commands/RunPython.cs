using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class RunPython : IBasicCommand, IHelp
    {
        string IHelp.Help => "Executes a line of python code";
        string IBasicCommand.Name => "py";

        async Task<Message> ICommand.Run(Call call)
        {
            await Task.CompletedTask;
            if (call.RawText.Length > 4)
            {
                string text = call.RawText.Substring(3);
                Message message = new();
                try
                {
                    TextWriter lastOut = Console.Out;
                    ScriptEngine pythonEngine = Python.CreateEngine();
                    pythonEngine.Runtime.IO.RedirectToConsole();

                    StringWriter output = new();
                    Console.SetOut(output);

                    ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromString(text);
                    pythonScript.Execute();

                    message.Append(output.GetStringBuilder().ToString());
                    Console.SetOut(lastOut);
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
