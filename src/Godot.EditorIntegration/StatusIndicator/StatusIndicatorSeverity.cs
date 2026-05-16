namespace Godot.EditorIntegration;

// Must be kept in sync with the C++ 'DotNetStatusIndicator::Severity' enum in 'dotnet_status_indicator.h'.
internal enum StatusIndicatorSeverity
{
    None,
    Loading,
    Warning,
    Error,
}
