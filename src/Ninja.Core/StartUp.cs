// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.Core;

public class StartUp : IStartUp
{
    public static CommandHandler[] GetCommandHandlers()
    {
        var subCommands = ClassScanner
            .AllAccessibleClass()
            .Where(t => t.IsSubclassOf(typeof(CommandHandler)))
            .Select(t => Activator.CreateInstance(t) as CommandHandler
                ?? throw new TypeLoadException($"Failed when creating new instance from type: {t}"))
            .ToArray();
        return subCommands;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddSingleton<CacheService>();
        services.AddTransient<NugetService>();
        services.AddTransient<Extractor>();
        services.AddTransient<ProjectsEnumerator>();

        var detectors = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass)
            .Where(t => t.GetInterfaces().Contains(typeof(IActionDetector)));

        foreach (var detector in detectors)
        {
            services.AddTransient(detector);
        }
    }
}
