using PryanikyTest.Application.Features.Products.Commands;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Domain.Entities;
using FluentValidation;
using MediatR;

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
    private readonly IMediator _mediator;

    public DeleteSellerHandler(IApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(DeleteSellerCommand request, CancellationToken cancellationToken)
    {
        var seller = await _dbContext.Sellers
            .Include(seller => seller.Products)
            .FirstOrDefaultAsync(seller => seller.Id == request.SellerId, cancellationToken);

        if(seller == null) throw new EntityNotFoundException(nameof(Seller), request.SellerId);

        // Deleting all products of seller
        foreach(var product in seller.Products)
            await _mediator.Send(new DeleteProductCommand(product.Id), cancellationToken);

        _dbContext.Sellers.Remove(seller);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}