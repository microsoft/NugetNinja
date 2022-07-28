// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.Framework;

public interface INinjaPlugin
{
    public CommandHandler Install();
}
