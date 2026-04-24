namespace Godot.UpgradeAssistant;

/// <summary>
/// Statistics about the upgrade results.
/// </summary>
internal sealed class SummaryStats
{
    /// <summary>
    /// The total number of problems reported by the assistant.
    /// </summary>
    public int ProblemsReported { get; init; }

    /// <summary>
    /// The number of problems that remain unresolved after applying all available fixes.
    /// </summary>
    public int ProblemsUnresolved { get; init; }

    /// <summary>
    /// The number of problems that have at least one fix available.
    /// </summary>
    public int FixesAvailable { get; init; }

    /// <summary>
    /// The number of problems that were successfully resolved by applying fixes.
    /// </summary>
    public int FixesApplied { get; init; }
}
