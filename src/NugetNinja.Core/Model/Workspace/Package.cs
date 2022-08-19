// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Core;

public class Package
{
    public Package(string name, string versionText)
    {
        Name = name;
        VersionText = versionText;

        if (versionText.Contains('(') ||
            versionText.Contains(')') ||
            versionText.Contains('[') ||
            versionText.Contains(']'))
        {
            versionText = versionText
                .Replace("(", string.Empty)
                .Replace(")", string.Empty)
                .Replace("[", string.Empty)
                .Replace("]", string.Empty)
                .Split(',')[0];
        }

        Version = new NugetVersion(versionText); 
    }

    public string Name { get; set; }

    public NugetVersion Version { get; set; }
    public string VersionText { get; set; }

    public override string ToString()
    {
        return Name;
    }
}
