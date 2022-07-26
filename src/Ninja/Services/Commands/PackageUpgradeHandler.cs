// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class PackageUpgradeHandler : DetectorBasedCommandHandler<PackageReferenceUpgradeDetector>
{
    public override string Name => "package-reference-upgrade";

    public override string Description => "The command to upgrade all package references to possible latest and avoid conflicts.";

    public override string[] Alias => new string[] { "upgrade-pkg" };
}
