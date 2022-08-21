// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.MissingPropertyPlugin;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class MissingPropertyPlugin : INinjaPlugin
{
    public CommandHandler[] Install()
    {
        return new CommandHandler[] { new MissingPropertyHandler() };
    }
}
