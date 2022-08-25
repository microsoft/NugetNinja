// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Microsoft.NugetNinja.PrBot;

public class GitRepo
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Obsolete(error: true, message: "This is for Entity Framework!")]
    public GitRepo() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public GitRepo(
        string repoName,
        string org)
    {
        Name = repoName;
        Org = org;
    }

    [Key]
    public int Id { get; set; }
    
    public string Org { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
