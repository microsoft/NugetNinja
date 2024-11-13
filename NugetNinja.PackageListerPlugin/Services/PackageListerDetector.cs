// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PackageListerPlugin;

public class PackageListerDetector : IActionDetector
{
    private readonly ILogger<PackageListerDetector> _logger;
    private readonly bool _fillInOutputType = false;
    private readonly bool _enforceNullable = false;
    private readonly bool _enforceImplicitUsings = false;
    private readonly string[] _notSupportedRuntimes = {
        "net5.0",
        "netcoreapp3.0",
        "netcoreapp2.2",
        "netcoreapp2.1",
        "netcoreapp1.1",
        "netcoreapp1.0",
    };

    private readonly string _suggestedRuntime = "net8.0";

    public PackageListerDetector(
        ILogger<PackageListerDetector> logger)
    {
        _logger = logger;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        _logger.LogInformation("Analyzing package list");
        await Task.CompletedTask;
        var allPackages = new List<(string,List<Package>)>();
        foreach (var project in context.AllProjects)
        {
            allPackages.Add((project.PathOnDisk,project.PackageReferences));
        }
        yield return new PackageList(allPackages, _logger);
    }
}
