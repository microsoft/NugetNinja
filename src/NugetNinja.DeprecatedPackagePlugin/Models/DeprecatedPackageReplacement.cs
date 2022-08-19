// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageReplacement : IAction
{
    public DeprecatedPackageReplacement(Project source, Package target, string? alternative)
    {
        SourceProjectName = source;
        Package = target;
        Alternative = alternative;
    }

    public Project SourceProjectName { get; }
    public Package Package { get; }
    public string? Alternative { get; }

    public string BuildMessage()
    {
        var alternativeText = string.IsNullOrWhiteSpace(Alternative) ?
            string.Empty :
            $"Please consider to replace that to: '{Alternative}'.";
        return $"The project: '{SourceProjectName}' referenced a deprecated package: {Package} {Package.Version}! {alternativeText}";
    }

    public Task TakeActionAsync()
    {
        // To DO: Remove this reference.
        return Task.CompletedTask;
    }
}
