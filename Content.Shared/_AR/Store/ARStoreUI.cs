using Content.Shared._AR.Currency;
using Content.Shared._AR.Store.Prototypes;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Store;

[Serializable, NetSerializable]
public enum ARStoreUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class ARStoreUiUpdateState : BoundUserInterfaceState
{
    public readonly Dictionary<ProtoId<ARCurrencyPrototype>, int> Currencies;
    public readonly ProtoId<ARStorePresetPrototype> PresetId;

    public ARStoreUiUpdateState(Dictionary<ProtoId<ARCurrencyPrototype>, int> currencies, ProtoId<ARStorePresetPrototype> presetId)
    {
        Currencies = currencies;
        PresetId = presetId;
    }
}

[Serializable, NetSerializable]
public sealed class ARStoreUiUpdateBalanceState : BoundUserInterfaceState
{
    public readonly Dictionary<ProtoId<ARCurrencyPrototype>, int> Currencies;

    public ARStoreUiUpdateBalanceState(Dictionary<ProtoId<ARCurrencyPrototype>, int> currencies)
    {
        Currencies = currencies;
    }
}

[Serializable, NetSerializable]
public sealed class ARStoreUiBuyItemMessage : BoundUserInterfaceMessage
{
    public readonly ARStoreItemEntry Entry;

    public ARStoreUiBuyItemMessage(ARStoreItemEntry entry)
    {
        Entry = entry;
    }
}
