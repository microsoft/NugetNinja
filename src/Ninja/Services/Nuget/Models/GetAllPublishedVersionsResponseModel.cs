// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.NugetNinja;

public class GetAllPublishedVersionsResponseModel
{
    [JsonPropertyName("versions")]
    public List<string>? Versions { get; set; }
}
