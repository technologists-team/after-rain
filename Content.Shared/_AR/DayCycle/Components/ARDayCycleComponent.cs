using Content.Shared._CP14.DayCycle;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.DayCycle.Components;

/// <summary>
/// Stores all the necessary data for the day and night cycle system to work.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState, Access(typeof(ARSharedDayCycleSystem))]
public sealed partial class ARDayCycleComponent : Component
{
    [ViewVariables]
    public int NextTimeEntryIndex => CurrentTimeEntryIndex + 1 >= TimeEntries.Count ? 0 : CurrentTimeEntryIndex + 1;

    [ViewVariables]
    public DayCycleEntry CurrentTimeEntry => TimeEntries[CurrentTimeEntryIndex];

    [ViewVariables]
    public DayCycleEntry NextCurrentTimeEntry => TimeEntries[NextTimeEntryIndex];

    [ViewVariables]
    public Color StartColor => CurrentTimeEntry.Color;

    [ViewVariables]
    public Color EndColor => NextCurrentTimeEntry.Color;

    [ViewVariables]
    public ProtoId<ARDayCyclePeriodPrototype> CurrentPeriod => CurrentTimeEntry.Period;

    [DataField(required: true), ViewVariables, AutoNetworkedField]
    public List<DayCycleEntry> TimeEntries = new();

    [DataField, ViewVariables, AutoNetworkedField]
    public int CurrentTimeEntryIndex;

    [DataField, ViewVariables, AutoNetworkedField]
    public TimeSpan EntryStartTime;

    [DataField, ViewVariables, AutoNetworkedField]
    public TimeSpan EntryEndTime;
}

[DataDefinition, NetSerializable, Serializable]
public readonly partial record struct DayCycleEntry()
{
    /// <summary>
    /// The color of the world's lights at the beginning of this time of day
    /// </summary>
    [DataField]
    public Color Color { get; init; } = Color.White;

    /// <summary>
    /// Duration of color shift to the next time of day
    /// </summary>
    [DataField]
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(60);

    [DataField]
    public ProtoId<ARDayCyclePeriodPrototype> Period { get; init; } = "ARDay";
}

/// <summary>
/// Event raised on map entity, wen day cycle changed.
/// </summary>
[ByRefEvent]
public readonly record struct DayCycleChangedEvent(DayCycleEntry Entry);
