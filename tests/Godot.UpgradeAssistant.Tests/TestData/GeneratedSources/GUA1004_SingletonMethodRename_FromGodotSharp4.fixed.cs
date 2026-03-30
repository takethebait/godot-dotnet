using Godot;

public partial class MyNode : Node
{
    public void MyMethod()
    {
        if (RenderingServer.Singleton.HasOSFeature("vulkan"))
        {
            GD.Print("supported");
        }
    }
}
