using System.Threading.Tasks;
using Godot.UpgradeAssistant.Providers;

namespace Godot.UpgradeAssistant.Tests;

using Verifier = CSharpCodeFixVerifier<ApiMapCodeFix, ApiMapAnalyzer>;

public class ApiMapTests
{
    [Fact]
    public static async Task GenericCollectionTypesUpgradeFromGodotSharp4ToGodotDotNet()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1004_GenericCollectionTypes_FromGodotSharp4.cs",
            "GUA1004_GenericCollectionTypes_FromGodotSharp4.fixed.cs",
            GodotReferenceAssemblies.GodotSharp4,
            GodotReferenceAssemblies.GodotDotNet);

        verifier.SetAnalyzerConfigText("""
            is_global = true
            build_property.IsGodotDotNetEnabled = true
            build_property.TargetGodotVersion = 4.5.0
            """);

        await verifier.RunAsync();
    }

    [Fact]
    public static async Task NonGenericCollectionTypesUpgradeFromGodotSharp4ToGodotDotNet()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1004_NonGenericCollectionTypes_FromGodotSharp4.cs",
            "GUA1004_NonGenericCollectionTypes_FromGodotSharp4.fixed.cs",
            GodotReferenceAssemblies.GodotSharp4,
            GodotReferenceAssemblies.GodotDotNet);

        verifier.SetAnalyzerConfigText("""
            is_global = true
            build_property.IsGodotDotNetEnabled = true
            build_property.TargetGodotVersion = 4.5.0
            """);

        await verifier.RunAsync();
    }

    [Fact]
    public static async Task SingletonTypesUpgradeFromGodotSharp4ToGodotDotNet()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1004_SingletonTypes_FromGodotSharp4.cs",
            "GUA1004_SingletonTypes_FromGodotSharp4.fixed.cs",
            GodotReferenceAssemblies.GodotSharp4,
            GodotReferenceAssemblies.GodotDotNet);

        verifier.SetAnalyzerConfigText("""
            is_global = true
            build_property.IsGodotDotNetEnabled = true
            build_property.TargetGodotVersion = 4.5.0
            """);

        await verifier.RunAsync();
    }

    [Fact]
    public static async Task SingletonMethodRenameUpgradeFromGodotSharp4ToGodotDotNet()
    {
        var verifier = Verifier.MakeVerifier(
            "GUA1004_SingletonMethodRename_FromGodotSharp4.cs",
            "GUA1004_SingletonMethodRename_FromGodotSharp4.fixed.cs",
            GodotReferenceAssemblies.GodotSharp4,
            GodotReferenceAssemblies.GodotDotNet);

        verifier.SetAnalyzerConfigText("""
            is_global = true
            build_property.IsGodotDotNetEnabled = true
            build_property.TargetGodotVersion = 4.5.0
            """);

        await verifier.RunAsync();
    }
}
