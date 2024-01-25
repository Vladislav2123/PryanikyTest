using FluentValidation;
using MediatR;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;

namespace PryanikyTest.Application.Features.Customers.Commands;

public record DeleteCustomerCommand(Guid CustomerId) : IRequest<Unit>;

public class DeleteCustomerValidator : AbstractValidator<DeleteCustomerCommand>
{
    public DeleteCustomerValidator()
    {
        RuleFor(command => command.CustomerId)
            .SetValidator(new GuidValidator());
    }
}

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteCustomerHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _dbContext.Customers
            .FindAsync(request.CustomerId, cancellationToken);

        if(customer == null) throw new EntityNotFoundException(nameof(Customer), request.CustomerId);

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}