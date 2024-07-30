using Content.Shared.FixedPoint;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Emission;

[DataDefinition, Serializable, NetSerializable]
public partial struct AREmissionQuantity
{
    [DataField]
    public ProtoId<AREmissionPrototype> Id;

    [DataField]
    public FixedPoint2 Quantity;
}
