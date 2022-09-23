// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin;

public class MissingPropertyPlugin : INinjaPlugin
{
    public CommandHandler[] Install() => new CommandHandler[] { new MissingPropertyHandler() };
}
