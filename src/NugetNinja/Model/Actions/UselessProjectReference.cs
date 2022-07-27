// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja;

public class UselessProjectReference : IAction
{
    public UselessProjectReference(Project source, Project target)
    {
        SourceProjectName = source;
        TargetProjectName = target;
    }

    public Project SourceProjectName { get; set; }
    public Project TargetProjectName { get; set; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProjectName}' don't have to reference project '{TargetProjectName}' because it already has its access via another path!";
    }

    public void TakeAction()
    {
        // To DO: Remove this reference.
        throw new NotImplementedException();
    }
}
