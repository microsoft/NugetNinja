// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackagePlugin : INinjaPlugin
{
    public CommandHandler[] Install() => new CommandHandler[] { new DeprecatedPackageHandler() };
}
