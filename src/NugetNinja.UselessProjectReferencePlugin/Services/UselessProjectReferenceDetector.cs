// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessProjectReferencePlugin;

public class UselessProjectReferenceDetector : IActionDetector
{
    private readonly ProjectsEnumerator _enumerator;

    public UselessProjectReferenceDetector(ProjectsEnumerator enumerator) => _enumerator = enumerator;

    public IAsyncEnumerable<IAction> AnalyzeAsync(Model context) => Analyze(context).ToAsyncEnumerable();

    private IEnumerable<IAction> Analyze(Model context) => context.AllProjects.SelectMany(AnalyzeProject);

    private IEnumerable<UselessProjectReference> AnalyzeProject(Project context)
    {
        var directReferences = context.ProjectReferences;

        var allRecursiveReferences = new List<Project>();
        foreach (var recursiveReferences in 
                 directReferences.Select(directReference => _enumerator.EnumerateAllBuiltProjects(directReference, includeSelf: false)))
        {
            allRecursiveReferences.AddRange(recursiveReferences);
        }

        foreach (var directReference in directReferences.Where(directReference => allRecursiveReferences.Contains(directReference)))
        {
            yield return new(context, directReference);
        }
    }
}
