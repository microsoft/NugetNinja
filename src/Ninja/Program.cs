// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.NugetNinja;
using Microsoft.NugetNinja.Framework;

var rootCommand = new RootCommand(@"Nuget Ninja, a tool for detecting dependencies of .NET projects.")
    .AddGlobalOptions();

foreach (var subCommandInstance in StartUp.GetCommandHandlers())
{
    rootCommand.Add(subCommandInstance.BuildAsCommand());
}

return await rootCommand.InvokeAsync(args);
