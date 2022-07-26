// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja;

public abstract class DetectorBasedCommandHandler<T> : ServiceCommandHandler<DetectorStarter<T>, StartUp> 
    where T : IActionDetector
{
}
