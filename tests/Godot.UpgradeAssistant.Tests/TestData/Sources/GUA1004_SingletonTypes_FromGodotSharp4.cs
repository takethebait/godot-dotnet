using Godot;

public partial class MyNode : Node
{
    public void MyMethod(double delta)
    {
        var value = 0.0;

        if ({|GUA1004:Input|}.IsActionPressed("move_right"))
        {
            value += delta;
        }

        if (Godot.{|GUA1004:Input|}.IsActionPressed("move_left"))
        {
            value += delta;
        }

        if (global::Godot.{|GUA1004:Input|}.IsActionPressed("move_up"))
        {
            value += delta;
        }

        GD.Print(value);
    }
}
