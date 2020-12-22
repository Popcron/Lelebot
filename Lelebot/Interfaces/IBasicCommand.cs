namespace Lelebot
{
    public interface IBasicCommand : ICommand
    {
        string Name { get; }
        bool ICommand.ShouldRun(Call call) => call.BaseCommand == Name;
    }
}