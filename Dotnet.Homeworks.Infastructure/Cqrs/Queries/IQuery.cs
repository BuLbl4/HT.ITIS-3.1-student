using Dotnet.Homeworks.Infastructure.Utils;
using MediatR;

namespace Dotnet.Homeworks.Infastructure.Cqrs.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    public Result<TResponse> Result { get; set; }
}