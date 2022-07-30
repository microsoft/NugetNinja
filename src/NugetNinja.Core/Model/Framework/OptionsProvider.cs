// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;

namespace Microsoft.NugetNinja.Core;

public static class OptionsProvider
{
    public static readonly Option<string> PathOptions = new Option<string>(
        aliases: new[] { "--path", "-p" },
        description: "Path of the projects to be changed.")
    {
        IsRequired = true
    };

    public static readonly Option<bool> DryRunOption = new Option<bool>(
        aliases: new[] { "--dry-run", "-d" },
        description: "Preview changes without actually making them");

    public static readonly Option<bool> VerboseOption = new Option<bool>(
        aliases: new[] { "--verbose", "-v" },
        description: "Show detailed log");

    public static Option[] GetGlobalOptions()
    {
        return new Option[]
        {
            PathOptions,
            DryRunOption,
            VerboseOption
        };
    }

    public static RootCommand AddGlobalOptions(this RootCommand command)
    {
        var options = GetGlobalOptions();
        foreach (var option in options)
        {
            command.AddGlobalOption(option);
        }
        return command;
    }

    public static RootCommand AddPlugins(this RootCommand command, params INinjaPlugin[] pluginInstallers)
    {
        foreach (var plugin in pluginInstallers)
        {
            command.Add(plugin.Install().BuildAsCommand());
        }
        return command;
    }
}
