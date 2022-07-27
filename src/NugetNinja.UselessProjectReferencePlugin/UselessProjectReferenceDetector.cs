// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessProjectReferencePlugin;

public class UselessProjectReferenceDetector : IActionDetector
{
    private readonly ProjectsEnumerator enumerator;

    public UselessProjectReferenceDetector(ProjectsEnumerator enumerator)
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

    public string GetCommandAlias()
    {
        throw new NotImplementedException();
    }

    public string GetHelp()
    {
        throw new NotImplementedException();
    }

    private IEnumerable<UselessProjectReference> AnalyzeProject(Project context)
    {
        var directReferences = context.ProjectReferences;

        var allRecursiveReferences = new List<Project>();
        foreach (var directReference in directReferences)
        {
            var recursiveReferences = enumerator.EnumerateAllBuiltProjects(directReference, includeSelf: false);
            allRecursiveReferences.AddRange(recursiveReferences);
        }

        foreach (var directReference in directReferences)
        {
            if (allRecursiveReferences.Contains(directReference))
            {
                yield return new UselessProjectReference(context, directReference);
            }
        }
    }
}
