// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.NugetNinja.Core;

public class ResourcesItem
{
    [JsonPropertyName("@id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("@type")]
    public string Type { get; set; } = string.Empty;
}

public class NugetServerIndex
{
    [JsonPropertyName("resources")]
    public IReadOnlyCollection<ResourcesItem>? Resources { get; init; }

    [JsonPropertyName("version")]
    public string Version { get; set; } = string.Empty;
}

public class NugetServerEndPoints
{
    public NugetServerEndPoints(string packageBaseAddress, string registrationsBaseUrl)
    {
        PackageBaseAddress = packageBaseAddress;
        RegistrationsBaseUrl = registrationsBaseUrl;
    }

    /// <summary>
    /// Base URL of Azure storage where NuGet package registration info is stored
    /// 
    /// Sample: https://api.nuget.org/v3/registration5-semver1/
    /// </summary>
    public string RegistrationsBaseUrl { get;set; }

    /// <summary>
    /// Base URL of where NuGet packages are stored, in the format https://api.nuget.org/v3-flatcontainer/{id-lower}/{version-lower}/{id-lower}.{version-lower}.nupkg
    /// 
    /// Sample: https://api.nuget.org/v3-flatcontainer/
    /// </summary>
    public string PackageBaseAddress { get; set; }
}

public class RegistrationIndex
{
    [JsonPropertyName("catalogEntry")]
    public string? CatalogEntry { get; set; }
}

public class AlternatePackage
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }
}

public class Deprecation
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("alternatePackage")]
    public AlternatePackage? AlternatePackage { get; set; }
}

public class CatalogInformation
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("deprecation")]
    public Deprecation? Deprecation { get; set; }

    [JsonPropertyName("vulnerabilities")]
    public IReadOnlyCollection<Vulnerability>? Vulnerabilities { get; init; }
}

public class Vulnerability
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("@id")]
    public string Id { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("@type")]
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("advisoryUrl")]
    public string AdvisoryUrl { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("severity")]
    public string Severity { get; set; } = string.Empty;
}
