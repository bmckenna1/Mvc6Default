using System.Threading.Tasks;

namespace Mvc6Default.Security.Interfaces
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
