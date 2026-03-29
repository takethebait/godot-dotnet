using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public GodotArray<Node> ConcatArrays(GodotArray<Node> left, GodotArray<Node> right)
    {
        return [.. left, .. right];
    }

    public void AssignConcat(GodotArray<Node> left, GodotArray<Node> right)
    {
        GodotArray<Node> array = [.. left, .. right];
        _ = array;
    }

    public void AssignConcat(GodotArray<Node> a, GodotArray<Node> b, GodotArray<Node> c)
    {
        GodotArray<Node> array = [.. a, .. b, .. c];
        _ = array;
    }

    public void AssignConcat(GodotArray<Node> a, GodotArray<Node> b, GodotArray<Node> c, GodotArray<Node> d)
    {
        GodotArray<Node> array = [.. a, .. b, .. c, .. d];
        _ = array;
    }

    public void AssignToVarConcat(GodotArray<Node> left, GodotArray<Node> right)
    {
        var array = (GodotArray<Node>)[.. left, .. right];
        _ = array;
    }

    public void AssignToVarConcat(GodotArray left, GodotArray right)
    {
        var array = (GodotArray)([.. left, .. right]);
        _ = array;
    }
}
