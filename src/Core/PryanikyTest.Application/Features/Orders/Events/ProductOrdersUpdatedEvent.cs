using MediatR;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Application.Abstractions;

namespace PryanikyTest.Application.Features.Orders.Events;

public record ProductOrdersUpdatedEvent(ICollection<Guid> ProductIds) : INotification;

public class ProductOrdersUpdatedHandler : INotificationHandler<ProductOrdersUpdatedEvent>
{
    private readonly IApplicationDbContext _dbContext;

    public ProductOrdersUpdatedHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ProductOrdersUpdatedEvent notification, CancellationToken cancellationToken)
    {
        foreach (var productId in notification.ProductIds)
        {
            var product = await _dbContext.Products
                .Include(product => product.ProductOrders)
                .FirstOrDefaultAsync(product => product.Id == productId);

            product.TotalOrders = product.ProductOrders.Sum(order => order.Amount);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
