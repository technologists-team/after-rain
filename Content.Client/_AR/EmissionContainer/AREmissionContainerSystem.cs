using Content.Shared._AR.EmissionContainer;
using Content.Shared.FixedPoint;
using Robust.Client.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Client._AR.EmissionContainer;

public sealed class AREmissionContainerSystem : ARSharedEmissionContainerSystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<AREmissionContainerComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(Entity<AREmissionContainerComponent> entity, ref AppearanceChangeEvent args)
    {
        var color = Color.White;
        var total = FixedPoint2.Zero;

        foreach (var content in entity.Comp.Solution)
        {
            var emission = _prototype.Index(content.Id);

            total += content.Quantity;

            var interpolateValue = content.Quantity.Float() / total.Float();
            color = Color.InterpolateBetween(color, emission.Color, interpolateValue);
        }

        args.Sprite?.LayerSetColor(AREmissionContainerVisuals.LayerContent, color);
    }
}
