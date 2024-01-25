using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;
using FluentValidation;
using MediatR;

namespace PryanikyTest.Application.Features.Orders.Commands;

public record UpdateOrderStatusCommand(
    Guid OrderId,
    bool Completed
) : IRequest<Unit>;

public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusValidator()
    {
        RuleFor(command => command.OrderId)
            .SetValidator(new GuidValidator());

        RuleFor(command => command.Completed)
            .NotNull();
    }
}

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateOrderStatusHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .FindAsync(request.OrderId, cancellationToken);

        if(order == null) throw new EntityNotFoundException(nameof(Order), request.OrderId);

        order.Completed = request.Completed;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
