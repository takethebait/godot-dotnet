using System;

namespace Godot.Bindings.Tests;

public class Vector2Tests
{
    [Fact]
    public void AngleMethods()
    {
        var vectorX = new Vector2(1f, 0f);
        var vectorY = new Vector2(0f, 1f);

        Assert.Equal(Mathf.Tau / 4f, vectorX.AngleTo(vectorY), ApproxEqualityComparer.Instance);
        Assert.Equal(-Mathf.Tau / 4f, vectorY.AngleTo(vectorX), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.Tau * 3f / 8f, vectorX.AngleToPoint(vectorY), ApproxEqualityComparer.Instance);
        Assert.Equal(-Mathf.Tau / 8f, vectorY.AngleToPoint(vectorX), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void AxisMethods()
    {
        var vector = new Vector2(1.2f, 3.4f);

        Assert.Equal(Vector2.Axis.Y, vector.MaxAxisIndex());
        Assert.Equal(Vector2.Axis.X, vector.MinAxisIndex());
        Assert.Equal(1.2f, vector[(int)vector.MinAxisIndex()]);

        vector[(int)Vector2.Axis.Y] = 3.7f;
        Assert.Equal(3.7f, vector[(int)Vector2.Axis.Y]);
    }

    [Fact]
    public void InterpolationMethods()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(4, 5);

        Assert.Equal(new Vector2(2.5f, 3.5f), vector1.Lerp(vector2, 0.5f));
        Assert.Equal(new Vector2(2, 3), vector1.Lerp(vector2, 1.0f / 3.0f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(0.538953602313995361f, 0.84233558177947998f), vector1.Normalized().Slerp(vector2.Normalized(), 0.5f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.508990883827209473f, 0.860771894454956055f), vector1.Normalized().Slerp(vector2.Normalized(), 1.0f / 3.0f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(5f, 5f) * MathfExtensions.Sqrt12, new Vector2(5, 0).Slerp(new Vector2(0, 5), 0.5f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1.5f, 1.5f), new Vector2(1, 1).Slerp(new Vector2(2, 2), 0.5f), ApproxEqualityComparer.Instance);
        Assert.Equal(Vector2.Zero, Vector2.Zero.Slerp(Vector2.Zero, 0.5f));
        Assert.Equal(new Vector2(0.5f, 0.5f), Vector2.Zero.Slerp(new Vector2(1, 1), 0.5f));
        Assert.Equal(new Vector2(0.5f, 0.5f), new Vector2(1, 1).Slerp(Vector2.Zero, 0.5f));
        Assert.Equal(new Vector2(5.9076470794008017626f, 8.07918879020090480697f), new Vector2(4, 6).Slerp(new Vector2(8, 10), 0.5f), ApproxEqualityComparer.Instance);
        Assert.Equal(4.31959610746631919f, vector1.Slerp(vector2, 0.5f).Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(vector1.AngleTo(vector2), vector1.AngleTo(vector1.Slerp(vector2, 0.5f)) * 2, ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(2.375f, 3.5f), vector1.CubicInterpolate(vector2, Vector2.Zero, new Vector2(7, 7), 0.5f));
        Assert.Equal(new Vector2(1.851851940155029297f, 2.962963104248046875f), vector1.CubicInterpolate(vector2, Vector2.Zero, new Vector2(7, 7), 1.0f / 3.0f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(4, 0), new Vector2(1, 0).MoveToward(new Vector2(10, 0), 3));
    }

    [Fact]
    public void LengthMethods()
    {
        var vector1 = new Vector2(10, 10);
        var vector2 = new Vector2(20, 30);

        Assert.Equal(200, vector1.LengthSquared());
        Assert.Equal(10 * Mathf.Sqrt2, vector1.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(1300, vector2.LengthSquared());
        Assert.Equal(36.05551275463989293119f, vector2.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(500, vector1.DistanceSquaredTo(vector2));
        Assert.Equal(22.36067977499789696409f, vector1.DistanceTo(vector2), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void LimitingMethods()
    {
        var vector = new Vector2(10, 10);

        Assert.Equal(new Vector2(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12), vector.LimitLength(), ApproxEqualityComparer.Instance);
        Assert.Equal(5 * new Vector2(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12), vector.LimitLength(5), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(0, 10), new Vector2(-5, 15).Clamp(Vector2.Zero, vector), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(5, 15), vector.Clamp(new Vector2(0, 15), new Vector2(5, 20)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void NormalizationMethods()
    {
        Assert.True(new Vector2(1, 0).IsNormalized());
        Assert.False(new Vector2(1, 1).IsNormalized());
        Assert.Equal(new Vector2(1, 0), new Vector2(1, 0).Normalized());
        Assert.Equal(new Vector2(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12), new Vector2(1, 1).Normalized(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.509802390301732898898f, -0.860291533634174266891f), new Vector2(3.2f, -5.4f).Normalized(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var decimal1 = new Vector2(2.3f, 4.9f);
        var decimal2 = new Vector2(1.2f, 3.4f);
        var power1 = new Vector2(0.75f, 1.5f);
        var power2 = new Vector2(0.5f, 0.125f);
        var int1 = new Vector2(4, 5);
        var int2 = new Vector2(1, 2);

        Assert.Equal(new Vector2(3.5f, 8.3f), decimal1 + decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1.25f, 1.625f), power1 + power2);
        Assert.Equal(new Vector2(5, 7), int1 + int2);

        Assert.Equal(new Vector2(1.1f, 1.5f), decimal1 - decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.25f, 1.375f), power1 - power2);
        Assert.Equal(new Vector2(3, 3), int1 - int2);

        Assert.Equal(new Vector2(2.76f, 16.66f), decimal1 * decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.375f, 0.1875f), power1 * power2);
        Assert.Equal(new Vector2(4, 10), int1 * int2);

        Assert.Equal(new Vector2(1.91666666666666666f, 1.44117647058823529f), decimal1 / decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1.5f, 12.0f), power1 / power2);
        Assert.Equal(new Vector2(4f, 2.5f), int1 / int2);

        Assert.Equal(new Vector2(4.6f, 9.8f), decimal1 * 2f, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1.5f, 3f), power1 * 2f);
        Assert.Equal(new Vector2(8, 10), int1 * 2f);

        Assert.Equal(new Vector2(1.15f, 2.45f), decimal1 / 2f, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.375f, 0.75f), power1 / 2f);
        Assert.Equal(new Vector2(2f, 2.5f), int1 / 2f);

        Assert.Equal(new Vector2I(2, 4), (Vector2I)decimal1);
        Assert.Equal(new Vector2I(1, 3), (Vector2I)decimal2);
        Assert.Equal(new Vector2(1, 2), new Vector2I(1, 2));
    }

    [Fact]
    public void OtherMethods()
    {
        var vector = new Vector2(1.2f, 3.4f);

        Assert.Equal(1.2f / 3.4f, vector.Aspect(), ApproxEqualityComparer.Instance);

        Assert.Equal(-vector.Normalized(), vector.DirectionTo(Vector2.Zero), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12), new Vector2(1, 1).DirectionTo(new Vector2(2, 2)), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(1.2f, 1.4f), vector.PosMod(2), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.8f, 0.6f), (-vector).PosMod(2), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.2f, 1.4f), vector.PosMod(new Vector2(1, 2)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(0.8f, 2.6f), (-vector).PosMod(new Vector2(2, 3)), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(1.2f, 3.4f), vector.Rotated(Mathf.Tau), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(-3.4f, 1.2f), vector.Rotated(Mathf.Tau / 4f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(-3.544486372867091398996f, -0.660769515458673623883f), vector.Rotated(Mathf.Tau / 3f), ApproxEqualityComparer.Instance);
        Assert.Equal(vector.Rotated(Mathf.Tau / -2f), vector.Rotated(Mathf.Tau / 2f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(1, 3), vector.Snapped(new Vector2(1, 1)));
        Assert.Equal(new Vector2(3, 6), new Vector2(3.4f, 5.6f).Snapped(new Vector2(1, 1)));
        Assert.Equal(new Vector2(1.25f, 3.5f), vector.Snapped(new Vector2(0.25f, 0.25f)));

        Assert.Equal(new Vector2(1.2f, 2.5f), vector.Min(new Vector2(3.0f, 2.5f)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(5.3f, 3.4f), vector.Max(new Vector2(5.3f, 2.0f)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PlaneMethods()
    {
        var vector = new Vector2(1.2f, 3.4f);
        var vectorY = new Vector2(0, 1);
        var vectorNormal = new Vector2(0.95879811270838721622267f, 0.2840883296913739899919f);

        Assert.Equal(new Vector2(1.2f, -3.4f), vector.Bounce(vectorY));
        Assert.Equal(new Vector2(-2.85851197982345523329f, 2.197477931904161412358f), vector.Bounce(vectorNormal), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(-1.2f, 3.4f), vector.Reflect(vectorY));
        Assert.Equal(new Vector2(2.85851197982345523329f, -2.197477931904161412358f), vector.Reflect(vectorNormal), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(0f, 3.4f), vector.Project(vectorY));
        Assert.Equal(new Vector2(2.0292559899117276166f, 0.60126103404791929382f), vector.Project(vectorNormal), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector2(1.2f, 0f), vector.Slide(vectorY));
        Assert.Equal(new Vector2(-0.8292559899117276166456f, 2.798738965952080706179f), vector.Slide(vectorNormal), ApproxEqualityComparer.Instance);

        var vectorNonNormal = new Vector2(5.4f, 1.6f);
        Assert.Throws<ArgumentException>(() => vector.Bounce(vectorNonNormal));
        Assert.Throws<ArgumentException>(() => vector.Reflect(vectorNonNormal));
        Assert.Throws<ArgumentException>(() => vector.Slide(vectorNonNormal));
    }

    [Fact]
    public void RoundingMethods()
    {
        var vector1 = new Vector2(1.2f, 5.6f);
        var vector2 = new Vector2(1.2f, -5.6f);

        Assert.Equal(vector1, vector1.Abs());
        Assert.Equal(vector1, vector2.Abs());

        Assert.Equal(new Vector2(2, 6), vector1.Ceil());
        Assert.Equal(new Vector2(2, -5), vector2.Ceil());

        Assert.Equal(new Vector2(1, 5), vector1.Floor());
        Assert.Equal(new Vector2(1, -6), vector2.Floor());

        Assert.Equal(new Vector2(1, 6), vector1.Round());
        Assert.Equal(new Vector2(1, -6), vector2.Round());

        Assert.Equal(new Vector2I(1, 1), vector1.Sign());
        Assert.Equal(new Vector2I(1, -1), vector2.Sign());
    }

    [Fact]
    public void LinearAlgebraMethods()
    {
        var vectorX = new Vector2(1, 0);
        var vectorY = new Vector2(0, 1);
        var a = new Vector2(3.5f, 8.5f);
        var b = new Vector2(5.2f, 4.6f);

        Assert.Equal(1f, vectorX.Cross(vectorY));
        Assert.Equal(-1f, vectorY.Cross(vectorX));
        Assert.Equal(-28.1f, a.Cross(b), ApproxEqualityComparer.Instance);
        Assert.Equal(-28.1f, new Vector2(-a.X, a.Y).Cross(new Vector2(b.X, -b.Y)), ApproxEqualityComparer.Instance);

        Assert.Equal(0f, vectorX.Dot(vectorY));
        Assert.Equal(1f, vectorX.Dot(vectorX));
        Assert.Equal(100f, (vectorX * 10f).Dot(vectorX * 10f));
        Assert.Equal(57.3f, a.Dot(b), ApproxEqualityComparer.Instance);
        Assert.Equal(-57.3f, new Vector2(-a.X, a.Y).Dot(new Vector2(b.X, -b.Y)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        Assert.True(new Vector2(0, 1).IsFinite());

        float[] infinite = [float.NaN, float.PositiveInfinity, float.NegativeInfinity];

        foreach (float x in infinite)
        {
            Assert.False(new Vector2(x, 1).IsFinite());
            Assert.False(new Vector2(0, x).IsFinite());
        }

        foreach (float x in infinite)
        {
            foreach (float y in infinite)
            {
                Assert.False(new Vector2(x, y).IsFinite());
            }
        }
    }
}
