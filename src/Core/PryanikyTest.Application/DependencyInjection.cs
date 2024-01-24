using System.Reflection;
using LibraryApp.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace PryanikyTest.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg =>
            cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly())));
    }
}
