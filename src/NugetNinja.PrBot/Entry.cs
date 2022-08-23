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
    private readonly RunAllOfficialPluginsService _runAllOfficialPluginsService;
    private readonly WorkspaceManager _workspaceManager;
    private readonly IConfiguration _configuration;
    private readonly RepoDbContext _repoDbContext;
    private readonly ILogger<Entry> _logger;
    private static readonly string WorkspaceFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "NugetNinjaWorkspace");

    public Entry(
        RunAllOfficialPluginsService runAllOfficialPluginsService,
        WorkspaceManager workspaceManager,
        IConfiguration configuration,
        RepoDbContext repoDbContext,
        ILogger<Entry> logger)
    {
        _runAllOfficialPluginsService = runAllOfficialPluginsService;
        _workspaceManager = workspaceManager;
        _configuration = configuration;
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
            await _repoDbContext.Repos.AddAsync(new GitRepo("NugetNinja", "main", "https://github.com/Microsoft/NugetNinja.git", RepoProvider.GitHub));
            await _repoDbContext.SaveChangesAsync();
        }

        foreach (var repo in await _repoDbContext.Repos.ToListAsync())
        {
            _logger.LogInformation($"Cloning repository: {repo.Name}...");
            var workPath = Path.Combine(WorkspaceFolder, $"workspace-{repo.Name}");
            await _workspaceManager.ResetRepo(workPath, repo.DefaultBranch, repo.CloneEndpoint);
            await _runAllOfficialPluginsService.OnServiceStartedAsync(workPath, true);
        }
    }
}
