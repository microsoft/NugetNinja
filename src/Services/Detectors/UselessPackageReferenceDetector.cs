// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Services.Analyser;

namespace Microsoft.NugetNinja;

public class UselessPackageReferenceDetector : IActionDetector
{
    private readonly ProjectsEnumerator enumerator;

    public UselessPackageReferenceDetector(ProjectsEnumerator enumerator)
    {
        this.enumerator = enumerator;
    }

    public IEnumerable<IAction> Analyze(Model context)
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
