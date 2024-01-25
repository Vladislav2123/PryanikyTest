using System.Text.Json;
using PryanikyTest.Domain.Exceptions;
using ValidationException = PryanikyTest.Domain.Exceptions.ValidationException;

namespace PryanikyTest.API.ExceptionsHandling;

public class GlobalExceptionsHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(Exception exception) 
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        int statusCode = GetStatusCode(exception);

        // Anonymous type as response
        var response = new
        {
            status = statusCode,
            message = exception.Message,
            errors = GetErrors(exception)
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private int GetStatusCode(Exception exception) => exception switch
    {
        
        EntityAlreadyExistException => StatusCodes.Status400BadRequest,
        EmailAlreadyInUseException => StatusCodes.Status400BadRequest,
        EntityNotFoundException => StatusCodes.Status404NotFound,
        ValidationException => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status500InternalServerError
    };

    private IReadOnlyDictionary<string, string[]>? GetErrors(Exception exception)
	{
		IReadOnlyDictionary<string, string[]>? errors = null;

		if (exception is ValidationException validationException)
		{
			errors = validationException.ErrorsDictionary;
		}

		return errors;
	}
}
