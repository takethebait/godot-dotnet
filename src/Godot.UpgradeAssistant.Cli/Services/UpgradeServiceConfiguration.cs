using System.Collections.Immutable;

namespace Godot.UpgradeAssistant.Cli.Services;

internal sealed class UpgradeServiceConfiguration
{
    /// <summary>
    /// Path to the 'project.godot' file.
    /// </summary>
    public required string GodotProjectFilePath { get; init; }

    /// <summary>
    /// Path to the .NET solution file.
    /// </summary>
    public required string DotNetSolutionFilePath { get; init; }

    /// <summary>
    /// Path to the .NET project file.
    /// </summary>
    public required string DotNetProjectFilePath { get; init; }

    /// <summary>
    /// Target Godot version to upgrade to.
    /// </summary>
    public required SemVer TargetGodotVersion { get; init; }

    /// <summary>
    /// Indicates whether the Godot .NET packages should be used instead of GodotSharp
    /// for a target Godot version that supports both.
    /// </summary>
    public bool EnableGodotDotNetPreview { get; init; }

    /// <summary>
    /// Export configuration for the upgrade assistant results.
    /// Each entry contains the exporter that will write the results and the path that the file will be written to.
    /// If the path is null or empty, the export step will be skipped for that entry.
    /// </summary>
    public ImmutableArray<ExportEntry> ExportEntries { get; init; } = [];

    /// <summary>
    /// When enabled only an analysis is performed, skipping the upgrade step.
    /// </summary>
    public bool IsDryRun { get; init; }
}
