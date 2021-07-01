using Infrastructure.Adapter.Email.Models;
using System.Threading.Tasks;

namespace Infrastructure.Adapter.Email.Interfaces
{
    public interface IEmailService
    {
        Task<EmailResponse> SendEmail(EmailRequest emailRequest);
        Task<EmailResponse> SendEmailWithTemplate(EmailTemplateRequest emailWithTemplateRequest);
        Task<EmailResponse> SendEmailWhitAttachments(EmailRequestAttachment emailRequest);
    }
}
