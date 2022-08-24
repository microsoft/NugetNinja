﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Net;
using System.Text;
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
        return await SendHttpAndGetJson<Repository>(endpoint, HttpMethod.Get);
    }

    public async Task<List<Repository>> GetRepos(string userName)
    {
        _logger.LogInformation($"Listing all repositories based on user name: {userName}...");
        var endpoint = $@"https://api.github.com/users/{userName}/repos";
        return await SendHttpAndGetJson<List<Repository>>(endpoint, HttpMethod.Get);
    }

    public async Task ForkRepo(string org, string repo)
    {
        _logger.LogInformation($"Forking repository on GitHub with org: {org}, repo: {repo}...");

        var endpoint = $@"https://api.github.com/repos/{org}/{repo}/forks";
        await SendHttp(endpoint, HttpMethod.Post);
    }

    public async Task<List<PullRequest>> GetPullRequest(string org, string repo, string head)
    {
        _logger.LogInformation($"Getting pull requests on GitHub with org: {org}, repo: {repo}...");

        var endpoint = $@"https://api.github.com/repos/{org}/{repo}/pulls?head={head}";
        return await SendHttpAndGetJson<List<PullRequest>>(endpoint, HttpMethod.Get);
    }

    public async Task CreatePullRequest(string org, string repo, string head, string @base)
    {
        _logger.LogInformation($"Creating a new pull request on GitHub with org: {org}, repo: {repo}...");

        var endpoint = $@"https://api.github.com/repos/{org}/{repo}/pulls";
        await SendHttp(endpoint, HttpMethod.Post, new
        {
            title = "Auto dependencies upgrade by bot.",
            body = @"
Auto dependencies upgrade by bot. This is automatically generated by bot.

The bot tries to fetch all possible updates and modify the project files automatically.

This pull request may break or change the behavior of this application. Review with cautious!",
            head,
            @base
        });
    }

    private async Task<string> SendHttp(string endPoint, HttpMethod method, object? body = null)
    {
        var request = new HttpRequestMessage(method, endPoint)
        {
            Content = body != null ?
                new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json") :
                new FormUrlEncodedContent(new Dictionary<string, string>())
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
        var json = await SendHttp(endPoint, method);
        var repos = JsonSerializer.Deserialize<T>(json) ?? throw new WebException($"The remote server returned non-json content: '{json}'");
        return repos;
    }
}