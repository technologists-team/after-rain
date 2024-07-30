using System.Collections;
using Content.Shared.FixedPoint;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Emission;

[DataDefinition, Serializable, NetSerializable]
public sealed partial class AREmissionSolution : IEnumerable<AREmissionQuantity>, ISerializationHooks
{
    [DataField]
    public HashSet<AREmissionQuantity> Contents = new();

    [Access(typeof(AREmissionSolution)), DataField]
    public FixedPoint2 Total { get; set; } = FixedPoint2.Zero;

    [DataField]
    public FixedPoint2 MaxVolume { get; set; } = FixedPoint2.Zero;

    public IEnumerator<AREmissionQuantity> GetEnumerator()
    {
        return Contents.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    void ISerializationHooks.AfterDeserialization()
    {
        Total = FixedPoint2.Zero;
        foreach (var reagent in Contents)
        {
            Total += reagent.Quantity;
        }

        if (MaxVolume == FixedPoint2.Zero)
            MaxVolume = Total;
    }
}
