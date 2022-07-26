// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using System.Reflection;
using Microsoft.NugetNinja;
using Microsoft.NugetNinja.Framework;

var rootCommand = new RootCommand(@"Nuget Ninja, a tool for detecting dependencies of .NET projects.");

var pathOption = new Option<string>(
    aliases: new[] { "--path", "-p" },
    description: "Path of the projects to be changed.")
{
    IsRequired = true
};

var dryRunOption = new Option<bool>(
    aliases: new[] { "--dry-run", "-d" },
    description: "Preview changes without actually making them");

var verboseOption = new Option<bool>(
    aliases: new[] { "--verbose", "-v" },
    description: "Show detailed log");

rootCommand.AddGlobalOption(dryRunOption);
rootCommand.AddGlobalOption(verboseOption);
rootCommand.AddGlobalOption(pathOption);

var subCommands = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass)
    .Where(t => !t.IsAbstract)
    .Where(t => t.IsSubclassOf(typeof(CommandHandler)));

foreach(var subCommandType in subCommands)
{
    var instance = Activator.CreateInstance(subCommandType) as CommandHandler 
        ?? throw new TypeLoadException($"Failed when creating new instance from type: {subCommandType}");
    rootCommand.Add(instance.BuildAsCommand(pathOption, dryRunOption, verboseOption));
}

return await rootCommand.InvokeAsync(args);
