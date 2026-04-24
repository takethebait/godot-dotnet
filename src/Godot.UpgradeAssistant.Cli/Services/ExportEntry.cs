namespace Godot.UpgradeAssistant.Cli.Services;

/// <summary>
/// Describes export configuration for the upgrade assistant results.
/// </summary>
public sealed class ExportEntry
{
    /// <summary>
    /// Exporter that will write the results summary.
    /// </summary>
    public required IExporter Exporter { get; init; }

    /// <summary>
    /// The path to the exported results summary.
    /// If <see langword="null"/> the export step is skipped.
    /// </summary>
    public string? ExportFilePath { get; init; }
}
