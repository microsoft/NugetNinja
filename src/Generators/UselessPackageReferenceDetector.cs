﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class UselessPackageReferenceDetector : IActionGenerator
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

    public string GetCommandAlias()
    {
        // To do:
        throw new NotImplementedException();
    }

    public string GetHelp()
    {
        // To do
        throw new NotImplementedException();
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