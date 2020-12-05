using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
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
                Log.WriteLine(text);
                try
                {
                    Message message = new();
                    ScriptEngine pythonEngine = Python.CreateEngine();
                    ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromString(text);
                    message.Append(pythonScript.Execute());
                    return message;
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }
    }
}
