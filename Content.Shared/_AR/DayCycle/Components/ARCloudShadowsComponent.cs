using System.Numerics;
using Robust.Shared.GameStates;
using Robust.Shared.Utility;

namespace Content.Shared._AR.DayCycle.Components;

/// <summary>
/// If added to the map, renders cloud shadows on the map
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ARCloudShadowsComponent : Component
{
    [DataField, AutoNetworkedField]
    public Vector2 CloudSpeed = new(0.5f, 0f);

    [DataField]
    public float MaxSpeed = 1.5f;

    [DataField, AutoNetworkedField]
    public float Alpha = 1f;

    [DataField]
    public float Scale = 2.5f;

    [DataField]
    public ResPath ParallaxPath = new("/Textures/_AR/Parallaxes/Shadows.png");
}
