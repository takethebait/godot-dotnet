namespace Godot.Bindings.Tests;

public class Transform3DTests
{
    private static Transform3D CreateDummyTransform() =>
        new Transform3D(
            new Basis(new Vector3(1, 2, 3), new Vector3(4, 5, 6), new Vector3(7, 8, 9)),
            new Vector3(10, 11, 12));

    [Fact]
    public void Translation()
    {
        var offset = new Vector3(1, 2, 3);

        // Both versions should give the same result applied to identity.
        Assert.Equal(Transform3D.Identity.Translated(offset), Transform3D.Identity.TranslatedLocal(offset));

        // Check both versions against left and right multiplications.
        var orig = CreateDummyTransform();
        var t = Transform3D.Identity.Translated(offset);
        Assert.Equal(t * orig, orig.Translated(offset));
        Assert.Equal(orig * t, orig.TranslatedLocal(offset));
    }

    [Fact]
    public void Scaling()
    {
        var scaling = new Vector3(1, 2, 3);

        // Both versions should give the same result applied to identity.
        Assert.Equal(Transform3D.Identity.Scaled(scaling), Transform3D.Identity.ScaledLocal(scaling));

        // Check both versions against left and right multiplications.
        var orig = CreateDummyTransform();
        var s = Transform3D.Identity.Scaled(scaling);
        Assert.Equal(s * orig, orig.Scaled(scaling));
        Assert.Equal(orig * s, orig.ScaledLocal(scaling));
    }

    [Fact]
    public void Rotation()
    {
        var axis = new Vector3(1, 2, 3).Normalized();
        const float Phi = 1.0f;

        // Both versions should give the same result applied to identity.
        Assert.Equal(Transform3D.Identity.Rotated(axis, Phi), Transform3D.Identity.RotatedLocal(axis, Phi));

        // Check both versions against left and right multiplications.
        var orig = CreateDummyTransform();
        var r = Transform3D.Identity.Rotated(axis, Phi);
        Assert.Equal(r * orig, orig.Rotated(axis, Phi));
        Assert.Equal(orig * r, orig.RotatedLocal(axis, Phi));
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        var y = new Vector3(0, 1, 2);
        var infiniteVec = new Vector3(float.NaN, float.NaN, float.NaN);
        var x = new Basis(y, y, y);
        var infiniteBasis = new Basis(infiniteVec, infiniteVec, infiniteVec);

        Assert.True(new Transform3D(x, y).IsFinite());

        Assert.False(new Transform3D(x, infiniteVec).IsFinite());
        Assert.False(new Transform3D(infiniteBasis, y).IsFinite());

        Assert.False(new Transform3D(infiniteBasis, infiniteVec).IsFinite());
    }

    [Fact]
    public void RotateAroundGlobalOrigin()
    {
        // Start with the default orientation, but not centered on the origin.
        // Rotating should rotate both our basis and the origin.
        var transform = Transform3D.Identity;
        transform.Origin = new Vector3(0, 0, 1);

        var expected = Transform3D.Identity;
        expected.Origin = new Vector3(0, 0, -1);
        expected.Basis[0] = new Vector3(-1, 0, 0);
        expected.Basis[2] = new Vector3(0, 0, -1);

        var rotatedTransform = transform.Rotated(new Vector3(0, 1, 0), float.Pi);
        Assert.Equal(expected, rotatedTransform, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void RotateInPlaceLocalRotation()
    {
        // Start with the default orientation.
        // Local rotation should not change the origin, only the basis.
        var transform = Transform3D.Identity;
        transform.Origin = new Vector3(1, 2, 3);

        var expected = Transform3D.Identity;
        expected.Origin = new Vector3(1, 2, 3);
        expected.Basis[0] = new Vector3(-1, 0, 0);
        expected.Basis[2] = new Vector3(0, 0, -1);

        var rotatedTransform = transform.RotatedLocal(new Vector3(0, 1, 0), float.Pi);
        Assert.Equal(expected, rotatedTransform, ApproxEqualityComparer.Instance);
    }
}
