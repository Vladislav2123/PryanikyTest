using PryanikyTest.Application.Features.Orders.Dto;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Domain.Entities;
using FluentValidation;
using AutoMapper;
using MediatR;
using PryanikyTest.Application.Features.Orders.Events;

namespace PryanikyTest.Application.Features.Orders.Commands;

public record ProductOrderVm(
    Guid ProductId,
    int Amount
);

public class ProductOrderValidator : AbstractValidator<ProductOrderVm>
{
    public ProductOrderValidator()
    {
        RuleFor(vm => vm.ProductId)
            .SetValidator(new GuidValidator());

        RuleFor(vm => vm.Amount)
            .NotNull()
            .GreaterThan(0);
    }
}

public record CreateOrderCommand(
    ICollection<ProductOrderVm> ProductOrders,
    Guid CustomerId
) : IRequest<OrderCreatedDto>;

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(command => command.CustomerId)
            .SetValidator(new GuidValidator());

        RuleFor(command => command.ProductOrders)
            .Must(orders => orders.Any())
            .WithMessage("At least one product in order required");

        RuleForEach(command => command.ProductOrders)
            .SetValidator(new ProductOrderValidator());
    }
}

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderCreatedDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IPublisher _publisher;

    public CreateOrderHandler(IApplicationDbContext dbContext, IMapper mapper, IPublisher publisher)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _publisher = publisher;
    }

    public async Task<OrderCreatedDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Validation of client existent
        if(await _dbContext.Customers
            .AnyAsync(customer => customer.Id == request.CustomerId, cancellationToken) == false)
                throw new EntityNotFoundException(nameof(Customer), request.CustomerId);

        // Validation of products in the order existent
        foreach(var productOrder in request.ProductOrders)
        {
            if (await _dbContext.Products
                .AnyAsync(product => product.Id == productOrder.ProductId, cancellationToken) == false)
                    throw new EntityNotFoundException(nameof(Product), productOrder.ProductId);
        }

        Guid newOrderId = Guid.NewGuid();

        // Adding Product Orders to database
        ICollection<ProductOrder> productOrders = GetProductOrders(request.ProductOrders, newOrderId);
        await _dbContext.ProductOrders.AddRangeAsync(productOrders, cancellationToken);

        var order = new Order
        {
            Id = newOrderId,
            ProductOrders = productOrders,
            CreationDate = DateTime.UtcNow,
            Completed = false,
            CustomerId = request.CustomerId
        };

        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        await _publisher.Publish(new ProductOrdersUpdatedEvent(
            productOrders.Select(order => order.ProductId).ToList()));

        return _mapper.Map<OrderCreatedDto>(order);
    }

    /// <summary>
    /// Mapping ProductOrderVm collection to ProductOrder collection.
    /// </summary>
    private ICollection<ProductOrder> GetProductOrders(ICollection<ProductOrderVm> productOrderVms, Guid orderId)
    {
        var result = new List<ProductOrder>(productOrderVms.Count);

        foreach (var vm in productOrderVms)
        {
            // If Product Order with same product already in the list, 
            // then add amount to this product order and skip current vm
            var sameProductOrder = result.Find(product => product.ProductId == vm.ProductId);
            if(sameProductOrder != null)
            {
                sameProductOrder.Amount += vm.Amount;
                continue;
            }

            result.Add(new ProductOrder
            {
                Id = Guid.NewGuid(),
                ProductId = vm.ProductId,
                Amount = vm.Amount,
                OrderId = orderId,
            });
        }

        return result;
    }
}


