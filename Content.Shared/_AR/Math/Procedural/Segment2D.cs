using System.Numerics;
using Robust.Shared.Serialization;

namespace Content.Shared._AR.Math.Procedural;

[Serializable, NetSerializable]
public sealed class Segment2D
{
    public float Angle { get; private set; }
    public float Length { get; private set; }

    public Vector2 Position { get; private set; }
    public Vector2 TargetPosition { get; private set; }

    public Segment2D(Vector2 position, float angle, float length)
    {
        Position = position;
        Angle = angle;
        Length = length;

        Update();
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

    public void Update()
    {
        var delta = new Vector2(MathF.Cos(Angle), MathF.Sin(Angle)) * Length;
        TargetPosition = Position + delta;
    }

    public void SetPosition(Vector2 position)
    {
        Position = position;
    }

    public Vector2[] GetBox(float width)
    {
        var direction = TargetPosition - Position;
        var normal = new Vector2(direction.Y, -direction.X).Normalized();

        var point1 = Position + normal.Normalized() * width / 2f;
        var point2 = Position - normal.Normalized() * width / 2f;
        var point3 = TargetPosition + normal.Normalized() * width / 2f;
        var point4 = TargetPosition - normal.Normalized() * width / 2f;

        return new[] { point4, point3, point1, point2 };
    }
}
