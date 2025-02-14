using System;
using BeatSaberMarkupLanguage.MenuButtons;
using Zenject;

namespace MenuSelector.Menu;

internal class MenuButtonManager : IInitializable, IDisposable
{
    private readonly MenuButton menuButton;
    private readonly MenuButtons menuButtons;
    private readonly MainFlowCoordinator mainFlowCoordinator;
    private readonly MenuSelectorFlowCoordinator menuSelectorFlowCoordinator;

    public MenuButtonManager(
        MenuButtons menuButtons,
        MainFlowCoordinator mainFlowCoordinator,
        MenuSelectorFlowCoordinator menuSelectorFlowCoordinator)
    {
        menuButton = new("Menu Selector", PresentFlowCoordinator);
        this.menuButtons = menuButtons;
        this.mainFlowCoordinator = mainFlowCoordinator;
        this.menuSelectorFlowCoordinator = menuSelectorFlowCoordinator;
    }
    
    public void Initialize()
    {
        menuSelectorFlowCoordinator.DidFinish += DismissFlowCoordinator;
        menuButtons.RegisterButton(menuButton);
    }

    public void Dispose()
    {
        menuSelectorFlowCoordinator.DidFinish -= DismissFlowCoordinator;   
    }

    private void PresentFlowCoordinator() => mainFlowCoordinator.PresentFlowCoordinator(menuSelectorFlowCoordinator);
    private void DismissFlowCoordinator() => mainFlowCoordinator.DismissFlowCoordinator(menuSelectorFlowCoordinator);
}