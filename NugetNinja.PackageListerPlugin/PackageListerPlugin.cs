// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PackageListerPlugin;

public class PackageListerPlugin : INinjaPlugin
{
    public CommandHandler[] Install() => new CommandHandler[] { new PackageListingHandler() };
}


