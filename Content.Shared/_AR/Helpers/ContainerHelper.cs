using Content.Shared.Hands.Components;
using Robust.Shared.Containers;
using Robust.Shared.Map.Components;
using Robust.Shared.Physics;

namespace Content.Shared._AR.Helpers;

public static class ContainerHelper
{
    public static bool InMapOrGrid(EntityUid entityUid, IEntityManager entityManager)
    {
        var parent = entityManager.GetComponent<TransformComponent>(entityUid).ParentUid;
        return entityManager.HasComponent<MapComponent>(parent) || entityManager.HasComponent<MapGridComponent>(parent);
    }

    public static IEnumerable<EntityUid> EnumerateElementsFromHands(Entity<HandsComponent?> entity, IEntityManager entityManager)
    {
        if (entity.Comp is null && !entityManager.TryGetComponent(entity, out entity.Comp))
           yield break;

        foreach (var (_, hand) in entity.Comp.Hands)
        {
            if (hand.HeldEntity is null)
                continue;

            yield return hand.HeldEntity.Value;
        }
    }

    public static IEnumerable<EntityUid> EnumerateElements(Entity<ContainerManagerComponent?> entity, IEntityManager entityManager)
    {
        if (entity.Comp is null && !entityManager.TryGetComponent(entity, out entity.Comp))
            yield break;

        foreach (var container in entity.Comp.Containers.Values)
        {
            foreach (var element in container.ContainedEntities)
            {
                if (!entityManager.TryGetComponent<ContainerManagerComponent>(element, out var manager))
                {
                    yield return element;
                    continue;
                }

                foreach (var entityUid in GetElements((element, manager), entityManager))
                {
                    yield return entityUid;
                }
            }
        }
    }

    public static HashSet<EntityUid> GetElements(Entity<ContainerManagerComponent?> entity, IEntityManager entityManager)
    {
        if (entity.Comp is null && !entityManager.TryGetComponent(entity, out entity.Comp))
            return new HashSet<EntityUid>();

        var result = new HashSet<EntityUid>();
        foreach (var container in entity.Comp.Containers.Values)
        {
            foreach (var element in container.ContainedEntities)
            {
                if (!entityManager.TryGetComponent<ContainerManagerComponent>(element, out var manager))
                {
                    result.Add(element);
                    continue;
                }

                foreach (var entityUid in GetElements((element, manager), entityManager))
                {
                    result.Add(entityUid);
                }
            }
        }

        return result;
    }
}
