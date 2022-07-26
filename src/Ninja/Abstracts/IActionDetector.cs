// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public interface IActionDetector
{
    public IAsyncEnumerable<IAction> AnalyzeAsync(Model context);
}
