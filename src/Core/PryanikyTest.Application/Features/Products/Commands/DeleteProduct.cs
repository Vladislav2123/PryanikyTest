using Microsoft.EntityFrameworkCore;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace PryanikyTest.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid ProductId) : IRequest<Unit>;

public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidator()
    {
        RuleFor(command => command.ProductId)
           .SetValidator(new GuidValidator());
    }
}

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteProductHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Include(product => product.ProductOrders)
            .FirstOrDefaultAsync(product => product.Id == request.ProductId, cancellationToken);

        if(product == null) throw new EntityNotFoundException(nameof(product), request.ProductId);

        // Deleting product orders of product
        foreach(var productOrder in product.ProductOrders)
        {
            var order = await _dbContext.Orders
                .Include(order => order.ProductOrders)
                .FirstOrDefaultAsync(order => order.Id == productOrder.OrderId, cancellationToken);

            _dbContext.ProductOrders.Remove(productOrder);

            if(order.ProductOrders.Count == 1) _dbContext.Orders.Remove(order);
        }

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

