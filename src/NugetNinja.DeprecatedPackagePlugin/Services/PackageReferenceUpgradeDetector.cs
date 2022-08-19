// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageDetector : IActionDetector
{
    private readonly ILogger<DeprecatedPackageDetector> _logger;
    private readonly NugetService _nugetService;

    public DeprecatedPackageDetector(
        ILogger<DeprecatedPackageDetector> logger,
        NugetService nugetService)
    {
        _logger = logger;
        _nugetService = nugetService;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        foreach (var project in context.AllProjects)
        {
            foreach (var package in project.PackageReferences)
            {
                CatalogInformation? catalogInformation;
                try
                {
                    catalogInformation = await _nugetService.GetPackageDeprecationInfo(package);
                }
                catch (Exception e)
                {
                    _logger.LogCritical(e, $"Failed to get package deprecation info by name: '{package}'.");
                    continue;
                }
                if (catalogInformation.Deprecation != null)
                {
                    yield return new DeprecatedPackageReplacement(project, package, catalogInformation.Deprecation.AlternatePackage?.Id);
                }
                else if(catalogInformation.Vulnerabilities.Any())
                {
                    yield return new VulnerablePackageReplacement(project, package);
                }
            }
        }
    }
}
