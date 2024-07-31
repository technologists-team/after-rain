using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Math;

[Serializable, NetSerializable]
public sealed class ARFabric
{
    public Vector2 Position;
    public Vector2 Target;
    public float Speed = 1;

    private readonly List<ARSegment> _segments;

    public float MaxReach { get; private set; }
    public IReadOnlyList<ARSegment> Segments => _segments;

    public ARFabric(Vector2 position, int segments, float angle, float segmentLength)
    {
        Position = position;
        _segments = new List<ARSegment>
        {
            new(position, angle, segmentLength)
        };

        MaxReach += segmentLength;

        for (var i = 1; i < segments; i++)
        {
            AddSegment(angle, segmentLength);
        }
    }

    public bool CanReach(Vector2 target)
    {
        return (Position - target).LengthSquared() <= MaxReach * MaxReach;
    }

    public void AddSegment(float angle, float length)
    {
        var previous = _segments[^1];

        MaxReach += length;
        var segment = new ARSegment(Vector2.Zero, angle, length);

        previous.Parent = segment;
        _segments.Add(segment);

        segment.Follow(previous.Position);
    }

    public void Update()
    {
        for (var i = 0; i < _segments.Count; i++)
        {
            var segment = _segments[i];
            segment.UpdateTarget();

            if (i == 0) {
                segment.Follow(Target);
                continue;
            }

            var previous = _segments[i - 1];
            segment.Follow(previous.Position);
        }

        var last = _segments.Count - 1;
        var lastSegment = _segments[last];

        lastSegment.Position = Position;
        lastSegment.UpdateTarget();

        for (var i = last - 1; i >= 0; i--)
        {
            var segment = _segments[i];
            var nextSegment = _segments[i + 1];

            segment.Position = nextSegment.TargetPosition;

            segment.UpdateTarget();
        }
    }
}
