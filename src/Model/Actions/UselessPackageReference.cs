// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Model.Actions;

public class UselessPackageReference : IAction
{
    public UselessPackageReference(Project source, Package target)
    {
        SourceProjectName = source;
        TargetPackageName = target;
    }

    public Project SourceProjectName { get; set; }
    public Package TargetPackageName { get; set; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProjectName}' don't have to reference package '{TargetPackageName}' because it already has its access via another path!";
    }

    public void TakeAction()
    {
        // To DO: Remove this reference.
        throw new NotImplementedException();
    }
}
