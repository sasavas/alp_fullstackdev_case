using ForceGetCase.Application.Common.Email;

namespace ForceGetCase.Application.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage emailMessage);
}
