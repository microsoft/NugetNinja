// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class UselessProjectReferenceDetector : IActionGenerator
{
    private readonly ProjectsEnumerator enumerator;

    public string[] CommandAliases => new[] { "check-project-reference", "cpj" };
    public string CommandDescription => "Check for useless project references and try to remove them.";

    public UselessProjectReferenceDetector(ProjectsEnumerator enumerator)
    {
        this.enumerator = enumerator;
    }

    public IEnumerable<IAction> Analyze(Model context)
    {
        foreach (var rootProject in context.AllProjects)
        {
            var uselessReferences = this.AnalyzeProject(rootProject);
            foreach(var reference in uselessReferences)
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
