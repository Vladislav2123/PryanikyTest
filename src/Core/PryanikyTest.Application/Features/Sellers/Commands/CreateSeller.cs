using PryanikyTest.Application.Features.Sellers.Dto;
using MediatR;
using FluentValidation;
using PryanikyTest.Application.Abstractions;
using AutoMapper;
using PryanikyTest.Domain.Exceptions;
using PryanikyTest.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace PryanikyTest.Application.Features.Sellers.Commands;

public record CreateSellerCommand(
    string Name,
    string Email,
    string Password
 ) : IRequest<SellerCreatedDto>;

public class CreateSellerValidator : AbstractValidator<CreateSellerCommand>
{
    public CreateSellerValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(64)
            .MinimumLength(2);

        RuleFor(command => command.Email)
            .NotEmpty()
            .MaximumLength(100)
            .EmailAddress();

        RuleFor(command => command.Password)
            .MinimumLength(6)
            .MaximumLength(32);
    }
}

public class CreateSellerHandler : IRequestHandler<CreateSellerCommand, SellerCreatedDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateSellerHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<SellerCreatedDto> Handle(CreateSellerCommand request, CancellationToken cancellationToken)
    {
        var sameEmailSeller = await _dbContext.Sellers
            .FirstOrDefaultAsync(seller => seller.Email == request.Email, cancellationToken);
        
        if(sameEmailSeller != null) throw new EmailAlreadyInUseException(request.Email, nameof(Seller));

        var newSeller = new Seller
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = request.Password,
            Products = new List<Product>()
        };

        await _dbContext.Sellers.AddAsync(newSeller, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<SellerCreatedDto>(newSeller);
    }
}