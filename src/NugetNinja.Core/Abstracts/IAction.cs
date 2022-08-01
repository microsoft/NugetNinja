// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Core;

public interface IAction
{
    public string BuildMessage();

    public Task TakeActionAsync();
}
