// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;

namespace Microsoft.NugetNinja.Core;

public static class Helper
{
    public static string? AppVersion
    {
        get
        {
            var asm = Assembly.GetEntryAssembly();
            if (null == asm) return "N/A";

            // e.g. 11.2.0.0
            var fileVersion = asm.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version;

            // e.g. 11.2-preview+e57ab0321ae44bd778c117646273a77123b6983f
            var version = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
            if (!string.IsNullOrWhiteSpace(version) && version.IndexOf('+') > 0)
            {
                var gitHash = version[(version.IndexOf('+') + 1)..]; // e57ab0321ae44bd778c117646273a77123b6983f
                var prefix = version[..version.IndexOf('+')]; // 11.2-preview

                if (gitHash.Length <= 6) return version;

                // consider valid hash
                var gitHashShort = gitHash[..6];
                return !string.IsNullOrWhiteSpace(gitHashShort) ? $"{prefix} ({gitHashShort})" : fileVersion;
            }

            return version ?? fileVersion;
        }
    }
}