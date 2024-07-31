using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Math;

[Serializable, NetSerializable]
public sealed class ARSegment
{
    public float Angle;
    public float Length;

    public Vector2 Position;
    public Vector2 TargetPosition;

    public ARSegment? Parent;
    public ARSegment? Child;

    public ARSegment(ARSegment parent, float angle, float length)
    {
        Parent = parent;
        Position = parent.TargetPosition;
        Angle = angle;
        Length = length;

        UpdateTarget();
    }

    public ARSegment(Vector2 position, float angle, float length)
    {
        Position = position;
        Angle = angle;
        Length = length;

        UpdateTarget();
    }

    public ARSegment CreateParent(float angle, float length)
    {
        Parent = new ARSegment(Vector2.Zero, length, angle);
        Parent.Follow(Position);

        return Parent;
    }

    public void FollowParent()
    {
        if (Child is null)
            return;

        Follow(Child.Position);
    }

    public void FollowChild()
    {
        if (Child is null)
            return;

        Follow(Child.Position);
    }

    public void Follow(Vector2 target)
    {
        var direction = target - Position;

        Angle = (float)direction.ToAngle();

        // set magnitude
        direction = direction.Normalized() * Length;
        direction *= -1;

        Position = target + direction;
    }

    public void UpdateTarget()
    {
        var delta = new Vector2(MathF.Cos(Angle), MathF.Sin(Angle)) * Length;
        TargetPosition = Position + delta;
    }
}
