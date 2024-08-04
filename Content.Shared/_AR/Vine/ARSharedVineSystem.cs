using Content.Shared._AR.Math;
using Content.Shared._AR.Math.Procedural;
using Content.Shared.Mobs.Components;

namespace Content.Shared._AR.Vine;

public abstract class ARSharedVineSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ARVineComponent, MapInitEvent>(OnMapInit);
    }

    private void OnMapInit(Entity<ARVineComponent> entity, ref MapInitEvent args)
    {
        entity.Comp.Fabric = new IKFabric2D(
            _transform.GetWorldPosition(entity),
            entity.Comp.Segments,
            entity.Comp.SegmentLength,
            entity.Comp.Fixed
        );
    }

    protected void UpdateTarget(Entity<ARVineComponent> entity)
    {
        var coords = Transform(entity).Coordinates;
        var targets = _entityLookup.GetEntitiesInRange<MobStateComponent>(coords, entity.Comp.Fabric.MaxReach);

        var distance = float.MaxValue;
        foreach (var target in targets)
        {
            var delta = (_transform.GetWorldPosition(target) - _transform.GetWorldPosition(entity)).LengthSquared();
            if (delta > distance * distance)
                continue;

            entity.Comp.Target = _transform.GetWorldPosition(target);
            distance = delta;
        }
    }

    protected void Update(Entity<ARVineComponent> entity)
    {
        UpdateTarget(entity);

        entity.Comp.Fabric.SetPosition(_transform.GetWorldPosition(entity));
        entity.Comp.Fabric.SetTarget(entity.Comp.Target);
        entity.Comp.Fabric.SetFixed(entity.Comp.Fixed);
        entity.Comp.Fabric.Update();
    }
}
