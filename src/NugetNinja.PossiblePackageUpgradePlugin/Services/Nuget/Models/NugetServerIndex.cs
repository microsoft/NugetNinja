// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Microsoft.NugetNinja.PossiblePackageUpgradePlugin.Services.Nuget.Models
{
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
        public List<ResourcesItem> Resources { get; set; } = new List<ResourcesItem>();

        [JsonPropertyName("version")]
        public string Version { get; set; } = string.Empty;
    }
}
