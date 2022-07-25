// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

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
}
