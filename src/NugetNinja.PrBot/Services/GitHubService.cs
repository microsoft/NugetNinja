// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
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

    public async Task<Repository> GetRepo(string orgName, string repoName)
    {
        _logger.LogInformation($"Getting repository details based on org: {orgName}, repo: {repoName}...");
        var endpoint = $@"https://api.github.com/repos/{orgName}/{repoName}";
        return await this.SendHttpAndGetJson<Repository>(endpoint, HttpMethod.Get);
    }

    public async Task<List<Repository>> GetRepos(string userName)
    {
        _logger.LogInformation($"Listing all repositories based on user name: {userName}...");
        var endpoint = $@"https://api.github.com/users/{userName}/repos";
        return await this.SendHttpAndGetJson<List<Repository>>(endpoint, HttpMethod.Get);
    }

    public async Task ForkRepo(string org, string repo)
    {
        _logger.LogInformation($"Forking repository on GitHub with org: {org}, repo: {repo}...");

        var endpoint = $@"https://api.github.com/repos/{org}/{repo}/forks";
        await SendHttp(endpoint, HttpMethod.Post);
    }

    private async Task<string> SendHttp(string endPoint, HttpMethod method)
    {
        var request = new HttpRequestMessage(method, endPoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>())
        };

        request.Headers.Add("Authorization", $"token {_configuration["GitHubToken"]}");
        request.Headers.Add("accept", "application/json");
        request.Headers.Add("User-Agent", ".NET HTTP Client");

        var response = await _httpClient.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            var resultContent = e.Message + await response.Content.ReadAsStringAsync();
            throw new WebException(resultContent);
        }

        var json = await response.Content.ReadAsStringAsync();
        return json;
    }

    private async Task<T> SendHttpAndGetJson<T>(string endPoint, HttpMethod method)
    {
        var json = await this.SendHttp(endPoint, method);
        var repos = JsonSerializer.Deserialize<T>(json) ?? throw new WebException($"The remote server returned non-json content: '{json}'");
        return repos;
    }
}
