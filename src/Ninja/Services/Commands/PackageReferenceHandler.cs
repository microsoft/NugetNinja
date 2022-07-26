// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class PackageReferenceHandler : DetectorBasedCommandHandler<UselessPackageReferenceDetector>
{
    public override string Name => "package-reference-cleaner";

    public override string Description => "The command to clean up useless package references.";

    public override string[] Alias => new string[] { "clean-pkg" };

}
