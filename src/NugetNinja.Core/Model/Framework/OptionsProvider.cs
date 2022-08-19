// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;

namespace Microsoft.NugetNinja.Core;

public static class OptionsProvider
{
    public static readonly Option<string> PathOptions = new(
        aliases: new[] { "--path", "-p" },
        description: "Path of the projects to be changed.")
    {
        IsRequired = true
    };

    public static readonly Option<bool> DryRunOption = new(
        aliases: new[] { "--dry-run", "-d" },
        description: "Preview changes without actually making them");

    public static readonly Option<bool> VerboseOption = new(
        aliases: new[] { "--verbose", "-v" },
        description: "Show detailed log");

    public static Option<bool> AllowPreviewOption =
        new(
            aliases: new[] { "--allow-preview" },
            description: "Allow using preview versions of packages from Nuget.");

    public static Option<string> CustomNugetServer =
        new(
            aliases: new[] { "--nuget-server" },
            description: "If you want to use a customized nuget server instead of the official nuget.org, you can set it with a value like: https://nuget.myserver/v3/index.json");

    public static Option<string> PatToken =
        new(
            aliases: new[] { "--token" },
            description: "The PAT token which has privilege to access the nuget server. See: https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate");


    public static Option[] GetGlobalOptions()
    {
        return new Option[]
        {
            PathOptions,
            DryRunOption,
            VerboseOption,
            AllowPreviewOption,
            CustomNugetServer,
            PatToken
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
            foreach (var pluginFeature in plugin.Install())
            {
                command.Add(pluginFeature.BuildAsCommand());
            }
        }
        return command;
    }
}
