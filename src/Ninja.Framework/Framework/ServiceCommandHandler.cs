// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Framework;

public abstract class ServiceCommandHandler<E, S> : CommandHandler 
    where E : class, IEntryService
    where S : class, IStartUp, new()
{
    public override Task Execute(string path, bool dryRun, bool verbose)
    {
        var services = new ServiceCollection();
        services.AddLogging(logging =>
        {
            logging.AddConsole();
            logging.SetMinimumLevel(verbose ? LogLevel.Trace : LogLevel.Warning);
        });

        var startUp = new S();
        startUp.ConfigureServices(services);

        services.AddTransient<E>();

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<E>();
        var logger = serviceProvider.GetRequiredService<ILogger<E>>();

        var fullPath = Path.GetFullPath(path);
        logger.LogTrace($"Starting service: '{typeof(E).Name}'. Full path is: '{fullPath}', Dry run is: '{dryRun}'.");
        return service.OnServiceStartedAsync(fullPath, shouldTakeAction: !dryRun);
    }
}
