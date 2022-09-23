// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin;

public class ResetRuntime : IAction
{
    private readonly int _inserted;
    private readonly int _deprecated;
    public Project Project { get; }
    public string[] NewRuntimes { get; }

    public ResetRuntime(
        Project project,
        string[] newRuntimes,
        int inserted,
        int deprecated)
    {
        _inserted = inserted;
        _deprecated = deprecated;
        Project = project;
        NewRuntimes = newRuntimes;
    }

    public string BuildMessage()
    {
        return $"The project: '{Project}' with runtimes: '{string.Join(',', Project.GetTargetFrameworks())}' should insert {_inserted} runtime(s) and deprecate {_deprecated} runtime(s) to '{string.Join(',', NewRuntimes)}'.";
    }

    public async Task TakeActionAsync()
    {
        if (NewRuntimes.Length > 1)
        {
            await Project.AddOrUpdateProperty(nameof(Project.TargetFrameworks), string.Join(';', NewRuntimes));
            await Project.RemoveProperty(nameof(Project.TargetFramework));
        }
        else
        {
            await Project.AddOrUpdateProperty(nameof(Project.TargetFramework), NewRuntimes.FirstOrDefault() ?? string.Empty);
            await Project.RemoveProperty(nameof(Project.TargetFrameworks));
        }
    }
}
