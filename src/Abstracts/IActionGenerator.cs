// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public interface IActionGenerator
{
    public string[] CommandAliases { get; }

    public string CommandDescription { get; }

    public IEnumerable<IAction> Analyze(Model context);

}
