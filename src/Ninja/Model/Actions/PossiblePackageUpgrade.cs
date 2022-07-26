// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

internal class PossiblePackageUpgrade : IAction
{
    public PossiblePackageUpgrade(Project source, Package target, Version newVersion)
    {
        SourceProjectName = source;
        Package = target;
        NewVersion = newVersion;
    }

    public Project SourceProjectName { get; }
    public Package Package { get; }
    public Version NewVersion { get; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProjectName}' can upgrade the package '{Package}' from '{Package.VersionText}' to '{NewVersion}'.";
    }

    public void TakeAction()
    {
        // To DO: Remove this reference.
        throw new NotImplementedException();
    }
}
