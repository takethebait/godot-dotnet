namespace Godot.Bindings.Tests;

public class Rect2Tests
{
    [Fact]
    public void BasicGetters()
    {
        var rect = new Rect2(0, 100, 1280, 720);
        Assert.Equal(new Vector2(0, 100), rect.Position, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1280, 720), rect.Size, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(1280, 820), rect.End, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(640, 460), rect.GetCenter(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector2(640.5f, 460.5f), new Rect2(0, 100, 1281, 721).GetCenter(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void BasicSetters()
    {
        var rect = new Rect2(0, 100, 1280, 720);
        rect.End = new Vector2(4000, 4000);
        Assert.Equal(new Rect2(0, 100, 4000, 3900), rect, ApproxEqualityComparer.Instance);

        rect = new Rect2(0, 100, 1280, 720);
        rect.Position = new Vector2(4000, 4000);
        Assert.Equal(new Rect2(4000, 4000, 1280, 720), rect, ApproxEqualityComparer.Instance);

        rect = new Rect2(0, 100, 1280, 720);
        rect.Size = new Vector2(4000, 4000);
        Assert.Equal(new Rect2(0, 100, 4000, 4000), rect, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void AreaGetters()
    {
        Assert.Equal(921_600f, new Rect2(0, 100, 1280, 720).Area, ApproxEqualityComparer.Instance);
        Assert.Equal(921_600f, new Rect2(0, 100, -1280, -720).Area, ApproxEqualityComparer.Instance);
        Assert.Equal(-921_600f, new Rect2(0, 100, 1280, -720).Area, ApproxEqualityComparer.Instance);
        Assert.Equal(-921_600f, new Rect2(0, 100, -1280, 720).Area, ApproxEqualityComparer.Instance);
        Assert.Equal(0, new Rect2(0, 100, 0, 720).Area, ApproxEqualityComparer.Instance);

        Assert.True(new Rect2(0, 100, 1280, 720).HasArea());
        Assert.False(new Rect2(0, 100, 0, 500).HasArea());
        Assert.False(new Rect2(0, 100, 500, 0).HasArea());
        Assert.False(new Rect2(0, 100, 0, 0).HasArea());
    }

    [Fact]
    public void AbsoluteCoordinates()
    {
        Assert.Equal(new Rect2(0, 100, 1280, 720), new Rect2(0, 100, 1280, 720).Abs(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(0, -100, 1280, 720), new Rect2(0, -100, 1280, 720).Abs(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(-1280, -820, 1280, 720), new Rect2(0, -100, -1280, -720).Abs(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(-1280, 100, 1280, 720), new Rect2(0, 100, -1280, 720).Abs(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Intersection()
    {
        Assert.Equal(new Rect2(0, 300, 100, 100), new Rect2(0, 100, 1280, 720).Intersection(new Rect2(0, 300, 100, 100)), ApproxEqualityComparer.Instance);
        // The resulting Rect2 is 100 pixels high because the first Rect2 is vertically offset by 100 pixels.
        Assert.Equal(new Rect2(1200, 700, 80, 100), new Rect2(0, 100, 1280, 720).Intersection(new Rect2(1200, 700, 100, 100)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(), new Rect2(0, 100, 1280, 720).Intersection(new Rect2(-4000, -4000, 100, 100)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Enclosing()
    {
        Assert.True(new Rect2(0, 100, 1280, 720).Encloses(new Rect2(0, 300, 100, 100)));
        Assert.False(new Rect2(0, 100, 1280, 720).Encloses(new Rect2(1200, 700, 100, 100)));
        Assert.False(new Rect2(0, 100, 1280, 720).Encloses(new Rect2(-4000, -4000, 100, 100)));
    }

    [Fact]
    public void Expanding()
    {
        Assert.Equal(new Rect2(0, 100, 1280, 720), new Rect2(0, 100, 1280, 720).Expand(new Vector2(500, 600)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(0, 0, 1280, 820), new Rect2(0, 100, 1280, 720).Expand(new Vector2(0, 0)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void GetSupport()
    {
        var rect = new Rect2(new Vector2(-1.5f, 2), new Vector2(4, 5));
        Assert.Equal(new Vector2(2.5f, 2), rect.GetSupport(new Vector2(1, 0)));
        Assert.Equal(new Vector2(2.5f, 7), rect.GetSupport(new Vector2(0.5f, 1)));
        Assert.Equal(new Vector2(2.5f, 7), rect.GetSupport(new Vector2(0.5f, 1)));
        Assert.Equal(new Vector2(-1.5f, 2), rect.GetSupport(new Vector2(0, -1)));
        Assert.Equal(new Vector2(-1.5f, 2), rect.GetSupport(new Vector2(0, -0.1f)));
        Assert.Equal(new Vector2(-1.5f, 2), rect.GetSupport(new Vector2()));
    }

    [Fact]
    public void Growing()
    {
        Assert.Equal(new Rect2(-100, 0, 1480, 920), new Rect2(0, 100, 1280, 720).Grow(100), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(100, 200, 1080, 520), new Rect2(0, 100, 1280, 720).Grow(-100), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(4000, 4100, -6720, -7280), new Rect2(0, 100, 1280, 720).Grow(-4000), ApproxEqualityComparer.Instance);

        Assert.Equal(new Rect2(-100, -100, 1680, 1320), new Rect2(0, 100, 1280, 720).GrowIndividual(100, 200, 300, 400), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(100, -100, 1480, 520), new Rect2(0, 100, 1280, 720).GrowIndividual(-100, 200, 300, -400), ApproxEqualityComparer.Instance);

        Assert.Equal(new Rect2(0, -400, 1280, 1220), new Rect2(0, 100, 1280, 720).GrowSide(Side.Top, 500), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(0, 600, 1280, 220), new Rect2(0, 100, 1280, 720).GrowSide(Side.Top, -500), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void HasPoint()
    {
        var rect = new Rect2(0, 100, 1280, 720);
        Assert.True(rect.HasPoint(new Vector2(500, 600)));
        Assert.False(rect.HasPoint(new Vector2(0, 0)));

        Assert.True(rect.HasPoint(rect.Position));
        Assert.True(rect.HasPoint(rect.Position + new Vector2(1, 1)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2(1, -1)));
        Assert.False(rect.HasPoint(rect.Position + rect.Size));
        Assert.False(rect.HasPoint(rect.Position + rect.Size + new Vector2(1, 1)));
        Assert.True(rect.HasPoint(rect.Position + rect.Size + new Vector2(-1, -1)));
        Assert.False(rect.HasPoint(rect.Position + rect.Size + new Vector2(-1, 1)));

        Assert.True(rect.HasPoint(rect.Position + new Vector2(0, 10)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2(rect.Size.X, 10)));
        Assert.True(rect.HasPoint(rect.Position + new Vector2(10, 0)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2(10, rect.Size.Y)));

        rect = new Rect2(-4000, -200, 1280, 720);
        Assert.True(rect.HasPoint(rect.Position + new Vector2(0, 10)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2(rect.Size.X, 10)));
        Assert.True(rect.HasPoint(rect.Position + new Vector2(10, 0)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2(10, rect.Size.Y)));
    }

    [Fact]
    public void Intersects()
    {
        Assert.True(new Rect2(0, 100, 1280, 720).Intersects(new Rect2(0, 300, 100, 100)));
        Assert.True(new Rect2(0, 100, 1280, 720).Intersects(new Rect2(1200, 700, 100, 100)));
        Assert.False(new Rect2(0, 100, 1280, 720).Intersects(new Rect2(-4000, -4000, 100, 100)));
    }

    [Fact]
    public void Merging()
    {
        Assert.Equal(new Rect2(0, 100, 1280, 720), new Rect2(0, 100, 1280, 720).Merge(new Rect2(0, 300, 100, 100)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(0, 100, 1300, 720), new Rect2(0, 100, 1280, 720).Merge(new Rect2(1200, 700, 100, 100)), ApproxEqualityComparer.Instance);
        Assert.Equal(new Rect2(-4000, -4000, 5280, 4820), new Rect2(0, 100, 1280, 720).Merge(new Rect2(-4000, -4000, 100, 100)), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        var x = new Vector2(0, 1);
        var infinite = new Vector2(float.NaN, float.NaN);

        Assert.True(new Rect2(x, x).IsFinite());
        Assert.False(new Rect2(infinite, x).IsFinite());
        Assert.False(new Rect2(x, infinite).IsFinite());
        Assert.False(new Rect2(infinite, infinite).IsFinite());
    }
}
