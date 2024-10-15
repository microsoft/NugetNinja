// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PackageListerPlugin;

public class PackageListingHandler : DetectorBasedCommandHandler<PackageListerDetector, StartUp>
{
    public override string Name => "list-packages";

    public override string Description => "Generates a list of all packages used in the solution.";
}
