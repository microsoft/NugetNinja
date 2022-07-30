// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageHandler : DetectorBasedCommandHandler<DeprecatedPackageDetector, StartUp>
{
    public override string Name => "remove-deprecated";

    public override string Description => "The command to replace all deprecated packages to new packages.";

    public readonly Option<string> CustomNugetServer =
        new Option<string>(
            aliases: new[] { "--nuget-server" },
            description: "If you want to use a customized nuget server instead of the official nuget.org, you can set it with a value like: https://nuget.myserver/v3/index.json");

    public readonly Option<string> PatToken=
    new Option<string>(
        aliases: new[] { "--token" },
        description: "The PAT token which has privilege to access the nuget server. See: https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate");

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
            ExecutePackageUpgradeHandler,
            OptionsProvider.PathOptions,
            OptionsProvider.DryRunOption,
            OptionsProvider.VerboseOption,
            CustomNugetServer,
            PatToken);
    }

    public Task ExecutePackageUpgradeHandler(string path, bool dryRun, bool verbose, string customNugetServer, string patToken)
    {
        var services = base.BuildServices(verbose);
        services.AddSingleton(new DeprecatedPackageHandlerOptions(customNugetServer, patToken));
        return base.RunFromServices(services, path, dryRun);
    }
}
