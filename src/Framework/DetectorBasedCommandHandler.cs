// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.NugetNinja;

public abstract class DetectorBasedCommandHandler<T> 
    : ServiceCommandHandler<DetectorStarterService<T>> where T : IActionDetector
{
}
