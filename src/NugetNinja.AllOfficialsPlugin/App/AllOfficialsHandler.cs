// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.AllOfficialsPlugin;

public class AllOfficialsHandler : ServiceCommandHandler<RunAllOfficialPluginsService, StartUp>
{
    public override string Name => "all-officials";

    public override string Description => "The command to run all officially supported features.";

    public override string[] Alias => new[] { "all" };
}
