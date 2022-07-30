// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.Core;

public class NugetService
{
    private readonly CacheService _cacheService;
    private readonly HttpClient _httpClient;
    private readonly ILogger<NugetService> _logger;
    private static readonly string NugetRootServer = "https://api.nuget.org";
    private static readonly string NugetJsonFetchFormat = $"{NugetRootServer}/v3-flatcontainer/{{0}}/index.json";

    public NugetService(
        CacheService cacheService,
        HttpClient httpClient,
        ILogger<NugetService> logger)
    {
        _cacheService = cacheService;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<NugetVersion> GetLatestVersion(string packageName, bool allowPreview = false)
    {
        var all = await this.GetAllPublishedVersions(packageName, allowPreview);
        return all.OrderByDescending(t => t).First();
    }

    public async Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersions(string packageName, bool allowPreview)
    {
        return await _cacheService.RunWithCache($"all-nuget-published-versions-package-{packageName}-preview-{allowPreview}-cache", 
            () => this.GetAllPublishedVersionsFromNuget(packageName, allowPreview));
    }

    private async Task<IReadOnlyCollection<NugetVersion>> GetAllPublishedVersionsFromNuget(string packageName, bool allowPreview)
    {
        var requestUrl = string.Format(NugetJsonFetchFormat, packageName.ToLower().Trim());
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
                ?.Where(v => allowPreview || !v.IsPreviewVersion()) // Exclude preview versions.
                .ToList()
                .AsReadOnly()
                ?? throw new WebException($"Couldn't find a valid version from Nuget.org with package: '{packageName}'!");
        }
        else
        {
            throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {requestUrl}.");
        }
    }
}
