namespace ArcadeGame.Models;

public readonly struct Vector2
{
    public double X { get; }
    public double Y { get; }

    public Vector2(double x, double y) { X = x; Y = y; }

    public static Vector2 Zero => new(0, 0);
    public double Length => Math.Sqrt(X * X + Y * Y);

    public Vector2 Normalized()
    {
        var length = Length;
        return length > 1e-9 ? new Vector2(X / length, Y / length) : Zero;
    }

    public static Vector2 operator +(Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    public static Vector2 operator -(Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    public static Vector2 operator *(Vector2 a, double s) => new(a.X * s, a.Y * s);

    public override string ToString() => $"({X:F2}; {Y:F2})";
}