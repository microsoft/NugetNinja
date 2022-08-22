// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

var services = new ServiceCollection();
services.AddLogging(logging =>
{
    logging
        .AddFilter("Microsoft.Extensions", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning);
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
});
services.AddMemoryCache();
services.AddHttpClient();
services.AddSingleton<CacheService>();
services.AddTransient<RetryEngine>();
services.AddTransient<NugetService>();
