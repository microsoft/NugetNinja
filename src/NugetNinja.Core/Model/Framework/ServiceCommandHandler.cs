// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Core;

public abstract class ServiceCommandHandler<TE, TS> : CommandHandler
    where TE : class, IEntryService
    where TS : class, IStartUp, new()
{
    public override void OnCommandBuilt(Command command)
    {
        command.SetHandler(
            Execute,
            OptionsProvider.PathOptions,
            OptionsProvider.DryRunOption,
            OptionsProvider.VerboseOption,
            OptionsProvider.AllowPreviewOption,
            OptionsProvider.CustomNugetServer,
            OptionsProvider.PatToken);
    }

    public Task Execute(string path, bool dryRun, bool verbose, bool allowPreview, string customNugetServer, string patToken)
    {
        var services = BuildServices(verbose, allowPreview, customNugetServer, patToken);
        return RunFromServices(services, path, dryRun);
    }

    protected virtual ServiceCollection BuildServices(bool verbose, bool allowPreview, string customNugetServer, string patToken)
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

        var startUp = new TS();
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddSingleton<CacheService>();
        services.AddTransient<Extractor>();
        services.AddTransient<ProjectsEnumerator>();
        services.AddTransient<NugetService>();

        if (!string.IsNullOrWhiteSpace(customNugetServer))
        {
            NugetService.CustomNugetServer = customNugetServer;
        }
        NugetService.AllowPreview = allowPreview;
        NugetService.PatToken = patToken;
        
        startUp.ConfigureServices(services);
        services.AddTransient<TE>();
        return services;
    }

    protected virtual Task RunFromServices(ServiceCollection services, string path, bool dryRun)
    {
        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<TE>();
        var logger = serviceProvider.GetRequiredService<ILogger<TE>>();

        var fullPath = Path.GetFullPath(path);
        logger.LogTrace($"Starting service: '{typeof(TE).Name}'. Full path is: '{fullPath}', Dry run is: '{dryRun}'.");
        return service.OnServiceStartedAsync(fullPath, shouldTakeAction: !dryRun);
    }
}
