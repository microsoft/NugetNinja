// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin;

public class ObsoletePackageReference : IAction
{
    public ObsoletePackageReference(Project source, string target)
    {
        SourceProject = source;
        TargetPackage = target;
    }

    public Project SourceProject { get; set; }
    public string TargetPackage { get; set; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProject}' don't have to reference package '{TargetPackage}' because it is deprecated.";
    }

    public Task TakeActionAsync()
    {
        return SourceProject.RemovePackageReferenceAsync(TargetPackage);
    }
}
