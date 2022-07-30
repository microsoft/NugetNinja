// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Core;

public interface IEntryService
{
    public Task OnServiceStartedAsync(string path, bool shouldTakeAction);
}
