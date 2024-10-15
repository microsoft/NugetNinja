// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol.Core.Types;
using NugetNinja.PackageListerPlugin.Models;

namespace Microsoft.NugetNinja.PackageListerPlugin;

public class PackageList : IAction
{

    private List<(string project, List<Package> packages)> _allPackages;
    private readonly ILogger<PackageListerDetector> _logger;

    public PackageList(List<(string, List<Package>)> allPackages1, Extensions.Logging.ILogger<PackageListerDetector> logger)
    {
        _allPackages = allPackages1;
        _logger = logger;
    }

    public string BuildMessage()
    {
        var sb = new StringBuilder();
        foreach (var package in _allPackages)
        {
            sb.AppendLine($"Project: {package.project}\n");
            foreach (var p in package.packages)
            {
                sb.AppendLine($"  {p.Name} {p.Version}\n");
            }
        }
        return sb.ToString();
    }

    public async Task TakeActionAsync()
    {
        _logger.LogInformation($"Preparing summary file");
        var flatList = _allPackages.SelectMany(p => p.packages.Select(package => (p.project, package))).ToList();
        var byPackage = flatList.GroupBy(p => p.package.Name).ToList();
        var allLines = new List<PackageListLine>();
        foreach (var el in byPackage)
        {
            try
            {
                var description = SanitizeNugetDescription(await GetNugetDescriptionAsync(el.Key));
                allLines.Add(new PackageListLine(el.Key, GetAllVersions(el), description, null, GetAllProjects(el)));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error processing package {el.Key}");
            }
        }
        allLines = allLines.OrderBy(p => p.PackageId).ToList();
        var outFile = "all_packages.csv";
        _logger.LogInformation($"Writing package list to {Path.GetFullPath(outFile)}");
        using (var writer = new StreamWriter("all_packages.csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(allLines);
        }
        await Task.CompletedTask;
    }

    public async Task<string> GetNugetDescriptionAsync(string packageId)
    {
        _logger.LogInformation($"Getting description from nuget.org for {packageId}");
        var providers = NuGet.Protocol.Core.Types.Repository.Provider.GetCoreV3();
        var source = new PackageSource("https://api.nuget.org/v3/index.json");
        var repository = new SourceRepository(source, providers);
        var resource = await repository.GetResourceAsync<PackageMetadataResource>();
        var metadata = await resource.GetMetadataAsync(packageId, includePrerelease: false, includeUnlisted: false, new SourceCacheContext(), NullLogger.Instance, CancellationToken.None);

        var packageMetadata = metadata.FirstOrDefault();
        return packageMetadata?.Description ?? "Description not found";
    }

    private static string SanitizeNugetDescription(string desc)
    {
        const string marker = "Commonly used types:";
        int index = desc.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (index >= 0)
        {
            desc = desc.Substring(0, index);
        }
        return desc.Replace("\n", " ").Replace("\r", " ").Trim();
    }

    public string GetAllVersions(IEnumerable<(string project, Package package)> packages)
    {
        var allVersions = packages.Select(p => p.package.SourceVersionText).Distinct().ToList();
        return string.Join(", ", allVersions);
    }

    public string GetAllProjects(IEnumerable<(string project, Package package)> packages)
    {
        var allProjects = packages.Select(p => p.project).Distinct().ToList();
        return string.Join(", ", allProjects);
    }
}
