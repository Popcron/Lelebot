using IronPython.Hosting;
using Microsoft.Scripting.Hosting;
using System.Threading.Tasks;

namespace Lelebot.Commands
{
    public class RunCode : IBaseCommand, IHelp
    {
        string IHelp.Help => "Executes a line of code";
        string IBaseCommand.BaseCommand => "code";

        async Task<Message> ICommand.Run(Call call)
        {
            Message message = new();
            ScriptEngine pythonEngine = Python.CreateEngine();
            ScriptSource pythonScript = pythonEngine.CreateScriptSourceFromString(call.RawText);
            message.Append(pythonScript.Execute());

            await Task.CompletedTask;
            return message;
        }
    }
}
