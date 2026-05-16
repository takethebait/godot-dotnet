namespace Godot.EditorIntegration;

// Must be kept in sync with the C++ 'InitState' enum in 'dotnet_module_state.h'.
internal enum InitState
{
    Uninitialized,
    Initializing,
    Initialized,
    Failed,
}

// Must be kept in sync with the C++ 'AssemblyLoadFailedState' enum in 'dotnet_module_state.h'.
internal enum AssemblyLoadFailedState
{
    None,
    ProjectNotFound,
    DllNotFound,
    FailedToLoad,
}

// This enum merges the 'InitState' and 'AssemblyLoadFailState' enums from the C++ side,
// to represent all possible states in a single value.
internal enum AssemblyLoadState
{
    NotLoaded,
    Loading,
    Loaded,
    ProjectNotFound,
    DllNotFound,
    FailedToLoad,
}
