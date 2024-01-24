﻿using AutoMapper;
using System.Reflection;

namespace PryanikyTest.Application.Mapping;
/// <summary>
/// Register mappings from assembly.
/// </summary>
public class AssemblyMappingProfile : Profile
{
    public AssemblyMappingProfile(Assembly assembly)
    {
        // Getting mapping types from assembly
        var types = assembly.GetExportedTypes()
            .Where(type => type.GetInterfaces()
            .Any(type => type.IsInterface && type == typeof(IMappping)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);
            var method = type.GetMethod("CreateMap");
            method?.Invoke(instance, new object[] { this });
        }
    }
}
