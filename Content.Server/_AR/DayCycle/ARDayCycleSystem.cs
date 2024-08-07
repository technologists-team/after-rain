using Content.Shared._AR.DayCycle;
using Content.Shared._AR.DayCycle.Components;
using Content.Shared._CP14.DayCycle;
using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;

namespace Content.Server._AR.DayCycle;

public sealed partial class ARDayCycleSystem : ARSharedDayCycleSystem
{
    public const int MinTimeEntryCount = 2;
    private const float MaxTimeDiff = 0.05f;

    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedMapSystem _maps = default!;
    [Dependency] private readonly ITileDefinitionManager _tileDefManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ARDayCycleComponent, MapInitEvent>(OnMapInitDayCycle);
    }


    private void OnMapInitDayCycle(Entity<ARDayCycleComponent> dayCycle, ref MapInitEvent args)
    {
        Init(dayCycle);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var dayCycleQuery = EntityQueryEnumerator<ARDayCycleComponent, MapLightComponent>();
        while (dayCycleQuery.MoveNext(out var uid, out var dayCycle, out var mapLight))
        {
            var entity = new Entity<ARDayCycleComponent, MapLightComponent>(uid, dayCycle, mapLight);

            if (dayCycle.TimeEntries.Count < MinTimeEntryCount)
                continue;

            SetAmbientColor((entity, entity), GetCurrentColor(entity, _timing.CurTime.TotalSeconds));

            if (_timing.CurTime <= dayCycle.EntryEndTime)
                continue;

            SetTimeEntry((uid, dayCycle),  dayCycle.NextTimeEntryIndex);
        }
    }

    public void Init(Entity<ARDayCycleComponent> dayCycle)
    {
        if (dayCycle.Comp.TimeEntries.Count < MinTimeEntryCount)
        {
            Log.Warning($"Attempting to init a daily cycle with the number of time entries less than {MinTimeEntryCount}");
            return;
        }

        dayCycle.Comp.CurrentTimeEntryIndex = 0;
        dayCycle.Comp.EntryStartTime = _timing.CurTime;
        dayCycle.Comp.EntryEndTime = _timing.CurTime + dayCycle.Comp.CurrentTimeEntry.Duration;

        Dirty(dayCycle);
    }

    public void AddTimeEntry(Entity<ARDayCycleComponent> dayCycle, DayCycleEntry entry)
    {
        dayCycle.Comp.TimeEntries.Add(entry);
        Dirty(dayCycle);
    }

    public void SetTimeEntry(Entity<ARDayCycleComponent> dayCycle, int nextEntry)
    {
        nextEntry = Math.Clamp(nextEntry, 0, dayCycle.Comp.TimeEntries.Count - 1);

        dayCycle.Comp.CurrentTimeEntryIndex = nextEntry;
        dayCycle.Comp.EntryStartTime = dayCycle.Comp.EntryEndTime;
        dayCycle.Comp.EntryEndTime += dayCycle.Comp.CurrentTimeEntry.Duration;

        var ev = new DayCycleChangedEvent(dayCycle.Comp.CurrentTimeEntry);
        RaiseLocalEvent(dayCycle, ref ev, true);

        Dirty(dayCycle);
    }

    private void SetAmbientColor(Entity<MapLightComponent> light, Color color)
    {
        if (color == light.Comp.AmbientLightColor)
            return;

        light.Comp.AmbientLightColor = color;
        Dirty(light);
    }

    private Color GetCurrentColor(Entity<ARDayCycleComponent> dayCycle, double totalSeconds)
    {
        var timeScale = GetTimeScale(dayCycle, totalSeconds);
        return Color.InterpolateBetween(dayCycle.Comp.StartColor, dayCycle.Comp.EndColor, timeScale);
    }

    private float GetTimeScale(Entity<ARDayCycleComponent> dayCycle, double totalSeconds)
    {
        return GetLerpValue(dayCycle.Comp.EntryStartTime.TotalSeconds, dayCycle.Comp.EntryEndTime.TotalSeconds, totalSeconds);
    }

    private static float GetLerpValue(double start, double end, double current)
    {
        if (Math.Abs(start - end) < MaxTimeDiff)
            return 0f;

        var distanceFromStart = current - start;
        var totalDistance = end - start;

        return MathHelper.Clamp01((float)(distanceFromStart / totalDistance));
    }
}
