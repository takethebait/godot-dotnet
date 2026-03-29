using System.Threading.Tasks;
using Godot.UpgradeAssistant.Providers;

namespace Godot.UpgradeAssistant.Tests;

using Verifier = CSharpCodeFixVerifier<GodotArrayConcatenationCodeFix, GodotArrayConcatenationAnalyzer>;

public class GodotArrayConcatenationTests
{
    [Fact]
    public static async Task UpgradeFromGodotSharp3ToGodotDotNet()
    {
        // We need to use the batch verifier to test multiple code fixes applied at once.
        // The target version renames the GodotArray type, so when testing changes to
        // this type, we also need to apply the renaming code fix to ensure the resulting
        // code compiles.
        var verifier = Verifier.MakeBatchVerifier(
            "GUA1012_GodotArrayConcatenation_FromGodotSharp3.cs",
            "GUA1012_GodotArrayConcatenation_FromGodotSharp3.fixed.cs",
            GodotReferenceAssemblies.GodotSharp3,
            GodotReferenceAssemblies.GodotDotNet);

        // The renames must be applied after the concatenation fix, otherwise the concatenation fix
        // won't be able to find the add operator on the renamed type.
        verifier.AddAnalyzerAfter<ApiMapAnalyzer>();
        verifier.AddCodeFixProviderAfter<ApiMapCodeFix>();
        verifier.SetAnalyzerConfigText("""
            is_global = true
            build_property.IsGodotDotNetEnabled = true
            build_property.TargetGodotVersion = 4.5.0
            """);

        await verifier.RunAsync();
    }

    [Fact]
    public static async Task UpgradeAlreadyLatest()
    {
        await Verifier.Verify(
            "GUA1012_GodotArrayConcatenation_FromGodotDotNet.cs",
            "GUA1012_GodotArrayConcatenation_FromGodotDotNet.fixed.cs",
            GodotReferenceAssemblies.GodotDotNet,
            GodotReferenceAssemblies.GodotDotNet);
    }
}
