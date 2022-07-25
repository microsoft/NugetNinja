// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace Microsoft.NugetNinja;

public class Entry
{
    private readonly Extractor extractor;
    private readonly IEnumerable<IActionGenerator> generators;
    private readonly ILogger<Entry> logger;

    public Entry(
        Extractor extractor,
        IEnumerable<IActionGenerator> generators,
        ILogger<Entry> logger)
    {
        this.extractor = extractor;
        this.generators = generators;
        this.logger = logger;
    }

    public async Task<int> RunAsync(string[] args)
    {
        var rootCommand = new RootCommand(@"🥷 Nuget Ninja, a tool for detecting dependencies of .NET projects.");

        var dryRunOption = new Option<bool>(
            aliases: new[] { "--dry-run", "-d" },
            description: "Preview changes without actually making them");

        var pathOption = new Option<string>(
            aliases: new[] { "--path", "-p" },
            description: "Path of the project to be scanned")
        {
            IsRequired = true
        };

        rootCommand.AddGlobalOption(dryRunOption);
        rootCommand.AddGlobalOption(pathOption);

        foreach (var generator in this.generators)
        {
            var subCommand = new Command(generator.CommandAliases.First(), generator.CommandDescription);
            foreach (var alias in generator.CommandAliases.Skip(1))
            {
                subCommand.AddAlias(alias);
            }

            subCommand.SetHandler(async (path, isDryRun) =>
                {
                    logger.LogTrace($"Args: path = {path}, isDryRun = {isDryRun}");

                    var model = await extractor.Parse(path);
                    var actions = generator.Analyze(model);
                    foreach (var action in actions)
                    {
                        action.BuildMessage();
                        if (!isDryRun)
                        {
                            action.TakeAction();
                        }
                    }
                },
                pathOption, dryRunOption);

            rootCommand.Add(subCommand);
        }

        return await rootCommand.InvokeAsync(args);
    }
}
