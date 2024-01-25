using FluentValidation;
using MediatR;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;

namespace PryanikyTest.Application.Features.Sellers.Commands;

public record DeleteSellerCommand(Guid SellerId) : IRequest<Unit>;

public class DeleteSellerValidator : AbstractValidator<DeleteSellerCommand>
{
    public DeleteSellerValidator()
    {
        RuleFor(command => command.SellerId)
            .SetValidator(new GuidValidator());   
    }
}

public class DeleteSellerHandler : IRequestHandler<DeleteSellerCommand, Unit>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteSellerHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Unit> Handle(DeleteSellerCommand request, CancellationToken cancellationToken)
    {
        var seller = await _dbContext.Sellers
            .FindAsync(request.SellerId, cancellationToken);

        if(seller == null) throw new EntityNotFoundException(nameof(Seller), request.SellerId);

        _dbContext.Sellers.Remove(seller);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}