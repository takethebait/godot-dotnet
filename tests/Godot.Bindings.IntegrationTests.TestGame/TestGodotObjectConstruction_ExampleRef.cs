namespace Godot.Bindings.IntegrationTests.TestGame;


[GodotClass]
public partial class TestGodotObjectConstruction_ExampleRef : RefCounted
{
    private bool _wasPostInitialized;

    protected internal override void _Notification(int what)
    {
        if (what == NotificationPostInitialize)
        {
            _wasPostInitialized = true;
        }
    }

    [BindMethod(Name = "was_post_initialized")]
    public bool WasPostInitialized()
    {
        return _wasPostInitialized;
    }
}
