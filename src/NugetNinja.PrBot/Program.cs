// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.NugetNinja.Core;

var services = new ServiceCollection();
services.AddMemoryCache();
services.AddHttpClient();
services.AddSingleton<CacheService>();
services.AddTransient<RetryEngine>();
services.AddTransient<NugetService>();
