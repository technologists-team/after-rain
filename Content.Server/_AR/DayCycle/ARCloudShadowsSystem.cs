using Content.Shared._AR.DayCycle.Components;
using Robust.Shared.Random;

namespace Content.Server._AR.DayCycle;

public sealed class ARCloudShadowsSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ARCloudShadowsComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<ARCloudShadowsComponent> entity, ref MapInitEvent args)
    {
        entity.Comp.CloudSpeed = _random.NextVector2(-entity.Comp.MaxSpeed, entity.Comp.MaxSpeed);
    }
}
