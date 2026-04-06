namespace Godot.Bindings.Tests;

public class Vector2ITests
{
    [Fact]
    public void AxisMethods()
    {
        var vector = new Vector2I(2, 3);

        Assert.Equal(Vector2I.Axis.Y, vector.MaxAxisIndex());
        Assert.Equal(Vector2I.Axis.X, vector.MinAxisIndex());
        Assert.Equal(2, vector[(int)vector.MinAxisIndex()]);

        vector[(int)Vector2I.Axis.Y] = 5;
        Assert.Equal(5, vector[(int)Vector2I.Axis.Y]);
    }

    [Fact]
    public void ClampMethod()
    {
        var vector = new Vector2I(10, 10);

        Assert.Equal(new Vector2I(0, 10), new Vector2I(-5, 15).Clamp(Vector2I.Zero, vector));
        Assert.Equal(new Vector2I(5, 15), vector.Clamp(new Vector2I(0, 15), new Vector2I(5, 20)));
    }

    [Fact]
    public void LengthMethods()
    {
        var vector1 = new Vector2I(10, 10);
        var vector2 = new Vector2I(20, 30);

        Assert.Equal(200, vector1.LengthSquared());
        Assert.Equal(10f * Mathf.Sqrt2, vector1.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(1300, vector2.LengthSquared());
        Assert.Equal(36.05551275463989293119f, vector2.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(500, vector1.DistanceSquaredTo(vector2));
        Assert.Equal(22.36067977499789696409f, vector1.DistanceTo(vector2), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var vector1 = new Vector2I(5, 9);
        var vector2 = new Vector2I(2, 3);

        Assert.Equal(new Vector2I(7, 12), vector1 + vector2);
        Assert.Equal(new Vector2I(3, 6), vector1 - vector2);
        Assert.Equal(new Vector2I(10, 27), vector1 * vector2);
        Assert.Equal(new Vector2I(2, 3), vector1 / vector2);

        Assert.Equal(new Vector2I(10, 18), vector1 * 2);
        Assert.Equal(new Vector2I(2, 4), vector1 / 2);

        Assert.Equal(new Vector2(5, 9), (Vector2)vector1);
        Assert.Equal(new Vector2(2, 3), (Vector2)vector2);
        Assert.Equal(new Vector2I(1, 2), (Vector2I)new Vector2(1.1f, 2.9f));
    }

    [Fact]
    public void OtherMethods()
    {
        var vector = new Vector2I(1, 3);

        Assert.Equal(1.0f / 3.0f, vector.Aspect(), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2I(1, 2), vector.Min(new Vector2I(3, 2)));
        Assert.Equal(new Vector2I(5, 3), vector.Max(new Vector2I(5, 2)));

        Assert.Equal(new Vector2I(0, 4), vector.Snapped(new Vector2I(4, 2)));
    }

    [Fact]
    public void AbsAndSignMethods()
    {
        var vector1 = new Vector2I(1, 3);
        var vector2 = new Vector2I(1, -3);

        Assert.Equal(vector1, vector1.Abs());
        Assert.Equal(vector1, vector2.Abs());

        Assert.Equal(new Vector2I(1, 1), vector1.Sign());
        Assert.Equal(new Vector2I(1, -1), vector2.Sign());
    }
}
