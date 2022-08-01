// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class UselessPackageReference : IAction
{
    public UselessPackageReference(Project source, Package target)
    {
        SourceProjectName = source;
        TargetPackage = target;
    }

    public Project SourceProjectName { get; set; }
    public Package TargetPackage { get; set; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProjectName}' don't have to reference package '{TargetPackage}' because it already has its access via another path!";
    }

    public Task TakeActionAsync()
    {
        return this.SourceProjectName.RemoveReferenceAsync(TargetPackage.Name, "PackageReference");
    }
}
