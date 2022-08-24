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

            if (string.IsNullOrWhiteSpace(project.OutputType) && _fillInOutputType)
                yield return new MissingProperty(project, nameof(project.OutputType), "Library");

            var versionSuggestion = this.AnalyseVersion(project);
            if (versionSuggestion != null)
            {
                yield return versionSuggestion;
            }

            if (string.IsNullOrWhiteSpace(project.Nullable) && _enforceNullable)
                yield return new MissingProperty(project, nameof(project.Nullable), "enable");
            if (string.IsNullOrWhiteSpace(project.ImplicitUsings))
                yield return new MissingProperty(project, nameof(project.ImplicitUsings), "enable");
            if (string.IsNullOrWhiteSpace(project.Version))
            {
                _logger.LogTrace($"Skip scanning properties for project: '{project}' because it seems won't publish to Nuget. It doesn't have a version property.");
                continue;
            }

            if (string.IsNullOrWhiteSpace(project.PackageLicenseExpression) && string.IsNullOrWhiteSpace(project.PackageLicenseFile))
                yield return new MissingProperty(project, nameof(project.PackageLicenseExpression), "MIT");
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

    private ResetRuntime? AnalyseVersion(Project project)
    {
        var runtimes = project.GetTargetFrameworks();
        for (int i = 0; i < runtimes.Length; i++)
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

        var inserted = project.GetTargetFrameworks().Except(cleanedRuntimes).ToList();
        var deprecated = cleanedRuntimes.Except(project.GetTargetFrameworks()).ToList();
        if (inserted.Any() || deprecated.Any())
        {
            return new ResetRuntime(project, cleanedRuntimes, inserted.Count, deprecated.Count);
        }
        return null;
    }
}
