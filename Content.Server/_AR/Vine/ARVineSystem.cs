using Content.Shared._AR.Vine;

namespace Content.Server._AR.Vine;

public sealed class ARVineSystem : ARSharedVineSystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<ARVineComponent>();
        while (query.MoveNext(out var uid, out var vine))
        {
            Update((uid, vine));
            Dirty(uid, vine);
        }
    }
}
