using Dotnet.Homeworks.Shared.MessagingContracts.Email;
using MassTransit;

namespace Dotnet.Homeworks.MainProject.Services;

public class CommunicationService : ICommunicationService
{
    private readonly IPublishEndpoint _endpoint;

    public CommunicationService(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }

    public async Task SendEmailAsync(SendEmail sendEmailDto, CancellationToken cancellationToken)
    {
        await _endpoint.Publish(new SendEmail(
                sendEmailDto.ReceiverName,
                sendEmailDto.ReceiverEmail,
                sendEmailDto.Subject,
                sendEmailDto.Content),
            cancellationToken);
    }
}