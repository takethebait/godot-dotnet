using System;

namespace Godot.Bindings.Tests;

public class AabbTests
{
    [Fact]
    public void BasicGetters()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.Equal(new Vector3(-1.5f, 2, -2.5f), aabb.Position, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(4, 5, 6), aabb.Size, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(2.5f, 7, 3.5f), aabb.End, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.5f, 4.5f, 0.5f), aabb.GetCenter(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void BasicSetters()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        aabb.End = new Vector3(100, 0, 100);
        Assert.Equal(new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(101.5f, -2, 102.5f)), aabb, ApproxEqualityComparer.Instance);

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        aabb.Position = new Vector3(-1000, -2000, -3000);
        Assert.Equal(new Aabb(new Vector3(-1000, -2000, -3000), new Vector3(4, 5, 6)), aabb, ApproxEqualityComparer.Instance);

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        aabb.Size = new Vector3(0, 0, -50);
        Assert.Equal(new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(0, 0, -50)), aabb, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void VolumeGetters()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));

        Assert.Equal(120f, aabb.Volume, ApproxEqualityComparer.Instance);
        Assert.True(aabb.HasVolume());

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(-4, 5, 6));
        Assert.Equal(-120f, aabb.Volume, ApproxEqualityComparer.Instance);

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(-4, -5, 6));
        Assert.Equal(120f, aabb.Volume, ApproxEqualityComparer.Instance);

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(-4, -5, -6));
        Assert.Equal(-120f, aabb.Volume, ApproxEqualityComparer.Instance);

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 0, 6));
        Assert.False(aabb.HasVolume());

        Assert.False(new Aabb().HasVolume());
    }

    [Fact]
    public void SurfaceGetters()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.True(aabb.HasSurface());

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 0, 6));
        Assert.True(aabb.HasSurface());

        aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 0, 0));
        Assert.True(aabb.HasSurface());

        Assert.False(new Aabb().HasSurface());
    }

    [Fact]
    public void Intersection()
    {
        var aabbBig = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));

        var aabbSmall = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(1, 1, 1));
        Assert.True(aabbBig.Intersects(aabbSmall));

        aabbSmall = new Aabb(new Vector3(0.5f, 1.5f, -2), new Vector3(1, 1, 1));
        Assert.True(aabbBig.Intersects(aabbSmall));

        aabbSmall = new Aabb(new Vector3(10, -10, -10), new Vector3(1, 1, 1));
        Assert.False(aabbBig.Intersects(aabbSmall));

        aabbSmall = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(1, 1, 1));
        Assert.Equal(aabbSmall, aabbBig.Intersection(aabbSmall), ApproxEqualityComparer.Instance);

        aabbSmall = new Aabb(new Vector3(0.5f, 1.5f, -2), new Vector3(1, 1, 1));
        Assert.Equal(new Aabb(new Vector3(0.5f, 2, -2), new Vector3(1, 0.5f, 1)), aabbBig.Intersection(aabbSmall), ApproxEqualityComparer.Instance);

        aabbSmall = new Aabb(new Vector3(10, -10, -10), new Vector3(1, 1, 1));
        Assert.Equal(new Aabb(), aabbBig.Intersection(aabbSmall), ApproxEqualityComparer.Instance);

        Assert.True(aabbBig.IntersectsPlane(new Plane(new Vector3(0, 1, 0), 4)));
        Assert.True(aabbBig.IntersectsPlane(new Plane(new Vector3(0, -1, 0), -4)));
        Assert.False(aabbBig.IntersectsPlane(new Plane(new Vector3(0, 1, 0), 200)));

        Assert.True(aabbBig.IntersectsSegment(new Vector3(1, 3, 0), new Vector3(0, 3, 0)));
        Assert.True(aabbBig.IntersectsSegment(new Vector3(0, 3, 0), new Vector3(0, -300, 0)));
        Assert.True(aabbBig.IntersectsSegment(new Vector3(-50, 3, -50), new Vector3(50, 3, 50)));
        Assert.False(aabbBig.IntersectsSegment(new Vector3(-50, 25, -50), new Vector3(50, 25, 50)));
        Assert.True(aabbBig.IntersectsSegment(new Vector3(0, 3, 0), new Vector3(0, 3, 0)));
        Assert.False(aabbBig.IntersectsSegment(new Vector3(0, 300, 0), new Vector3(0, 300, 0)));

        Assert.True(aabbBig.IntersectsRay(new Vector3(-100, 3, 0), new Vector3(1, 0, 0)));
        Assert.False(aabbBig.IntersectsRay(new Vector3(10, 10, 0), new Vector3(0, 1, 0)));
        Assert.True(aabbBig.IntersectsRay(new Vector3(1, 1, 1), new Vector3(0, 1, 0)));
        Assert.False(aabbBig.IntersectsRay(new Vector3(-10, 0, 0), new Vector3(-1, 0, 0)));
        Assert.True(aabbBig.IntersectsRay(new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        Assert.True(aabbBig.IntersectsRay(aabbBig.Position, new Vector3(-1, 0, 0)));
        Assert.True(aabbBig.IntersectsRay(new Vector3(-1, 3, -2), new Vector3(0, 0, 0)));
        Assert.False(aabbBig.IntersectsRay(new Vector3(-1000, 3, -2), new Vector3(0, 0, 0)));

        // Finding ray intersections.
        var aabbSimple = new Aabb(Vector3.Zero, new Vector3(1, 1, 1));
        Vector3 intersectionPoint;

        // Borders.
        aabbSimple.IntersectsRay(new Vector3(0.5f, 0, 0.5f), new Vector3(0, 1, 0), out intersectionPoint);
        Assert.Equal(new Vector3(0.5f, 0, 0.5f), intersectionPoint, ApproxEqualityComparer.Instance);
        aabbSimple.IntersectsRay(new Vector3(0.5f, 1, 0.5f), new Vector3(0, -1, 0), out intersectionPoint);
        Assert.Equal(new Vector3(0.5f, 1, 0.5f), intersectionPoint, ApproxEqualityComparer.Instance);

        // Inside.
        aabbSimple.IntersectsRay(new Vector3(0.5f, 0.1f, 0.5f), new Vector3(0, 1, 0), out intersectionPoint);
        Assert.Equal(new Vector3(0.5f, 0.1f, 0.5f), intersectionPoint, ApproxEqualityComparer.Instance);

        // Zero sized AABB.
        var aabbZero = new Aabb(Vector3.Zero, new Vector3(1, 0, 1));
        aabbZero.IntersectsRay(new Vector3(0.5f, 0, 0.5f), new Vector3(0, 1, 0), out intersectionPoint);
        Assert.Equal(new Vector3(0.5f, 0, 0.5f), intersectionPoint, ApproxEqualityComparer.Instance);
        aabbZero.IntersectsRay(new Vector3(0.5f, 0, 0.5f), new Vector3(0, -1, 0), out intersectionPoint);
        Assert.Equal(new Vector3(0.5f, 0, 0.5f), intersectionPoint, ApproxEqualityComparer.Instance);
        aabbZero.IntersectsRay(new Vector3(0.5f, -1, 0.5f), new Vector3(0, 1, 0), out intersectionPoint);
        Assert.Equal(new Vector3(0.5f, 0, 0.5f), intersectionPoint, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Merging()
    {
        var aabbBig = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));

        var aabbSmall = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(1, 1, 1));
        Assert.Equal(aabbBig, aabbBig.Merge(aabbSmall), ApproxEqualityComparer.Instance);

        aabbSmall = new Aabb(new Vector3(0.5f, 1.5f, -2), new Vector3(1, 1, 1));
        Assert.Equal(new Aabb(new Vector3(-1.5f, 1.5f, -2.5f), new Vector3(4, 5.5f, 6)), aabbBig.Merge(aabbSmall), ApproxEqualityComparer.Instance);

        aabbSmall = new Aabb(new Vector3(10, -10, -10), new Vector3(1, 1, 1));
        Assert.Equal(new Aabb(new Vector3(-1.5f, -10, -10), new Vector3(12.5f, 17, 13.5f)), aabbBig.Merge(aabbSmall), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Encloses()
    {
        var aabbBig = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));

        Assert.True(aabbBig.Encloses(aabbBig));

        var aabbSmall = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(1, 1, 1));
        Assert.True(aabbBig.Encloses(aabbSmall));

        aabbSmall = new Aabb(new Vector3(1.5f, 6, 2.5f), new Vector3(1, 1, 1));
        Assert.True(aabbBig.Encloses(aabbSmall));

        aabbSmall = new Aabb(new Vector3(0.5f, 1.5f, -2), new Vector3(1, 1, 1));
        Assert.False(aabbBig.Encloses(aabbSmall));

        aabbSmall = new Aabb(new Vector3(10, -10, -10), new Vector3(1, 1, 1));
        Assert.False(aabbBig.Encloses(aabbSmall));
    }

    [Fact]
    public void GetEndpoints()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.Equal(new Vector3(-1.5f, 2, -2.5f), aabb.GetEndpoint(0), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(-1.5f, 2, 3.5f), aabb.GetEndpoint(1), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(-1.5f, 7, -2.5f), aabb.GetEndpoint(2), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(-1.5f, 7, 3.5f), aabb.GetEndpoint(3), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(2.5f, 2, -2.5f), aabb.GetEndpoint(4), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(2.5f, 2, 3.5f), aabb.GetEndpoint(5), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(2.5f, 7, -2.5f), aabb.GetEndpoint(6), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(2.5f, 7, 3.5f), aabb.GetEndpoint(7), ApproxEqualityComparer.Instance);

        Assert.Throws<ArgumentOutOfRangeException>(() => aabb.GetEndpoint(8));
        Assert.Throws<ArgumentOutOfRangeException>(() => aabb.GetEndpoint(-1));
    }

    [Fact]
    public void GetLongestOrShortestAxis()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.Equal(new Vector3(0, 0, 1), aabb.GetLongestAxis());
        Assert.Equal(Vector3.Axis.Z, aabb.GetLongestAxisIndex());
        Assert.Equal(6, aabb.GetLongestAxisSize());

        Assert.Equal(new Vector3(1, 0, 0), aabb.GetShortestAxis());
        Assert.Equal(Vector3.Axis.X, aabb.GetShortestAxisIndex());
        Assert.Equal(4, aabb.GetShortestAxisSize());
    }

    [Fact]
    public void GetSupport()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.Equal(new Vector3(2.5f, 2, -2.5f), aabb.GetSupport(new Vector3(1, 0, 0)));
        Assert.Equal(new Vector3(2.5f, 7, 3.5f), aabb.GetSupport(new Vector3(0.5f, 1, 1)));
        Assert.Equal(new Vector3(2.5f, 7, -2.5f), aabb.GetSupport(new Vector3(0.5f, 1, -400)));
        Assert.Equal(new Vector3(-1.5f, 2, -2.5f), aabb.GetSupport(new Vector3(0, -1, 0)));
        Assert.Equal(new Vector3(-1.5f, 2, -2.5f), aabb.GetSupport(new Vector3(0, -0.1f, 0)));
        Assert.Equal(new Vector3(-1.5f, 2, -2.5f), aabb.GetSupport(new Vector3()));
    }

    [Fact]
    public void Grow()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.Equal(new Aabb(new Vector3(-1.75f, 1.75f, -2.75f), new Vector3(4.5f, 5.5f, 6.5f)), aabb.Grow(0.25f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Aabb(new Vector3(-1.25f, 2.25f, -2.25f), new Vector3(3.5f, 4.5f, 5.5f)), aabb.Grow(-0.25f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Aabb(new Vector3(8.5f, 12, 7.5f), new Vector3(-16, -15, -14)), aabb.Grow(-10), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void HasPoint()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.True(aabb.HasPoint(new Vector3(-1, 3, 0)));
        Assert.True(aabb.HasPoint(new Vector3(2, 3, 0)));
        Assert.False(aabb.HasPoint(new Vector3(-20, 0, 0)));

        Assert.True(aabb.HasPoint(new Vector3(-1.5f, 3, 0)));
        Assert.True(aabb.HasPoint(new Vector3(2.5f, 3, 0)));
        Assert.True(aabb.HasPoint(new Vector3(0, 2, 0)));
        Assert.True(aabb.HasPoint(new Vector3(0, 7, 0)));
        Assert.True(aabb.HasPoint(new Vector3(0, 3, -2.5f)));
        Assert.True(aabb.HasPoint(new Vector3(0, 3, 3.5f)));
    }

    [Fact]
    public void Expanding()
    {
        var aabb = new Aabb(new Vector3(-1.5f, 2, -2.5f), new Vector3(4, 5, 6));
        Assert.Equal(aabb, aabb.Expand(new Vector3(-1, 3, 0)), ApproxEqualityComparer.Instance);
        Assert.Equal(aabb, aabb.Expand(new Vector3(2, 3, 0)), ApproxEqualityComparer.Instance);
        Assert.Equal(aabb, aabb.Expand(new Vector3(-1.5f, 3, 0)), ApproxEqualityComparer.Instance);
        Assert.Equal(aabb, aabb.Expand(new Vector3(2.5f, 3, 0)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Aabb(new Vector3(-20, 0, -2.5f), new Vector3(22.5f, 7, 6)), aabb.Expand(new Vector3(-20, 0, 0)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        var x = new Vector3(0, 1, 2);
        var infinite = new Vector3(float.NaN, float.NaN, float.NaN);

        Assert.True(new Aabb(x, x).IsFinite());
        Assert.False(new Aabb(infinite, x).IsFinite());
        Assert.False(new Aabb(x, infinite).IsFinite());
        Assert.False(new Aabb(infinite, infinite).IsFinite());
    }
}
