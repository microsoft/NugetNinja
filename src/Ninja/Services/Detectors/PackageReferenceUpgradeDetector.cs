// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja;

public class PackageReferenceUpgradeDetector : IActionDetector
{
    private readonly NugetService _nugetService;
    private readonly ILogger<PackageReferenceUpgradeDetector> _logger;

    public PackageReferenceUpgradeDetector(
        NugetService nugetService,
        ILogger<PackageReferenceUpgradeDetector> logger)
    {
        _nugetService = nugetService;
        _logger = logger;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        foreach (var project in context.AllProjects)
        {
            foreach (var package in project.PackageReferences)
            {
                var latest = await this._nugetService.GetLatestVersion(package.Name);
                if (package.Version < latest)
                {
                    yield return new PossiblePackageUpgrade(project, package, latest);
                }
            }
        }
    }
}
