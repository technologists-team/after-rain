using System.Numerics;
using Content.Shared._AR.Vine;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.Utility;
using Robust.Shared.Enums;

namespace Content.Client._AR.Vine;

public sealed class ARVineOverlay : Overlay
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IResourceCache _resourceCache = default!;

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
                var vertices = new DrawVertexUV2D[6];

                var box = segment.GetBox(0.25f);
                var uv = new Box2(0.0f, 1.0f, 1.0f, 0.0f);

                var topLeft = box[0];
                var topRight = box[1];
                var bottomRight = box[2];
                var bottomLeft = box[3];

                var vertex1 = new DrawVertexUV2D(topRight, uv.TopRight);
                var vertex2 = new DrawVertexUV2D(bottomRight, uv.BottomRight);
                var vertex3 = new DrawVertexUV2D(bottomLeft, uv.BottomLeft);
                var vertex4 = new DrawVertexUV2D(topLeft, uv.TopLeft);

                vertices[0] = vertex1;
                vertices[1] = vertex2;
                vertices[2] = vertex4;
                vertices[3] = vertex2;
                vertices[4] = vertex3;
                vertices[5] = vertex4;

                handle.DrawPrimitives(DrawPrimitiveTopology.TriangleList, vine.BodySprite.GetTexture(_resourceCache), vertices, Color.White);
            }
        }
    }
}
