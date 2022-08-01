// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class VulnerablePackageReplacement : IAction
{
    public VulnerablePackageReplacement(Project source, Package target)
    {
        SourceProjectName = source;
        Package = target;
    }

    public Project SourceProjectName { get; }
    public Package Package { get; }

    public string BuildMessage()
    {
        return $@"The project: '{SourceProjectName}' referenced a package {Package} {Package.Version} which has known vulnerabilities! Please consider to upgrade\remove\replace it!";
    }

    public Task TakeActionAsync()
    {
        // To DO: Remove this reference.
        throw new NotImplementedException();
    }
}
