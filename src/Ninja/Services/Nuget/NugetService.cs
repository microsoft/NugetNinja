// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja;

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

    public async Task<Version> GetLatestVersion(string packageName)
    {
        var all = await this.GetAllPublishedVersions(packageName);
        return all.OrderByDescending(t => t).First();
    }

    public async Task<IReadOnlyCollection<Version>> GetAllPublishedVersions(string packageName)
    {
        return await _cacheService.RunWithCache($"all-nuget-published-versions-package-{packageName}-cache", 
            () => this.GetAllPublishedVersionsFromNuget(packageName));
    }

    private async Task<IReadOnlyCollection<Version>> GetAllPublishedVersionsFromNuget(string packageName)
    {
        var requestUrl = string.Format(NugetJsonFetchFormat, packageName.ToLower().Trim());
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        this._logger.LogTrace($"Calling Nuget to fetch all published versions with package name: '{packageName}'...");
        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<GetAllPublishedVersionsResponseModel>(responseJson);
            return responseModel
                ?.Versions
                ?.Where(v => !v.Contains("-")) // Exclude preview versions.
                ?.Select(v => StringExtensions.ConvertToVersion(v))
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
