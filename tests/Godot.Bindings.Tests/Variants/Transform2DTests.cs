namespace Godot.Bindings.Tests;

public class Transform2DTests
{
    private static Transform2D CreateDummyTransform()
    {
        return new Transform2D(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6));
    }

    [Fact]
    public void ConstructorFromAngleAndPosition()
    {
        const float Rotation = Mathf.Pi / 4;
        var translation = new Vector2(20, -20);

        var test = new Transform2D(Rotation, translation);
        var expected = Transform2D.Identity.Rotated(Rotation).Translated(translation);
        Assert.Equal(expected, test);
    }

    [Fact]
    public void ConstructorFromAngleScaleSkewAndPosition()
    {
        const float Rotation = Mathf.Pi / 2;
        var scale = new Vector2(2, 0.5f);
        const float Skew = Mathf.Pi / 4;
        var translation = new Vector2(30, 0);

        var test = new Transform2D(Rotation, scale, Skew, translation);
        var expected = Transform2D.Identity.Scaled(scale).Rotated(Rotation).Translated(translation);
        expected = WithSkew(expected, Skew);
        Assert.Equal(expected, test, ApproxEqualityComparer.Instance);

        static Transform2D WithSkew(Transform2D transform, float angle)
        {
            float det = transform.Determinant();
            transform[1] = float.Sign(det)
                                 * transform[0].Rotated(float.Pi * 0.5f + angle).Normalized()
                                 * transform[1].Length();
            return transform;
        }
    }

    [Fact]
    public void ConstructorFromRawValues()
    {
        var test = new Transform2D(1f, 2f, 3f, 4f, 5f, 6f);
        var expected = new Transform2D(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6));
        Assert.Equal(expected, test);
    }

    [Fact]
    public void Xform()
    {
        var v = new Vector2(2, 3);
        var t = new Transform2D(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6));
        var expected = new Vector2(1 * 2 + 3 * 3 + 5 * 1, 2 * 2 + 4 * 3 + 6 * 1);
        Assert.Equal(expected, t * v);
    }

    [Fact]
    public void BasisXform()
    {
        var v = new Vector2(2, 2);
        var t1 = new Transform2D(new Vector2(1, 2), new Vector2(3, 4), new Vector2(0, 0));

        // Both versions should be the same when the origin is (0,0).
        Assert.Equal(t1.BasisXform(v), t1 * v);

        var t2 = new Transform2D(new Vector2(1, 2), new Vector2(3, 4), new Vector2(5, 6));

        // Each version should be different when the origin is not (0,0).
        Assert.NotEqual(t2.BasisXform(v), t2 * v);
    }

    [Fact]
    public void AffineInverse()
    {
        var orig = CreateDummyTransform();
        var affineInverted = orig.AffineInverse();
        var affineInvertedAgain = affineInverted.AffineInverse();
        Assert.Equal(orig, affineInvertedAgain);
    }

    [Fact]
    public void Orthonormalized()
    {
        var t = CreateDummyTransform();
        var orthonormalizedT = t.Orthonormalized();

        // Check each basis has length 1.
        Assert.Equal(1, orthonormalizedT[0].LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1, orthonormalizedT[1].LengthSquared(), ApproxEqualityComparer.Instance);

        Vector2 vx = new Vector2(orthonormalizedT[0].X, orthonormalizedT[1].X);
        Vector2 vy = new Vector2(orthonormalizedT[0].Y, orthonormalizedT[1].Y);

        // Check the basis are orthogonal.
        Assert.Equal(1, TDotX(orthonormalizedT, vx), ApproxEqualityComparer.Instance);
        Assert.Equal(0, TDotX(orthonormalizedT, vy), ApproxEqualityComparer.Instance);
        Assert.Equal(0, TDotY(orthonormalizedT, vx), ApproxEqualityComparer.Instance);
        Assert.Equal(1, TDotY(orthonormalizedT, vy), ApproxEqualityComparer.Instance);

        static float TDotX(Transform2D transform, Vector2 vector)
        {
            return transform[0][0] * vector.X + transform[1][0] * vector.Y;
        }
        static float TDotY(Transform2D transform, Vector2 vector)
        {
            return transform[0][1] * vector.X + transform[1][1] * vector.Y;
        }
    }

    [Fact]
    public void Translation()
    {
        var offset = new Vector2(1, 2);

        // Both versions should give the same result applied to identity.
        Assert.Equal(Transform2D.Identity.Translated(offset), Transform2D.Identity.TranslatedLocal(offset));

        // Check both versions against left and right multiplications.
        var orig = CreateDummyTransform();
        var t = Transform2D.Identity.Translated(offset);
        Assert.Equal(t * orig, orig.Translated(offset));
        Assert.Equal(orig * t, orig.TranslatedLocal(offset));
    }

    [Fact]
    public void Scaling()
    {
        var scaling = new Vector2(1, 2);

        // Both versions should give the same result applied to identity.
        Assert.Equal(Transform2D.Identity.Scaled(scaling), Transform2D.Identity.ScaledLocal(scaling));

        // Check both versions against left and right multiplications.
        var orig = CreateDummyTransform();
        var s = Transform2D.Identity.Scaled(scaling);
        Assert.Equal(s * orig, orig.Scaled(scaling));
        Assert.Equal(orig * s, orig.ScaledLocal(scaling));
    }

    [Fact]
    public void Rotation()
    {
        const float Phi = 1.0f;

        // Both versions should give the same result applied to identity.
        Assert.Equal(Transform2D.Identity.Rotated(Phi), Transform2D.Identity.RotatedLocal(Phi));

        // Check both versions against left and right multiplications.
        var orig = CreateDummyTransform();
        var r = Transform2D.Identity.Rotated(Phi);
        Assert.Equal(r * orig, orig.Rotated(Phi));
        Assert.Equal(orig * r, orig.RotatedLocal(Phi));
    }

    [Fact]
    public void Interpolation()
    {
        var rotateScaleSkewPos = new Transform2D(
            Mathf.DegToRad(170f), new Vector2(3.6f, 8.0f), Mathf.DegToRad(20f), new Vector2(2.4f, 6.8f));
        var rotateScaleSkewPosHalfway = new Transform2D(
            Mathf.DegToRad(85f), new Vector2(2.3f, 4.5f), Mathf.DegToRad(10f), new Vector2(1.2f, 3.4f));

        Transform2D interpolated = Transform2D.Identity.InterpolateWith(rotateScaleSkewPos, 0.5f);
        Assert.Equal(rotateScaleSkewPosHalfway.Origin, interpolated.Origin, ApproxEqualityComparer.Instance);
        Assert.Equal(rotateScaleSkewPosHalfway.Rotation, interpolated.Rotation, ApproxEqualityComparer.Instance);
        Assert.Equal(rotateScaleSkewPosHalfway.Scale, interpolated.Scale, ApproxEqualityComparer.Instance);
        Assert.Equal(rotateScaleSkewPosHalfway.Skew, interpolated.Skew, ApproxEqualityComparer.Instance);
        Assert.Equal(rotateScaleSkewPosHalfway, interpolated, ApproxEqualityComparer.Instance);

        interpolated = rotateScaleSkewPos.InterpolateWith(Transform2D.Identity, 0.5f);
        Assert.Equal(rotateScaleSkewPosHalfway, interpolated, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        var x = new Vector2(0, 1);
        var infinite = new Vector2(float.NaN, float.NaN);

        Assert.True(new Transform2D(x, x, x).IsFinite());

        Assert.False(new Transform2D(infinite, x, x).IsFinite());
        Assert.False(new Transform2D(x, infinite, x).IsFinite());
        Assert.False(new Transform2D(x, x, infinite).IsFinite());

        Assert.False(new Transform2D(infinite, infinite, x).IsFinite());
        Assert.False(new Transform2D(infinite, x, infinite).IsFinite());
        Assert.False(new Transform2D(x, infinite, infinite).IsFinite());

        Assert.False(new Transform2D(infinite, infinite, infinite).IsFinite());
    }

    [Fact]
    public void IsConformalChecks()
    {
        Assert.True(Transform2D.Identity.IsConformal());

        Assert.True(new Transform2D(1.2f, Vector2.Zero).IsConformal());

        Assert.True(new Transform2D(new Vector2(1, 0), new Vector2(0, -1), Vector2.Zero).IsConformal());

        Assert.True(new Transform2D(new Vector2(1.2f, 0), new Vector2(0, 1.2f), Vector2.Zero).IsConformal());

        Assert.True(new Transform2D(new Vector2(1.2f, 3.4f), new Vector2(3.4f, -1.2f), Vector2.Zero).IsConformal());

        Assert.False(new Transform2D(new Vector2(1.2f, 0), new Vector2(0, 3.4f), Vector2.Zero).IsConformal());

        Assert.False(new Transform2D(new Vector2(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12), new Vector2(0, 1), Vector2.Zero).IsConformal());
    }
}
