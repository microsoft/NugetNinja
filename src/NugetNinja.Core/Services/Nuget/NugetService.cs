// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

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

    public async Task<NugetVersion> GetLatestVersion(string packageName, string nugetServer, string patToken, bool allowPreview = false)
    {
        var all = await this.GetAllPublishedVersions(packageName, nugetServer, patToken, allowPreview);
        return all.OrderByDescending(t => t).First();
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

        var request = new HttpRequestMessage(HttpMethod.Get, serverRoot);
        if (!string.IsNullOrWhiteSpace(patToken))
        {
            request.Headers.Add("Authorization", StringExtensions.PatToHeader(patToken));
        }
        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<NugetServerIndex>(responseJson);
            var packageBaseAddress = responseModel
                ?.Resources
                ?.FirstOrDefault(r => r.Type == "PackageBaseAddress/3.0.0")
                ?.Id
                ?? throw new WebException($"Couldn't find a valid PackageBaseAddress from nuget server with path: '{serverRoot}'!");
            var registrationsBaseUrl = responseModel
                ?.Resources
                ?.FirstOrDefault(r => r.Type == "RegistrationsBaseUrl")
                ?.Id
                ?? throw new WebException($"Couldn't find a valid RegistrationsBaseUrl from nuget server with path: '{serverRoot}'!");
            return new NugetServerEndPoints(packageBaseAddress, registrationsBaseUrl);
        }
        else
        {
            throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {serverRoot}.");
        }
    }

    private async Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersionsFromNuget(string packageName, string nugetServer, string patToken, bool allowPreview)
    {
        var apiEndpoint = await this.GetApiEndpoint(serverRoot: nugetServer, patToken);
        var requestUrl = $"{apiEndpoint.PackageBaseAddress.TrimEnd('/')}/{packageName.ToLower()}/index.json";
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        if (!string.IsNullOrWhiteSpace(patToken))
        {
            request.Headers.Add("Authorization", StringExtensions.PatToHeader(patToken));
        }
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
