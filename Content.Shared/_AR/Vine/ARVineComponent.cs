using System.Numerics;
using Content.Shared._AR.Math;
using Content.Shared._AR.Math.Procedural;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared._AR.Vine;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ARVineComponent : Component
{
    [DataField]
    public int Segments = 20;

    [DataField]
    public float SegmentLength = 1f;

    [ViewVariables, AutoNetworkedField]
    public bool Fixed = true;

    [ViewVariables, AutoNetworkedField]
    public Vector2 Target;

    [ViewVariables, AutoNetworkedField]
    public IKFabric2D Fabric;

    [ViewVariables, AutoNetworkedField]
    public SpriteSpecifier.Texture BodySprite = new(new ResPath("/Textures/_AR/Mobs/tentacle.rsi/red_body.png"));
}
