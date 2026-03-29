using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public void UseGenericCollections()
    {
        var typedArray = new {|GUA1004:Array|}();
        typedArray.Add(1);
        typedArray.Add(2);

        var typedDict = new {|GUA1004:Dictionary|}();
        typedDict["a"] = 1;

        var fullyQualifiedArray = new Godot.Collections.{|GUA1004:Array|}();
        var fullyQualifiedDict = new Godot.Collections.{|GUA1004:Dictionary|}();
    }
}
