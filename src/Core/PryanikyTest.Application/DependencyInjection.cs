using System.Reflection;
using PryanikyTest.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace PryanikyTest.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Adding AutoMapper Assyembly Mapping Profile
        services.AddAutoMapper(cfg =>
            cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));
    }
}
