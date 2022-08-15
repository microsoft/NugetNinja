// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class UselessPackageReferenceDetector : IActionDetector
{
    private readonly NugetService _nugetService;
    private readonly ProjectsEnumerator enumerator;

    public UselessPackageReferenceDetector(
        NugetService nugetService,
        ProjectsEnumerator enumerator)
    {
        _nugetService = nugetService;
        this.enumerator = enumerator;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        var refe = await this._nugetService.GetPackageDependencies(new Package("aiursoft.sdk", "6.0.7"), NugetService.DefaultNugetServer, string.Empty);
        foreach (var rootProject in context.AllProjects)
        {
            var uselessReferences = this.AnalyzeProject(rootProject);
            await foreach (var reference in uselessReferences)
            {
                yield return reference;
            }
        }
    }

    private async IAsyncEnumerable<UselessPackageReference> AnalyzeProject(Project context)
    {
        var relatedProjects = enumerator.EnumerateAllBuiltProjects(context, false);
        var accessiablePackages = new List<Package>();
        foreach (var relatedProject in relatedProjects)
        {
            accessiablePackages.AddRange(relatedProject.PackageReferences);
            foreach(var package in relatedProject.PackageReferences)
            {
                var recursivePackagesBroughtUp = await this._nugetService.GetPackageDependencies(
                    package: package,
                    nugetServer: NugetService.DefaultNugetServer,
                    patToken: string.Empty);
                accessiablePackages.AddRange(recursivePackagesBroughtUp);
            }
        }

        foreach (var directReference in context.PackageReferences)
        {
            var accessiablePackagesForThisProject = accessiablePackages.ToList();
            foreach (var otherDirectReference in context.PackageReferences.Where(p => p != directReference))
            {
                var references = await this._nugetService.GetPackageDependencies(
                    package: otherDirectReference,
                    nugetServer: NugetService.DefaultNugetServer,
                    patToken: string.Empty);
                accessiablePackagesForThisProject.AddRange(references);
            }

            if (accessiablePackagesForThisProject.Any(pa => pa.Name == directReference.Name))
            {
                yield return new UselessPackageReference(context, directReference);
            }
        }
    }
}
