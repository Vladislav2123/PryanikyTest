using System.Reflection;
using PryanikyTest.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using FluentValidation;
using MediatR.NotificationPublishers;

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

        services.AddMediatR(cfg =>
        {
			cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.NotificationPublisher = new ForeachAwaitPublisher();
        });
        
        // Adding Validation Behavior, for handling all validators from assembly
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
