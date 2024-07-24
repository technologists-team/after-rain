using Robust.Shared.Prototypes;

namespace Content.Shared._AR.Currency;

[Prototype("ARCurrency")]
public sealed class ARCurrencyPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public LocId Name;

    [DataField]
    public EntProtoId? EntityId;

    public string LocName => Loc.GetString(Name, ("amount", 1));
}
