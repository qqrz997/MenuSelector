using MenuSelector.Menu;
using Zenject;

namespace MenuSelector.Installers;
internal class MenuInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<MenuSelectorViewController>().FromNewComponentAsViewController().AsSingle();
        Container.Bind<MenuSelectorFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesTo<MenuButtonManager>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuObjectManager>().AsSingle();
    }
}