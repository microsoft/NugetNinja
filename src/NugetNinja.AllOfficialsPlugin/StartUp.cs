// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
