using System.Numerics;
using Content.Shared._AR.Floating;
using Content.Shared._AR.Helpers;
using Robust.Client.Animations;
using Robust.Client.GameObjects;
using Robust.Shared.Animations;

namespace Content.Client._AR.Floating;

public sealed class ARFloatingSystem : EntitySystem
{
    [Dependency] private readonly AnimationPlayerSystem _animationPlayer = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ARFloatingComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<ARFloatingComponent, AnimationCompletedEvent>(OnAnimationComplete);
    }

    private void OnStartup(Entity<ARFloatingComponent> entity, ref ComponentStartup args)
    {
        FloatAnimation(entity, entity.Comp.Offset, entity.Comp.AnimationKey, entity.Comp.AnimationTime);
    }

    private void OnAnimationComplete(Entity<ARFloatingComponent> entity, ref AnimationCompletedEvent args)
    {
        if (args.Key != entity.Comp.AnimationKey)
            return;

        FloatAnimation(entity, entity.Comp.Offset, entity.Comp.AnimationKey, entity.Comp.AnimationTime);
    }

    private void FloatAnimation(EntityUid uid, Vector2 offset, string animationKey, float animationTime, bool stop = false)
    {
        if (stop)
        {
            _animationPlayer.Stop(uid, animationKey);
            return;
        }

        var animation = new Animation
        {
            // We multiply by the number of extra keyframes to make time for them
            Length = TimeSpan.FromSeconds(animationTime * 3),
            AnimationTracks =
            {
                new AnimationTrackComponentProperty
                {
                    ComponentType = typeof(SpriteComponent),
                    Property = nameof(SpriteComponent.Offset),
                    InterpolationMode = AnimationInterpolationMode.Cubic,
                    KeyFrames =
                    {
                        new AnimationTrackProperty.KeyFrame(Vector2.Zero, 0f * animationTime),
                        new AnimationTrackProperty.KeyFrame(offset, 0.25f * animationTime),
                        new AnimationTrackProperty.KeyFrame(-offset, 0.75f * animationTime),
                        new AnimationTrackProperty.KeyFrame(Vector2.Zero, 1f * animationTime),
                    }
                }
            }
        };

        if (!_animationPlayer.HasRunningAnimation(uid, animationKey))
            _animationPlayer.Play(uid, animation, animationKey);
    }
}
