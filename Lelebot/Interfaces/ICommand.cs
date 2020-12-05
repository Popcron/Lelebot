using System.Threading.Tasks;

namespace Lelebot
{
    public interface ICommand
    {
        bool ShouldRun(Call call);
        Task<Message> Run(Call call);
    }
}