// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.Core;

public abstract class DetectorBasedCommandHandler<T, S> : ServiceCommandHandler<DetectorStarter<T>, S> 
    where T : IActionDetector
    where S : class, IStartUp, new()
{
}
