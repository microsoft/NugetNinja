// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Core;

public class Extractor
{
    private readonly ILogger<Extractor> _logger;

    public Extractor(ILogger<Extractor> logger)
    {
        _logger = logger;
    }

    public async Task<Model> Parse(string rootPath)
    {
        var csprojs = Directory
            .EnumerateFiles(rootPath, "*.csproj", SearchOption.AllDirectories)
            .ToArray();

        var model = new Model();

        foreach (var csprojPath in csprojs)
        {
            _logger.LogTrace($"Parsing {csprojPath}...");
            await model.IncludeProject(csprojPath);
        }

        return model;
    }
}
