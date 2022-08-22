// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PrBot;

public enum RepoProvider
{
    GitHub,
    AzureDevOps
}

public class GitRepo
{
    public GitRepo(
        string name, 
        string cloneEndpoint,
        RepoProvider repoProvider)
    {
        Name = name;
        CloneEndpoint = cloneEndpoint;
        Provider = repoProvider;
    }

    [Key]
    public int Id { get; set; }

    public string Name { get; set; }

    public string CloneEndpoint { get; set; }
    public string? CloneToken { get; set; }

    public string NugetServer { get; set; } = NugetService.DefaultNugetServer;
    public string? NugetPatToken { get; set; }

    public RepoProvider Provider { get; set; }
}
