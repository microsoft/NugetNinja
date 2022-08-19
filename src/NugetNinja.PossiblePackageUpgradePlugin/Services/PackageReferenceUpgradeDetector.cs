// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PackageReferenceUpgradeDetector : IActionDetector
{
    private readonly NugetService _nugetService;

    public PackageReferenceUpgradeDetector(NugetService nugetService)
    {
        _nugetService = nugetService;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        foreach (var project in context.AllProjects)
        {
            foreach (var package in project.PackageReferences)
            {
                var latest = await _nugetService.GetLatestVersion(package.Name);
                if (package.Version < latest)
                {
                    yield return new PossiblePackageUpgrade(project, package, latest);
                }
            }
        }
    }
}
