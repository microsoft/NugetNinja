// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Microsoft.NugetNinja.PrBot;

public class Entry
{
    private readonly RepoDbContext _repoDbContext;
    private readonly ILogger<Entry> _logger;

    public Entry(
        RepoDbContext repoDbContext,
        ILogger<Entry> logger)
    {
        _repoDbContext = repoDbContext;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Starting Nuget Ninja PR bot...");

        _logger.LogInformation("Migrating database...");
        await _repoDbContext.Database.MigrateAsync();
    }
}
