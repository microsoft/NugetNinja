// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Services.Analyser;

namespace Microsoft.NugetNinja;

public abstract class ServiceCommandHandler<T> : CommandHandler where T : class, IEntryService
{
    public override Task Execute(string path, bool dryRun, bool verbose)
    {
        var services = new ServiceCollection();
        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(verbose ? LogLevel.Trace : LogLevel.Warning);
        });

        var detectors = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass)
            .Where(t => t.GetInterfaces().Contains(typeof(IActionDetector)));

        services.AddTransient<Extractor>();
        services.AddTransient<ProjectsEnumerator>();
        services.AddTransient<T>();

        foreach(var detector in detectors)
        {
            services.AddTransient(detector);
        }

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<T>();
        var logger = serviceProvider.GetRequiredService<ILogger<T>>();

        var fullPath = Path.GetFullPath(path);
        logger.LogTrace($"Starting service: '{typeof(T).Name}'. Full path is: '{fullPath}', Dry run is: '{dryRun}'.");
        return service.OnServiceStartedAsync(fullPath, shouldTakeAction: !dryRun);
    }
}
