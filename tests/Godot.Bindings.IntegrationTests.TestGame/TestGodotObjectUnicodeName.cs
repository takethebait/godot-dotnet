namespace Godot.Bindings.IntegrationTests.TestGame;

[GodotClass]
public partial class TestGodotObjectUnicodeNamePrzykład : RefCounted
{
    [BindMethod(Name = "get_the_word")]
    public string GetTheWord()
    {
        return "słowo to przykład";
    }
}
