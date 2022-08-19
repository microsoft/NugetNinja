// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageHandler : DetectorBasedCommandHandler<DeprecatedPackageDetector, StartUp>
{
    public override string Name => "remove-deprecated";

    public override string Description => "The command to replace all deprecated packages to new packages.";
}
