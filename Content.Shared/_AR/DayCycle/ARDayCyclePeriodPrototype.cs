using Robust.Shared.Prototypes;

namespace Content.Shared._AR.DayCycle;

[Prototype("ARDayCyclePeriod")]
public sealed class ARDayCyclePeriodPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;
}
