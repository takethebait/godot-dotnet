namespace Godot.Bindings.Tests;

public class Vector4Tests
{
    [Fact]
    public void AxisMethods()
    {
        var vector = new Vector4(1.2f, 3.4f, 5.6f, -0.9f);
        Assert.Equal(Vector4.Axis.Z, vector.MaxAxisIndex());
        Assert.Equal(Vector4.Axis.W, vector.MinAxisIndex());
        Assert.Equal(5.6f, vector[(int)vector.MaxAxisIndex()]);
        Assert.Equal(-0.9f, vector[(int)vector.MinAxisIndex()]);

        vector[(int)Vector4.Axis.Y] = 3.7f;
        Assert.Equal(3.7f, vector[(int)Vector4.Axis.Y]);
    }

    [Fact]
    public void InterpolationMethods()
    {
        var vector1 = new Vector4(1, 2, 3, 4);
        var vector2 = new Vector4(4, 5, 6, 7);
        Assert.Equal(new Vector4(2.5f, 3.5f, 4.5f, 5.5f), vector1.Lerp(vector2, 0.5f));
        Assert.Equal(new Vector4(2, 3, 4, 5), vector1.Lerp(vector2, 1.0f / 3.0f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(2.375f, 3.5f, 4.625f, 5.75f), vector1.CubicInterpolate(vector2, Vector4.Zero, new Vector4(7, 7, 7, 7), 0.5f));
        Assert.Equal(new Vector4(1.851851940155029297f, 2.962963104248046875f, 4.074074268341064453f, 5.185185185185f), vector1.CubicInterpolate(vector2, Vector4.Zero, new Vector4(7, 7, 7, 7), 1.0f / 3.0f), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void LengthMethods()
    {
        var vector1 = new Vector4(10, 10, 10, 10);
        var vector2 = new Vector4(20, 30, 40, 50);

        Assert.Equal(400, vector1.LengthSquared());
        Assert.Equal(20f, vector1.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(5400, vector2.LengthSquared());
        Assert.Equal(73.484692283495f, vector2.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(54.772255750517f, vector1.DistanceTo(vector2), ApproxEqualityComparer.Instance);
        Assert.Equal(3000f, vector1.DistanceSquaredTo(vector2), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void LimitingMethods()
    {
        var vector = new Vector4(10, 10, 10, 10);
        Assert.Equal(new Vector4(0, 5, 10, 0), new Vector4(-5, 5, 15, -15).Clamp(Vector4.Zero, vector));
        Assert.Equal(new Vector4(5, 10, 15, 18), vector.Clamp(new Vector4(0, 10, 15, 18), new Vector4(5, 10, 20, 25)));
    }

    [Fact]
    public void NormalizationMethods()
    {
        Assert.True(new Vector4(1, 0, 0, 0).IsNormalized());
        Assert.False(new Vector4(1, 1, 1, 1).IsNormalized());
        Assert.Equal(new Vector4(1, 0, 0, 0), new Vector4(1, 0, 0, 0).Normalized());
        Assert.Equal(new Vector4(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12, 0, 0), new Vector4(1, 1, 0, 0).Normalized(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), new Vector4(1, 1, 1, 1).Normalized(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var decimal1 = new Vector4(2.3f, 4.9f, 7.8f, 3.2f);
        var decimal2 = new Vector4(1.2f, 3.4f, 5.6f, 1.7f);
        var power1 = new Vector4(0.75f, 1.5f, 0.625f, 0.125f);
        var power2 = new Vector4(0.5f, 0.125f, 0.25f, 0.75f);
        var int1 = new Vector4(4, 5, 9, 2);
        var int2 = new Vector4(1, 2, 3, 1);

        Assert.Equal(new Vector4(-2.3f, -4.9f, -7.8f, -3.2f), -decimal1);

        Assert.Equal(new Vector4(3.5f, 8.3f, 13.4f, 4.9f), decimal1 + decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(1.25f, 1.625f, 0.875f, 0.875f), power1 + power2);
        Assert.Equal(new Vector4(5, 7, 12, 3), int1 + int2);

        Assert.Equal(new Vector4(1.1f, 1.5f, 2.2f, 1.5f), decimal1 - decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.25f, 1.375f, 0.375f, -0.625f), power1 - power2);
        Assert.Equal(new Vector4(3, 3, 6, 1), int1 - int2);

        Assert.Equal(new Vector4(2.76f, 16.66f, 43.68f, 5.44f), decimal1 * decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.375f, 0.1875f, 0.15625f, 0.09375f), power1 * power2);
        Assert.Equal(new Vector4(4, 10, 27, 2), int1 * int2);

        Assert.Equal(new Vector4(1.91666666666666666f, 1.44117647058823529f, 1.39285714285714286f, 1.88235294118f), decimal1 / decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(1.5f, 12.0f, 2.5f, 1.0f / 6.0f), power1 / power2);
        Assert.Equal(new Vector4(4, 2.5f, 3, 2), int1 / int2);

        Assert.Equal(new Vector4(4.6f, 9.8f, 15.6f, 6.4f), decimal1 * 2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(1.5f, 3, 1.25f, 0.25f), power1 * 2);
        Assert.Equal(new Vector4(8, 10, 18, 4), int1 * 2);

        Assert.Equal(new Vector4(1.15f, 2.45f, 3.9f, 1.6f), decimal1 / 2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.375f, 0.75f, 0.3125f, 0.0625f), power1 / 2);
        Assert.Equal(new Vector4(2, 2.5f, 4.5f, 1), int1 / 2);
    }

    [Fact]
    public void OtherMethods()
    {
        var vector = new Vector4(1.2f, 3.4f, 5.6f, 1.6f);
        Assert.Equal(-vector.Normalized(), vector.DirectionTo(Vector4.Zero), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), new Vector4(1, 1, 1, 1).DirectionTo(new Vector4(2, 2, 2, 2)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(1 / 1.2f, 1 / 3.4f, 1 / 5.6f, 1 / 1.6f), vector.Inverse(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(1.2f, 1.4f, 1.6f, 1.6f), vector.PosMod(2), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.8f, 0.6f, 0.4f, 0.4f), (-vector).PosMod(2), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.2f, 1.4f, 2.6f, 1.6f), vector.PosMod(new Vector4(1, 2, 3, 4)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0.8f, 2.6f, 2.4f, 3.4f), (-vector).PosMod(new Vector4(2, 3, 4, 5)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(1, 3, 6, 2), vector.Snapped(new Vector4(1, 1, 1, 1)));
        Assert.Equal(new Vector4(1.25f, 3.5f, 5.5f, 1.5f), vector.Snapped(new Vector4(0.25f, 0.25f, 0.25f, 0.25f)));
        Assert.Equal(new Vector4(1.2f, 2.5f, 2.0f, 1.6f), vector.Min(new Vector4(3.0f, 2.5f, 2.0f, 3.4f)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(5.3f, 3.4f, 5.6f, 4.2f), vector.Max(new Vector4(5.3f, 2.0f, 3.0f, 4.2f)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void RoundingMethods()
    {
        var vector1 = new Vector4(1.2f, 3.4f, 5.6f, 1.6f);
        var vector2 = new Vector4(1.2f, -3.4f, -5.6f, -1.6f);
        Assert.Equal(vector1, vector1.Abs());
        Assert.Equal(vector1, vector2.Abs());

        Assert.Equal(new Vector4(2, 4, 6, 2), vector1.Ceil());
        Assert.Equal(new Vector4(2, -3, -5, -1), vector2.Ceil());

        Assert.Equal(new Vector4(1, 3, 5, 1), vector1.Floor());
        Assert.Equal(new Vector4(1, -4, -6, -2), vector2.Floor());

        Assert.Equal(new Vector4(1, 3, 6, 2), vector1.Round());
        Assert.Equal(new Vector4(1, -3, -6, -2), vector2.Round());

        Assert.Equal(new Vector4I(1, 1, 1, 1), vector1.Sign());
        Assert.Equal(new Vector4I(1, -1, -1, -1), vector2.Sign());
    }

    [Fact]
    public void LinearAlgebraMethods()
    {
        var vectorX = new Vector4(1, 0, 0, 0);
        var vectorY = new Vector4(0, 1, 0, 0);
        var vector1 = new Vector4(1.7f, 2.3f, 1, 9.1f);
        var vector2 = new Vector4(-8.2f, -16, 3, 2.4f);

        Assert.Equal(0.0f, vectorX.Dot(vectorY));
        Assert.Equal(1.0f, vectorX.Dot(vectorX));
        Assert.Equal(100.0f, (vectorX * 10).Dot(vectorX * 10));
        Assert.Equal(-25.9f * 8, (vector1 * 2).Dot(vector2 * 4), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        float[] infinite = [float.NaN, float.PositiveInfinity, float.NegativeInfinity];

        Assert.True(new Vector4(0, 1, 2, 3).IsFinite());

        foreach (float x in infinite)
        {
            Assert.False(new Vector4(x, 1, 2, 3).IsFinite());
            Assert.False(new Vector4(0, x, 2, 3).IsFinite());
            Assert.False(new Vector4(0, 1, x, 3).IsFinite());
            Assert.False(new Vector4(0, 1, 2, x).IsFinite());
        }

        foreach (float x in infinite)
        {
            foreach (float y in infinite)
            {
                Assert.False(new Vector4(x, y, 2, 3).IsFinite());
                Assert.False(new Vector4(x, 1, y, 3).IsFinite());
                Assert.False(new Vector4(x, 1, 2, y).IsFinite());
                Assert.False(new Vector4(0, x, y, 3).IsFinite());
                Assert.False(new Vector4(0, x, 2, y).IsFinite());
                Assert.False(new Vector4(0, 1, x, y).IsFinite());
            }
        }

        foreach (float x in infinite)
        {
            foreach (float y in infinite)
            {
                foreach (float z in infinite)
                {
                    Assert.False(new Vector4(0, x, y, z).IsFinite());
                    Assert.False(new Vector4(x, 1, y, z).IsFinite());
                    Assert.False(new Vector4(x, y, 2, z).IsFinite());
                    Assert.False(new Vector4(x, y, z, 3).IsFinite());
                }
            }
        }

        foreach (float x in infinite)
        {
            foreach (float y in infinite)
            {
                foreach (float z in infinite)
                {
                    foreach (float w in infinite)
                    {
                        Assert.False(new Vector4(x, y, z, w).IsFinite());
                    }
                }
            }
        }
    }
}
