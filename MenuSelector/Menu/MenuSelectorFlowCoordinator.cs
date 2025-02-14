using System;
using HMUI;
using Zenject;

namespace MenuSelector.Menu;

internal class MenuSelectorFlowCoordinator : FlowCoordinator
{
    [Inject] private readonly MenuSelectorViewController menuSelectorViewController = null!;
    
    public event Action? DidFinish;
    
    protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
    {
        if (firstActivation)
        {
            showBackButton = true;
            SetTitle("Menu Selector");
        }

        if (addedToHierarchy)
        {
            ProvideInitialViewControllers(menuSelectorViewController);
        }
    }

    protected override void BackButtonWasPressed(ViewController topViewController)
    {
        base.BackButtonWasPressed(topViewController);
        DidFinish?.Invoke();
    }
}