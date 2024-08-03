using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared._AR.Floating;

[RegisterComponent, NetworkedComponent]
public sealed partial class ARFloatingComponent : Component
{
    [DataField]
    public float AnimationTime = 2f;

    [DataField]
    public string AnimationKey = "floating";

    [DataField]
    public Vector2 Offset = new(0, 0.35f);
}
