using System.Numerics;
using Content.Shared._AR.Vine;
using Robust.Client.Graphics;
using Robust.Shared.Enums;

namespace Content.Client._AR.Vine;

public sealed class ARVineOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IEyeManager _eyeManager = default!;

    private readonly EntityLookupSystem _entityLookup;

    public override OverlaySpace Space => OverlaySpace.WorldSpaceEntities;

    public ARVineOverlay()
    {
        IoCManager.InjectDependencies(this);

        _entityLookup = _entityManager.System<EntityLookupSystem>();
    }

    protected override void Draw(in OverlayDrawArgs args)
    {
        var viewport = args.WorldAABB;
        var query = _entityManager.EntityQueryEnumerator<ARVineComponent>();

        var handle = args.WorldHandle;

        while (query.MoveNext(out var uid, out var vine))
        {
            var aabb = _entityLookup.GetWorldAABB(uid);

            // if not on screen, continue
            if (!aabb.Intersects(in viewport))
                continue;

            foreach (var segment in vine.Fabric.Segments)
            {
                handle.DrawLine(segment.Position, segment.TargetPosition, Color.Green);
            }
        }
    }
}
