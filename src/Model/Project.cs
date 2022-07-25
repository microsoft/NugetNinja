// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class Project
{
    public Project(string pathOnDisk)
    {
        PathOnDisk = pathOnDisk;
    }

    public string PathOnDisk { get; set; }

    public List<Project> ProjectReferences { get; init; } = new List<Project>();

    public List<Package> PackageReferences { get; init; } = new List<Package>();

    public override string ToString()
    {
        return Path.GetFileNameWithoutExtension(PathOnDisk);
    }
}
