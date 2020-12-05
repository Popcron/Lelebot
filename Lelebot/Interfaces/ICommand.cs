using System.Threading.Tasks;

namespace Lelebot
{
    public interface ICommand
    {
        string BaseCommand { get; }
        Task Run();
    }
}