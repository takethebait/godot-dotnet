using System;

namespace Godot.Bindings.Tests;

public class QuaternionTests
{
    private static Quaternion QuatEulerYxzDeg(Vector3 angleDeg)
    {
        float yaw = Mathf.DegToRad(angleDeg[1]);
        float pitch = Mathf.DegToRad(angleDeg[0]);
        float roll = Mathf.DegToRad(angleDeg[2]);

        // Generate YXZ (Z-then-X-then-Y) Quaternion using single-axis Euler
        // constructor and quaternion product, both tested separately.
        var qY = Quaternion.FromEuler(new Vector3(0.0f, yaw, 0.0f));
        var qP = Quaternion.FromEuler(new Vector3(pitch, 0.0f, 0.0f));
        var qR = Quaternion.FromEuler(new Vector3(0.0f, 0.0f, roll));
        // Roll-Z is followed by Pitch-X, then Yaw-Y.
        Quaternion qYxz = qY * qP * qR;

        return qYxz;
    }

    [Fact]
    public void ConstructAxisAngle1()
    {
        // Easy to visualize: 120 deg about X-axis.
        var q = new Quaternion(new Vector3(1.0f, 0.0f, 0.0f), Mathf.DegToRad(120.0f));

        Assert.Equal(0.866025f, q.X, ApproxEqualityComparer.Instance); // Sine of half the angle.
        Assert.Equal(0.0f, q.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, q.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.5f, q.W, ApproxEqualityComparer.Instance); // Cosine of half the angle.
    }

    [Fact]
    public void ConstructAxisAngle2()
    {
        // Easy to visualize: 30 deg about Y-axis.
        var q = new Quaternion(new Vector3(0.0f, 1.0f, 0.0f), Mathf.DegToRad(30.0f));

        Assert.Equal(0.0f, q.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.258819f, q.Y, ApproxEqualityComparer.Instance); // Sine of half the angle.
        Assert.Equal(0.0f, q.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.965926f, q.W, ApproxEqualityComparer.Instance); // Cosine of half the angle.
    }

    [Fact]
    public void ConstructAxisAngle3()
    {
        // Easy to visualize: 60 deg about Z-axis.
        var q = new Quaternion(new Vector3(0.0f, 0.0f, 1.0f), Mathf.DegToRad(60.0f));

        Assert.Equal(0.0f, q.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, q.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.5f, q.Z, ApproxEqualityComparer.Instance); // Sine of half the angle.
        Assert.Equal(0.866025f, q.W, ApproxEqualityComparer.Instance); // Cosine of half the angle.
    }

    [Fact]
    public void ConstructAxisAngle4()
    {
        // More complex & hard to visualize, so test w/ data from online calculator.
        var axis = new Vector3(1.0f, 2.0f, 0.5f);
        var q = new Quaternion(axis.Normalized(), Mathf.DegToRad(35.0f));

        Assert.Equal(0.131239f, q.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.262478f, q.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0656194f, q.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.953717f, q.W, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ConstructEulerSingleAxis()
    {
        float yaw = Mathf.DegToRad(45.0f);
        float pitch = Mathf.DegToRad(30.0f);
        float roll = Mathf.DegToRad(10.0f);

        var qY = Quaternion.FromEuler(new Vector3(0.0f, yaw, 0.0f));
        Assert.Equal(0.0f, qY.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.382684f, qY.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qY.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.923879f, qY.W, ApproxEqualityComparer.Instance);

        var qP = Quaternion.FromEuler(new Vector3(pitch, 0.0f, 0.0f));
        Assert.Equal(0.258819f, qP.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qP.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qP.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.965926f, qP.W, ApproxEqualityComparer.Instance);

        var qR = Quaternion.FromEuler(new Vector3(0.0f, 0.0f, roll));
        Assert.Equal(0.0f, qR.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qR.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0871558f, qR.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.996195f, qR.W, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ConstructEulerYxzDynamicAxes()
    {
        float yaw = Mathf.DegToRad(45.0f);
        float pitch = Mathf.DegToRad(30.0f);
        float roll = Mathf.DegToRad(10.0f);

        // Generate YXZ comparison data (Z-then-X-then-Y) using single-axis Euler
        // constructor and quaternion product, both tested separately.
        var qY = Quaternion.FromEuler(new Vector3(0.0f, yaw, 0.0f));
        var qP = Quaternion.FromEuler(new Vector3(pitch, 0.0f, 0.0f));
        var qR = Quaternion.FromEuler(new Vector3(0.0f, 0.0f, roll));

        // Intrinsically, Yaw-Y then Pitch-X then Roll-Z.
        // Extrinsically, Roll-Z is followed by Pitch-X, then Yaw-Y.
        var checkYxz = qY * qP * qR;

        var eulerYxz = new Vector3(pitch, yaw, roll);
        var q = Quaternion.FromEuler(eulerYxz);
        Assert.Equal(checkYxz[0], q[0], ApproxEqualityComparer.Instance);
        Assert.Equal(checkYxz[1], q[1], ApproxEqualityComparer.Instance);
        Assert.Equal(checkYxz[2], q[2], ApproxEqualityComparer.Instance);
        Assert.Equal(checkYxz[3], q[3], ApproxEqualityComparer.Instance);

        Assert.Equal(checkYxz, q, ApproxEqualityComparer.Instance);
        Assert.Equal(eulerYxz, q.GetEuler(), ApproxEqualityComparer.Instance);
        Assert.Equal(eulerYxz, checkYxz.GetEuler(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ConstructBasisEuler()
    {
        float yaw = Mathf.DegToRad(45.0f);
        float pitch = Mathf.DegToRad(30.0f);
        float roll = Mathf.DegToRad(10.0f);

        var eulerYxz = new Vector3(pitch, yaw, roll);
        var qYxz = Quaternion.FromEuler(eulerYxz);
        var basisAxes = Basis.FromEuler(eulerYxz);
        var q = new Quaternion(basisAxes);

        Assert.Equal(qYxz, q, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ConstructBasisAxes()
    {
        // Arbitrary Euler angles.
        var eulerYxz = new Vector3(
            Mathf.DegToRad(31.41f),
            Mathf.DegToRad(-49.16f),
            Mathf.DegToRad(12.34f));

        // Basis vectors from online calculation of rotation matrix.
        var iUnit = new Vector3(0.5545787f, 0.1823950f, 0.8118957f);
        var jUnit = new Vector3(-0.5249245f, 0.8337420f, 0.1712555f);
        var kUnit = new Vector3(-0.6456754f, -0.5211586f, 0.5581192f);

        // Quaternion from online calculation.
        var qCalc = new Quaternion(0.2016913f, -0.4245716f, 0.206033f, 0.8582598f);

        // Quaternion from local calculation.
        var qLocal = QuatEulerYxzDeg(new Vector3(31.41f, -49.16f, 12.34f));

        // Quaternion from Euler angles constructor.
        var qEuler = Quaternion.FromEuler(eulerYxz);
        Assert.Equal(qCalc, qLocal, ApproxEqualityComparer.Instance);
        Assert.Equal(qLocal, qEuler, ApproxEqualityComparer.Instance);

        // Calculate Basis and construct Quaternion.
        var basisAxes = Basis.FromEuler(eulerYxz);
        var q = new Quaternion(basisAxes);

        Assert.Equal(iUnit, basisAxes.Column0, ApproxEqualityComparer.Instance);
        Assert.Equal(jUnit, basisAxes.Column1, ApproxEqualityComparer.Instance);
        Assert.Equal(kUnit, basisAxes.Column2, ApproxEqualityComparer.Instance);

        Assert.Equal(qCalc, q, ApproxEqualityComparer.Instance);
        Assert.NotEqual(qCalc, q.Inverse(), ApproxEqualityComparer.Instance);
        Assert.Equal(qLocal, q, ApproxEqualityComparer.Instance);
        Assert.Equal(qEuler, q, ApproxEqualityComparer.Instance);
        Assert.Equal(0.2016913f, q.X, ApproxEqualityComparer.Instance);
        Assert.Equal(-0.4245716f, q.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.206033f, q.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.8582598f, q.W, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void ConstructShortestArcFor180DegreeArc()
    {
        var up = new Vector3(0, 1, 0);
        var down = new Vector3(0, -1, 0);
        var left = new Vector3(-1, 0, 0);
        var right = new Vector3(1, 0, 0);
        var forward = new Vector3(0, 0, -1);
        var back = new Vector3(0, 0, 1);

        // When we have a 180 degree rotation quaternion which was defined as
        // A to B, logically when we transform A we expect to get B.
        var leftToRight = new Quaternion(left, right);
        var rightToLeft = new Quaternion(right, left);
        Assert.Equal(right, leftToRight * left, ApproxEqualityComparer.Instance);
        Assert.Equal(left, new Quaternion(right, left) * right, ApproxEqualityComparer.Instance);
        Assert.Equal(down, new Quaternion(up, down) * up, ApproxEqualityComparer.Instance);
        Assert.Equal(up, new Quaternion(down, up) * down, ApproxEqualityComparer.Instance);
        Assert.Equal(back, new Quaternion(forward, back) * forward, ApproxEqualityComparer.Instance);
        Assert.Equal(forward, new Quaternion(back, forward) * back, ApproxEqualityComparer.Instance);

        // With (arbitrary) opposite vectors that are not axis-aligned as parameters.
        var diagonalUp = new Vector3(1.2f, 2.3f, 4.5f).Normalized();
        var diagonalDown = -diagonalUp;
        var q1 = new Quaternion(diagonalUp, diagonalDown);
        Assert.Equal(diagonalUp, q1 * diagonalDown, ApproxEqualityComparer.Instance);
        Assert.Equal(diagonalDown, q1 * diagonalUp, ApproxEqualityComparer.Instance);

        // For the consistency of the rotation direction, they should be symmetrical to the plane.
        Assert.Equal(leftToRight, rightToLeft.Inverse(), ApproxEqualityComparer.Instance);

        // If vectors are same, no rotation.
        Assert.Equal(Quaternion.Identity, new Quaternion(diagonalUp, diagonalUp), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void GetEulerOrders()
    {
        float x = Mathf.DegToRad(30.0f);
        float y = Mathf.DegToRad(45.0f);
        float z = Mathf.DegToRad(10.0f);

        var euler = new Vector3(x, y, z);

        foreach (EulerOrder order in Enum.GetValues<EulerOrder>())
        {
            var basis = Basis.FromEuler(euler, order);
            var q = new Quaternion(basis);
            var check = q.GetEuler(order);
            Assert.Equal(euler, check, ApproxEqualityComparer.Instance);
            Assert.Equal(basis.GetEuler(order), check, ApproxEqualityComparer.Instance);
        }
    }

    [Fact]
    public void ProductBook()
    {
        // Example from "Quaternions and Rotation Sequences" by Jack Kuipers, p. 108.
        var p = new Quaternion(1.0f, -2.0f, 1.0f, 3.0f);
        var q = new Quaternion(-1.0f, 2.0f, 3.0f, 2.0f);

        var pq = p * q;
        Assert.Equal(-9.0f, pq.X, ApproxEqualityComparer.Instance);
        Assert.Equal(-2.0f, pq.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(11.0f, pq.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(8.0f, pq.W, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void Product()
    {
        float yaw = Mathf.DegToRad(45.0f);
        float pitch = Mathf.DegToRad(30.0f);
        float roll = Mathf.DegToRad(10.0f);

        var qY = Quaternion.FromEuler(new Vector3(0.0f, yaw, 0.0f));
        Assert.Equal(0.0f, qY.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.382684f, qY.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qY.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.923879f, qY.W, ApproxEqualityComparer.Instance);

        var qP = Quaternion.FromEuler(new Vector3(pitch, 0.0f, 0.0f));
        Assert.Equal(0.258819f, qP.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qP.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qP.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.965926f, qP.W, ApproxEqualityComparer.Instance);

        var qR = Quaternion.FromEuler(new Vector3(0.0f, 0.0f, roll));
        Assert.Equal(0.0f, qR.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0f, qR.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(0.0871558f, qR.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.996195f, qR.W, ApproxEqualityComparer.Instance);

        // Test ZYX dynamic-axes since test data is available online.
        // Rotate first about X axis, then new Y axis, then new Z axis.
        // (Godot uses YXZ Yaw-Pitch-Roll order).
        var qYp = qY * qP;
        Assert.Equal(0.239118f, qYp.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.369644f, qYp.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(-0.099046f, qYp.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.892399f, qYp.W, ApproxEqualityComparer.Instance);

        var qRyp = qR * qYp;
        Assert.Equal(0.205991f, qRyp.X, ApproxEqualityComparer.Instance);
        Assert.Equal(0.389078f, qRyp.Y, ApproxEqualityComparer.Instance);
        Assert.Equal(-0.0208912f, qRyp.Z, ApproxEqualityComparer.Instance);
        Assert.Equal(0.897636f, qRyp.W, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void XformUnitVectors()
    {
        // Easy to visualize: 120 deg about X-axis.
        // Transform the i, j, & k unit vectors.
        var q = new Quaternion(new Vector3(1.0f, 0.0f, 0.0f), Mathf.DegToRad(120.0f));
        var iT = q * new Vector3(1.0f, 0.0f, 0.0f);
        var jT = q * new Vector3(0.0f, 1.0f, 0.0f);
        var kT = q * new Vector3(0.0f, 0.0f, 1.0f);

        Assert.Equal(new Vector3(1.0f, 0.0f, 0.0f), iT, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.0f, -0.5f, 0.866025f), jT, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.0f, -0.866025f, -0.5f), kT, ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, iT.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, jT.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, kT.LengthSquared(), ApproxEqualityComparer.Instance);

        // Easy to visualize: 30 deg about Y-axis.
        q = new Quaternion(new Vector3(0.0f, 1.0f, 0.0f), Mathf.DegToRad(30.0f));
        iT = q * new Vector3(1.0f, 0.0f, 0.0f);
        jT = q * new Vector3(0.0f, 1.0f, 0.0f);
        kT = q * new Vector3(0.0f, 0.0f, 1.0f);

        Assert.Equal(new Vector3(0.866025f, 0.0f, -0.5f), iT, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.0f, 1.0f, 0.0f), jT, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.5f, 0.0f, 0.866025f), kT, ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, iT.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, jT.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, kT.LengthSquared(), ApproxEqualityComparer.Instance);

        // Easy to visualize: 60 deg about Z-axis.
        q = new Quaternion(new Vector3(0.0f, 0.0f, 1.0f), Mathf.DegToRad(60.0f));
        iT = q * new Vector3(1.0f, 0.0f, 0.0f);
        jT = q * new Vector3(0.0f, 1.0f, 0.0f);
        kT = q * new Vector3(0.0f, 0.0f, 1.0f);

        Assert.Equal(new Vector3(0.5f, 0.866025f, 0.0f), iT, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(-0.866025f, 0.5f, 0.0f), jT, ApproxEqualityComparer.Instance);
        Assert.Equal(new Vector3(0.0f, 0.0f, 1.0f), kT, ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, iT.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, jT.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(1.0f, kT.LengthSquared(), ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void XformVector()
    {
        // Arbitrary quaternion rotates an arbitrary vector.
        var eulerYzx = new Vector3(
            Mathf.DegToRad(31.41f),
            Mathf.DegToRad(-49.16f),
            Mathf.DegToRad(12.34f));
        var basisAxes = Basis.FromEuler(eulerYzx);
        var q = new Quaternion(basisAxes);

        var vArb = new Vector3(3.0f, 4.0f, 5.0f);
        var vRot = q * vArb;
        var vCompare = basisAxes * vArb;

        Assert.Equal(vArb.LengthSquared(), vRot.LengthSquared(), ApproxEqualityComparer.Instance);
        Assert.Equal(vCompare, vRot, ApproxEqualityComparer.Instance);
    }

    [Fact]
    public void FiniteNumberChecks()
    {
        float x = float.NaN;

        Assert.True(new Quaternion(0, 1, 2, 3).IsFinite());

        Assert.False(new Quaternion(x, 1, 2, 3).IsFinite());
        Assert.False(new Quaternion(0, x, 2, 3).IsFinite());
        Assert.False(new Quaternion(0, 1, x, 3).IsFinite());
        Assert.False(new Quaternion(0, 1, 2, x).IsFinite());

        Assert.False(new Quaternion(x, x, 2, 3).IsFinite());
        Assert.False(new Quaternion(x, 1, x, 3).IsFinite());
        Assert.False(new Quaternion(x, 1, 2, x).IsFinite());
        Assert.False(new Quaternion(0, x, x, 3).IsFinite());
        Assert.False(new Quaternion(0, x, 2, x).IsFinite());
        Assert.False(new Quaternion(0, 1, x, x).IsFinite());

        Assert.False(new Quaternion(0, x, x, x).IsFinite());
        Assert.False(new Quaternion(x, 1, x, x).IsFinite());
        Assert.False(new Quaternion(x, x, 2, x).IsFinite());
        Assert.False(new Quaternion(x, x, x, 3).IsFinite());

        Assert.False(new Quaternion(x, x, x, x).IsFinite());
    }
}
