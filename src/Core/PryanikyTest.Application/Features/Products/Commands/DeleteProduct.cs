using FluentValidation;
using MediatR;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Exceptions;

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
            .FindAsync(request.ProductId, cancellationToken);

        if(product == null) throw new EntityNotFoundException(nameof(product), request.ProductId);

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}

