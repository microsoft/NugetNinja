// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja;
using System.Reflection;

var services = new ServiceCollection();
services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Trace);
});

services.AddSingleton<Entry>();
services.AddTransient<Extractor>();

var generators = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass)
    .Where(t => t.GetInterfaces().Contains(typeof(IActionGenerator)));

foreach (var generator in generators)
{
    services.AddTransient(typeof(IActionGenerator), generator);
}

services.AddTransient<ProjectsEnumerator>();

var serviceProvider = services.BuildServiceProvider();
var entry = serviceProvider.GetRequiredService<Entry>();
return await entry.RunAsync(args);
