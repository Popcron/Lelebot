namespace Lelebot
{
    public interface IBaseCommand : ICommand
    {
        string BaseCommand { get; }
        bool ICommand.ShouldRun(Call call) => call.BaseCommand == BaseCommand;
    }
}