using System.Collections.Frozen;
using Content.Shared._AR.Currency;
using Content.Shared._AR.Store.Prototypes;
using Robust.Shared.Prototypes;

namespace Content.Server._AR.Store;

[RegisterComponent]
public sealed partial class ARStoreComponent : Component
{
    [DataField]
    public string Name = string.Empty;

    [DataField]
    public ProtoId<ARStorePresetPrototype> PresetId = "ARDebug";

    [ViewVariables]
    public ARStorePresetPrototype? Preset;

    [ViewVariables]
    public FrozenDictionary<ARStoreCategoryPrototype, FrozenSet<ARStoreItemEntry>> Categories
        = FrozenDictionary<ARStoreCategoryPrototype, FrozenSet<ARStoreItemEntry>>.Empty;

    [ViewVariables]
    public FrozenDictionary<ARStoreItemEntry, ARStoreItemPrototype> Entries
        = FrozenDictionary<ARStoreItemEntry, ARStoreItemPrototype>.Empty;

    [ViewVariables]
    public FrozenSet<ProtoId<ARCurrencyPrototype>> Currencies
        = FrozenSet<ProtoId<ARCurrencyPrototype>>.Empty;

    [ViewVariables]
    public string LocName => Loc.GetString(Name);
}
