// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageDetector : IActionDetector
{
    private readonly DeprecatedPackageHandlerOptions _options;
    private readonly NugetService _nugetService;

    public DeprecatedPackageDetector(
        DeprecatedPackageHandlerOptions options,
        NugetService nugetService)
    {
        _options = options;
        _nugetService = nugetService;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        if (string.IsNullOrWhiteSpace(_options.CustomNugetServer))
        {
            _options.CustomNugetServer = NugetService.DefaultNugetServer;
        }

        foreach (var project in context.AllProjects)
        {
            foreach (var package in project.PackageReferences)
            {
                var catalogInformation = await _nugetService.GetPackageDeprecationInfo(
                    package,
                    _options.CustomNugetServer,
                    _options.PatToken);
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
