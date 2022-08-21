// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja.Core;

public abstract class DetectorBasedCommandHandler<T, TS> : ServiceCommandHandler<DetectorStarter<T>, TS> 
    where T : IActionDetector
    where TS : class, IStartUp, new()
{
}
