// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja;

public class ProjectReferenceHandler<S> : DetectorBasedCommandHandler<UselessProjectReferenceDetector, S>
    where S : class, IStartUp, new()
{
    public override string Name => "project-reference-clean";

    public override string Description => "The command to clean up useless project references.";

    public override string[] Alias => new string[] { "clean-prj" };
}
