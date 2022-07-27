// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.NugetNinja.Framework;

public interface IStartUp
{
    public void ConfigureServices(IServiceCollection services);
}
