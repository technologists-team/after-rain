using Robust.Shared.Prototypes;

namespace Content.Shared._AR.Store.Prototypes;

[Prototype("ARStoreCategory")]
public sealed class ARStoreCategoryPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public LocId Name = "ar-store-category-ui-default-title";

    [DataField]
    public HashSet<ARStoreItemEntry> Entries = new();
}
