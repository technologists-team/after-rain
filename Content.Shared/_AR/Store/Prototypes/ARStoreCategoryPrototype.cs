using Robust.Shared.Prototypes;

namespace Content.Shared._AR.Store.Prototypes;

[Prototype("ARStoreCategory")]
public sealed class ARStoreCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public string Name = "name";

    [DataField]
    public HashSet<ARStoreItemEntry> Entries = new();

    [ViewVariables]
    public LocId LocName => Loc.TryGetString($"ar-store-category-{ID}-name", out var value) ? value : Name;
}
