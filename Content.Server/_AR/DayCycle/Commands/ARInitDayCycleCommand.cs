using Content.Server.Administration;
using Content.Shared._AR.DayCycle.Components;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server._AR.DayCycle.Commands;

[AdminCommand(AdminFlags.VarEdit)]
public sealed class ARInitDayCycleCommand : LocalizedCommands
{
    private const string Name = "ar-initdaycycle";
    private const int ArgumentCount = 1;

    public override string Command => Name;
    public override string Description =>
        "Re-initializes the day and night system, but reset the current time entry stage";
    public override string Help => $"{Name} <mapUid>";

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

        var entityManager = IoCManager.Resolve<EntityManager>();
        var dayCycleSystem = entityManager.System<ARDayCycleSystem>();
        var entity = entityManager.GetEntity(netEntity);

        if (!entityManager.TryGetComponent<ARDayCycleComponent>(entity, out var dayCycle))
        {
            shell.WriteError(Loc.GetString("shell-entity-with-uid-lacks-component", ("uid", entity), ("componentName", nameof(ARDayCycleComponent))));
            return;
        }

        if (dayCycle.TimeEntries.Count < ARDayCycleSystem.MinTimeEntryCount)
        {
            shell.WriteError($"Attempting to init a daily cycle with the number of time entries less than {ARDayCycleSystem.MinTimeEntryCount}");
            return;
        }

        dayCycleSystem.Init((entity, dayCycle));
    }

    public override CompletionResult GetCompletion(IConsoleShell shell, string[] args)
    {
        return args.Length switch
        {
            1 => CompletionResult.FromOptions(CompletionHelper.Components<ARDayCycleComponent>(args[0])),
            _ => CompletionResult.Empty,
        };
    }
}
