using Robust.Shared.Prototypes;

namespace Content.Shared._AR.Store.Prototypes;

[Prototype("ARStorePreset")]
public sealed class ARStorePresetPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public HashSet<ProtoId<ARStoreCategoryPrototype>> Categories = new();
}
