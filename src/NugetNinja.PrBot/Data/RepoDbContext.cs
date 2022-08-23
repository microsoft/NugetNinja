// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace Microsoft.NugetNinja.PrBot;

public class RepoDbContext : DbContext
{
    public RepoDbContext(DbContextOptions<RepoDbContext> options) : base(options)
    {
    }

    public DbSet<GitRepo> Repos => Set<GitRepo>();
}
