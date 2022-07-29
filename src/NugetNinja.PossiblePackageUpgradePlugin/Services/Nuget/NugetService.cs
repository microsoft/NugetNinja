// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.PossiblePackageUpgradePlugin.Services.Nuget.Models;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class NugetService
{
    private readonly CacheService _cacheService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<NugetService> _logger;

    public NugetService(
        CacheService cacheService,
        HttpClient httpClient,
        ILogger<NugetService> logger)
    {
        _cacheService = cacheService;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<NugetVersion> GetLatestVersion(string packageName, string nugetServer, bool allowPreview = false)
    {
        var all = await this.GetAllPublishedVersions(packageName, nugetServer, allowPreview);
        return all.OrderByDescending(t => t).First();
    }

    public Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersions(string packageName, string nugetServer, bool allowPreview)
    {
        return _cacheService.RunWithCache($"all-nuget-{nugetServer}-published-versions-package-{packageName}-preview-{allowPreview}-cache",
            () => this.GetAllPublishedVersionsFromNuget(packageName, nugetServer, allowPreview));
    }

    public Task<string> GetApiEndpoint(string serverRoot)
    {
        return _cacheService.RunWithCache($"nuget-server-endpoint-{serverRoot}-cache",
            () => this.GetApiEndpointFromNuget(serverRoot));
    }

    private async Task<string> GetApiEndpointFromNuget(string serverRoot)
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

        var request = new HttpRequestMessage(HttpMethod.Get, serverRoot);
        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<NugetServerIndex>(responseJson);
            return responseModel
                ?.Resources
                ?.FirstOrDefault(r => r.Type == "PackageBaseAddress/3.0.0")
                ?.Id
                ?? throw new WebException($"Couldn't find a valid package base address from nuget server with path: '{serverRoot}'!");
        }
        else
        {
            throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {serverRoot}.");
        }
    }

    private async Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersionsFromNuget(string packageName, string nugetServer, bool allowPreview)
    {
        var apiEndpoint = await this.GetApiEndpoint(serverRoot: nugetServer);
        var requestUrl = $"{apiEndpoint.TrimEnd('/')}/{packageName.ToLower()}/index.json";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        _logger.LogTrace($"Calling Nuget to fetch all published versions with package name: '{packageName}'...");
        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<GetAllPublishedVersionsResponseModel>(responseJson);
            return responseModel
                ?.Versions
                ?.Select(v => new NugetVersion(v))
                ?.Where(v => allowPreview || !v.IsPreviewVersion())
                .ToList()
                .AsReadOnly()
                ?? throw new WebException($"Couldn't find a valid version from Nuget with package: '{packageName}'!");
        }
        else
        {
            throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {requestUrl}.");
        }
    }
}
