using Dotnet.Homeworks.Domain.Abstractions.Repositories;
using Dotnet.Homeworks.Domain.Entities;
using MongoDB.Driver;

namespace Dotnet.Homeworks.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly IMongoCollection<Order> _mongoCollection;

    public OrderRepository(IMongoDatabase database)
    {
        _mongoCollection = database.GetCollection<Order>("Orders");
    }
    
    public async Task<IEnumerable<Order>> GetAllOrdersFromUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.OrdererId, userId);
        
        return await _mongoCollection
            .Find(filter)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, orderId);
        
        return await _mongoCollection
            .Find(filter)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteOrderByGuidAsync(Guid orderId, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, orderId);
        var deleteResult = await _mongoCollection.DeleteOneAsync(filter, cancellationToken);

        if (deleteResult.DeletedCount == 0)
        {
            throw new ArgumentNullException($"Order with Id: {orderId} was not found");
        }
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var filter = Builders<Order>.Filter.Eq(x => x.Id, order.Id);
        await _mongoCollection.ReplaceOneAsync(filter, order, cancellationToken: cancellationToken);
    }

    public async Task<Guid> InsertOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _mongoCollection.InsertOneAsync(order, cancellationToken: cancellationToken);

        return order.Id;
    }
}