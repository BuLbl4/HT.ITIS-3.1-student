using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Infrastructure.Cqrs.Queries;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Features.Products.Queries.GetProducts;

internal sealed class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, GetProductsDto>
{
    private readonly IProductRepository _repository;

    public GetProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetProductsDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allProducts = await _repository.GetAllProductsAsync(cancellationToken);
            var allProductsDto = allProducts.Select(p => new GetProductDto(p.Id, p.Name));

            return ResultFactory.CreateResult<Result<GetProductsDto>>(true, new GetProductsDto(allProductsDto));
        }
        catch (Exception ex)
        {
            return ResultFactory.CreateResult<Result<GetProductsDto>>(false, ex.Message);
        }
    }
}