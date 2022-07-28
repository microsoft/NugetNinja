// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PossiblePackageUpgradePlugin : INinjaPlugin
{
    public CommandHandler Install()
    {
        return new PackageUpgradeHandler<StartUp>();
    }
}
