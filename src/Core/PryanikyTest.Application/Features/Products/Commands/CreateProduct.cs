using PryanikyTest.Application.Features.Products.Dto;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Validation;
using PryanikyTest.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using PryanikyTest.Domain.Entities;
using FluentValidation;
using AutoMapper;
using MediatR;

namespace PryanikyTest.Application.Features.Products.Commands;

public record CreateProductCommand(
    Guid SellerId,
    string Name,
    string? Description,
    int Price)
    : IRequest<ProductCreatedDto>;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(command => command.SellerId)
            .SetValidator(new GuidValidator());

        RuleFor(command => command.Name)
            .MinimumLength(2)
            .MaximumLength(64);

        RuleFor(command => command.Description)
            .MaximumLength(300);

        RuleFor(command => command.Price)
            .NotNull()
            .GreaterThan(0);
    }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductCreatedDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateProductHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ProductCreatedDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var seller = await _dbContext.Sellers
            .FindAsync(request.SellerId, cancellationToken);

        if (seller == null) throw new EntityNotFoundException(nameof(Seller), request.SellerId);

        var sameNameProduct = await _dbContext.Products
            .FirstOrDefaultAsync(product => product.Name == request.Name, cancellationToken);

        if (sameNameProduct != null) throw new EntityAlreadyExistException(nameof(Product));

        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreationDate = DateTime.UtcNow,
            SellerId = request.SellerId,
        };

        await _dbContext.Products.AddAsync(newProduct, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<ProductCreatedDto>(newProduct);
    }
}


