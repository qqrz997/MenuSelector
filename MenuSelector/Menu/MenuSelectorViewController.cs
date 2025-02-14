using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using MenuSelector.Utilities;
using Zenject;

namespace MenuSelector.Menu;

[HotReload(RelativePathToLayout = @".\mainView.bsml")]
[ViewDefinition("MenuSelector.Menu.mainView.bsml")]
internal class MenuSelectorViewController : BSMLAutomaticViewController
{
    [Inject] private readonly PluginConfig pluginConfig = null!;
    [Inject] private readonly MenuObjectManager menuObjectManager = null!;

    public IList<object> EnvironmentOptions => menuObjectManager.MenuEnvironmentNames.ToList<object>();
    
    public object EnvironmentOptionsFormatter(string value) => MenuEnvironmentObjectExtensions.FormatEnvironmentName(value);
    
    public bool Enabled
    {
        get => pluginConfig.Enabled;
        set
        {
            pluginConfig.Enabled = value;
            menuObjectManager.Refresh();
        }
    }

    public string DefaultEnvironment
    {
        get => pluginConfig.DefaultEnvironment;
        set
        {
            pluginConfig.DefaultEnvironment = value;
            menuObjectManager.Refresh();
        }
    }

    public string LobbyEnvironment
    {
        get => pluginConfig.LobbyEnvironment;
        set => pluginConfig.LobbyEnvironment = value;
    }
}