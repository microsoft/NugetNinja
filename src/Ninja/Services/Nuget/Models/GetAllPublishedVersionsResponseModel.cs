// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.NugetNinja;

internal class GetAllPublishedVersionsResponseModel
{
    [JsonPropertyName("versions")]
    internal List<string>? Versions { get; set; }
}
