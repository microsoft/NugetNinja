// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Logging;
using Microsoft.NugetNinja.Framework;

namespace Microsoft.NugetNinja.Core;

public class DetectorStarter<T> : IEntryService where T : IActionDetector
{
    private readonly ILogger<DetectorStarter<T>> _logger;
    private readonly Extractor _extractor;
    private readonly T _detector;

    public DetectorStarter(
        ILogger<DetectorStarter<T>> logger,
        Extractor extractor,
        T detector)
    {
        _logger = logger;
        _extractor = extractor;
        _detector = detector;
    }

    public async Task OnServiceStartedAsync(string path, bool shouldTakeAction)
    {
        _logger.LogInformation($"Parsing files to build project structure based on path: '{path}'...");
        var model = await _extractor.Parse(path);

        _logger.LogInformation($"Analysing possible actions via {typeof(T).Name}");
        var actions = _detector.AnalyzeAsync(model);
        await foreach (var action in actions)
        {
            _logger.LogWarning(action.BuildMessage());
            if (shouldTakeAction)
            {
                action.TakeAction();
            }
        }
    }
}
