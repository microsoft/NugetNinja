// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Reflection;

namespace Microsoft.NugetNinja.Core;

public static class ClassScanner
{
    private static IEnumerable<Assembly> ScanAssemblies(Assembly entry)
    {
        yield return entry;
        var references = entry
            .GetReferencedAssemblies()
            .Where(t => !t.Name?.StartsWith("System") ?? false);
        foreach (var referenced in references)
        {
            foreach (var scanned in ScanAssemblies(Assembly.Load(referenced)))
            {
                yield return scanned;
            }
        }
    }

    public static IReadOnlyCollection<Type> AllAccessibleClass()
    {
        var entry = Assembly.GetEntryAssembly() ?? throw new InvalidOperationException("Couldn't load entry assembly!");
        return ScanAssemblies(entry)
            .Distinct()
            .SelectMany(t => t.GetTypes())
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsNestedPrivate)
            .Where(t => !t.IsGenericType)
            .Where(t => !t.IsInterface)
            .ToList()
            .AsReadOnly();
    }

    public static IReadOnlyCollection<Type> AllClassUnder(Assembly assembly)
    {
        return ScanAssemblies(assembly)
            .Distinct()
            .SelectMany(t => t.GetTypes())
            .Where(t => !t.IsAbstract)
            .Where(t => !t.IsNestedPrivate)
            .Where(t => !t.IsGenericType)
            .Where(t => !t.IsInterface)
            .ToList()
            .AsReadOnly();
    }
}
