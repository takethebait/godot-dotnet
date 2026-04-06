namespace Godot.Bindings.Tests;

public class Vector4ITests
{
    [Fact]
    public void AxisMethods()
    {
        var vector = new Vector4I(1, 2, 3, 4);
        Assert.Equal(Vector4I.Axis.W, vector.MaxAxisIndex());
        Assert.Equal(Vector4I.Axis.X, vector.MinAxisIndex());
        Assert.Equal(4, vector[(int)vector.MaxAxisIndex()]);
        Assert.Equal(1, vector[(int)vector.MinAxisIndex()]);

        vector[(int)Vector4I.Axis.Y] = 5;
        Assert.Equal(5, vector[(int)Vector4I.Axis.Y]);
    }

    [Fact]
    public void ClampMethod()
    {
        var vector = new Vector4I(10, 10, 10, 10);
        Assert.Equal(new Vector4I(0, 5, 10, 10), new Vector4I(-5, 5, 15, int.MaxValue).Clamp(Vector4I.Zero, vector));
        Assert.Equal(new Vector4I(5, 10, 15, -5), vector.Clamp(new Vector4I(0, 10, 15, -10), new Vector4I(5, 10, 20, -5)));
    }

    [Fact]
    public void LengthMethods()
    {
        var vector1 = new Vector4I(10, 10, 10, 10);
        var vector2 = new Vector4I(20, 30, 40, 50);

        Assert.Equal(400, vector1.LengthSquared());
        Assert.Equal(20f, vector1.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(5400, vector2.LengthSquared());
        Assert.Equal(73.4846922835f, vector2.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(3000, vector1.DistanceSquaredTo(vector2));
        Assert.Equal(54.772255750517f, vector1.DistanceTo(vector2), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var vector1 = new Vector4I(4, 5, 9, 2);
        var vector2 = new Vector4I(1, 2, 3, 4);

        Assert.Equal(new Vector4I(-4, -5, -9, -2), -vector1);
        Assert.Equal(new Vector4I(5, 7, 12, 6), vector1 + vector2);
        Assert.Equal(new Vector4I(3, 3, 6, -2), vector1 - vector2);
        Assert.Equal(new Vector4I(4, 10, 27, 8), vector1 * vector2);
        Assert.Equal(new Vector4I(4, 2, 3, 0), vector1 / vector2);

        Assert.Equal(new Vector4I(8, 10, 18, 4), vector1 * 2);
        Assert.Equal(new Vector4I(2, 2, 4, 1), vector1 / 2);

        Assert.Equal(new Vector4(4, 5, 9, 2), (Vector4)vector1);
        Assert.Equal(new Vector4(1, 2, 3, 4), (Vector4)vector2);
        Assert.Equal(new Vector4I(1, 2, 3, 100), (Vector4I)new Vector4(1.1f, 2.9f, 3.9f, 100.5f));
    }

    [Fact]
    public void OtherMethods()
    {
        var vector = new Vector4I(1, 3, -7, 13);
        Assert.Equal(new Vector4I(1, 2, -7, 8), vector.Min(new Vector4I(3, 2, 5, 8)));
        Assert.Equal(new Vector4I(5, 3, 4, 13), vector.Max(new Vector4I(5, 2, 4, 8)));
        Assert.Equal(new Vector4I(0, 4, -5, 16), vector.Snapped(new Vector4I(4, 2, 5, 8)));
    }

    [Fact]
    public void AbsAndSignMethods()
    {
        var vector1 = new Vector4I(1, 3, 5, 7);
        var vector2 = new Vector4I(1, -3, -5, 7);
        Assert.Equal(vector1, vector1.Abs());
        Assert.Equal(vector1, vector2.Abs());

        Assert.Equal(new Vector4I(1, 1, 1, 1), vector1.Sign());
        Assert.Equal(new Vector4I(1, -1, -1, 1), vector2.Sign());
    }
}
