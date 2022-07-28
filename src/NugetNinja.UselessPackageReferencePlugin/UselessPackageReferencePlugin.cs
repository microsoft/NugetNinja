// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.UselessPackageReferencePlugin;

public class UselessPackageReferencePlugin : INinjaPlugin
{
    public CommandHandler Install()
    {
        return new PackageReferenceHandler<StartUp>();
    }
}
