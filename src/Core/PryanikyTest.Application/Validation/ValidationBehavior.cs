using System.Xml;
using FluentValidation;
using MediatR;
using ValidationException = PryanikyTest.Domain.Exceptions.ValidationException;

namespace PryanikyTest.Application;
/// <summary>
/// Pipline Behavior for handling validators from DI container
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any() == false) return await next();

        var context = new ValidationContext<TRequest>(request);

        // Handling all current request validators and adding adding errors to dictionary
        var failuresDictionary = _validators
            .Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .GroupBy(
            failure => failure.PropertyName,
            failure => failure.ErrorMessage,
            (propertyName, errorMessages) => new
            {
                Key = propertyName,
                Values = errorMessages.Distinct().ToArray()
            })
            .ToDictionary(x => x.Key, x => x.Values);

        // Throwing exception if there is any validaiton failures
        if (failuresDictionary.Any())
        {
            throw new ValidationException(request.GetType().Name, failuresDictionary);
        }

        return await next();
    }
}
