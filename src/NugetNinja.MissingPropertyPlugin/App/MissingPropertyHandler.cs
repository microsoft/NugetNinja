// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.MissingPropertyPlugin;

public class MissingPropertyHandler : DetectorBasedCommandHandler<MissingPropertyDetector, StartUp>
{
    public override string Name => "fill-properties";

    public override string Description => "The command to fill all missing properties for .csproj files.";
}
