// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.DeprecatedPackagePlugin;

public class DeprecatedPackageHandlerOptions
{
    public DeprecatedPackageHandlerOptions(string customNugetServer, string patToken)
    {
        CustomNugetServer = customNugetServer;
        PatToken = patToken;
    }

    public string CustomNugetServer { get; set; }
    public string PatToken { get; }
}
