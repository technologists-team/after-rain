using Content.Shared._AR.Emission;
using Robust.Shared.GameStates;

namespace Content.Shared._AR.EmissionContainer;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class AREmissionContainerComponent : Component
{
    [DataField, AutoNetworkedField]
    public AREmissionSolution Solution = new();
}
