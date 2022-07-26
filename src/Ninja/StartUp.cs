// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja;

public class StartUp : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        var detectors = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass)
            .Where(t => t.GetInterfaces().Contains(typeof(IActionDetector)));

        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddSingleton<CacheService>();
        services.AddTransient<NugetService>();
        services.AddTransient<Extractor>();
        services.AddTransient<ProjectsEnumerator>();

        foreach (var detector in detectors)
        {
            services.AddTransient(detector);
        }
    }
}
