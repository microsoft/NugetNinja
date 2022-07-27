// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.Framework;
using Microsoft.NugetNinja.UselessPackageReferencePlugin;

namespace Microsoft.NugetNinja;

public class StartUp : IStartUp
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddHttpClient();
        services.AddSingleton<CacheService>();
        services.AddTransient<NugetService>();
        services.AddTransient<Extractor>();
        services.AddTransient<ProjectsEnumerator>();

        services.AddTransient<UselessProjectReferenceDetector>();
        services.AddTransient<UselessPackageReferenceDetector>();
        services.AddTransient<PackageReferenceUpgradeDetector>();
    }
}
