using System;

namespace Godot.Bindings.Tests;

public class Vector3Tests
{
    [Fact]
    public void AngleMethods()
    {
        var vectorX = new Vector3(1, 0, 0);
        var vectorY = new Vector3(0, 1, 0);
        var vectorYZ = new Vector3(0, 1, 1);

        Assert.Equal(Mathf.Tau / 4f, vectorX.AngleTo(vectorY), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.Tau / 4f, vectorX.AngleTo(vectorYZ), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.Tau / 4f, vectorYZ.AngleTo(vectorX), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.Tau / 8f, vectorY.AngleTo(vectorYZ), ApproxEqualityComparer.Instance);

        Assert.Equal(Mathf.Tau / 4f, vectorX.SignedAngleTo(vectorY, vectorY), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.Tau / -4f, vectorX.SignedAngleTo(vectorYZ, vectorY), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.Tau / 4f, vectorYZ.SignedAngleTo(vectorX, vectorY), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void AxisMethods()
    {
        var vector = new Vector3(1.2f, 3.4f, 5.6f);

        Assert.Equal(Vector3.Axis.Z, vector.MaxAxisIndex());
        Assert.Equal(Vector3.Axis.X, vector.MinAxisIndex());
        Assert.Equal(5.6f, vector[(int)vector.MaxAxisIndex()]);
        Assert.Equal(1.2f, vector[(int)vector.MinAxisIndex()]);

        vector[(int)Vector3.Axis.Y] = 3.7f;
        Assert.Equal(3.7f, vector[(int)Vector3.Axis.Y]);
    }

    [Fact]
    public void InterpolationMethods()
    {
        var vector1 = new Vector3(1, 2, 3);
        var vector2 = new Vector3(4, 5, 6);

        Assert.Equal(new Vector3(2.5f, 3.5f, 4.5f), vector1.Lerp(vector2, 0.5f));
        Assert.Equal(new Vector3(2, 3, 4), vector1.Lerp(vector2, 1.0f / 3.0f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(0.363866806030273438f, 0.555698215961456299f, 0.747529566287994385f), vector1.Normalized().Slerp(vector2.Normalized(), 0.5f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.332119762897491455f, 0.549413740634918213f, 0.766707837581634521f), vector1.Normalized().Slerp(vector2.Normalized(), 1.0f / 3.0f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(3.535533905029296875f, 2.121320486068725586f, 2.828427314758300781f), new Vector3(5, 0, 0).Slerp(new Vector3(0, 3, 4), 0.5f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1.5f, 1.5f, 1.5f), new Vector3(1, 1, 1).Slerp(new Vector3(2, 2, 2), 0.5f), ApproxEqualityComparer.Instance);

        Assert.Equal(Vector3.Zero, Vector3.Zero.Slerp(Vector3.Zero, 0.5f));
        Assert.Equal(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Zero.Slerp(new Vector3(1, 1, 1), 0.5f));
        Assert.Equal(new Vector3(0.5f, 0.5f, 0.5f), new Vector3(1, 1, 1).Slerp(Vector3.Zero, 0.5f));

        Assert.Equal(new Vector3(5.90194219811429941053f, 8.06758688849378394534f, 2.558307894718317120038f), new Vector3(4, 6, 2).Slerp(new Vector3(8, 10, 3), 0.5f), ApproxEqualityComparer.Instance);

        Assert.Equal(6.25831088708303172f, vector1.Slerp(vector2, 0.5f).Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(vector1.AngleTo(vector2), vector1.AngleTo(vector1.Slerp(vector2, 0.5f)) * 2f, ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(2.375f, 3.5f, 4.625f), vector1.CubicInterpolate(vector2, Vector3.Zero, new Vector3(7, 7, 7), 0.5f));
        Assert.Equal(new Vector3(1.851851940155029297f, 2.962963104248046875f, 4.074074268341064453f), vector1.CubicInterpolate(vector2, Vector3.Zero, new Vector3(7, 7, 7), 1.0f / 3.0f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(4, 0, 0), new Vector3(1, 0, 0).MoveToward(new Vector3(10, 0, 0), 3f));
    }

    [Fact]
    public void LengthMethods()
    {
        var vector1 = new Vector3(10, 10, 10);
        var vector2 = new Vector3(20, 30, 40);

        Assert.Equal(300f, vector1.LengthSquared());
        Assert.Equal(10 * MathfExtensions.Sqrt3, vector1.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(2900f, vector2.LengthSquared());
        Assert.Equal(53.8516480713450403125f, vector2.Length(), ApproxEqualityComparer.Instance);
        Assert.Equal(1400f, vector1.DistanceSquaredTo(vector2));
        Assert.Equal(37.41657386773941385584f, vector1.DistanceTo(vector2), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void LimitingMethods()
    {
        var vector = new Vector3(10, 10, 10);

        Assert.Equal(new Vector3(MathfExtensions.Sqrt13, MathfExtensions.Sqrt13, MathfExtensions.Sqrt13), vector.LimitLength(), ApproxEqualityComparer.Instance);
        Assert.Equal(5f * new Vector3(MathfExtensions.Sqrt13, MathfExtensions.Sqrt13, MathfExtensions.Sqrt13), vector.LimitLength(5f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(0, 5, 10), new Vector3(-5, 5, 15).Clamp(Vector3.Zero, vector));
        Assert.Equal(new Vector3(5, 10, 15), vector.Clamp(new Vector3(0, 10, 15), new Vector3(5, 10, 20)));
    }

    [Fact]
    public void NormalizationMethods()
    {
        Assert.True(new Vector3(1, 0, 0).IsNormalized());
        Assert.False(new Vector3(1, 1, 1).IsNormalized());

        Assert.Equal(new Vector3(1, 0, 0), new Vector3(1, 0, 0).Normalized());
        Assert.Equal(new Vector3(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12, 0), new Vector3(1, 1, 0).Normalized(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(MathfExtensions.Sqrt13, MathfExtensions.Sqrt13, MathfExtensions.Sqrt13), new Vector3(1, 1, 1).Normalized(), ApproxEqualityComparer.Instance);

        var vector = new Vector3(3.2f, -5.4f, 6f);
        Assert.Equal(new Vector3(0.368522751763902980457f, -0.621882143601586279522f, 0.6909801595573180883585f), vector.Normalized(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var decimal1 = new Vector3(2.3f, 4.9f, 7.8f);
        var decimal2 = new Vector3(1.2f, 3.4f, 5.6f);
        var power1 = new Vector3(0.75f, 1.5f, 0.625f);
        var power2 = new Vector3(0.5f, 0.125f, 0.25f);
        var int1 = new Vector3(4, 5, 9);
        var int2 = new Vector3(1, 2, 3);

        Assert.Equal(new Vector3(3.5f, 8.3f, 13.4f), decimal1 + decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1.25f, 1.625f, 0.875f), power1 + power2);
        Assert.Equal(new Vector3(5, 7, 12), int1 + int2);

        Assert.Equal(new Vector3(1.1f, 1.5f, 2.2f), decimal1 - decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.25f, 1.375f, 0.375f), power1 - power2);
        Assert.Equal(new Vector3(3, 3, 6), int1 - int2);

        Assert.Equal(new Vector3(2.76f, 16.66f, 43.68f), decimal1 * decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.375f, 0.1875f, 0.15625f), power1 * power2);
        Assert.Equal(new Vector3(4, 10, 27), int1 * int2);

        Assert.Equal(new Vector3(1.91666666666666666f, 1.44117647058823529f, 1.39285714285714286f), decimal1 / decimal2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1.5f, 12.0f, 2.5f), power1 / power2);
        Assert.Equal(new Vector3(4, 2.5f, 3), int1 / int2);

        Assert.Equal(new Vector3(4.6f, 9.8f, 15.6f), decimal1 * 2f, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1.5f, 3f, 1.25f), power1 * 2f);
        Assert.Equal(new Vector3(8, 10, 18), int1 * 2f);

        Assert.Equal(new Vector3(1.15f, 2.45f, 3.9f), decimal1 / 2f, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.375f, 0.75f, 0.3125f), power1 / 2f);
        Assert.Equal(new Vector3(2, 2.5f, 4.5f), int1 / 2f);

        Assert.Equal(new Vector3I(2, 4, 7), (Vector3I)decimal1);
        Assert.Equal(new Vector3I(1, 3, 5), (Vector3I)decimal2);

        Assert.Equal(new Vector3(1, 2, 3), new Vector3I(1, 2, 3));
    }

    [Fact]
    public void OtherMethods()
    {
        var vector = new Vector3(1.2f, 3.4f, 5.6f);

        Assert.Equal(-vector.Normalized(), vector.DirectionTo(Vector3.Zero), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(MathfExtensions.Sqrt13, MathfExtensions.Sqrt13, MathfExtensions.Sqrt13), new Vector3(1, 1, 1).DirectionTo(new Vector3(2, 2, 2)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1f / 1.2f, 1f / 3.4f, 1f / 5.6f), vector.Inverse(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1.2f, 1.4f, 1.6f), vector.PosMod(2f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.8f, 0.6f, 0.4f), (-vector).PosMod(2f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.2f, 1.4f, 2.6f), vector.PosMod(new Vector3(1, 2, 3)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.8f, 2.6f, 2.4f), (-vector).PosMod(new Vector3(2, 3, 4)), ApproxEqualityComparer.Instance);

        Assert.Equal(vector, vector.Rotated(new Vector3(0, 1, 0), Mathf.Tau), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(5.6f, 3.4f, -1.2f), vector.Rotated(new Vector3(0, 1, 0), Mathf.Tau / 4f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(1.2f, -6.54974226119285642f, 0.1444863728670914f), vector.Rotated(new Vector3(1, 0, 0), Mathf.Tau / 3f), ApproxEqualityComparer.Instance);
        Assert.Equal(vector.Rotated(new Vector3(0, 0, 1), Mathf.Tau / 2f), vector.Rotated(new Vector3(0, 0, 1), Mathf.Tau / -2f), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(1, 3, 6), vector.Snapped(new Vector3(1, 1, 1)));
        Assert.Equal(new Vector3(1.25f, 3.5f, 5.5f), vector.Snapped(new Vector3(0.25f, 0.25f, 0.25f)));

        Assert.Equal(new Vector3(1.2f, 2.5f, 2.0f), vector.Min(new Vector3(3.0f, 2.5f, 2.0f)), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(5.3f, 3.4f, 5.6f), vector.Max(new Vector3(5.3f, 2.0f, 3.0f)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PlaneMethods()
    {
        var vector = new Vector3(1.2f, 3.4f, 5.6f);
        var vectorY = new Vector3(0, 1, 0);
        var vectorNormal = new Vector3(0.88763458893247992491f, 0.26300284116517923701f, 0.37806658417494515320f);

        Assert.Equal(new Vector3(1.2f, -3.4f, 5.6f), vector.Bounce(vectorY));
        Assert.Equal(new Vector3(-6.0369629829775736287f, 1.25571467171034855444f, 2.517589840583626047f), vector.Bounce(vectorNormal), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(-1.2f, 3.4f, -5.6f), vector.Reflect(vectorY));
        Assert.Equal(new Vector3(6.0369629829775736287f, -1.25571467171034855444f, -2.517589840583626047f), vector.Reflect(vectorNormal), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(0, 3.4f, 0), vector.Project(vectorY));
        Assert.Equal(new Vector3(3.61848149148878681437f, 1.0721426641448257227776f, 1.54120507970818697649f), vector.Project(vectorNormal), ApproxEqualityComparer.Instance);

        Assert.Equal(new Vector3(1.2f, 0, 5.6f), vector.Slide(vectorY));
        Assert.Equal(new Vector3(-2.41848149148878681437f, 2.32785733585517427722237f, 4.0587949202918130235f), vector.Slide(vectorNormal), ApproxEqualityComparer.Instance);

        var vectorNonNormal = new Vector3(5.4f, 1.6f, 2.3f);
        Assert.Throws<ArgumentException>(() => vector.Bounce(vectorNonNormal));
        Assert.Throws<ArgumentException>(() => vector.Reflect(vectorNonNormal));
        Assert.Throws<ArgumentException>(() => vector.Slide(vectorNonNormal));
    }

    [Fact]
    public void RoundingMethods()
    {
        var vector1 = new Vector3(1.2f, 3.4f, 5.6f);
        var vector2 = new Vector3(1.2f, -3.4f, -5.6f);

        Assert.Equal(vector1, vector1.Abs());
        Assert.Equal(vector1, vector2.Abs());

        Assert.Equal(new Vector3(2, 4, 6), vector1.Ceil());
        Assert.Equal(new Vector3(2, -3, -5), vector2.Ceil());

        Assert.Equal(new Vector3(1, 3, 5), vector1.Floor());
        Assert.Equal(new Vector3(1, -4, -6), vector2.Floor());

        Assert.Equal(new Vector3(1, 3, 6), vector1.Round());
        Assert.Equal(new Vector3(1, -3, -6), vector2.Round());

        Assert.Equal(new Vector3I(1, 1, 1), vector1.Sign());
        Assert.Equal(new Vector3I(1, -1, -1), vector2.Sign());
    }

    [Fact]
    public void LinearAlgebraMethods()
    {
        var vectorX = new Vector3(1, 0, 0);
        var vectorY = new Vector3(0, 1, 0);
        var vectorZ = new Vector3(0, 0, 1);
        var a = new Vector3(3.5f, 8.5f, 2.3f);
        var b = new Vector3(5.2f, 4.6f, 7.8f);

        Assert.Equal(vectorZ, vectorX.Cross(vectorY));
        Assert.Equal(-vectorZ, vectorY.Cross(vectorX));
        Assert.Equal(vectorX, vectorY.Cross(vectorZ));
        Assert.Equal(vectorY, vectorZ.Cross(vectorX));
        Assert.Equal(new Vector3(55.72f, -15.34f, -28.1f), a.Cross(b), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(55.72f, 15.34f, -28.1f), new Vector3(-a.X, a.Y, -a.Z).Cross(new Vector3(b.X, -b.Y, b.Z)), ApproxEqualityComparer.Instance);

        Assert.Equal(0.0f, vectorX.Dot(vectorY));
        Assert.Equal(1.0f, vectorX.Dot(vectorX));
        Assert.Equal(100.0f, (vectorX * 10f).Dot(vectorX * 10f));

        Assert.Equal(75.24f, a.Dot(b), ApproxEqualityComparer.Instance);
        Assert.Equal(-75.24f, new Vector3(-a.X, a.Y, -a.Z).Dot(new Vector3(b.X, -b.Y, b.Z)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        float[] infinite = [float.NaN, float.PositiveInfinity, float.NegativeInfinity];

        Assert.True(new Vector3(0, 1, 2).IsFinite());

        foreach (float x in infinite)
        {
            Assert.False(new Vector3(x, 1, 2).IsFinite());
            Assert.False(new Vector3(0, x, 2).IsFinite());
            Assert.False(new Vector3(0, 1, x).IsFinite());
        }

        foreach (float x in infinite)
        {
            foreach (float y in infinite)
            {
                Assert.False(new Vector3(x, y, 2).IsFinite());
                Assert.False(new Vector3(x, 1, y).IsFinite());
                Assert.False(new Vector3(0, x, y).IsFinite());
            }
        }

        foreach (float x in infinite)
        {
            foreach (float y in infinite)
            {
                foreach (float z in infinite)
                {
                    Assert.False(new Vector3(x, y, z).IsFinite());
                }
            }
        }
    }
}
