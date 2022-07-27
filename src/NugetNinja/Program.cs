// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.NugetNinja;
using Microsoft.NugetNinja.Framework;
using Microsoft.NugetNinja.UselessPackageReferencePlugin;

var rootCommand = new RootCommand(@"Nuget Ninja, a tool for detecting dependencies of .NET projects.")
    .AddGlobalOptions();

var handlers = new CommandHandler[]
{ 
    new PackageReferenceHandler<StartUp>(),
    new ProjectReferenceHandler<StartUp>(),
    new PackageUpgradeHandler<StartUp>()
};

foreach (var commandHandlers in handlers)
{
    rootCommand.Add(commandHandlers.BuildAsCommand());
}

return await rootCommand.InvokeAsync(args);
