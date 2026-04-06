namespace Godot.Bindings.Tests;

public class Vector3ITests
{
    [Fact]
    public void AxisMethods()
    {
        var vector = new Vector3I(1, 2, 3);

        Assert.Equal(Vector3I.Axis.Z, vector.MaxAxisIndex());
        Assert.Equal(Vector3I.Axis.X, vector.MinAxisIndex());
        Assert.Equal(3, vector[(int)vector.MaxAxisIndex()]);
        Assert.Equal(1, vector[(int)vector.MinAxisIndex()]);

        vector[(int)Vector3I.Axis.Y] = 5;
        Assert.Equal(5, vector[(int)Vector3I.Axis.Y]);
    }

    [Fact]
    public void ClampMethod()
    {
        var vector = new Vector3I(10, 10, 10);

        Assert.Equal(new Vector3I(0, 5, 10), new Vector3I(-5, 5, 15).Clamp(Vector3I.Zero, vector));
        Assert.Equal(new Vector3I(5, 10, 15), vector.Clamp(new Vector3I(0, 10, 15), new Vector3I(5, 10, 20)));
    }

    [Fact]
    public void LengthMethods()
    {
        var vector1 = new Vector3I(10, 10, 10);
        var vector2 = new Vector3I(20, 30, 40);

        Assert.Equal(300, vector1.LengthSquared());
        Assert.Equal(10 * MathfExtensions.Sqrt3, vector1.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(2900, vector2.LengthSquared());
        Assert.Equal(53.8516480713450403125f, vector2.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(1400, vector1.DistanceSquaredTo(vector2));
        Assert.Equal(37.41657386773941385584f, vector1.DistanceTo(vector2), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var vector1 = new Vector3I(4, 5, 9);
        var vector2 = new Vector3I(1, 2, 3);

        Assert.Equal(new Vector3I(5, 7, 12), vector1 + vector2);
        Assert.Equal(new Vector3I(3, 3, 6), vector1 - vector2);
        Assert.Equal(new Vector3I(4, 10, 27), vector1 * vector2);
        Assert.Equal(new Vector3I(4, 2, 3), vector1 / vector2);

        Assert.Equal(new Vector3I(8, 10, 18), vector1 * 2);
        Assert.Equal(new Vector3I(2, 2, 4), vector1 / 2);

        Assert.Equal(new Vector3(4, 5, 9), (Vector3)vector1);
        Assert.Equal(new Vector3(1, 2, 3), (Vector3)vector2);
        Assert.Equal(new Vector3I(1, 2, 3), (Vector3I)new Vector3(1.1f, 2.9f, 3.9f));
    }

    [Fact]
    public void OtherMethods()
    {
        var vector = new Vector3I(1, 3, -7);

        Assert.Equal(new Vector3I(1, 2, -7), vector.Min(new Vector3I(3, 2, 5)));
        Assert.Equal(new Vector3I(5, 3, 4), vector.Max(new Vector3I(5, 2, 4)));

        Assert.Equal(new Vector3I(0, 4, -5), vector.Snapped(new Vector3I(4, 2, 5)));
    }

    [Fact]
    public void AbsAndSignMethods()
    {
        var vector1 = new Vector3I(1, 3, 5);
        var vector2 = new Vector3I(1, -3, -5);

        Assert.Equal(vector1, vector1.Abs());
        Assert.Equal(vector1, vector2.Abs());

        Assert.Equal(new Vector3I(1, 1, 1), vector1.Sign());
        Assert.Equal(new Vector3I(1, -1, -1), vector2.Sign());
    }
}
