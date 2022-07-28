// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.UselessProjectReferencePlugin;

public class UselessProjectReferencePlugin : INinjaPlugin
{
    public CommandHandler Install()
    {
        return new ProjectReferenceHandler<StartUp>();
    }
}
