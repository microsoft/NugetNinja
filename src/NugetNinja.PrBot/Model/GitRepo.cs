// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PrBot;

public enum RepoProvider
{
    GitHub,
    AzureDevOps
}

public class GitRepo
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Obsolete(error: true, message: "This is for Entity Framework!")]
    public GitRepo() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public GitRepo(
        string name,
        string defaultBranch,
        string cloneEndpoint,
        RepoProvider repoProvider)
    {
        Name = name;
        DefaultBranch = defaultBranch;
        CloneEndpoint = cloneEndpoint;
        Provider = repoProvider;
    }

    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string DefaultBranch { get; set; }

    public string CloneEndpoint { get; set; }
    public string? CloneToken { get; set; }

    public string NugetServer { get; set; } = NugetService.DefaultNugetServer;
    public string? NugetPatToken { get; set; }

    public RepoProvider Provider { get; set; }
}
