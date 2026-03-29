using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Operations;

namespace Godot.UpgradeAssistant.Providers;

[RequiresGodotDotNet]
[DiagnosticAnalyzer(LanguageNames.CSharp)]
internal sealed class GodotArrayConcatenationAnalyzer : DiagnosticAnalyzer
{
    private static DiagnosticDescriptor Rule =>
        Descriptors.GUA1012_GodotArrayConcatenation;

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
        [Rule];

    public override void Initialize(DiagnosticAnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeBinaryExpression, SyntaxKind.AddExpression);
    }

    private static void AnalyzeBinaryExpression(SyntaxNodeAnalysisContext context)
    {
        var binaryExpression = (BinaryExpressionSyntax)context.Node;

        var semanticModel = context.SemanticModel;

        if (!IsGodotArrayAddExpression(binaryExpression, semanticModel))
        {
            return;
        }

        // Only report the outermost binary expression in a chain.
        // If the parent is also a matching GodotArray add expression, skip this one.
        if (binaryExpression.Parent is BinaryExpressionSyntax parentExpression
         && IsGodotArrayAddExpression(parentExpression, semanticModel))
        {
            return;
        }

        context.ReportDiagnostic(Diagnostic.Create(
            descriptor: Rule,
            location: binaryExpression.GetLocation()));
    }

    private static bool IsGodotArrayAddExpression(BinaryExpressionSyntax binaryExpression, SemanticModel semanticModel)
    {
        if (!binaryExpression.IsKind(SyntaxKind.AddExpression))
        {
            return false;
        }

        if (semanticModel.GetOperation(binaryExpression) is not IBinaryOperation binaryOperation)
        {
            // Unable to get the binary operation.
            return false;
        }

        var operatorMethod = binaryOperation.OperatorMethod;
        if (operatorMethod is null)
        {
            // Not a user-defined operator.
            return false;
        }

        return IsGodotArrayAddOperator(operatorMethod);
    }

    private static bool IsGodotArrayAddOperator(IMethodSymbol operatorMethod)
    {
        if (!operatorMethod.DeclaredInGodotSharp())
        {
            return false;
        }

        var containingType = operatorMethod.ContainingType;
        return IsGodotArrayType(containingType);
    }

    private static bool IsGodotArrayType([NotNullWhen(true)] ITypeSymbol? typeSymbol)
    {
        if (typeSymbol is null)
        {
            return false;
        }

        return typeSymbol.EqualsType("Godot.Collections.Array", "GodotSharp")
            || typeSymbol.EqualsGenericType("Godot.Collections.Array<T>", "GodotSharp");
    }
}
