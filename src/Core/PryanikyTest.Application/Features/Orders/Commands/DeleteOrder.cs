using Microsoft.EntityFrameworkCore;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;
using FluentValidation;
using MediatR;
using PryanikyTest.Application.Features.Orders.Events;

namespace PryanikyTest.Application.Features.Orders.Commands;

public record DeleteOrderCommand(Guid OrderId) : IRequest<Unit>;

public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderValidator()
    {
        RuleFor(command => command.OrderId)
            .SetValidator(new GuidValidator());
    }
}

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;

    public DeleteOrderHandler(IApplicationDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .Include(order => order.ProductOrders)
            .FirstOrDefaultAsync(order => order.Id == request.OrderId, cancellationToken);

        if (order == null) throw new EntityNotFoundException(nameof(Order), request.OrderId);

        // Deleting product orders of the order
        _dbContext.ProductOrders.RemoveRange(order.ProductOrders);
        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(new ProductOrdersUpdatedEvent(
            order.ProductOrders.Select(order => order.ProductId).ToList()));

        return Unit.Value;        
    }
}
