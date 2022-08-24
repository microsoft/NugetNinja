// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    private readonly string _githubUserEmail;
    private readonly string WorkspaceFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NugetNinjaWorkspace");
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

        if (await _repoDbContext.Repos.CountAsync() < 2)
        {
            _logger.LogInformation("Seeding test database...");
            _repoDbContext.Repos.RemoveRange(_repoDbContext.Repos);
            await _repoDbContext.SaveChangesAsync();
            await _repoDbContext.Repos.AddAsync(new GitRepo("NugetNinja", "Microsoft", "main", "https://github.com/Microsoft/NugetNinja.git", RepoProvider.GitHub));
            await _repoDbContext.Repos.AddAsync(new GitRepo("Infrastructures", "AiursoftWeb", "master", "https://github.com/AiursoftWeb/Infrastructures.git", RepoProvider.GitHub));
            await _repoDbContext.SaveChangesAsync();
        }

        foreach (var repo in await _repoDbContext.Repos.ToListAsync())
        {
            _logger.LogInformation($"Cloning repository: {repo.Name}...");
            var workPath = Path.Combine(WorkspaceFolder, $"workspace-{repo.Name}");
            await _workspaceManager.ResetRepo(workPath, repo.DefaultBranch, repo.CloneEndpoint);
            await _runAllOfficialPluginsService.OnServiceStartedAsync(workPath, true);

            if (!await _workspaceManager.PendingCommit(workPath))
            {
                _logger.LogInformation($"{repo} has no suggestion that we can make. Ignore.");
                continue;
            }

            _logger.LogInformation($"{repo} is pending some fix. We will try to create\\update related pull request.");
            await _workspaceManager.SetUserConfig(workPath, username: _githubUserName, email: _githubUserEmail);
            var saved = await _workspaceManager.CommitToBranch(workPath, "Auto csproj fix and update by bot.", branch: _workingBranch);

            if (!saved)
            {
                _logger.LogInformation($"{repo} has no suggestion that we can make. Ignore.");
                continue;
            }

            await _gitHubService.ForkRepo(repo.Org, repo.Name);
            await _workspaceManager.Push(workPath, _workingBranch, $"https://{_githubUserName}:{_githubToken}@github.com/{_githubUserName}/{repo.Name}.git", force: true);
        }
    }
}
