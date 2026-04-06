namespace Godot.Bindings.Tests;

public class Rect2ITests
{
    [Fact]
    public void BasicGetters()
    {
        var rect = new Rect2I(0, 100, 1280, 720);
        Assert.Equal(new Vector2I(0, 100), rect.Position);
        Assert.Equal(new Vector2I(1280, 720), rect.Size);
        Assert.Equal(new Vector2I(1280, 820), rect.End);
        Assert.Equal(new Vector2I(640, 460), rect.GetCenter());
        Assert.Equal(new Vector2I(640, 460), new Rect2I(0, 100, 1281, 721).GetCenter());
    }

    [Fact]
    public void BasicSetters()
    {
        var rect = new Rect2I(0, 100, 1280, 720);
        rect.End = new Vector2I(4000, 4000);
        Assert.Equal(new Rect2I(0, 100, 4000, 3900), rect);

        rect = new Rect2I(0, 100, 1280, 720);
        rect.Position = new Vector2I(4000, 4000);
        Assert.Equal(new Rect2I(4000, 4000, 1280, 720), rect);

        rect = new Rect2I(0, 100, 1280, 720);
        rect.Size = new Vector2I(4000, 4000);
        Assert.Equal(new Rect2I(0, 100, 4000, 4000), rect);
    }

    [Fact]
    public void AreaGetters()
    {
        Assert.Equal(921_600, new Rect2I(0, 100, 1280, 720).Area);
        Assert.Equal(921_600, new Rect2I(0, 100, -1280, -720).Area);
        Assert.Equal(-921_600, new Rect2I(0, 100, 1280, -720).Area);
        Assert.Equal(-921_600, new Rect2I(0, 100, -1280, 720).Area);
        Assert.Equal(0, new Rect2I(0, 100, 0, 720).Area);

        Assert.True(new Rect2I(0, 100, 1280, 720).HasArea());
        Assert.False(new Rect2I(0, 100, 0, 500).HasArea());
        Assert.False(new Rect2I(0, 100, 500, 0).HasArea());
        Assert.False(new Rect2I(0, 100, 0, 0).HasArea());
    }

    [Fact]
    public void AbsoluteCoordinates()
    {
        Assert.Equal(new Rect2I(0, 100, 1280, 720), new Rect2I(0, 100, 1280, 720).Abs());
        Assert.Equal(new Rect2I(0, -100, 1280, 720), new Rect2I(0, -100, 1280, 720).Abs());
        Assert.Equal(new Rect2I(-1280, -820, 1280, 720), new Rect2I(0, -100, -1280, -720).Abs());
        Assert.Equal(new Rect2I(-1280, 100, 1280, 720), new Rect2I(0, 100, -1280, 720).Abs());
    }

    [Fact]
    public void Intersection()
    {
        Assert.Equal(new Rect2I(0, 300, 100, 100), new Rect2I(0, 100, 1280, 720).Intersection(new Rect2I(0, 300, 100, 100)));
        // The resulting Rect2I is 100 pixels high because the first Rect2I is vertically offset by 100 pixels.
        Assert.Equal(new Rect2I(1200, 700, 80, 100), new Rect2I(0, 100, 1280, 720).Intersection(new Rect2I(1200, 700, 100, 100)));
        Assert.Equal(new Rect2I(), new Rect2I(0, 100, 1280, 720).Intersection(new Rect2I(-4000, -4000, 100, 100)));
    }

    [Fact]
    public void Enclosing()
    {
        Assert.True(new Rect2I(0, 100, 1280, 720).Encloses(new Rect2I(0, 300, 100, 100)));
        Assert.False(new Rect2I(0, 100, 1280, 720).Encloses(new Rect2I(1200, 700, 100, 100)));
        Assert.False(new Rect2I(0, 100, 1280, 720).Encloses(new Rect2I(-4000, -4000, 100, 100)));
        Assert.True(new Rect2I(0, 100, 1280, 720).Encloses(new Rect2I(0, 100, 1280, 720)));
    }

    [Fact]
    public void Expanding()
    {
        Assert.Equal(new Rect2I(0, 100, 1280, 720), new Rect2I(0, 100, 1280, 720).Expand(new Vector2I(500, 600)));
        Assert.Equal(new Rect2I(0, 0, 1280, 820), new Rect2I(0, 100, 1280, 720).Expand(new Vector2I(0, 0)));
    }

    [Fact]
    public void Growing()
    {
        Assert.Equal(new Rect2I(-100, 0, 1480, 920), new Rect2I(0, 100, 1280, 720).Grow(100));
        Assert.Equal(new Rect2I(100, 200, 1080, 520), new Rect2I(0, 100, 1280, 720).Grow(-100));
        Assert.Equal(new Rect2I(4000, 4100, -6720, -7280), new Rect2I(0, 100, 1280, 720).Grow(-4000));

        Assert.Equal(new Rect2I(-100, -100, 1680, 1320), new Rect2I(0, 100, 1280, 720).GrowIndividual(100, 200, 300, 400));
        Assert.Equal(new Rect2I(100, -100, 1480, 520), new Rect2I(0, 100, 1280, 720).GrowIndividual(-100, 200, 300, -400));

        Assert.Equal(new Rect2I(0, -400, 1280, 1220), new Rect2I(0, 100, 1280, 720).GrowSide(Side.Top, 500));
        Assert.Equal(new Rect2I(0, 600, 1280, 220), new Rect2I(0, 100, 1280, 720).GrowSide(Side.Top, -500));
    }

    [Fact]
    public void HasPoint()
    {
        var rect = new Rect2I(0, 100, 1280, 720);
        Assert.True(rect.HasPoint(new Vector2I(500, 600)));
        Assert.False(rect.HasPoint(new Vector2I(0, 0)));

        Assert.True(rect.HasPoint(rect.Position));
        Assert.True(rect.HasPoint(rect.Position + new Vector2I(1, 1)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2I(1, -1)));
        Assert.False(rect.HasPoint(rect.Position + rect.Size));
        Assert.False(rect.HasPoint(rect.Position + rect.Size + new Vector2I(1, 1)));
        Assert.True(rect.HasPoint(rect.Position + rect.Size + new Vector2I(-1, -1)));
        Assert.False(rect.HasPoint(rect.Position + rect.Size + new Vector2I(-1, 1)));

        Assert.True(rect.HasPoint(rect.Position + new Vector2I(0, 10)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2I(rect.Size.X, 10)));
        Assert.True(rect.HasPoint(rect.Position + new Vector2I(10, 0)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2I(10, rect.Size.Y)));

        rect = new Rect2I(-4000, -200, 1280, 720);
        Assert.True(rect.HasPoint(rect.Position + new Vector2I(0, 10)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2I(rect.Size.X, 10)));
        Assert.True(rect.HasPoint(rect.Position + new Vector2I(10, 0)));
        Assert.False(rect.HasPoint(rect.Position + new Vector2I(10, rect.Size.Y)));
    }

    [Fact]
    public void Intersects()
    {
        Assert.True(new Rect2I(0, 100, 1280, 720).Intersects(new Rect2I(0, 300, 100, 100)));
        Assert.True(new Rect2I(0, 100, 1280, 720).Intersects(new Rect2I(1200, 700, 100, 100)));
        Assert.False(new Rect2I(0, 100, 1280, 720).Intersects(new Rect2I(-4000, -4000, 100, 100)));
        Assert.False(new Rect2I(0, 0, 2, 2).Intersects(new Rect2I(2, 2, 2, 2)));
    }

    [Fact]
    public void Merging()
    {
        Assert.Equal(new Rect2I(0, 100, 1280, 720), new Rect2I(0, 100, 1280, 720).Merge(new Rect2I(0, 300, 100, 100)));
        Assert.Equal(new Rect2I(0, 100, 1300, 720), new Rect2I(0, 100, 1280, 720).Merge(new Rect2I(1200, 700, 100, 100)));
        Assert.Equal(new Rect2I(-4000, -4000, 5280, 4820), new Rect2I(0, 100, 1280, 720).Merge(new Rect2I(-4000, -4000, 100, 100)));
    }
}
