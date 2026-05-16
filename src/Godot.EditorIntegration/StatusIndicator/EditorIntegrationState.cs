using System.Reflection;

namespace Godot.EditorIntegration;

internal static class EditorIntegrationState
{
    /// <summary>
    /// Version of the 'Godot.EditorIntegration' assembly.
    /// </summary>
    public static string Version { get; set; } = SR.StatusIndicatorPanel_EditorIntegrationVersionLabel_Unknown;

    /// <summary>
    /// Version of the .NET SDK used by the editor integration.
    /// </summary>
    public static string DotNetSdkVersion { get; set; } = SR.StatusIndicatorPanel_DotNetSdkLabel_NotFound;

    /// <summary>
    /// Path to the .NET SDK used by the editor integration.
    /// </summary>
    public static string DotNetSdkPath { get; set; } = "";

    internal static void SetDotNetSdkInfo(string version, string path)
    {
        DotNetSdkVersion = version;
        DotNetSdkPath = path;
    }

    static EditorIntegrationState()
    {
        var assembly = typeof(DotNetEditorPlugin).Assembly;
        var assemblyInformationalVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        string? informationalVersion = assemblyInformationalVersionAttribute?.InformationalVersion;
        if (!string.IsNullOrEmpty(informationalVersion))
        {
            Version = informationalVersion;
        }
    }
}
