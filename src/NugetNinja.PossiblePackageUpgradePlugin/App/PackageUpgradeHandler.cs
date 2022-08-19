// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PackageUpgradeHandler : DetectorBasedCommandHandler<PackageReferenceUpgradeDetector, StartUp>
{
    public override string Name => "upgrade-pkg";

    public override string Description => "The command to upgrade all package references to possible latest and avoid conflicts.";
}
