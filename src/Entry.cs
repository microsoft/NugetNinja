// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

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

    public async Task RunAsync(string[] args)
    {
        logger.LogInformation("Starting NugetNinja...");

        if (args.Length < 1)
        {
            logger.LogWarning("Usage: [Working path]");
            return;
        }

        var workingPath = args[0];
        var model = await extractor.Parse(workingPath);

        foreach(var generator in this.generators)
        {
            var actions = generator.Analyze(model);
            foreach (var action in actions)
            {
                logger.LogWarning(action.BuildMessage());
            }
        }

        logger.LogTrace("Stopping NugetNinja...");
    }
}
