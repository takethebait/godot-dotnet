using System.Collections.Immutable;
using System.IO;
using Godot.UpgradeAssistant.Cli.Services;

namespace Godot.UpgradeAssistant.Cli.Commands;

internal sealed class AssistantCommandOptions
{
    public required FileInfo GodotProject { get; init; }

    public required FileInfo DotNetSolution { get; init; }

    public required FileInfo DotNetProject { get; init; }

    public SemVer TargetGodotVersion { get; init; }

    public bool EnableGodotDotNetPreview { get; init; }

    public FileInfo? ResultsJsonFilePath { get; init; }

    public FileInfo? SummaryFilePath { get; init; }

    public ImmutableArray<ExportEntry> GetExportEntries()
    {
        var exportEntries = ImmutableArray.CreateBuilder<ExportEntry>();

        if (ResultsJsonFilePath is not null)
        {
            exportEntries.Add(new ExportEntry()
            {
                Exporter = new JsonExporter(),
                ExportFilePath = ResultsJsonFilePath.FullName,
            });
        }

        if (SummaryFilePath is not null)
        {
            exportEntries.Add(new ExportEntry()
            {
                Exporter = new HtmlExporter(),
                ExportFilePath = SummaryFilePath.FullName,
            });
        }

        return exportEntries.DrainToImmutable();
    }
}
