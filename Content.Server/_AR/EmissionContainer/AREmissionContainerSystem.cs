using Content.Shared._AR.EmissionContainer;
using Robust.Server.GameObjects;

namespace Content.Server._AR.EmissionContainer;

public sealed class AREmissionContainerSystem : ARSharedEmissionContainerSystem
{
    [Dependency] private readonly AppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AREmissionContainerComponent, ComponentInit>(OnInit);
    }

    private void OnInit(Entity<AREmissionContainerComponent> entity, ref ComponentInit args)
    {
        UpdateVisuals(entity);
    }

    private void UpdateVisuals(Entity<AREmissionContainerComponent> entity)
    {
        var active = entity.Comp.Solution.Total != 0;
        _appearance.SetData(entity, AREmissionContainerVisuals.Active, active);
    }
}
