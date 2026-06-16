using System.CodeDom.Compiler;
using System.Diagnostics;
using Godot.BindingsGeneration.Reflection;

namespace Godot.BindingsGeneration;

internal sealed class ConvertToVariantTakingOwnership : MethodBody
{
    private readonly string _targetTypeName;

    private readonly bool _isTypePackedArray;

    private readonly bool _isTypeAPointerInVariant;

    public override bool RequiresUnsafeCode => true;

    public ConvertToVariantTakingOwnership(string targetTypeName, bool isTypePackedArray, bool isTypeAPointerInVariant)
    {
        _targetTypeName = targetTypeName;
        _isTypePackedArray = isTypePackedArray;
        _isTypeAPointerInVariant = isTypeAPointerInVariant;
    }

    public override void Write(MethodBase owner, IndentedTextWriter writer)
    {
        TypeInfo? returnType = owner is MethodInfo method ? method.ReturnType : null;

        Debug.Assert(returnType is not null && returnType.IsValueType, "Variant's ConvertTo methods must return a struct type.");

        // If the Variant is already the requested type, use the existing value.
        // Packed arrays aren't available from the Variant interop struct,
        // so a new instance is always created.
        if (!_isTypePackedArray)
        {
            writer.WriteLine("// Avoid the interop call if the Variant is already the type we want.");
            writer.WriteLine($"if (value.Type == global::Godot.VariantType.{_targetTypeName})");
            writer.OpenBlock();
            if (returnType == KnownTypes.SystemIntPtr)
            {
                // If the type is Object, we also need to check if the Object ID is zero, which means it's a null reference.
                writer.WriteLine("if (value.ObjectId == 0)");
                writer.OpenBlock();
                writer.WriteLine("// If the Object ID is zero, we can't retrieve it. It has likely been freed already.");
                writer.WriteLine("// It's effectively a dangling pointer, so we just return null.");
                writer.WriteLine("return default;");
                writer.CloseBlock();

                writer.WriteLine("// The Object pointer in Variant may still be a dangling pointer,");
                writer.WriteLine("// use the ObjectID to retrieve a valid pointer or null if the object was freed.");
                writer.WriteLine("return (nint)global::Godot.Bridge.GodotBridge.GDExtensionInterface.object_get_instance_from_id(value.ObjectId);");
            }
            else
            {
                writer.Write("return ");
                if (_isTypeAPointerInVariant)
                {
                    writer.Write('*');
                }
                writer.WriteLine($"value.{_targetTypeName};");
            }
            writer.CloseBlock();

            if (returnType == KnownTypes.NativeGodotString)
            {
                // When converting Variant to String, we need to add a special case
                // if the Variant is null to return an empty string; otherwise, the
                // interop call will convert it to the string "Null".
                writer.WriteLine("""// Avoid converting null Variants to the string "Null".""");
                writer.WriteLine("if (value.Type == global::Godot.VariantType.Nil)");
                writer.OpenBlock();
                writer.WriteLine("return default;");
                writer.CloseBlock();
            }

            if (returnType == KnownTypes.SystemIntPtr)
            {
                // If the type check failed, the Variant is not a GodotObject
                // so it can't be converted. Return a null pointer and avoid
                // a pointless interop call.
                writer.WriteLine("return default;");
                return;
            }
        }

        writer.WriteLine($"{returnType.FullNameWithGlobal} dest = default;");
        writer.WriteLine($"_variantTo{_targetTypeName}Constructor(&dest, value.GetUnsafeAddress());");
        writer.WriteLine("return dest;");
    }
}
