using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Currency;

[RegisterComponent]
public sealed partial class ARCurrencyComponent : Component
{
    [DataField]
    public HashSet<ARCurrencyEntry> Entries = new();
}

[Serializable, NetSerializable, DataDefinition]
public partial struct ARCurrencyEntry
{
    [DataField]
    public ProtoId<ARCurrencyPrototype> Id;

    [DataField]
    public int Cost = 1;
}
