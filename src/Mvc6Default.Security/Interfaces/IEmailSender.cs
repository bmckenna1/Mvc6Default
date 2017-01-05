using System.Threading.Tasks;

namespace Mvc6Default.Security.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
