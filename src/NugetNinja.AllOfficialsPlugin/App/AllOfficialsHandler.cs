// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.DeprecatedPackagePlugin;
using Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

namespace Microsoft.NugetNinja.AllOfficialsPlugin;

public class AllOfficialsHandler : ServiceCommandHandler<RunAllOfficialPluginsService, StartUp>
{
    public override string Name => "all-officials";

    public override string Description => "The command to run all officially supported features.";

    public readonly Option<bool> AllowPreviewOption =
        new Option<bool>(
            aliases: new[] { "--allow-preview" },
            description: "Allow using preview versions of packages from Nuget.");

    public readonly Option<string> CustomNugetServer =
        new Option<string>(
            aliases: new[] { "--nuget-server" },
            description: "If you want to use a customized nuget server instead of the official nuget.org, you can set it with a value like: https://nuget.myserver/v3/index.json");

    public readonly Option<string> PatToken =
        new Option<string>(
            aliases: new[] { "--token" },
            description: "The PAT token which has privilege to access the nuget server. See: https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate");

    public override string[] Alias => new string[] { "all" };

    public override Option[] GetOptions()
    {
        return new Option[]
        {
            CustomNugetServer,
            PatToken
        };
    }

    public override void OnCommandBuilt(Command command)
    {
        var globalOptions = OptionsProvider.GetGlobalOptions();
        command.SetHandler(
            ExecuteAllPlugins,
            OptionsProvider.PathOptions,
            OptionsProvider.DryRunOption,
            OptionsProvider.VerboseOption,
            AllowPreviewOption,
            CustomNugetServer,
            PatToken);
    }

    public Task ExecuteAllPlugins(string path, bool dryRun, bool verbose, bool allowPreview, string customNugetServer, string patToken)
    {
        var services = base.BuildServices(verbose);
        services.AddSingleton(new DeprecatedPackageHandlerOptions(customNugetServer, patToken));
        services.AddSingleton(new PackageUpgradeHandlerOptions(allowPreview, customNugetServer, patToken));
        return base.RunFromServices(services, path, dryRun);
    }
}
