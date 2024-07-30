using Robust.Shared.Prototypes;

namespace Content.Shared._AR.Emission;

[Prototype("AREmission")]
public sealed class AREmissionPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; } = string.Empty;

    [DataField, ViewVariables]
    public Color Color = Color.White;
}
