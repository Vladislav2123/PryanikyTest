using System.Reflection;
using PryanikyTest.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;

namespace PryanikyTest.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Adding AutoMapper Assyembly Mapping Profile
        services.AddAutoMapper(cfg =>
            cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));

        // Adding all validators from assebly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        // Adding Validation Behavior, for handling all validators from assembly
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
