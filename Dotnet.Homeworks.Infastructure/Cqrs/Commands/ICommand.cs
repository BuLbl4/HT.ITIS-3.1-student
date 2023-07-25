using Dotnet.Homeworks.Infastructure.Utils;
using MediatR;

namespace Dotnet.Homeworks.Infastructure.Cqrs.Commands;

public interface ICommand : IRequest<Result>
{
    public Result Result { get; set; }
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
    public Result<TResponse> Result { get; set; }
}