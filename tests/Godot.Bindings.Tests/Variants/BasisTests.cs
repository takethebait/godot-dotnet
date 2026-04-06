using Xunit.Abstractions;

namespace Godot.Bindings.Tests;

public class BasisTests
{
    private readonly ITestOutputHelper _output;

    public BasisTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private void TestRotation(Vector3 degOriginalEuler, EulerOrder rotOrder)
    {
        // This test:
        // 1. Converts the rotation vector from deg to rad.
        // 2. Converts euler to basis.
        // 3. Converts the above basis back into euler.
        // 4. Converts the above euler into basis again.
        // 5. Compares the basis obtained in step 2 with the basis of step 4
        //
        // The conversion "basis to euler", done in the step 3, may be different from
        // the original euler, even if the final rotation are the same.
        // This happens because there are more ways to represents the same rotation,
        // both valid, using eulers.
        // For this reason is necessary to convert that euler back to basis and finally
        // compares it.
        //
        // In this way we can assert that both functions: basis to euler / euler to basis
        // are correct.

        // Euler to rotation.
        var originalEuler = new Vector3(
            Mathf.DegToRad(degOriginalEuler.X),
            Mathf.DegToRad(degOriginalEuler.Y),
            Mathf.DegToRad(degOriginalEuler.Z));
        var toRotation = Basis.FromEuler(originalEuler, rotOrder);

        // Euler from rotation
        var eulerFromRotation = toRotation.GetEuler(rotOrder);
        var rotationFromComputedEuler = Basis.FromEuler(eulerFromRotation, rotOrder);

        _output.WriteLine($"Rotation order: {rotOrder}");
        _output.WriteLine($"Original Euler: {degOriginalEuler}");
        _output.WriteLine($"Euler from Rotation: {new Vector3(
            Mathf.RadToDeg(eulerFromRotation.X),
            Mathf.RadToDeg(eulerFromRotation.Y),
            Mathf.RadToDeg(eulerFromRotation.Z))}");

        var res = toRotation.Inverse() * rotationFromComputedEuler;

        Assert.True((res.Column0 - new Vector3(1.0f, 0.0f, 0.0f)).Length() <= 0.001f, $"Fail due to X {res.Column0}");
        Assert.True((res.Column1 - new Vector3(0.0f, 1.0f, 0.0f)).Length() <= 0.001f, $"Fail due to Y {res.Column1}");
        Assert.True((res.Column2 - new Vector3(0.0f, 0.0f, 1.0f)).Length() <= 0.001f, $"Fail due to Z {res.Column2}");

        // Double check `toRotation` decomposing with XYZ rotation order.
        var eulerXyzFromRotation = toRotation.GetEuler(EulerOrder.Xyz);
        var rotationFromXyzComputedEuler = Basis.FromEuler(eulerXyzFromRotation, EulerOrder.Xyz);

        res = toRotation.Inverse() * rotationFromXyzComputedEuler;

        Assert.True((res.Column0 - new Vector3(1.0f, 0.0f, 0.0f)).Length() <= 0.001f, $"Double check with XYZ rot order failed, due to X {res.Column0}");
        Assert.True((res.Column1 - new Vector3(0.0f, 1.0f, 0.0f)).Length() <= 0.001f, $"Double check with XYZ rot order failed, due to Y {res.Column1}");
        Assert.True((res.Column2 - new Vector3(0.0f, 0.0f, 1.0f)).Length() <= 0.001f, $"Double check with XYZ rot order failed, due to Z {res.Column2}");
    }

    [Fact]
    public void EulerConversions()
    {
        EulerOrder[] eulerOrdersToTest =
        [
            EulerOrder.Xyz,
            EulerOrder.Xzy,
            EulerOrder.Yzx,
            EulerOrder.Yxz,
            EulerOrder.Zxy,
            EulerOrder.Zyx,
        ];

        Vector3[] vectorsToTest =
        [
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(40.0f, 40.0f, 40.0f),
            new Vector3(-40.0f, -40.0f, -40.0f),
            new Vector3(0.0f, 0.0f, -90.0f),
            new Vector3(0.0f, -90.0f, 0.0f),
            new Vector3(-90.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 90.0f),
            new Vector3(0.0f, 90.0f, 0.0f),
            new Vector3(90.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, -30.0f),
            new Vector3(0.0f, -30.0f, 0.0f),
            new Vector3(-30.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 30.0f),
            new Vector3(0.0f, 30.0f, 0.0f),
            new Vector3(30.0f, 0.0f, 0.0f),
            new Vector3(0.5f, 50.0f, 20.0f),
            new Vector3(-0.5f, -50.0f, -20.0f),
            new Vector3(0.5f, 0.0f, 90.0f),
            new Vector3(0.5f, 0.0f, -90.0f),
            new Vector3(360.0f, 360.0f, 360.0f),
            new Vector3(-360.0f, -360.0f, -360.0f),
            new Vector3(-90.0f, 60.0f, -90.0f),
            new Vector3(90.0f, 60.0f, -90.0f),
            new Vector3(90.0f, -60.0f, -90.0f),
            new Vector3(-90.0f, -60.0f, -90.0f),
            new Vector3(-90.0f, 60.0f, 90.0f),
            new Vector3(90.0f, 60.0f, 90.0f),
            new Vector3(90.0f, -60.0f, 90.0f),
            new Vector3(-90.0f, -60.0f, 90.0f),
            new Vector3(60.0f, 90.0f, -40.0f),
            new Vector3(60.0f, -90.0f, -40.0f),
            new Vector3(-60.0f, -90.0f, -40.0f),
            new Vector3(-60.0f, 90.0f, 40.0f),
            new Vector3(60.0f, 90.0f, 40.0f),
            new Vector3(60.0f, -90.0f, 40.0f),
            new Vector3(-60.0f, -90.0f, 40.0f),
            new Vector3(-90.0f, 90.0f, -90.0f),
            new Vector3(90.0f, 90.0f, -90.0f),
            new Vector3(90.0f, -90.0f, -90.0f),
            new Vector3(-90.0f, -90.0f, -90.0f),
            new Vector3(-90.0f, 90.0f, 90.0f),
            new Vector3(90.0f, 90.0f, 90.0f),
            new Vector3(90.0f, -90.0f, 90.0f),
            new Vector3(20.0f, 150.0f, 30.0f),
            new Vector3(20.0f, -150.0f, 30.0f),
            new Vector3(-120.0f, -150.0f, 30.0f),
            new Vector3(-120.0f, -150.0f, -130.0f),
            new Vector3(120.0f, -150.0f, -130.0f),
            new Vector3(120.0f, 150.0f, -130.0f),
            new Vector3(120.0f, 150.0f, 130.0f),
            new Vector3(89.9f, 0.0f, 0.0f),
            new Vector3(-89.9f, 0.0f, 0.0f),
            new Vector3(0.0f, 89.9f, 0.0f),
            new Vector3(0.0f, -89.9f, 0.0f),
            new Vector3(0.0f, 0.0f, 89.9f),
            new Vector3(0.0f, 0.0f, -89.9f),
        ];

        foreach (var eulerOrder in eulerOrdersToTest)
        {
            foreach (var vector in vectorsToTest)
            {
                TestRotation(vector, eulerOrder);
            }
        }
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        var x = new Vector3(0, 1, 2);
        var infinite = new Vector3(float.NaN, float.NaN, float.NaN);

        Assert.True(new Basis(x, x, x).IsFinite());

        Assert.False(new Basis(infinite, x, x).IsFinite());
        Assert.False(new Basis(x, infinite, x).IsFinite());
        Assert.False(new Basis(x, x, infinite).IsFinite());

        Assert.False(new Basis(infinite, infinite, x).IsFinite());
        Assert.False(new Basis(infinite, x, infinite).IsFinite());
        Assert.False(new Basis(x, infinite, infinite).IsFinite());

        Assert.False(new Basis(infinite, infinite, infinite).IsFinite());
    }

    [Fact]
    public void IsConformalChecks()
    {
        Assert.True(Basis.Identity.IsConformal(), "Identity Basis should be conformal.");

        Assert.True(Basis.FromEuler(new Vector3(1.2f, 3.4f, 5.6f)).IsConformal());

        Assert.True(Basis.FromScale(new Vector3(-1, -1, -1)).IsConformal());

        Assert.True(Basis.FromScale(new Vector3(1.2f, 1.2f, 1.2f)).IsConformal());

        Assert.True(new Basis(new Vector3(3, 4, 0), new Vector3(4, -3, 0.0f), new Vector3(0, 0, 5)).IsConformal());

        Assert.False(Basis.FromScale(new Vector3(1.2f, 3.4f, 5.6f)).IsConformal());

        Assert.False(new Basis(new Vector3(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1)).IsConformal());

        Assert.True(new Basis(0, 0, 0, 0, 0, 0, 0, 0, 0).IsConformal());
    }

    [Fact]
    public void IsOrthonormalChecks()
    {
        Assert.True(Basis.Identity.IsOrthonormal());

        Assert.True(Basis.FromEuler(new Vector3(1.2f, 3.4f, 5.6f)).IsOrthonormal());

        Assert.True(Basis.FromScale(new Vector3(-1, -1, -1)).IsOrthonormal());

        Assert.False(Basis.FromScale(new Vector3(1.2f, 3.4f, 5.6f)).IsOrthonormal());

        Assert.False(new Basis(new Vector3(3, 4, 0), new Vector3(4, -3, 0), new Vector3(0, 0, 5)).IsOrthonormal());

        Assert.False(new Basis(new Vector3(MathfExtensions.Sqrt12, MathfExtensions.Sqrt12, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1)).IsOrthonormal());

        Assert.False(new Basis(0, 0, 0, 0, 0, 0, 0, 0, 0).IsOrthonormal());
    }
}
