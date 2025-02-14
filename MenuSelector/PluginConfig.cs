using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace MenuSelector;

internal class PluginConfig
{
    public virtual bool Enabled { get; set; } = true;
    
    public virtual string DefaultEnvironment { get; set; } = string.Empty;
    public virtual string LobbyEnvironment { get; set; } = string.Empty;
}