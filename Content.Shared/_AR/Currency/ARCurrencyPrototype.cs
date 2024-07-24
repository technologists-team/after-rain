using Robust.Shared.Prototypes;

namespace Content.Shared._AR.Currency;

[Prototype("ARCurrency")]
public sealed class ARCurrencyPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField]
    public string Name = string.Empty;

    [DataField]
    public EntProtoId? EntityId;

    [ViewVariables]
    public string LocName => Loc.TryGetString(Name,  out var value, ("amount", 1)) ? value : Name;
}
