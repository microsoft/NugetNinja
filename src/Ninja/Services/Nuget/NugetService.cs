// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja;

public class NugetService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NugetService> _logger;
    private static readonly string NugetRootServer = "https://api.nuget.org";
    private static readonly string NugetJsonFetchFormat = $"{NugetRootServer}/v3-flatcontainer/{{0}}/index.json";

    public NugetService(
        HttpClient httpClient,
        ILogger<NugetService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<string>> GetAllPublishedVersions(string packageName)
    {
        var requestUrl = string.Format(NugetJsonFetchFormat, packageName.ToLower().Trim());
        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            var responseJson = await response.Content.ReadAsStringAsync();
            var responseModel = JsonSerializer.Deserialize<GetAllPublishedVersionsResponseModel>(responseJson);
            return responseModel
                ?.Versions
                ?.AsReadOnly()
                ?? throw new WebException($"Couldn't find a valid version from Nuget.org with package: '{packageName}'!");
        }
        else
        {
            throw new WebException($"The remote server returned unexpected status code: {response.StatusCode} - {response.ReasonPhrase}. Url: {requestUrl}.");
        }
    }
}
