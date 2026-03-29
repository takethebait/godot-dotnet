using Godot;
using Godot.Collections;

public partial class MyNode : Node
{
    public {|GUA1004:Array<Node>|} ConcatArrays({|GUA1004:Array<Node>|} left, {|GUA1004:Array<Node>|} right)
    {
        return {|GUA1012:left + right|};
    }

    public void AssignConcat({|GUA1004:Array<Node>|} left, {|GUA1004:Array<Node>|} right)
    {
        {|GUA1004:Array<Node>|} array = {|GUA1012:left + right|};
        _ = array;
    }

    public void AssignConcat({|GUA1004:Array<Node>|} a, {|GUA1004:Array<Node>|} b, {|GUA1004:Array<Node>|} c)
    {
        {|GUA1004:Array<Node>|} array = {|GUA1012:a + b + c|};
        _ = array;
    }

    public void AssignConcat({|GUA1004:Array<Node>|} a, {|GUA1004:Array<Node>|} b, {|GUA1004:Array<Node>|} c, {|GUA1004:Array<Node>|} d)
    {
        {|GUA1004:Array<Node>|} array = {|GUA1012:a + b + c + d|};
        _ = array;
    }

    public void AssignToVarConcat({|GUA1004:Array<Node>|} left, {|GUA1004:Array<Node>|} right)
    {
        var array = {|GUA1012:left + right|};
        _ = array;
    }

    public void AssignToVarConcat({|GUA1004:Array|} left, {|GUA1004:Array|} right)
    {
        var array = {|GUA1012:left + right|};
        _ = array;
    }
}
