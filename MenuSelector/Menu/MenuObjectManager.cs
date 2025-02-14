using System.Collections.Generic;
using System.Linq;
using IPA.Config;
using MenuSelector.Utilities;
using SiraUtil.Affinity;
using SiraUtil.Logging;
using UnityEngine.PlayerLoop;
using Zenject;
using static MenuEnvironmentManager;
using static MenuEnvironmentManager.MenuEnvironmentType;

namespace MenuSelector.Menu;

internal class MenuObjectManager : IInitializable, IAffinity
{
    private readonly MenuEnvironmentManager menuEnvironmentManager;
    private readonly PluginConfig pluginConfig;
    private readonly SiraLog log;
    private readonly Dictionary<MenuEnvironmentType, string> vanillaMenuNames;

    public MenuObjectManager(
        MenuEnvironmentManager menuEnvironmentManager,
        PluginConfig pluginConfig, SiraLog log)
    {
        this.menuEnvironmentManager = menuEnvironmentManager;
        this.pluginConfig = pluginConfig;
        this.log = log;

        vanillaMenuNames = MenuEnvironmentObjects.ToDictionary(x => x.menuEnvironmentType, x => x.wrapper.name);
    }
    
    // If the mod is enabled, prevent the MenuEnvironmentManager from setting the default environment after we do
    [AffinityPrefix]
    [AffinityPatch(typeof(MenuEnvironmentManager), nameof(MenuEnvironmentManager.Start))]
    public bool StartPrefix() => !pluginConfig.Enabled;
    
    public IEnumerable<MenuEnvironmentObjects> MenuEnvironmentObjects => menuEnvironmentManager._data;
    public IEnumerable<string> MenuEnvironmentNames => menuEnvironmentManager._data.Select(x => x.wrapper.name);

    public void Initialize()
    {
        if (!MenuEnvironmentNames.Contains(pluginConfig.DefaultEnvironment))
        {
            pluginConfig.DefaultEnvironment = vanillaMenuNames[Default];
        }

        if (!MenuEnvironmentNames.Contains(pluginConfig.LobbyEnvironment))
        {
            pluginConfig.LobbyEnvironment = vanillaMenuNames[Lobby];
        }
        
        if (menuEnvironmentManager._prevMenuEnvironmentType == None)
        {
            menuEnvironmentManager._prevMenuEnvironmentType = Default;
        }
        
        Refresh();
    }

    public void Refresh()
    {
        var (defaultEnvName, lobbyEnvName) = pluginConfig.Enabled
            ? (pluginConfig.DefaultEnvironment, pluginConfig.LobbyEnvironment)
            : (vanillaMenuNames[Default], vanillaMenuNames[Lobby]);

        SetTypeEnvironment(defaultEnvName, Default);
        SetTypeEnvironment(lobbyEnvName, Lobby);
    }

    private void SetTypeEnvironment(string environmentName, MenuEnvironmentType type)
    {
        var environment = MenuEnvironmentObjects.FirstOrDefault(x => x.wrapper.name == environmentName);
        if (environment == null)
        {
            return;
        }
        
        var previousEnvironment = MenuEnvironmentObjects.FirstOrDefault(x => x.menuEnvironmentType == type);
        if (previousEnvironment != null)
        {
            previousEnvironment._menuEnvironmentType = None;
        }
        
        environment._menuEnvironmentType = type;

        UpdateEnvironmentForType(type);
    }

    private void UpdateEnvironmentForType(MenuEnvironmentType type)
    {
        if (menuEnvironmentManager._prevMenuEnvironmentType != type)
        {
            return;
        }
        
        foreach (var environmentObjects in MenuEnvironmentObjects)
        {
            environmentObjects.wrapper.SetActive(false);
        }

        MenuEnvironmentObjects
            .FirstOrDefault(x => x.menuEnvironmentType == type)?
            .wrapper.SetActive(true);
    }
}