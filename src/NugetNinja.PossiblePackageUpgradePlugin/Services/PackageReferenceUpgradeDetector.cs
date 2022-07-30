// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PackageReferenceUpgradeDetector : IActionDetector
{
    private readonly PackageUpgradeHandlerOptions _options;
    private readonly NugetService _nugetService;
    private readonly ILogger<PackageReferenceUpgradeDetector> _logger;

    public PackageReferenceUpgradeDetector(
        PackageUpgradeHandlerOptions options,
        NugetService nugetService,
        ILogger<PackageReferenceUpgradeDetector> logger)
    {
        _options = options;
        _nugetService = nugetService;
        _logger = logger;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        if (_options.UsePreview)
        {
            _logger.LogInformation($"AllowPreview flag set as : '{_options.UsePreview}'. Will use preview versions.");
        }

        if (string.IsNullOrWhiteSpace(_options.CustomNugetServer))
        {
            _options.CustomNugetServer = "https://api.nuget.org/v3/index.json";
        }

        foreach (var project in context.AllProjects)
        {
            foreach (var package in project.PackageReferences)
            {
                var latest = await _nugetService.GetLatestVersion(
                    package.Name,
                    _options.CustomNugetServer,
                    _options.PatToken,
                    _options.UsePreview);
                if (package.Version < latest)
                {
                    yield return new PossiblePackageUpgrade(project, package, latest);
                }
            }
        }
    }
}
