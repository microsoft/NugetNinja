// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class PackageReferenceHandler<S> : DetectorBasedCommandHandler<UselessPackageReferenceDetector, S>
    where S : class, IStartUp, new()
{
    public override string Name => "package-reference-clean";

    public override string Description => "The command to clean up useless package references.";

    public override string[] Alias => new string[] { "clean-pkg" };

}
