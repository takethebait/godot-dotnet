using System;

namespace Godot.Bindings.Tests;

public class ColorTests
{
    [Fact]
    public void ConstructorMethods()
    {
        var blueRgba = new Color(0.25098f, 0.376471f, 1, 0.501961f);
        var blueHtml = Color.FromHtml("#4060ff80");
        var blueHex = new Color(0x4060ff80u);
        var blueHex64 = new Color(0x4040_6060_ffff_8080ul);

        Assert.Equal(blueRgba, blueHtml, ApproxEqualityComparer.Instance);
        Assert.Equal(blueRgba, blueHex, ApproxEqualityComparer.Instance);
        Assert.Equal(blueRgba, blueHex64, ApproxEqualityComparer.Instance);

        Assert.Throws<ArgumentOutOfRangeException>(() => Color.FromHtml("invalid"));

        var greenRgba = new Color(0, 1, 0, 0.25f);
        var greenHsva = Color.FromHsv(120.0f / 360.0f, 1.0f, 1.0f, 0.25f);

        Assert.Equal(greenRgba, greenHsva, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Operators()
    {
        var blue = new Color(0.2f, 0.2f, 1);
        var darkRed = new Color(0.3f, 0.1f, 0.1f);

        // Color components may be negative. Also, the alpha component may be greater than 1.0.
        Assert.Equal(new Color(0.5f, 0.3f, 1.1f, 2), blue + darkRed, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(-0.1f, 0.1f, 0.9f, 0), blue - darkRed, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.4f, 0.4f, 2, 2), blue * 2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.1f, 0.1f, 0.5f, 0.5f), blue / 2, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.06f, 0.02f, 0.1f), blue * darkRed, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.666667f, 2, 10), blue / darkRed, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.8f, 0.8f, 0, 0), -blue, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ReadingMethods()
    {
        var darkBlue = new Color(0, 0, 0.5f, 0.4f);

        Assert.Equal(240.0f / 360.0f, darkBlue.H, ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, darkBlue.S, ApproxEqualityComparer.Instance);
        Assert.Equal(0.5f, darkBlue.V, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ConversionMethods()
    {
        var cyan = new Color(0, 1, 1);
        var cyanTransparent = new Color(0, 1, 1, 0);

        Assert.Equal("00ffffff", cyan.ToHtml());
        Assert.Equal("00ffff00", cyanTransparent.ToHtml());
        Assert.Equal(0xff00ffffu, cyan.ToArgb32());
        Assert.Equal(0xffffff00u, cyan.ToAbgr32());
        Assert.Equal(0x00ffffffu, cyan.ToRgba32());
        Assert.Equal(0xffff_0000_ffff_fffful, cyan.ToArgb64());
        Assert.Equal(0xffff_ffff_ffff_0000ul, cyan.ToAbgr64());
        Assert.Equal(0x0000_ffff_ffff_fffful, cyan.ToRgba64());
    }

    [Fact]
    public void LinearSrgbConversion()
    {
        var color = new Color(0.35f, 0.5f, 0.6f, 0.7f);
        var colorLinear = color.SrgbToLinear();
        var colorSrgb = color.LinearToSrgb();

        Assert.Equal(new Color(0.100481f, 0.214041f, 0.318547f, 0.7f), colorLinear, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.62621f, 0.735357f, 0.797738f, 0.7f), colorSrgb, ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.35f, 0.5f, 0.6f, 0.7f), colorLinear.LinearToSrgb(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.35f, 0.5f, 0.6f, 0.7f), colorSrgb.SrgbToLinear(), ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(1, 1, 1, 1), new Color(1, 1, 1, 1).SrgbToLinear());
        Assert.Equal(new Color(1, 1, 1, 1), new Color(1, 1, 1, 1).LinearToSrgb());
    }

    [Fact]
    public void NamedColors()
    {
        Assert.Equal(new Color(0xFF0000FFu), Color.FromString("red", default), ApproxEqualityComparer.Instance);

        // Named colors have their names automatically normalized.
        Assert.Equal(new Color(0xF5F5F5FFu), Color.FromString("white_smoke", default), ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0x6A5ACDFFu), Color.FromString("Slate Blue", default), ApproxEqualityComparer.Instance);

        Assert.Equal(default, Color.FromString("doesn't exist", default));
    }

    [Theory]
    [InlineData("#4080ff", true)]
    [InlineData("4080ff", true)]
    [InlineData("12345", false)]
    [InlineData("#fuf", false)]
    public void ValidationMethods(string color, bool expected)
    {
        Assert.Equal(expected, Color.HtmlIsValid(color));
    }

    [Fact]
    public void ManipulationMethods()
    {
        var blue = new Color(0, 0, 1, 0.4f);

        Assert.Equal(new Color(1, 1, 0, 0.4f), blue.Inverted(), ApproxEqualityComparer.Instance);

        var purple = new Color(0.5f, 0.2f, 0.5f, 0.25f);

        Assert.Equal(new Color(0.6f, 0.36f, 0.6f, 0.25f), purple.Lightened(0.2f), ApproxEqualityComparer.Instance);
        Assert.Equal(new Color(0.4f, 0.16f, 0.4f, 0.25f), purple.Darkened(0.2f), ApproxEqualityComparer.Instance);

        var red = new Color(1, 0, 0, 0.2f);
        var yellow = new Color(1, 1, 0, 0.8f);

        Assert.Equal(new Color(1, 0.5f, 0, 0.5f), red.Lerp(yellow, 0.5f), ApproxEqualityComparer.Instance);
    }
}
