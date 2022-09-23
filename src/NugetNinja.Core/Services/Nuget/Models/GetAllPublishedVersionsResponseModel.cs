// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Microsoft.NugetNinja.Core;

public class GetAllPublishedVersionsResponseModel
{
    [JsonPropertyName("versions")]
    public IReadOnlyCollection<string>? Versions { get; init; }
}
