// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessProjectReferencePlugin;

public class UselessProjectReferencePlugin : INinjaPlugin
{
    public CommandHandler[] Install() => new CommandHandler[] { new ProjectReferenceHandler() };
}
