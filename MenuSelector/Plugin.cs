using IPA;
using IPA.Config.Stores;
using IPA.Loader;
using MenuSelector.Installers;
using SiraUtil.Zenject;
using IpaConfig = IPA.Config.Config;
using IpaLogger = IPA.Logging.Logger;

namespace MenuSelector;

[Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
internal class Plugin
{
    internal static IpaLogger Log { get; private set; } = null!;
    
    [Init]
    public Plugin(IpaLogger ipaLogger, IpaConfig ipaConfig, Zenjector zenjector, PluginMetadata pluginMetadata)
    {
        Log = ipaLogger;

        var pluginConfig = ipaConfig.Generated<PluginConfig>();

        zenjector.UseLogger(Log);
        zenjector.Install<AppInstaller>(Location.App, pluginConfig);
        zenjector.Install<MenuInstaller>(Location.Menu);
            
        Log.Info($"{pluginMetadata.Name} {pluginMetadata.HVersion} initialized.");
    }
}