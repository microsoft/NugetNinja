// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageReplacement : IAction
{
    public DeprecatedPackageReplacement(Project source, Package target, Package? alternative)
    {
        SourceProject = source;
        Package = target;
        Alternative = alternative;
    }

    public Project SourceProject { get; }
    public Package Package { get; }
    public Package? Alternative { get; }

    public string BuildMessage()
    {
        var alternativeText = Alternative != null ?
            string.Empty :
            $"Please consider to replace that to: '{Alternative}'.";
        return $"The project: '{SourceProject}' referenced a deprecated package: {Package} {Package.Version}! {alternativeText}";
    }

    public async Task TakeActionAsync()
    {
        if (this.Alternative != null)
        {
            await SourceProject.ReplacePackageReferenceAsync(Package.Name, Alternative);
        }
        else
        {
            await SourceProject.RemovePackageReferenceAsync(Package.Name);
        }
    }
}
