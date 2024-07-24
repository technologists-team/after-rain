using Content.Shared._AR.Currency;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._AR.Store.Prototypes;

[Prototype("ARStoreItem")]
public sealed class ARStoreItemPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public LocId Name = "ar-store-item-ui-default-name";

    [DataField]
    public LocId Description = "ar-store-item-ui-default-description";

    [DataField]
    public SpriteSpecifier? Icon;

    [DataField]
    public EntProtoId? ProductId;

    [DataField]
    public int ProductCount = 1;
}

[Serializable, NetSerializable, DataDefinition]
public partial struct ARStoreItemEntry : IEquatable<ARStoreItemEntry>
{
    [DataField]
    public ProtoId<ARStoreItemPrototype> Id;

    // TODO: Add ability to sell for different currencies
    [DataField]
    public ProtoId<ARCurrencyPrototype> Currency;

    [DataField]
    public int Cost = 0;

    public bool Equals(ARStoreItemEntry other)
    {
        return Id == other.Id &&
               Currency == other.Currency &&
               Cost == other.Cost;
    }

    public override bool Equals(object? obj)
    {
        return obj is ARStoreItemEntry other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Currency, Cost);
    }

    public static bool operator ==(ARStoreItemEntry entryA, ARStoreItemEntry entryB)
    {
        return entryA.Equals(entryB);
    }

    public static bool operator !=(ARStoreItemEntry entryA, ARStoreItemEntry entryB)
    {
        return !entryA.Equals(entryB);
    }
}
