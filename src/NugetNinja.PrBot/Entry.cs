// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;
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
    private readonly RepoDbContext _repoDbContext;
    private readonly ILogger<Entry> _logger;

    public Entry(
        GitHubService gitHubService,
        RunAllOfficialPluginsService runAllOfficialPluginsService,
        WorkspaceManager workspaceManager,
        IConfiguration configuration,
        RepoDbContext repoDbContext,
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
        _repoDbContext = repoDbContext;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Starting Nuget Ninja PR bot...");

        _logger.LogInformation("Migrating database...");
        await _repoDbContext.Database.MigrateAsync();

        if (!await _repoDbContext.Repos.AnyAsync())
        {
            _logger.LogInformation("Seeding test database...");
            _repoDbContext.Repos.RemoveRange(_repoDbContext.Repos);
            await _repoDbContext.SaveChangesAsync();
            await _repoDbContext.Repos.AddAsync(new GitRepo("NugetNinja", "Microsoft"));
            await _repoDbContext.SaveChangesAsync();
        }

        foreach (var repo in await _repoDbContext.Repos.ToListAsync())
        {
            // Get details from GitHub
            var repoDetails = await _gitHubService.GetRepo(repo.Org, repo.Name);

            // Clone locally.
            _logger.LogInformation($"Cloning repository: {repo.Name}...");
            var workPath = Path.Combine(_workspaceFolder, $"workspace-{repo.Name}");
            await _workspaceManager.ResetRepo(
                path: workPath,
                branch: repoDetails.DefaultBranch ?? throw new NullReferenceException($"The default branch of {repoDetails.Name} is null!"),
                endPoint: repoDetails.CloneUrl ?? throw new NullReferenceException($"The clone endpoint branch of {repoDetails.Name} is null!"));

            // Run all plugins.
            await _runAllOfficialPluginsService.OnServiceStartedAsync(workPath, true);

            // Consider changes...
            if (!await _workspaceManager.PendingCommit(workPath))
            {
                _logger.LogInformation($"{repo} has no suggestion that we can make. Ignore.");
                continue;
            }
            _logger.LogInformation($"{repo} is pending some fix. We will try to create\\update related pull request.");
            await _workspaceManager.SetUserConfig(workPath, username: _githubUserDisplayName, email: _githubUserEmail);
            var saved = await _workspaceManager.CommitToBranch(workPath, "Auto csproj fix and update by bot.", branch: _workingBranch);
            if (!saved)
            {
                _logger.LogInformation($"{repo} has no suggestion that we can make. Ignore.");
                continue;
            }

            // Fork repo.
            while ((await _gitHubService.GetRepos(_githubUserName)).All(r => r.Name != repo.Name))
            {
                await _gitHubService.ForkRepo(repo.Org, repo.Name);
                // Wait a while. GitHub may need some time to fork the repo.
                await Task.Delay(3000); 
            }

            // Push to forked repo.
            await _workspaceManager.Push(workPath, _workingBranch, $"https://{_githubUserName}:{_githubToken}@github.com/{_githubUserName}/{repo.Name}.git", force: true);

            var existingPullRequestsByBot = await _gitHubService.GetPullRequest(repo.Org, repo.Name, head: $"{_githubUserName}:{_workingBranch}");
            if (existingPullRequestsByBot.All(p => p.State != "open"))
            {
                // Create a new pull request.
                await _gitHubService.CreatePullRequest(repo.Org, repo.Name, head: $"{_githubUserName}:{_workingBranch}", @base: repoDetails.DefaultBranch);
            }
            else
            {
                _logger.LogInformation($"Skipped creating new pull request for {repo} because there already exists.");
            }
        }
    }
}
