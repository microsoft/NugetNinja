// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public class Package
{
    public Package(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
