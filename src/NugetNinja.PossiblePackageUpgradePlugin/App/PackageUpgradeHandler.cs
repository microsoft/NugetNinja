// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PackageUpgradeHandler<S> : DetectorBasedCommandHandler<PackageReferenceUpgradeDetector, S>
    where S : class, IStartUp, new()
{
    public override string Name => "upgrade-pkg";

    public override string Description => "The command to upgrade all package references to possible latest and avoid conflicts.";

    public readonly Option<bool> AllowPreviewOption =
        new Option<bool>(
            aliases: new[] { "--allow-preview" },
            description: "Allow using preview versions of packages from Nuget.");

    public override Option[] GetOptions()
    {
        return new Option[]
        {
            AllowPreviewOption
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
            AllowPreviewOption);
    }

    public Task ExecutePackageUpgradeHandler(string path, bool dryRun, bool verbose, bool allowPreviewOption)
    {
        var services = base.BuildServices(verbose);
        services.AddSingleton(new PackageUpgradeHandlerOptions(allowPreviewOption));
        return base.RunFromServices(services, path, dryRun);
    }
}
