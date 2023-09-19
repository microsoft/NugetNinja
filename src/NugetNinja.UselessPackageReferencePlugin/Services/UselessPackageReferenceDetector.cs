// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class UselessPackageReferenceDetector : IActionDetector
{
    private readonly ILogger<UselessPackageReferenceDetector> _logger;
    private readonly NugetService _nugetService;
    private readonly ProjectsEnumerator _enumerator;

    public UselessPackageReferenceDetector(
        ILogger<UselessPackageReferenceDetector> logger,
        NugetService nugetService,
        ProjectsEnumerator enumerator)
    {
        _logger = logger;
        _nugetService = nugetService;
        _enumerator = enumerator;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        foreach (var uselessReferences in context.AllProjects.Select(AnalyzeProject))
        {
            await foreach (var reference in uselessReferences)
            {
                yield return reference;
            }
        }
    }

    private async IAsyncEnumerable<UselessPackageReference> AnalyzeProject(Project context)
    {
        var accessiblePackages = new List<Package>();
        var relatedProjects = _enumerator.EnumerateAllBuiltProjects(context, false);
        foreach (var relatedProject in relatedProjects)
        {
            accessiblePackages.AddRange(relatedProject.PackageReferences);
            foreach(var package in relatedProject.PackageReferences)
            {
                try
                {
                    var recursivePackagesBroughtUp = await _nugetService.GetPackageDependencies(package: package);
                    accessiblePackages.AddRange(recursivePackagesBroughtUp);
                }
                catch (Exception e)
                {
                    _logger.LogTrace(e, $"Failed to get package dependencies by name: '{package}'.");
                    _logger.LogCritical($"Failed to get package dependencies by name: '{package}'.");
                }
            }
        }

        foreach (var directReference in context.PackageReferences)
        {
            var accessiblePackagesForThisProject = accessiblePackages.ToList();
            foreach (var otherDirectReference in context.PackageReferences.Where(p => p != directReference))
            {
                try
                {
                    var references = await _nugetService.GetPackageDependencies(package: otherDirectReference);
                    accessiblePackagesForThisProject.AddRange(references);
                }
                catch (Exception e)
                {
                    _logger.LogTrace(e, $"Failed to get package dependencies by name: '{otherDirectReference}'.");
                    _logger.LogCritical($"Failed to get package dependencies by name: '{otherDirectReference}'.");
                }
            }

            if (accessiblePackagesForThisProject.Any(pa => pa.Name == directReference.Name && pa.Version >= directReference.Version))
            {
                yield return new(context, directReference);
            }
        }
    }

    private bool GreaterVersion(NugetVersion version1, NugetVersion version2)
    {
        return version1 > version2;
    }

    public static int CompareNuGetVersions(string version1, string version2)
    {
        if (version1 == null || version2 == null)
        {
            throw new ArgumentNullException("Both versions must be non-null");
        }

        string[] parts1 = version1.Split('.');
        string[] parts2 = version2.Split('.');

        int maxLength = Math.Max(parts1.Length, parts2.Length);

        for (int i = 0; i < maxLength; i++)
        {
            int part1 = (i < parts1.Length) ? ParseVersionPart(parts1[i]) : 0;
            int part2 = (i < parts2.Length) ? ParseVersionPart(parts2[i]) : 0;

            if (part1 < part2)
            {
                return -1;
            }
            else if (part1 > part2)
            {
                return 1;
            }
        }

        return 0;
    }

    private static int ParseVersionPart(string part)
    {
        if (int.TryParse(part, out int result))
        {
            return result;
        }
        else
        {
            // Handle pre-release labels (e.g., "alpha", "beta")
            return int.MaxValue;
        }
    }
}
