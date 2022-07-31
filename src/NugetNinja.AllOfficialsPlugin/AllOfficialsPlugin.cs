using Microsoft.NugetNinja.Core;

namespace Microsoft.NugetNinja.AllOfficialsPlugin;

public class AllOfficialsPlugin : INinjaPlugin
{
    public CommandHandler Install()
    {
        return new AllOfficialsHandler();
    }
}
