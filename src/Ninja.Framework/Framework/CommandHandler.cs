// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.CommandLine;

namespace Microsoft.NugetNinja.Framework;

public abstract class CommandHandler
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string[] Alias { get; }
    public abstract void OnCommandBuilt(Command command);

    public virtual CommandHandler[] GetSubCommandHandlers()
    {
        return Array.Empty<CommandHandler>();
    }

    public virtual Option[] GetOptions()
    {
        return Array.Empty<Option>();
    }

    public virtual Command BuildAsCommand()
    {
        var command = new Command(Name, Description);
        foreach (var alias in Alias)
        {
            command.AddAlias(alias);
        }

        foreach (var option in GetOptions())
        {
            command.AddOption(option);
        }

        OnCommandBuilt(command);

        return command;
    }
}
