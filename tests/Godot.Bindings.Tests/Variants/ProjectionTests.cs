namespace Godot.Bindings.Tests;

public class ProjectionTests
{
    [Fact]
    public void Construction()
    {
        var defaultProj = Projection.Identity;

        Assert.Equal(new Vector4(1, 0, 0, 0), defaultProj[0]);
        Assert.Equal(new Vector4(0, 1, 0, 0), defaultProj[1]);
        Assert.Equal(new Vector4(0, 0, 1, 0), defaultProj[2]);
        Assert.Equal(new Vector4(0, 0, 0, 1), defaultProj[3]);

        var fromVec4 = new Projection(
            new Vector4(1, 2, 3, 4),
            new Vector4(5, 6, 7, 8),
            new Vector4(9, 10, 11, 12),
            new Vector4(13, 14, 15, 16));

        Assert.Equal(new Vector4(1, 2, 3, 4), fromVec4[0]);
        Assert.Equal(new Vector4(5, 6, 7, 8), fromVec4[1]);
        Assert.Equal(new Vector4(9, 10, 11, 12), fromVec4[2]);
        Assert.Equal(new Vector4(13, 14, 15, 16), fromVec4[3]);

        var transform = new Transform3D(
            new Basis(
                new Vector3(1, 0, 0),
                new Vector3(0, 2, 0),
                new Vector3(0, 0, 3)),
            new Vector3(4, 5, 6));

        var fromTransform = new Projection(transform);

        Assert.Equal(new Vector4(1, 0, 0, 0), fromTransform[0]);
        Assert.Equal(new Vector4(0, 2, 0, 0), fromTransform[1]);
        Assert.Equal(new Vector4(0, 0, 3, 0), fromTransform[2]);
        Assert.Equal(new Vector4(4, 5, 6, 1), fromTransform[3]);
    }

    [Fact]
    public void Determinant()
    {
        var proj = new Projection(
            new Vector4(1, 5, 9, 13),
            new Vector4(2, 6, 11, 15),
            new Vector4(4, 7, 11, 15),
            new Vector4(4, 8, 12, 16));

        Assert.Equal(-12, proj.Determinant());
    }

    [Fact]
    public void ArbitraryProjectionMatrixInversion()
    {
        var proj = new Projection(
            new Vector4(1, 5, 9, 13),
            new Vector4(2, 6, 11, 15),
            new Vector4(4, 7, 11, 15),
            new Vector4(4, 8, 12, 16));

        var inverseTruth = new Projection(
            new Vector4(-4.0f / 12, 0, 1, -8.0f / 12),
            new Vector4(8.0f / 12, -1, -1, 16.0f / 12),
            new Vector4(-20.0f / 12, 2, -1, 5.0f / 12),
            new Vector4(1, -1, 1, -0.75f));

        var inverse = proj.Inverse();
        Assert.Equal(inverseTruth[0], inverse[0], ApproxEqualityComparer.Instance);
        Assert.Equal(inverseTruth[1], inverse[1], ApproxEqualityComparer.Instance);
        Assert.Equal(inverseTruth[2], inverse[2], ApproxEqualityComparer.Instance);
        Assert.Equal(inverseTruth[3], inverse[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void OrthogonalProjectionMatrixInversion()
    {
        var p = Projection.CreateOrthogonal(-125.0f, 125.0f, -125.0f, 125.0f, 0.01f, 25.0f);
        p = p.Inverse() * p;

        Assert.Equal(new Vector4(1, 0, 0, 0), p[0], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 1, 0, 0), p[1], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, 1, 0), p[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, 0, 1), p[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PerspectiveProjectionMatrixInversion()
    {
        var p = Projection.CreatePerspective(90.0f, 1.77777f, 0.05f, 4000.0f, false);
        p = p.Inverse() * p;

        Assert.Equal(new Vector4(1, 0, 0, 0), p[0], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 1, 0, 0), p[1], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, 1, 0), p[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, 0, 1), p[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void MatrixProduct()
    {
        var proj1 = new Projection(
            new Vector4(1, 5, 9, 13),
            new Vector4(2, 6, 11, 15),
            new Vector4(4, 7, 11, 15),
            new Vector4(4, 8, 12, 16));

        var proj2 = new Projection(
            new Vector4(0, 1, 2, 3),
            new Vector4(10, 11, 12, 13),
            new Vector4(20, 21, 22, 23),
            new Vector4(30, 31, 32, 33));

        var prod = proj1 * proj2;

        Assert.Equal(new Vector4(22, 44, 69, 93), prod[0], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(132, 304, 499, 683), prod[1], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(242, 564, 929, 1273), prod[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(352, 824, 1359, 1863), prod[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void VectorTransformation()
    {
        var proj = new Projection(
            new Vector4(1, 5, 9, 13),
            new Vector4(2, 6, 11, 15),
            new Vector4(4, 7, 11, 15),
            new Vector4(4, 8, 12, 16));

        var vec4 = new Vector4(1, 2, 3, 4);
        Assert.Equal(new Vector4(33, 70, 112, 152), proj * vec4, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(90, 107, 111, 120), vec4 * proj, ApproxEqualityComparer.Instance);

        var vec3 = new Vector3(1, 2, 3);
        Assert.Equal(new Vector3(21, 46, 76) / 104, proj * vec3, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ValuesAccess()
    {
        var proj = new Projection(
            new Vector4(00, 01, 02, 03),
            new Vector4(10, 11, 12, 13),
            new Vector4(20, 21, 22, 23),
            new Vector4(30, 31, 32, 33));

        Assert.Equal(new Vector4(00, 01, 02, 03), proj[0]);
        Assert.Equal(new Vector4(10, 11, 12, 13), proj[1]);
        Assert.Equal(new Vector4(20, 21, 22, 23), proj[2]);
        Assert.Equal(new Vector4(30, 31, 32, 33), proj[3]);
    }

    [Fact]
    public void FlippedY()
    {
        var proj = new Projection(
            new Vector4(00, 01, 02, 03),
            new Vector4(10, 11, 12, 13),
            new Vector4(20, 21, 22, 23),
            new Vector4(30, 31, 32, 33));

        var flipped = proj.FlippedY();

        Assert.Equal(proj[0], flipped[0]);
        Assert.Equal(-proj[1], flipped[1]);
        Assert.Equal(proj[2], flipped[2]);
        Assert.Equal(proj[3], flipped[3]);
    }

    [Fact]
    public void JitterOffset()
    {
        var proj = new Projection(
            new Vector4(00, 01, 02, 03),
            new Vector4(10, 11, 12, 13),
            new Vector4(20, 21, 22, 23),
            new Vector4(30, 31, 32, 33));

        var offsetted = proj.JitterOffseted(new Vector2(1, 2));

        Assert.Equal(proj[0], offsetted[0]);
        Assert.Equal(proj[1], offsetted[1]);
        Assert.Equal(proj[2], offsetted[2]);
        Assert.Equal(proj[3] + new Vector4(1, 2, 0, 0), offsetted[3]);
    }

    [Fact]
    public void PerspectiveZNearAdjusted()
    {
        var persp = Projection.CreatePerspective(90, 0.5f, 1, 50, false);
        var adjusted = persp.PerspectiveZNearAdjusted(2);

        Assert.Equal(persp[0], adjusted[0]);
        Assert.Equal(persp[1], adjusted[1]);
        Assert.Equal(new Vector4(persp[2][0], persp[2][1], -1.083333f, persp[2][3]), adjusted[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(persp[3][0], persp[3][1], -4.166666f, persp[3][3]), adjusted[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void DepthCorrection()
    {
        var corrected = Projection.CreateDepthCorrection(true);

        Assert.Equal(new Vector4(1, 0, 0, 0), corrected[0]);
        Assert.Equal(new Vector4(0, -1, 0, 0), corrected[1]);
        Assert.Equal(new Vector4(0, 0, -0.5f, 0), corrected[2]);
        Assert.Equal(new Vector4(0, 0, 0.5f, 1), corrected[3]);

        var notFlipped = Projection.CreateDepthCorrection(false);

        Assert.Equal(new Vector4(1, 0, 0, 0), notFlipped[0]);
        Assert.Equal(new Vector4(0, 1, 0, 0), notFlipped[1]);
        Assert.Equal(new Vector4(0, 0, -0.5f, 0), notFlipped[2]);
        Assert.Equal(new Vector4(0, 0, 0.5f, 1), notFlipped[3]);
    }

    [Fact]
    public void LightAtlasRect()
    {
        var rect = Projection.CreateLightAtlasRect(new Rect2(1, 2, 30, 40));

        Assert.Equal(new Vector4(30, 0, 0, 0), rect[0]);
        Assert.Equal(new Vector4(0, 40, 0, 0), rect[1]);
        Assert.Equal(new Vector4(0, 0, 1, 0), rect[2]);
        Assert.Equal(new Vector4(1, 2, 0, 1), rect[3]);
    }

    [Fact]
    public void CreateFitAabb()
    {
        var fit = Projection.CreateFitAabb(new Aabb(Vector3.Zero, new Vector3(0.1f, 0.2f, 0.4f)));

        Assert.Equal(new Vector4(20, 0, 0, 0), fit[0]);
        Assert.Equal(new Vector4(0, 10, 0, 0), fit[1]);
        Assert.Equal(new Vector4(0, 0, 5, 0), fit[2]);
        Assert.Equal(new Vector4(-1, -1, -1, 1), fit[3]);
    }

    [Fact]
    public void CreatePerspective()
    {
        var persp = Projection.CreatePerspective(90, 0.5f, 5, 15, false);

        Assert.Equal(new Vector4(2, 0, 0, 0), persp[0], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 1, 0, 0), persp[1], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, -2, -1), persp[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, -15, 0), persp[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void CreateFrustum()
    {
        var frustum = Projection.CreateFrustum(15, 20, 10, 12, 5, 15);

        Assert.Equal(new Vector4(2, 0, 0, 0), frustum[0], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 5, 0, 0), frustum[1], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(7, 11, -2, -1), frustum[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, -15, 0), frustum[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void CreateOrthogonal()
    {
        var ortho = Projection.CreateOrthogonal(15, 20, 10, 12, 5, 15);

        Assert.Equal(new Vector4(0.4f, 0, 0, 0), ortho[0], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 1, 0, 0), ortho[1], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(0, 0, -0.2f, 0), ortho[2], ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector4(-7, -11, -2, 1), ortho[3], ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void GetFovy()
    {
        float fov = Projection.GetFovy(90, 0.5f);
        Assert.Equal(53.1301f, fov, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PerspectiveValuesExtraction()
    {
        var persp = Projection.CreatePerspective(90, 0.5f, 1, 50, true);

        Assert.Equal(1, persp.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(50, persp.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(0.5f, persp.GetAspect(), ApproxEqualityComparer.Instance);
        Assert.Equal(90, persp.GetFov(), ApproxEqualityComparer.Instance);

        persp = Projection.CreatePerspective(38, 1.3f, 0.2f, 8, false);

        Assert.Equal(0.2f, persp.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(8, persp.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.3f, persp.GetAspect(), ApproxEqualityComparer.Instance);
        Assert.Equal(Projection.GetFovy(38, 1.3f), persp.GetFov(), ApproxEqualityComparer.Instance);

        persp = Projection.CreatePerspective(47, 2.5f, 0.9f, 14, true);

        Assert.Equal(0.9f, persp.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(14, persp.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(2.5f, persp.GetAspect(), ApproxEqualityComparer.Instance);
        Assert.Equal(47, persp.GetFov(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FrustumValuesExtraction()
    {
        var frustum = Projection.CreateFrustumAspect(1.0f, 4.0f / 3.0f, new Vector2(0.5f, -0.25f), 0.5f, 50, true);

        Assert.Equal(0.5f, frustum.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(50, frustum.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(4.0f / 3.0f, frustum.GetAspect(), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.RadToDeg(Mathf.Atan(2.0f)), frustum.GetFov(), ApproxEqualityComparer.Instance);

        frustum = Projection.CreateFrustumAspect(2.0f, 1.5f, new Vector2(-0.5f, 2), 2, 12, false);

        Assert.Equal(2, frustum.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(12, frustum.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.5f, frustum.GetAspect(), ApproxEqualityComparer.Instance);
        Assert.Equal(Mathf.RadToDeg(Mathf.Atan(1.0f) + Mathf.Atan(0.5f)), frustum.GetFov(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void OrthographicValuesExtraction()
    {
        var ortho = Projection.CreateOrthogonal(-2, 3, -0.5f, 1.5f, 1.2f, 15);

        Assert.Equal(1.2f, ortho.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(15, ortho.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(2.5f, ortho.GetAspect(), ApproxEqualityComparer.Instance);

        ortho = Projection.CreateOrthogonal(-7, 2, 2.5f, 5.5f, 0.5f, 6);

        Assert.Equal(0.5f, ortho.GetZNear(), ApproxEqualityComparer.Instance);
        Assert.Equal(6, ortho.GetZFar(), ApproxEqualityComparer.Instance);
        Assert.Equal(3, ortho.GetAspect(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void IsOrthogonal()
    {
        var persp = Projection.CreatePerspective(90, 0.5f, 1, 50, false);
        var ortho = Projection.CreateOrthogonal(15, 20, 10, 12, 5, 15);

        Assert.False(persp.IsOrthogonal());
        Assert.True(ortho.IsOrthogonal());
    }

    [Fact]
    public void PlanesExtraction()
    {
        var persp = Projection.CreatePerspective(90, 1, 1, 40, false);

        var near = persp.GetProjectionPlane(Projection.Planes.Near);
        var far = persp.GetProjectionPlane(Projection.Planes.Far);
        var left = persp.GetProjectionPlane(Projection.Planes.Left);
        var top = persp.GetProjectionPlane(Projection.Planes.Top);
        var right = persp.GetProjectionPlane(Projection.Planes.Right);
        var bottom = persp.GetProjectionPlane(Projection.Planes.Bottom);

        Assert.Equal(new Plane(0, 0, 1, -1), near, ApproxEqualityComparer.Instance);
        Assert.Equal(new Plane(0, 0, -1, 40), far, ApproxEqualityComparer.Instance);
        Assert.Equal(new Plane(-0.707107f, 0, 0.707107f, 0), left, ApproxEqualityComparer.Instance);
        Assert.Equal(new Plane(0, 0.707107f, 0.707107f, 0), top, ApproxEqualityComparer.Instance);
        Assert.Equal(new Plane(0.707107f, 0, 0.707107f, 0), right, ApproxEqualityComparer.Instance);
        Assert.Equal(new Plane(0, -0.707107f, 0.707107f, 0), bottom, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PerspectiveHalfExtents()
    {
        const float Sqrt3 = 1.7320508f;
        var persp = Projection.CreatePerspective(90, 1, 1, 40, false);

        Assert.Equal(new Vector2(1, 1) * 1, persp.GetViewportHalfExtents(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1, 1) * 40, persp.GetFarPlaneHalfExtents(), ApproxEqualityComparer.Instance);

        persp = Projection.CreatePerspective(120, Sqrt3, 0.8f, 10, true);
        Assert.Equal(new Vector2(Sqrt3, 1) * 0.8f, persp.GetViewportHalfExtents(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(Sqrt3, 1) * 10, persp.GetFarPlaneHalfExtents(), ApproxEqualityComparer.Instance);

        persp = Projection.CreatePerspective(60, 1.2f, 0.5f, 15, false);
        Assert.Equal(new Vector2(Sqrt3 / 3 * 1.2f, Sqrt3 / 3) * 0.5f, persp.GetViewportHalfExtents(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(Sqrt3 / 3 * 1.2f, Sqrt3 / 3) * 15, persp.GetFarPlaneHalfExtents(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void OrthographicHalfExtents()
    {
        var ortho = Projection.CreateOrthogonal(-3, 3, -1.5f, 1.5f, 1.2f, 15);

        Assert.Equal(new Vector2(3, 1.5f), ortho.GetViewportHalfExtents(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(3, 1.5f), ortho.GetFarPlaneHalfExtents(), ApproxEqualityComparer.Instance);

        ortho = Projection.CreateOrthogonal(-7, 7, -2.5f, 2.5f, 0.5f, 6);
        Assert.Equal(new Vector2(7, 2.5f), ortho.GetViewportHalfExtents(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(7, 2.5f), ortho.GetFarPlaneHalfExtents(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void LodMultiplier()
    {
        var proj = Projection.CreatePerspective(60, 1, 1, 40, false);
        Assert.Equal(2 * MathfExtensions.Sqrt3 / 3, proj.GetLodMultiplier(), ApproxEqualityComparer.Instance);

        proj = Projection.CreatePerspective(120, 1.5f, 0.5f, 20, false);
        Assert.Equal(3 * MathfExtensions.Sqrt3, proj.GetLodMultiplier(), ApproxEqualityComparer.Instance);

        proj = Projection.CreateOrthogonal(15, 20, 10, 12, 5, 15);
        Assert.Equal(5, proj.GetLodMultiplier(), ApproxEqualityComparer.Instance);

        proj = Projection.CreateOrthogonal(-5, 15, -8, 10, 1.5f, 10);
        Assert.Equal(20, proj.GetLodMultiplier(), ApproxEqualityComparer.Instance);

        proj = Projection.CreateFrustumAspect(1, 4.0f / 3.0f, new Vector2(0.5f, -0.25f), 0.5f, 50, false);
        Assert.Equal(8.0f / 3.0f, proj.GetLodMultiplier(), ApproxEqualityComparer.Instance);

        proj = Projection.CreateFrustumAspect(2, 1.2f, new Vector2(-0.1f, 0.8f), 1, 10, true);
        Assert.Equal(2, proj.GetLodMultiplier(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PixelsPerMeter()
    {
        var proj = Projection.CreatePerspective(60, 1, 1, 40, false);
        Assert.Equal((int)(1536.0f / MathfExtensions.Sqrt3), (int)proj.GetPixelsPerMeter(1024));

        proj = Projection.CreatePerspective(120, 1.5f, 0.5f, 20, false);
        Assert.Equal((int)(800.0f / MathfExtensions.Sqrt3), (int)proj.GetPixelsPerMeter(1200));

        proj = Projection.CreateOrthogonal(15, 20, 10, 12, 5, 15);
        Assert.Equal(100, (int)proj.GetPixelsPerMeter(500));

        proj = Projection.CreateOrthogonal(-5, 15, -8, 10, 1.5f, 10);
        Assert.Equal(32, (int)proj.GetPixelsPerMeter(640));

        proj = Projection.CreateFrustumAspect(1.0f, 4.0f / 3.0f, new Vector2(0.5f, -0.25f), 0.5f, 50, false);
        Assert.Equal(1536, (int)proj.GetPixelsPerMeter(2048));

        proj = Projection.CreateFrustumAspect(2.0f, 1.2f, new Vector2(-0.1f, 0.8f), 1, 10, true);
        Assert.Equal(400, (int)proj.GetPixelsPerMeter(800));
    }
}
