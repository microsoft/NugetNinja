// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.PrBot;

public class GitHubService
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly ILogger<GitHubService> _logger;

    public GitHubService(
        IConfiguration configuration,
        HttpClient httpClient,
        ILogger<GitHubService> logger)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task ForkRepo(string org, string repo)
    {
        _logger.LogInformation($"Forking repository on GitHub with org: {org}, repo: {repo}...");

        var endpoint = $@"https://api.github.com/repos/{org}/{repo}/forks";
        var request = new HttpRequestMessage(HttpMethod.Post, endpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        request.Headers.Add("Authorization", $"token {_configuration["GitHubToken"]}");
        request.Headers.Add("accept", "application/json");
        request.Headers.Add("User-Agent", ".NET HTTP Client");

        var result = await _httpClient.SendAsync(request);
        try
        {
            result.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            var resultContent = await result.Content.ReadAsStringAsync();
            throw new WebException(resultContent);
        }
    }
}
