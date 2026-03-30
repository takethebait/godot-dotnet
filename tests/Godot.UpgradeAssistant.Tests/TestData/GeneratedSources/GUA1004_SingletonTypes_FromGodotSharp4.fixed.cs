using Godot;

public partial class MyNode : Node
{
    public void MyMethod(double delta)
    {
        var value = 0.0;

        if (Input.Singleton.IsActionPressed("move_right"))
        {
            value += delta;
        }

        if (Godot.Input.Singleton.IsActionPressed("move_left"))
        {
            value += delta;
        }

        if (global::Godot.Input.Singleton.IsActionPressed("move_up"))
        {
            value += delta;
        }

        GD.Print(value);
    }
}
