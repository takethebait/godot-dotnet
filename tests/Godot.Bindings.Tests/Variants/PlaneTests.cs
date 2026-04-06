namespace Godot.Bindings.Tests;

public class PlaneTests
{
    [Fact]
    public void ConstructorMethods()
    {
        var plane = new Plane(32, 22, 16, 3);
        var planeVector = new Plane(new Vector3(32, 22, 16), 3);

        Assert.Equal(plane, planeVector);
    }

    [Fact]
    public void BasicGetters()
    {
        var plane = new Plane(32, 22, 16, 3);
        var planeNormalized = new Plane(32.0f / 42, 22.0f / 42, 16.0f / 42, 3.0f / 42);

        Assert.Equal(new Vector3(32, 22, 16), plane.Normal, ApproxEqualityComparer.Instance);
        Assert.Equal(planeNormalized, plane.Normalized(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void BasicSetters()
    {
        var plane = new Plane(32, 22, 16, 3);
        plane.Normal = new Vector3(4, 2, 3);

        Assert.Equal(new Plane(4, 2, 3, 3), plane, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void PlanePointOperations()
    {
        var plane = new Plane(32, 22, 16, 3);
        var yFacingPlane = new Plane(0, 1, 0, 4);

        Assert.Equal(new Vector3(32 * 3, 22 * 3, 16 * 3), plane.GetCenter(), ApproxEqualityComparer.Instance);

        Assert.True(yFacingPlane.IsPointOver(new Vector3(0, 5, 0)));
    }

    [Fact]
    public void HasPoint()
    {
        var xFacingPlane = new Plane(1, 0, 0, 0);
        var yFacingPlane = new Plane(0, 1, 0, 0);
        var zFacingPlane = new Plane(0, 0, 1, 0);

        var xAxisPoint = new Vector3(10, 0, 0);
        var yAxisPoint = new Vector3(0, 10, 0);
        var zAxisPoint = new Vector3(0, 0, 10);

        var xFacingPlaneWithDOffset = new Plane(1, 0, 0, 1);
        var yAxisPointWithDOffset = new Vector3(1, 10, 0);

        Assert.True(xFacingPlane.HasPoint(yAxisPoint));
        Assert.True(xFacingPlane.HasPoint(zAxisPoint));

        Assert.True(yFacingPlane.HasPoint(xAxisPoint));
        Assert.True(yFacingPlane.HasPoint(zAxisPoint));

        Assert.True(zFacingPlane.HasPoint(yAxisPoint));
        Assert.True(zFacingPlane.HasPoint(xAxisPoint));

        Assert.True(xFacingPlaneWithDOffset.HasPoint(yAxisPointWithDOffset));
    }

    [Fact]
    public void Intersection()
    {
        var xFacingPlane = new Plane(1, 0, 0, 1);
        var yFacingPlane = new Plane(0, 1, 0, 2);
        var zFacingPlane = new Plane(0, 0, 1, 3);

        var intersect3Result = xFacingPlane.Intersect3(yFacingPlane, zFacingPlane);
        Assert.NotNull(intersect3Result);
        Assert.Equal(new Vector3(1, 2, 3), intersect3Result.Value, ApproxEqualityComparer.Instance);

        var rayResult = xFacingPlane.IntersectsRay(new Vector3(0, 1, 1), new Vector3(2, 0, 0));
        Assert.NotNull(rayResult);
        Assert.Equal(new Vector3(1, 1, 1), rayResult.Value, ApproxEqualityComparer.Instance);

        var segmentResult = xFacingPlane.IntersectsSegment(new Vector3(0, 1, 1), new Vector3(2, 1, 1));
        Assert.NotNull(segmentResult);
        Assert.Equal(new Vector3(1, 1, 1), segmentResult.Value, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        var x = new Vector3(0, 1, 2);
        var infiniteVec = new Vector3(float.NaN, float.NaN, float.NaN);
        float y = 0;
        float infiniteY = float.NaN;

        Assert.True(new Plane(x, y).IsFinite());

        Assert.False(new Plane(x, infiniteY).IsFinite());
        Assert.False(new Plane(infiniteVec, y).IsFinite());

        Assert.False(new Plane(infiniteVec, infiniteY).IsFinite());
    }
}
