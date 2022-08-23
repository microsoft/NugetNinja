// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.PrBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                .Build()
                .Services
                .GetRequiredService<Entry>()
                .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => 
                {
                    logging
                        .AddFilter("Microsoft.Extensions", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning);
                    logging.AddConsole();
                    logging.SetMinimumLevel(LogLevel.Information);
                })
                .ConfigureServices(services =>
                {
                    services.AddMemoryCache();
                    services.AddHttpClient();
                    services.AddSingleton<CacheService>();
                    services.AddTransient<RetryEngine>();
                    services.AddTransient<NugetService>();
                    services.AddTransient<CommandRunner>();
                    services.AddTransient<WorkspaceManager>();
                    services.AddDbContextPool<RepoDbContext>(optionsBuilder =>
                        optionsBuilder.UseSqlite(connectionString: "DataSource=app.db;Cache=Shared"));
                    services.AddTransient<Entry>();
                });
        }
    }
}
