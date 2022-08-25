// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.AllOfficialsPlugin;

namespace Microsoft.NugetNinja.PrBot;

public class Entry
{
    private readonly string _githubToken;
    private readonly string _workingBranch;
    private readonly string _githubUserName;
    private readonly string _githubUserDisplayName;
    private readonly string _githubUserEmail;
    private readonly string _workspaceFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NugetNinjaWorkspace");
    private readonly GitHubService _gitHubService;
    private readonly RunAllOfficialPluginsService _runAllOfficialPluginsService;
    private readonly WorkspaceManager _workspaceManager;
    private readonly ILogger<Entry> _logger;

    public Entry(
        GitHubService gitHubService,
        RunAllOfficialPluginsService runAllOfficialPluginsService,
        WorkspaceManager workspaceManager,
        IConfiguration configuration,
        ILogger<Entry> logger)
    {
        _githubToken = configuration["GitHubToken"];
        _workingBranch = configuration["ContributionBranch"];
        _githubUserName = configuration["GitHubUserName"];
        _githubUserDisplayName = configuration["GitHubUserDisplayName"];
        _githubUserEmail = configuration["GitHubUserEmail"];
        _gitHubService = gitHubService;
        _runAllOfficialPluginsService = runAllOfficialPluginsService;
        _workspaceManager = workspaceManager;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Starting Nuget Ninja PR bot...");

        var myStars = await _gitHubService.GetMyStars(_githubUserName);

        _logger.LogInformation($"Got {myStars.Count} repositories as registered to create pull requests automatically.");
        _logger.LogInformation("\r\n\r\n");
        _logger.LogInformation("================================================================");
        _logger.LogInformation("\r\n\r\n");
        foreach (var repo in myStars)
        {
            try
            {
                _logger.LogInformation($"Processing repository {repo.Owner?.Login}/{repo.Name}...");
                await ProcessRepository(repo);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Crashed when processing repo: {repo.Owner?.Login}/{repo.Name}!");
            }
            finally
            {
                _logger.LogInformation("\r\n\r\n");
                _logger.LogInformation("================================================================");
                _logger.LogInformation("\r\n\r\n");
            }
        }
    }

    private async Task ProcessRepository(Repository repo)
    {
        if (string.IsNullOrWhiteSpace(repo.Owner?.Login) || string.IsNullOrWhiteSpace(repo.Name))
        {
            throw new InvalidDataException($"The repo with path: {repo.FullName} is having invalid data!");
        }

        // Clone locally.
        _logger.LogInformation($"Cloning repository: {repo.Name}...");
        var workPath = Path.Combine(_workspaceFolder, $"workspace-{repo.Name}");
        await _workspaceManager.ResetRepo(
            path: workPath,
            branch: repo.DefaultBranch ?? throw new NullReferenceException($"The default branch of {repo.Name} is null!"),
            endPoint: repo.CloneUrl ?? throw new NullReferenceException($"The clone endpoint branch of {repo.Name} is null!"));

        // Run all plugins.
        await _runAllOfficialPluginsService.OnServiceStartedAsync(workPath, true);

        // Consider changes...
        if (!await _workspaceManager.PendingCommit(workPath))
        {
            _logger.LogInformation($"{repo} has no suggestion that we can make. Ignore.");
            return;
        }
        _logger.LogInformation($"{repo} is pending some fix. We will try to create\\update related pull request.");
        await _workspaceManager.SetUserConfig(workPath, username: _githubUserDisplayName, email: _githubUserEmail);
        var saved = await _workspaceManager.CommitToBranch(workPath, "Auto csproj fix and update by bot.", branch: _workingBranch);
        if (!saved)
        {
            _logger.LogInformation($"{repo} has no suggestion that we can make. Ignore.");
            return;
        }

        // Fork repo.
        if (!await _gitHubService.RepoExists(_githubUserName, repo.Name))
        {
            await _gitHubService.ForkRepo(repo.Owner.Login, repo.Name);
            await Task.Delay(5000);
            while (!await _gitHubService.RepoExists(_githubUserName, repo.Name))
            {
                // Wait a while. GitHub may need some time to fork the repo.
                await Task.Delay(5000);
            }
        }

        // Push to forked repo.
        await _workspaceManager.Push(workPath, _workingBranch, $"https://{_githubUserName}:{_githubToken}@github.com/{_githubUserName}/{repo.Name}.git", force: true);

        var existingPullRequestsByBot = await _gitHubService.GetPullRequest(repo.Owner.Login, repo.Name, head: $"{_githubUserName}:{_workingBranch}");
        if (existingPullRequestsByBot.All(p => p.State != "open"))
        {
            // Create a new pull request.
            await _gitHubService.CreatePullRequest(repo.Owner.Login, repo.Name, head: $"{_githubUserName}:{_workingBranch}", @base: repo.DefaultBranch);
        }
        else
        {
            _logger.LogInformation($"Skipped creating new pull request for {repo} because there already exists.");
        }
    }
}
