using Godot;

public partial class MyNode : Node
{
    public void MyMethod()
    {
        if ({|GUA1004:RenderingServer.HasOsFeature|}("vulkan"))
        {
            GD.Print("supported");
        }
    }
}
