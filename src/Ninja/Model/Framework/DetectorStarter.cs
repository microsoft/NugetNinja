// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja;

public class DetectorStarter<D> : IEntryService where D : IActionDetector
{
    private readonly ILogger<DetectorStarter<D>> _logger;
    private readonly Extractor _extractor;
    private readonly D _detector;

    public DetectorStarter(
        ILogger<DetectorStarter<D>> logger,
        Extractor extractor,
        D detector)
    {
        _logger = logger;
        _extractor = extractor;
        _detector = detector;
    }

    public async Task OnServiceStartedAsync(string path, bool shouldTakeAction)
    {
        var model = await _extractor.Parse(path);
        var actions = _detector.Analyze(model);
        foreach (var action in actions)
        {
            _logger.LogWarning(action.BuildMessage());
            if (shouldTakeAction)
            {
                action.TakeAction();
            }
        }
    }
}
