﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.UselessProjectReferencePlugin;

public class ProjectReferenceHandler : DetectorBasedCommandHandler<UselessProjectReferenceDetector, StartUp>
{
    public override string Name => "clean-prj";

    public override string Description => "The command to clean up possible useless project references.";
}
