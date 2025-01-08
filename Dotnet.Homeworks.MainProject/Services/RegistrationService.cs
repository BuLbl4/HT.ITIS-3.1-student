using Dotnet.Homeworks.Domain.Entities;
using Dotnet.Homeworks.MainProject.Dto;
using Dotnet.Homeworks.Shared.MessagingContracts.Email;

namespace Dotnet.Homeworks.MainProject.Services;

public class RegistrationService : IRegistrationService
{
    private readonly ICommunicationService _communicationService;

    public RegistrationService(ICommunicationService communicationService)
    {
        _communicationService = communicationService;
    }

    public async Task RegisterAsync(RegisterUserDto userDto, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        
        await _communicationService.SendEmailAsync(new SendEmail(
            userDto.Name,
            userDto.Email,
            "Registration",
            "Registration was successful"),
            cancellationToken);
    }
}