using AutoMapper;
using FluentValidation;
using MediatR;
using PryanikyTest.Application.Abstractions;
using PryanikyTest.Application.Features.Customers.Dto;
using PryanikyTest.Domain.Entities;
using PryanikyTest.Domain.Exceptions;

namespace PryanikyTest.Application.Features.Customers.Commands;

public record CreateCustomerCommand(
    string Name,
    string Email,
    string Password
) : IRequest<CustomerCreatedDto>;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
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

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerCreatedDto>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCustomerHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CustomerCreatedDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var sameEmailCustomer = await _dbContext.Customers
            .FindAsync(request.Email, cancellationToken);

        if(sameEmailCustomer != null) throw new EmailAlreadyInUseException(request.Email, nameof(Customer));

        var newCustomer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };

        await _dbContext.Customers.AddAsync(newCustomer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerCreatedDto>(newCustomer);
    }
}