// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PackageListerPlugin;

public class PackageListingHandler : DetectorBasedCommandHandler<PackageListerDetector, StartUp>
{
    public override string Name => "list-packages";

    public override string Description => "The command to generate a csv file with all direct packages and nuget.org descriptions. Dry run will only list the packages on the console.";
}
