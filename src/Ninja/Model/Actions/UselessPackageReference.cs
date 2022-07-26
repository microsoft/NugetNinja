// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class UselessPackageReference : IAction
{
    public UselessPackageReference(Project source, Package target)
    {
        SourceProjectName = source;
        TargetPackage = target;
    }

    public Project SourceProjectName { get; set; }
    public Package TargetPackage { get; set; }

    public string BuildMessage()
    {
        return $"The project: '{SourceProjectName}' don't have to reference package '{TargetPackage}' because it already has its access via another path!";
    }

    public void TakeAction()
    {
        // To DO: Remove this reference.
        throw new NotImplementedException();
    }
}
