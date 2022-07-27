// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class UselessPackageReferenceDetector : IActionDetector
{
    private readonly ProjectsEnumerator enumerator;

    public UselessPackageReferenceDetector(ProjectsEnumerator enumerator)
    {
        this.enumerator = enumerator;
    }

    public IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        return this.Analyze(context).ToAsyncEnumerable();
    }

    private IEnumerable<IAction> Analyze(Model context)
    {
        foreach (var rootProject in context.AllProjects)
        {
            var uselessReferences = this.AnalyzeProject(rootProject);
            foreach (var reference in uselessReferences)
            {
                yield return reference;
            }
        }
    }

    private IEnumerable<UselessPackageReference> AnalyzeProject(Project context)
    {
        var allRelatedProjects = enumerator.EnumerateAllBuiltProjects(context, false);
        var allPackagesBroughtUp = allRelatedProjects.SelectMany(p => p.PackageReferences).ToArray();

        foreach (var directReference in context.PackageReferences)
        {
            if (allPackagesBroughtUp.Any(pa => pa.Name == directReference.Name))
            {
                yield return new UselessPackageReference(context, directReference);
            }
        }
    }
}
