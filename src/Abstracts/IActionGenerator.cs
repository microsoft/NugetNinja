// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public interface IActionGenerator
{
    public string GetHelp();

    public string GetCommandAlias();

    public IEnumerable<IAction> Analyze(Model context);
}
