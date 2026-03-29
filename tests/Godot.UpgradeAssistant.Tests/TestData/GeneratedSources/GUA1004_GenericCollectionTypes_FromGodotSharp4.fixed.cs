using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public void UseGenericCollections()
    {
        var typedArray = new GodotArray<int>();
        typedArray.Add(1);
        typedArray.Add(2);

        var typedDict = new GodotDictionary<string, int>();
        typedDict["a"] = 1;

        var fullyQualifiedArray = new Godot.Collections.GodotArray<Node>();
        var fullyQualifiedDict = new Godot.Collections.GodotDictionary<string, Node>();
    }
}
