// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin;

public class MissingPropertyDetector : IActionDetector
{
    private readonly ILogger<MissingPropertyDetector> _logger;
    private readonly bool _fillInOutputType = false;
    private readonly bool _enforceNullable = false;
    private readonly bool _enforceImplicitUsings = false;
    private readonly string[] _notSupportedRuntimes = {
        "net5.0",
        "netcoreapp3.0",
        "netcoreapp2.2",
        "netcoreapp2.1",
        "netcoreapp1.1",
        "netcoreapp1.0",
    };

    private readonly string _suggestedRuntime = "net6.0";

    public MissingPropertyDetector(
        ILogger<MissingPropertyDetector> logger)
    {
        _logger = logger;
    }

    public async IAsyncEnumerable<IAction> AnalyzeAsync(Model context)
    {
        await Task.CompletedTask;
        foreach (var project in context.AllProjects)
        {
            var versionSuggestion = AnalyzeVersion(project);
            if (versionSuggestion != null)
            {
                yield return versionSuggestion;
            }

            if (string.IsNullOrWhiteSpace(project.Nullable) && _enforceNullable)
                yield return new MissingProperty(project, nameof(project.Nullable), "enable");
            if (string.IsNullOrWhiteSpace(project.ImplicitUsings) && _enforceImplicitUsings)
                yield return new MissingProperty(project, nameof(project.ImplicitUsings), "enable");

            if (
                project.PackageReferences.Any(p => p.Name == "Microsoft.AspNetCore.App") ||
                project.PackageReferences.Any(p => p.Name == "Microsoft.AspNetCore.All") // Is an old Web Project.
                )
            {
                if (project.PackageReferences.FirstOrDefault(p => p.Name == "Microsoft.AspNetCore.App") is not null)
                    yield return new ObsoletePackageReference(project, "Microsoft.AspNetCore.App");
                if (project.PackageReferences.FirstOrDefault(p => p.Name == "Microsoft.AspNetCore.All") is not null)
                    yield return new ObsoletePackageReference(project, "Microsoft.AspNetCore.All");
                if (project.PackageReferences.FirstOrDefault(p => p.Name == "Microsoft.AspNetCore.Razor.Design") is not null)
                    yield return new ObsoletePackageReference(project, "Microsoft.AspNetCore.Razor.Design");

                if (project.Sdk?.Equals("Microsoft.NET.Sdk.Web", StringComparison.OrdinalIgnoreCase) == false)
                {
                    yield return new InsertFrameworkReference(project, "Microsoft.AspNetCore.App");
                }
            }

            // Skip executable programs.
            if (project.Executable())
            {
                _logger.LogTrace($"Skip scanning properties for project: '{project}' because it's an executable.");
                continue;
            }

            if (project.IsTest())
            {
                _logger.LogTrace($"Skip scanning properties for project: '{project}' because it's an unit test project.");
                continue;
            }

            // Fill in properties for class lib.
            if (string.IsNullOrWhiteSpace(project.OutputType) && _fillInOutputType)
                yield return new MissingProperty(project, nameof(project.OutputType), "Library");

            // To do: Load those properties from GitHub API.
            //if (string.IsNullOrWhiteSpace(project.PackageLicenseExpression) && string.IsNullOrWhiteSpace(project.PackageLicenseFile))
            //    yield return new MissingProperty(project, nameof(project.PackageLicenseExpression), "MIT");
            //if (string.IsNullOrWhiteSpace(project.Description))
            //    yield return new MissingProperty(project, nameof(project.Description), "A library that shared to nuget.");
            //if (string.IsNullOrWhiteSpace(project.Company))
            //    yield return new MissingProperty(project, nameof(project.Company), "Contoso");
            //if (string.IsNullOrWhiteSpace(project.Product))
            //    yield return new MissingProperty(project, nameof(project.Product), project.ToString());
            //if (string.IsNullOrWhiteSpace(project.Authors))
            //    yield return new MissingProperty(project, nameof(project.Authors), $"{project}Team");
            //if (string.IsNullOrWhiteSpace(project.PackageTags))
            //    yield return new MissingProperty(project, nameof(project.PackageTags), $"nuget tools extensions");
            //if (string.IsNullOrWhiteSpace(project.PackageProjectUrl))
            //    yield return new MissingProperty(project, nameof(project.PackageProjectUrl), $"https://github.com/Microsoft/Nugetninja");
            //if (string.IsNullOrWhiteSpace(project.RepositoryUrl))
            //    yield return new MissingProperty(project, nameof(project.RepositoryUrl), $"https://github.com/Microsoft/Nugetninja");
            //if (string.IsNullOrWhiteSpace(project.RepositoryType))
            //    yield return new MissingProperty(project, nameof(project.RepositoryType), $"git");
        }
    }

    private ResetRuntime? AnalyzeVersion(Project project)
    {
        var runtimes = project.GetTargetFrameworks();
        for (var i = 0; i < runtimes.Length; i++)
        {
            foreach (var notSupportedRuntime in _notSupportedRuntimes)
            {
                if (runtimes[i].Contains(notSupportedRuntime, StringComparison.OrdinalIgnoreCase))
                {
                    runtimes[i] = runtimes[i].ToLower().Replace(notSupportedRuntime, _suggestedRuntime);
                }
            }
        }

        var cleanedRuntimes = runtimes.Select(r => r.ToLower().Trim()).Distinct().ToArray();

        var deprecated = project.GetTargetFrameworks().Except(cleanedRuntimes).ToList();
        var inserted = cleanedRuntimes.Except(project.GetTargetFrameworks()).ToList();
        if (inserted.Any() || deprecated.Any())
        {
            return new ResetRuntime(project, cleanedRuntimes, inserted.Count, deprecated.Count);
        }
        return null;
    }
}
