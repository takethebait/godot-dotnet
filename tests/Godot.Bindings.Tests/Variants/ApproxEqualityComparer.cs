using System.Collections.Generic;

namespace Godot.Bindings.Tests;

internal sealed class ApproxEqualityComparer :
    IEqualityComparer<Aabb>,
    IEqualityComparer<Basis>,
    IEqualityComparer<Color>,
    IEqualityComparer<Plane>,
    IEqualityComparer<Quaternion>,
    IEqualityComparer<Rect2>,
    IEqualityComparer<Transform2D>,
    IEqualityComparer<Transform3D>,
    IEqualityComparer<Vector2>,
    IEqualityComparer<Vector3>,
    IEqualityComparer<Vector4>,
    IEqualityComparer<float>,
    IEqualityComparer<double>
{
    public static ApproxEqualityComparer Instance { get; } = new ApproxEqualityComparer();

    // AABB
    public bool Equals(Aabb x, Aabb y) => x.IsEqualApprox(y);
    public int GetHashCode(Aabb obj) => obj.GetHashCode();

    // Basis
    public bool Equals(Basis x, Basis y) => x.IsEqualApprox(y);
    public int GetHashCode(Basis obj) => obj.GetHashCode();

    // Color
    public bool Equals(Color x, Color y) => x.IsEqualApprox(y);
    public int GetHashCode(Color obj) => obj.GetHashCode();

    // Plane
    public bool Equals(Plane x, Plane y) => x.IsEqualApprox(y);
    public int GetHashCode(Plane obj) => obj.GetHashCode();

    // Quaternion
    public bool Equals(Quaternion x, Quaternion y) => x.IsEqualApprox(y);
    public int GetHashCode(Quaternion obj) => obj.GetHashCode();

    // Rect2
    public bool Equals(Rect2 x, Rect2 y) => x.IsEqualApprox(y);
    public int GetHashCode(Rect2 obj) => obj.GetHashCode();

    // Transform2D
    public bool Equals(Transform2D x, Transform2D y) => x.IsEqualApprox(y);
    public int GetHashCode(Transform2D obj) => obj.GetHashCode();

    // Transform3D
    public bool Equals(Transform3D x, Transform3D y) => x.IsEqualApprox(y);
    public int GetHashCode(Transform3D obj) => obj.GetHashCode();

    // Vector2
    public bool Equals(Vector2 x, Vector2 y) => x.IsEqualApprox(y);
    public int GetHashCode(Vector2 obj) => obj.GetHashCode();

    // Vector3
    public bool Equals(Vector3 x, Vector3 y) => x.IsEqualApprox(y);
    public int GetHashCode(Vector3 obj) => obj.GetHashCode();

    // Vector4
    public bool Equals(Vector4 x, Vector4 y) => x.IsEqualApprox(y);
    public int GetHashCode(Vector4 obj) => obj.GetHashCode();

    // float
    public bool Equals(float x, float y) => Mathf.IsEqualApprox(x, y);
    public int GetHashCode(float obj) => obj.GetHashCode();

    // double
    public bool Equals(double x, double y) => Mathf.IsEqualApprox(x, y);
    public int GetHashCode(double obj) => obj.GetHashCode();
}
