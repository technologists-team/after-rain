using Content.Server.Administration;
using Content.Shared._AR.DayCycle;
using Content.Shared._AR.DayCycle.Components;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.Prototypes;

namespace Content.Server._AR.DayCycle.Commands;

[AdminCommand(AdminFlags.VarEdit)]
public sealed class ARAddTimeEntryCommand : LocalizedCommands
{
    private const string Name = "ar-addtimeentry";
    private const int ArgumentCount = 4;

    public override string Command => Name;
    public override string Description => "Allows you to add a new time entry to the map list";
    public override string Help => $"{Name} <mapUid> <color> <duration> <periodId>";

    public override void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (args.Length != ArgumentCount)
        {
            shell.WriteError($"{Loc.GetString("shell-wrong-arguments-number")}\n{Help}");
            return;
        }

        if (!NetEntity.TryParse(args[0], out var netEntity))
        {
            shell.WriteError(Loc.GetString("shell-entity-uid-must-be-number"));
            return;
        }

        var entityManager = IoCManager.Resolve<IEntityManager>();
        var prototypeManager = IoCManager.Resolve<IPrototypeManager>();

        var dayCycleSystem = entityManager.System<ARDayCycleSystem>();
        var entity = entityManager.GetEntity(netEntity);

        if (!entityManager.TryGetComponent<ARDayCycleComponent>(entity, out var dayCycle))
        {
            shell.WriteError(Loc.GetString("shell-entity-with-uid-lacks-component", ("uid", entity), ("componentName", nameof(ARDayCycleComponent))));
            return;
        }

        if (!Color.TryParse(args[1], out var color))
        {
            shell.WriteError(Loc.GetString("parse-color-fail", ("args", args[1])));
            return;
        }

        if (!float.TryParse(args[2], out var duration))
        {
            shell.WriteError(Loc.GetString("parse-float-fail", ("args", args[2])));
            return;
        }

        if (!prototypeManager.TryIndex<ARDayCyclePeriodPrototype>(args[3], out var prototype))
        {
            shell.WriteError(Loc.GetString("parse-prototype-fail", ("args", args[3])));
            return;
        }

        var entry = new DayCycleEntry
        {
            Color = color,
            Duration = TimeSpan.FromSeconds(duration),
            Period = prototype.ID,
        };

        dayCycleSystem.AddTimeEntry((entity, dayCycle), entry);
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromOptions(CompletionHelper.Components<ARDayCycleComponent>(args[0])),
            4 => CompletionResult.FromOptions(CompletionHelper.PrototypeIDs<ARDayCyclePeriodPrototype>()),
            _ => CompletionResult.Empty,
        };
    }
}
