using System.Collections.Frozen;
using Content.Server.Hands.Systems;
using Content.Server.Stack;
using Content.Shared._AR.Currency;
using Content.Shared._AR.Helpers;
using Content.Shared._AR.Store;
using Content.Shared._AR.Store.Prototypes;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Prototypes;

namespace Content.Server._AR.Store;

public sealed class ARStoreSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;

    [Dependency] private readonly HandsSystem _hands = default!;
    [Dependency] private readonly StackSystem _stack = default!;
    [Dependency] private readonly UserInterfaceSystem _userInterface = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ARStoreComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ARStoreComponent, BeforeActivatableUIOpenEvent>(OnStoreBeforeOpen);

        SubscribeLocalEvent<ARStoreComponent, ARStoreUiBuyItemMessage>(OnStoreBuyItem);

    }

    private void OnStartup(Entity<ARStoreComponent> entity, ref ComponentStartup args)
    {
        Refresh(entity);
    }

    private void OnStoreBeforeOpen(Entity<ARStoreComponent> entity, ref BeforeActivatableUIOpenEvent args)
    {
        UpdateUI(entity, args.User);
    }

    private void OnStoreBuyItem(Entity<ARStoreComponent> entity, ref ARStoreUiBuyItemMessage args)
    {
        if (!ContainsEntry(entity, args.Entry))
            return;

        if (!TryBuyItem(entity, args.Actor, args.Entry))
            return;

        UpdateBalanceUI(entity, args.Actor);
    }

    private bool TryBuyItem(Entity<ARStoreComponent> entity, EntityUid entityUid, ARStoreItemEntry entry)
    {
        if (!entity.Comp.Entries.TryGetValue(entry, out var itemPrototype))
            return false;

        if (!TryRemoveBalance(entityUid, entry))
            return false;

        for (var i = 0; i < itemPrototype.ProductCount; i++)
        {
            var product = Spawn(itemPrototype.ProductId, Transform(entityUid).Coordinates);
            _hands.PickupOrDrop(entityUid, product);
        }

        return true;
    }

    private bool TryRemoveBalance(EntityUid entityUid, ARStoreItemEntry entry)
    {
        foreach (var handEntityUid in ContainerHelper.EnumerateElementsFromHands(entityUid, EntityManager))
        {
            if (!TryComp<ARCurrencyComponent>(handEntityUid, out var currency))
                continue;

            foreach (var currencyEntry in currency.Entries)
            {
                if (currencyEntry.Id != entry.Currency)
                    continue;

                var count = _stack.GetCount(handEntityUid);
                if (count < entry.Cost)
                    continue;

                _stack.SetCount(handEntityUid, count - entry.Cost);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Validate the data from the client,
    /// so that it is impossible to buy any prototype through our store.
    /// </summary>
    private bool ContainsEntry(Entity<ARStoreComponent> entity, ARStoreItemEntry entry)
    {
        foreach (var (_, entries) in entity.Comp.Categories)
        {
            foreach (var otherEntry in entries)
            {
                if (entry == otherEntry)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Method to optimize prototype search, caches all value by Id in a component.
    /// </summary>
    private void Refresh(Entity<ARStoreComponent> entity)
    {
        var preset = _prototype.Index(entity.Comp.PresetId);
        entity.Comp.Preset = preset;

        var categories = new Dictionary<ARStoreCategoryPrototype, FrozenSet<ARStoreItemEntry>>();
        var entries = new Dictionary<ARStoreItemEntry, ARStoreItemPrototype>();
        var currencies = new HashSet<ProtoId<ARCurrencyPrototype>>();

        foreach (var categoryId in preset.Categories)
        {
            var category = _prototype.Index(categoryId);

            var categoryEntries = new HashSet<ARStoreItemEntry>();
            foreach (var itemEntry in category.Entries)
            {
                var item = _prototype.Index(itemEntry.Id);
                var currency = _prototype.Index(itemEntry.Currency);

                categoryEntries.Add(itemEntry);
                entries.Add(itemEntry, item);
                currencies.Add(currency);
            }

            categories.Add(category, categoryEntries.ToFrozenSet());
        }

        entity.Comp.Categories = categories.ToFrozenDictionary();
        entity.Comp.Entries = entries.ToFrozenDictionary();
        entity.Comp.Currencies = currencies.ToFrozenSet();
    }

    private Dictionary<ProtoId<ARCurrencyPrototype>, int> GetBalanceFromHands(Entity<ARStoreComponent> entity, EntityUid entityUid)
    {
        var result = new Dictionary<ProtoId<ARCurrencyPrototype>, int>();
        foreach (var handEntityUid in ContainerHelper.EnumerateElementsFromHands(entityUid, EntityManager))
        {
            if (!TryComp<ARCurrencyComponent>(handEntityUid, out var currency))
                continue;

            var count = _stack.GetCount(handEntityUid);
            foreach (var entry in currency.Entries)
            {
                if (!entity.Comp.Currencies.Contains(entry.Id))
                    continue;

                result.TryAdd(entry.Id, 0);
                result[entry.Id] += count * entry.Cost;
            }
        }

        foreach (var currency in entity.Comp.Currencies)
        {
            if (result.ContainsKey(currency))
                continue;

            result[currency] = 0;
        }

        return result;
    }

    private void UpdateUI(Entity<ARStoreComponent> entity, EntityUid user)
    {
        var currencies = GetBalanceFromHands(entity, user);
        var preset = entity.Comp.PresetId;
        _userInterface.SetUiState(entity.Owner, ARStoreUiKey.Key, new ARStoreUiUpdateState(currencies, preset, entity.Comp.Name));
    }

    private void UpdateBalanceUI(Entity<ARStoreComponent> entity, EntityUid user)
    {
        var currencies = GetBalanceFromHands(entity, user);
        _userInterface.SetUiState(entity.Owner, ARStoreUiKey.Key, new ARStoreUiUpdateBalanceState(currencies));
    }
}
