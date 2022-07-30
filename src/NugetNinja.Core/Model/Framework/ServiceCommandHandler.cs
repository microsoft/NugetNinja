// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Core;

public abstract class ServiceCommandHandler<E, S> : CommandHandler
    where E : class, IEntryService
    where S : class, IStartUp, new()
{
    public override void OnCommandBuilt(Command command)
    {
        var globalOptions = OptionsProvider.GetGlobalOptions();
        command.SetHandler(
            Execute,
            OptionsProvider.PathOptions,
            OptionsProvider.DryRunOption,
            OptionsProvider.VerboseOption);
    }

    public Task Execute(string path, bool dryRun, bool verbose)
    {
        var services = this.BuildServices(verbose);
        return this.RunFromServices(services, path, dryRun);
    }

    protected virtual ServiceCollection BuildServices(bool verbose)
    {
        var services = new ServiceCollection();
        services.AddLogging(logging =>
        {
            logging
                .AddFilter("Microsoft.Extensions", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning);
            logging.AddSimpleConsole(options => 
            {
                options.IncludeScopes = verbose;
                options.SingleLine = true;
                options.TimestampFormat = "mm:ss ";
            });
            logging.SetMinimumLevel(verbose ? LogLevel.Trace : LogLevel.Warning);
        });

        var startUp = new S();
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddSingleton<CacheService>();
        services.AddTransient<Extractor>();
        services.AddTransient<ProjectsEnumerator>();
        services.AddTransient<NugetService>();
        startUp.ConfigureServices(services);
        services.AddTransient<E>();
        return services;
    }

    protected virtual Task RunFromServices(ServiceCollection services, string path, bool dryRun)
    {
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<E>();
        var logger = serviceProvider.GetRequiredService<ILogger<E>>();

        var fullPath = Path.GetFullPath(path);
        logger.LogTrace($"Starting service: '{typeof(E).Name}'. Full path is: '{fullPath}', Dry run is: '{dryRun}'.");
        return service.OnServiceStartedAsync(fullPath, shouldTakeAction: !dryRun);
    }
}
