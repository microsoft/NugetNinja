// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text.Json;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Core;

public class NugetService
{
    private readonly CacheService _cacheService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<NugetService> _logger;
    public const string DefaultNugetServer = "https://api.nuget.org/v3/index.json";

    public static string CustomNugetServer = DefaultNugetServer;
    public static string PatToken = string.Empty;
    public static bool AllowPreview = false;

    public NugetService(
        CacheService cacheService,
        HttpClient httpClient,
        ILogger<NugetService> logger)
    {
        _cacheService = cacheService;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<NugetVersion> GetLatestVersion(string packageName)
    {
        var all = await GetAllPublishedVersions(packageName);
        return all.OrderByDescending(t => t).First();
    }

    public Task<CatalogInformation> GetPackageDeprecationInfo(Package package)
    {
        return _cacheService.RunWithCache($"nuget-deprecation-info-{package}-version-{package.Version}-cache",
            () => GetPackageDeprecationInfoFromNuget(package));
    }

    public Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersions(string packageName)
    {
        return _cacheService.RunWithCache($"all-nuget-published-versions-package-{packageName}-preview-cache",
            () => GetAllPublishedVersionsFromNuget(packageName));
    }

    public Task<NugetServerEndPoints> GetApiEndpoint()
    {
        return _cacheService.RunWithCache($"nuget-server-endpoint-cache", GetApiEndpointFromNuget);
    }

    public Task<Package[]> GetPackageDependencies(Package package)
    {
        return _cacheService.RunWithCache($"nuget-package-{package.Name}-dependencies-{package.Version}-cache",
            () => GetPackageDependenciesFromNuget(package));
    }

    private async Task<NugetServerEndPoints> GetApiEndpointFromNuget()
    {
        var serverRoot = CustomNugetServer;
        if (serverRoot.EndsWith("/"))
        {
            serverRoot = serverRoot.TrimEnd('/');
        }
        if (!serverRoot.EndsWith("index.json"))
        {
            serverRoot += "/index.json";
        }
        if (!serverRoot.EndsWith("v3/index.json"))
        {
            serverRoot += "/v3/index.json";
        }

        var responseModel = await HttpGetJson<NugetServerIndex>(serverRoot, PatToken);
        var packageBaseAddress = responseModel
            .Resources
            .FirstOrDefault(r => r.Type.StartsWith("PackageBaseAddress"))
            ?.Id
            ?? throw new WebException($"Couldn't find a valid PackageBaseAddress from nuget server with path: '{serverRoot}'!");
        var registrationsBaseUrl = responseModel
            .Resources
            .FirstOrDefault(r => r.Type.StartsWith("RegistrationsBaseUrl"))
            ?.Id
            ?? throw new WebException($"Couldn't find a valid RegistrationsBaseUrl from nuget server with path: '{serverRoot}'!");
        return new NugetServerEndPoints(packageBaseAddress, registrationsBaseUrl);
    }

    private async Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersionsFromNuget(string packageName)
    {
        try
        {
            var apiEndpoint = await GetApiEndpoint();
            var requestUrl = $"{apiEndpoint.PackageBaseAddress.TrimEnd('/')}/{packageName.ToLower()}/index.json";
            var responseModel = await HttpGetJson<GetAllPublishedVersionsResponseModel>(requestUrl, PatToken);
            return responseModel
                .Versions
                ?.Select(v => new NugetVersion(v))
                .Where(v => AllowPreview || !v.IsPreviewVersion())
                .ToList()
                .AsReadOnly()
                ?? throw new WebException($"Couldn't find a valid version from Nuget with package: '{packageName}'!");
        }
        catch (Exception e)
        {
            _logger.LogTrace(e, $"Couldn't get version info based on package name: '{packageName}'.");
            _logger.LogCritical($"Couldn't get version info based on package name: '{packageName}'.");
            throw;
        }
    }

    private async Task<CatalogInformation> GetPackageDeprecationInfoFromNuget(Package package)
    {
        try
        {
            var apiEndpoint = await GetApiEndpoint();
            var requestUrl = $"{apiEndpoint.RegistrationsBaseUrl.TrimEnd('/')}/{package.Name.ToLower()}/{package.Version.ToString().ToLower()}.json";
            var packageContext = await HttpGetJson<RegistrationIndex>(requestUrl, PatToken);
            var packageCatalogUrl = packageContext.CatalogEntry ?? throw new WebException($"Couldn't ind a valid catalog entry for package: '{package}'!");
            return await HttpGetJson<CatalogInformation>(packageCatalogUrl, PatToken);
        }
        catch (Exception e)
        {
            _logger.LogTrace(e, $"Couldn't get version info based on package name: '{package}'.");
            _logger.LogCritical($"Couldn't get the deprecation information based on package: {package}.");
            throw;
        }
    }

    private async Task<Package[]> GetPackageDependenciesFromNuget(Package package)
    {
        try
        {
            var apiEndpoint = await GetApiEndpoint();
            var requestUrl = $"{apiEndpoint.PackageBaseAddress.TrimEnd('/')}/{package.Name.ToLower()}/{package.Version}/{package.Name.ToLower()}.nuspec";
            var nuspec = await HttpGetString(requestUrl, PatToken);
            var doc = new HtmlDocument();
            doc.LoadHtml(nuspec);
            var packageReferences = doc.DocumentNode
                .Descendants("dependency")
                .Select(p => new Package(
                    name: p.Attributes["id"].Value,
                    versionText: p.Attributes["version"].Value))
                .DistinctBy(p => p.Name)
                .ToArray();
            return packageReferences;
        }
        catch (Exception e)
        {
            _logger.LogTrace(e, $"Couldn't get version info based on package name: '{package}'.");
            _logger.LogCritical($"Couldn't get the deprecation information based on package: {package}.");
            throw;
        }
    }

    private async Task<T> HttpGetJson<T>(string url, string patToken)
    {
        var json = await HttpGetString(url, patToken);
        return JsonSerializer.Deserialize<T>(json) ?? throw new WebException($"The remote server returned non-json content: '{json}'");
    }

    private async Task<string> HttpGetString(string url, string patToken)
    {
        _logger.LogTrace($"Sending request to: {url}...");
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (!string.IsNullOrWhiteSpace(patToken))
        {
            request.Headers.Add("Authorization", StringExtensions.PatToHeader(patToken));
        }
        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var text = await response.Content.ReadAsStringAsync();
            return text;
        }

        throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {url}.");
    }
}
