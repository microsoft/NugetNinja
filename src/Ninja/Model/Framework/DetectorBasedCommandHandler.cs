// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja;

public abstract class DetectorBasedCommandHandler<D> 
    : ServiceCommandHandler<DetectorStarter<D>, StartUp> where D : IActionDetector
{
}
