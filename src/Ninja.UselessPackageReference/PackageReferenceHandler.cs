// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class PackageReferenceHandler : DetectorBasedCommandHandler<UselessPackageReferenceDetector>
{
    public override string Name => "package-reference-clean";

    public override string Description => "The command to clean up useless package references.";

    public override string[] Alias => new string[] { "clean-pkg" };

}
