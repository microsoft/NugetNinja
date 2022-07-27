// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Core;

public static class StringExtensions
{
    public static string GetAbsolutePath(string currentPath, string referencePath)
    {
        if (Path.IsPathRooted(referencePath))
        {
            return referencePath;
        }

        return Path.GetFullPath(Path.Combine(currentPath, referencePath));
    }

    public static Version ConvertToVersion(string version)
    {
        if (version.Contains("-"))
        {
            version = version.Substring(0, version.IndexOf('-'));
        }
        return Version.Parse(version);
    }
}
