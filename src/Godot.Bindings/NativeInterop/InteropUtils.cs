using System;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;
using Godot.Bridge;

namespace Godot.NativeInterop;

internal static partial class InteropUtils
{
    private readonly struct RegisterVirtualOverrideHandler
    {
        private readonly nint _methodPtr;

        public RegisterVirtualOverrideHandler(nint methodPtr)
        {
            _methodPtr = methodPtr;
        }

        public unsafe void Invoke([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type type, ClassRegistrationContext context)
        {
            var function = (delegate* managed<Type, ClassRegistrationContext, void>)_methodPtr;
            function(type, context);
        }
    }

    private static FrozenDictionary<StringName, GDExtensionInstanceBindingCallbacks> _bindingCallbacks;

    private static FrozenDictionary<StringName, RegisterVirtualOverrideHandler> _registerVirtualOverridesHelpers;

    static InteropUtils()
    {
        EnsureHelpersInitialized();
    }

    internal static bool TryGetBindingCallbacks(StringName className, [NotNullWhen(true)] out GDExtensionInstanceBindingCallbacks bindingCallbacks)
    {
        if (_bindingCallbacks.TryGetValue(className, out bindingCallbacks))
        {
            return true;
        }

        return false;
    }

    internal static bool TryGetBindingCallbacks(NativeGodotStringName className, [NotNullWhen(true)] out GDExtensionInstanceBindingCallbacks bindingCallbacks)
    {
        var bindingCallbacksLookup = _bindingCallbacks.GetAlternateLookup<NativeGodotStringName>();
        if (bindingCallbacksLookup.TryGetValue(className, out bindingCallbacks))
        {
            return true;
        }

        return false;
    }

    internal static void RegisterVirtualOverrides([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods | DynamicallyAccessedMemberTypes.NonPublicMethods)] Type type, ClassRegistrationContext context)
    {
        // It's fine if there is no helper for this class, it may just mean there are no virtual overrides to register.
        if (_registerVirtualOverridesHelpers.TryGetValue(context.NativeClassName, out var registerVirtualOverrides))
        {
            registerVirtualOverrides.Invoke(type, context);
        }
    }
}
