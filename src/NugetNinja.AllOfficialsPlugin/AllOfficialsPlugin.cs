// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.AllOfficialsPlugin;

public class AllOfficialsPlugin : INinjaPlugin
{
    public CommandHandler[] Install() => new CommandHandler[] { new AllOfficialsHandler() };
}
