// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin;

public class PackageUpgradeHandlerOptions
{
    public PackageUpgradeHandlerOptions(bool usePreview)
    {
        UsePreview = usePreview;
    }

    public bool UsePreview { get; }
}
