using System.Threading.Tasks;
using ForceGetCase.Application.Common.Email;
using ForceGetCase.Application.Services;

namespace ForceGetCase.Api.IntegrationTests.Helpers.Services;

public class EmailTestService : IEmailService
{
    public async Task SendEmailAsync(EmailMessage emailMessage)
    {
        await Task.Delay(100);
    }
}
