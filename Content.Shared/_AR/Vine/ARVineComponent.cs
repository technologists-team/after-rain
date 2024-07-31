using System.Numerics;
using Content.Shared._AR.Math;
using Robust.Shared.GameStates;

namespace Content.Shared._AR.Vine;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ARVineComponent : Component
{
    [DataField]
    public int Segments = 30;

    [DataField]
    public float SegmentLength = 0.15f;

    [ViewVariables, AutoNetworkedField]
    public Vector2 Target;

    [ViewVariables, AutoNetworkedField]
    public ARFabric Fabric;
}
