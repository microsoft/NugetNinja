// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja;

public class ProjectReferenceHandler : DetectorBasedCommandHandler<UselessProjectReferenceDetector>
{
    public override string Name => "project-reference-clean";

    public override string Description => "The command to clean up useless project references.";

    public override string[] Alias => new string[] { "clean-prj" };
}
