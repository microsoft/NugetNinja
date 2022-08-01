// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.Core;

public class NugetService
{
    private readonly CacheService _cacheService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<NugetService> _logger;
    public static readonly string DefaultNugetServer = "https://api.nuget.org/v3/index.json";

    public NugetService(
        CacheService cacheService,
        HttpClient httpClient,
        ILogger<NugetService> logger)
    {
        _cacheService = cacheService;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<NugetVersion> GetLatestVersion(string packageName, string nugetServer, string patToken, bool allowPreview = false)
    {
        var all = await this.GetAllPublishedVersions(packageName, nugetServer, patToken, allowPreview);
        return all.OrderByDescending(t => t).First();
    }

    public Task<CatalogInformation> GetPackageDeprecationInfo(Package package, string nugetServer, string patToken)
    {
        return _cacheService.RunWithCache($"nuget-{nugetServer}-deprecation-info-{package}-version-{package.Version}-cache",
            () => this.GetPackageDeprecationInfoFromNuget(package, nugetServer, patToken));
    }

    public Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersions(string packageName, string nugetServer, string patToken, bool allowPreview)
    {
        return _cacheService.RunWithCache($"all-nuget-{nugetServer}-published-versions-package-{packageName}-preview-{allowPreview}-cache",
            () => this.GetAllPublishedVersionsFromNuget(packageName, nugetServer, patToken, allowPreview));
    }

    public Task<NugetServerEndPoints> GetApiEndpoint(string serverRoot, string patToken)
    {
        return _cacheService.RunWithCache($"nuget-server-endpoint-{serverRoot}-cache",
            () => this.GetApiEndpointFromNuget(serverRoot, patToken));
    }

    private async Task<NugetServerEndPoints> GetApiEndpointFromNuget(string serverRoot, string patToken)
    {
        if (serverRoot.EndsWith("/"))
        {
            serverRoot = serverRoot.TrimEnd('/');
        }
        if (!serverRoot.EndsWith("index.json"))
        {
            serverRoot = serverRoot + "/index.json";
        }
        if (!serverRoot.EndsWith("v3/index.json"))
        {
            serverRoot = serverRoot + "/v3/index.json";
        }

        var responseModel = await this.HttpGetJson<NugetServerIndex>(serverRoot, patToken);
        var packageBaseAddress = responseModel
            ?.Resources
            ?.FirstOrDefault(r => r.Type.StartsWith("PackageBaseAddress"))
            ?.Id
            ?? throw new WebException($"Couldn't find a valid PackageBaseAddress from nuget server with path: '{serverRoot}'!");
        var registrationsBaseUrl = responseModel
            ?.Resources
            ?.FirstOrDefault(r => r.Type.StartsWith("RegistrationsBaseUrl"))
            ?.Id
            ?? throw new WebException($"Couldn't find a valid RegistrationsBaseUrl from nuget server with path: '{serverRoot}'!");
        return new NugetServerEndPoints(packageBaseAddress, registrationsBaseUrl);
    }

    private async Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersionsFromNuget(string packageName, string nugetServer, string patToken, bool allowPreview)
    {
        try
        {
            var apiEndpoint = await this.GetApiEndpoint(serverRoot: nugetServer, patToken);
            var requestUrl = $"{apiEndpoint.PackageBaseAddress.TrimEnd('/')}/{packageName.ToLower()}/index.json";
            var responseModel = await this.HttpGetJson<GetAllPublishedVersionsResponseModel>(requestUrl, patToken);
            return responseModel
                ?.Versions
                ?.Select(v => new NugetVersion(v))
                ?.Where(v => allowPreview || !v.IsPreviewVersion())
                .ToList()
                .AsReadOnly()
                ?? throw new WebException($"Couldn't find a valid version from Nuget with package: '{packageName}'!");
        }
        catch (Exception e)
        {
            _logger.LogTrace(e, $"Couldn't get version info based on package name: '{packageName}'.");
            _logger.LogCritical($"Couldn't get version info based on package name: '{packageName}'.");
            return new List<NugetVersion>()
            {
                new NugetVersion("0.0.1")
            };
        }
    }

    private async Task<CatalogInformation> GetPackageDeprecationInfoFromNuget(Package package, string nugetServer, string patToken)
    {
        try
        {
            var apiEndpoint = await this.GetApiEndpoint(serverRoot: nugetServer, patToken);
            var requestUrl = $"{apiEndpoint.RegistrationsBaseUrl.TrimEnd('/')}/{package.Name.ToLower()}/{package.Version.ToString().ToLower()}.json";
            var packageContext = await this.HttpGetJson<RegistrationIndex>(requestUrl, patToken);
            var packageCatalogUrl = packageContext?.CatalogEntry ?? throw new WebException($"Couldn'f ind a valid catalog entry for package: '{package}'!");
            return await this.HttpGetJson<CatalogInformation>(packageCatalogUrl, patToken);
        }
        catch (Exception ex)
        {
            if (nugetServer != DefaultNugetServer)
            {
                // fallback to default server. try again.
                return await this.GetPackageDeprecationInfoFromNuget(package, DefaultNugetServer, string.Empty);
            }
            this._logger.LogTrace(ex, $"Couldn't get the deprecation information based on package: {package}.");
            this._logger.LogCritical($"Couldn't get the deprecation information based on package: {package}.");
            return new CatalogInformation
            {
                Deprecation = null,
                Vulnerabilities = new List<Vulnerability>()
            };
        }
    }

    private async Task<T> HttpGetJson<T>(string url, string patToken)
    {
        this._logger.LogTrace($"Sending request to: {url}...");
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        if (!string.IsNullOrWhiteSpace(patToken))
        {
            request.Headers.Add("Authorization", StringExtensions.PatToHeader(patToken));
        }
        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json) ?? throw new WebException($"The remote server returned non-json content: '{json}'");
        }
        else
        {
            throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {url}.");
        }
    }
}
