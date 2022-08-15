// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.AllOfficialsPlugin;

public class StartUp : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        new DeprecatedPackagePlugin.StartUp().ConfigureServices(services);
        new PossiblePackageUpgradePlugin.StartUp().ConfigureServices(services);
        new UselessPackageReferencePlugin.StartUp().ConfigureServices(services);
        new UselessProjectReferencePlugin.StartUp().ConfigureServices(services);
    }
}
