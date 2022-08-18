// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PossiblePackageUpgrade : IAction
{
    public PossiblePackageUpgrade(Project source, Package target, NugetVersion newVersion)
    {
        SourceProjectName = source;
        Package = target;
        NewVersion = newVersion;
    }

    public Project SourceProjectName { get; }
    public Package Package { get; }
    public NugetVersion NewVersion { get; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProjectName}' should upgrade the package '{Package}' from '{Package.VersionText}' to '{NewVersion}'.";
    }

    public Task TakeActionAsync()
    {
        return SourceProjectName.SetPackageReferenceVersionAsync(Package.Name, NewVersion);
    }
}
