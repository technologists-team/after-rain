using Content.Shared._AR.Store;
using Robust.Client.UserInterface;

namespace Content.Client._AR.Store;

public sealed class ARStoreBoundUserInterface : BoundUserInterface
{
    private ARStoreWindow? _window;

    public ARStoreBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = this.CreateWindow<ARStoreWindow>();

        _window.OnBuyItem += entry =>
        {
            SendMessage(new ARStoreUiBuyItemMessage(entry));
        };
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        switch (state)
        {
            case ARStoreUiUpdateState updateState:
                _window?.UpdateState(updateState);
                break;

            case ARStoreUiUpdateBalanceState updateBalanceState:
                _window?.UpdateState(updateBalanceState);
                break;
        }
    }
}
