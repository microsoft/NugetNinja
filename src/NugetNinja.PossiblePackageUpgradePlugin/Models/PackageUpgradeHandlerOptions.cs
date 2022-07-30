// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PackageUpgradeHandlerOptions
{
    public PackageUpgradeHandlerOptions(bool usePreview, string customNugetServer, string patToken)
    {
        UsePreview = usePreview;
        CustomNugetServer = customNugetServer;
        PatToken = patToken;
    }

    public bool UsePreview { get; }
    public string CustomNugetServer { get; set; }
    public string PatToken { get; }
}
