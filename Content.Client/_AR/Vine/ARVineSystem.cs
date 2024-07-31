using Content.Shared._AR.Vine;
using Robust.Client.Graphics;

namespace Content.Client._AR.Vine;

public sealed class ARVineSystem : ARSharedVineSystem
{
    [Dependency] private readonly IOverlayManager _overlay = default!;

    private ARVineOverlay _vineOverlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        _vineOverlay = new ARVineOverlay();
        _overlay.AddOverlay(_vineOverlay);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ARVineComponent>();
        while (query.MoveNext(out var uid, out var vine))
        {
            Update((uid, vine));
        }
    }
}
