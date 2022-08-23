// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.NugetNinja.PrBot;

public class RepoDbContext : DbContext
{
    public RepoDbContext(DbContextOptions<RepoDbContext> options) : base(options)
    {
    }
    
    public DbSet<GitRepo> Repos { get; set; }
}
