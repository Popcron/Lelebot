using System.Threading.Tasks;

namespace Lelebot
{
    public interface ICommand
    {
        string BaseCommand { get; }
        Task<Message> Run(Call call);
    }
}