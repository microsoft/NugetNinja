// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Core;

public sealed class NugetVersion : ICloneable, IComparable<NugetVersion?>, IEquatable<NugetVersion?>
{
    public NugetVersion(string versionString)
    {
        SourceString = versionString;
        if (versionString.Contains("-"))
        {
            PrimaryVersion = Version.Parse(versionString.Split("-")[0]);
            AdditionalText = versionString.Split("-")[1].ToLower().Trim();
        }
        else
        {
            PrimaryVersion = Version.Parse(versionString);
        }
    }

    public string SourceString { get; set; }
    public Version PrimaryVersion { get; }
    public string AdditionalText { get; } = string.Empty;

    public static bool operator ==(NugetVersion? lvs, NugetVersion? rvs) => lvs?.Equals(rvs) ?? ReferenceEquals(lvs, rvs);

    public static bool operator !=(NugetVersion? lvs, NugetVersion? rvs) => !(lvs == rvs);

    public static bool operator <(NugetVersion? lvs, NugetVersion? rvs) => lvs?.CompareTo(rvs) < 0;

    public static bool operator >(NugetVersion? lvs, NugetVersion? rvs) => lvs?.CompareTo(rvs) > 0;

    public object Clone() => new NugetVersion(SourceString);

    public bool IsPreviewVersion() => !string.IsNullOrWhiteSpace(AdditionalText);

    public int CompareTo(NugetVersion? otherNugetVersion)
    {
        if (ReferenceEquals(otherNugetVersion, null))
        {
            throw new ArgumentNullException(paramName: nameof(otherNugetVersion));
        }

        if (!PrimaryVersion.Equals(otherNugetVersion.PrimaryVersion))
        {
            return PrimaryVersion.CompareTo(otherNugetVersion.PrimaryVersion);
        }

        if (!string.IsNullOrWhiteSpace(AdditionalText) &&
            !string.IsNullOrWhiteSpace(otherNugetVersion.AdditionalText))
        {
            return string.CompareOrdinal(AdditionalText, otherNugetVersion.AdditionalText);
        }
        if (!string.IsNullOrWhiteSpace(AdditionalText))
        {
            return -1;
        }
        if (!string.IsNullOrWhiteSpace(otherNugetVersion.AdditionalText))
        {
            return 1;
        }
        return 0;

    }

    public bool Equals(NugetVersion? otherNugetVersion)
    {
        if (ReferenceEquals(otherNugetVersion, null))
        {
            return false;
        }
        return
            PrimaryVersion.Equals(otherNugetVersion.PrimaryVersion) &&
            AdditionalText.Equals(otherNugetVersion.AdditionalText);
    }

    public override string ToString()
    {
        return $"{PrimaryVersion}-{AdditionalText}".TrimEnd('-');
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;
        if (ReferenceEquals(this, null))
            return false;
        if (ReferenceEquals(obj, null))
            return false;
        if (obj is NugetVersion nuVersion)
            return Equals(nuVersion);
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(PrimaryVersion, AdditionalText);
    }
}
