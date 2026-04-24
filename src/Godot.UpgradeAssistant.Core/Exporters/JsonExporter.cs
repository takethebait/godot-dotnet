using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Godot.UpgradeAssistant;

/// <summary>
/// Exporter implementation that writes the tool results to a JSON file.
/// </summary>
public sealed partial class JsonExporter : IExporter
{
    /// <inheritdoc/>
    public async Task ExportAsync(Summary summary, string outputPath, CancellationToken cancellationToken = default)
    {
        var stats = new SummaryStats()
        {
            ProblemsReported = summary.Problems.Count,
            ProblemsUnresolved = summary.Problems.Count(p => !p.HasFixApplied),
            FixesAvailable = summary.Problems.Count(p => p.HasFixAvailable),
            FixesApplied = summary.Problems.Count(p => p.HasFixApplied),
        };

        using var stream = File.Create(outputPath);
        await JsonSerializer.SerializeAsync(stream, stats, SummaryStatsJsonContext.Default.SummaryStats, cancellationToken).ConfigureAwait(false);
    }

    [JsonSerializable(typeof(SummaryStats))]
    private sealed partial class SummaryStatsJsonContext : JsonSerializerContext { }
}
