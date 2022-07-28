// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin
{
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
            services.AddTransient<PackageReferenceUpgradeDetector>();
        }
    }
}
