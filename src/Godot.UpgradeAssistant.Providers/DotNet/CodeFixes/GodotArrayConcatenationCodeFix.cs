using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Godot.Common.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace Godot.UpgradeAssistant.Providers;

[ExportCodeFixProvider(LanguageNames.CSharp)]
internal sealed class GodotArrayConcatenationCodeFix : CodeFixProvider
{
    private static DiagnosticDescriptor Rule =>
        Descriptors.GUA1012_GodotArrayConcatenation;

    public override ImmutableArray<string> FixableDiagnosticIds =>
        [Rule.Id];

    public override FixAllProvider? GetFixAllProvider() =>
        WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

        var diagnostic = context.Diagnostics.First();
        var diagnosticSpan = diagnostic.Location.SourceSpan;

        var binaryExpression = root?.FindNode(diagnosticSpan) as BinaryExpressionSyntax;
        if (binaryExpression is null)
        {
            // Can't apply the code fix without a syntax node.
            return;
        }

        var codeAction = CodeAction.Create(
            title: SR.GUA1012_GodotArrayConcatenation_CodeFix,
            equivalenceKey: nameof(GodotArrayConcatenationCodeFix),
            createChangedDocument: cancellationToken => ApplyFix(context.Document, binaryExpression, cancellationToken));

        context.RegisterCodeFix(codeAction, diagnostic);
    }

    private static async Task<Document> ApplyFix(Document document, BinaryExpressionSyntax binaryExpression, CancellationToken cancellationToken = default)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        if (root is null)
        {
            // If we couldn't get the syntax root, return the document unchanged.
            // This should be unreachable.
            return document;
        }

        var semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        if (semanticModel is null)
        {
            // If we couldn't get the semantic model, return the document unchanged.
            // This should be unreachable.
            return document;
        }

        if (!IsGodotArrayAddExpression(binaryExpression, semanticModel))
        {
            // If the binary expression is not a GodotArray addition, return the document unchanged.
            // This should be unreachable because we only report diagnostics on GodotArray addition expressions.
            return document;
        }

        if (binaryExpression.Parent is BinaryExpressionSyntax parentExpression
         && IsGodotArrayAddExpression(parentExpression, semanticModel))
        {
            // If this binary expression is not the outermost expression, the fix should not apply.
            // This should be unreachable because we only report diagnostics on outermost expressions.
            return document;
        }

        var collectionElements = CollectChainOperands(binaryExpression, semanticModel)
            .Select(operand => (CollectionElementSyntax)SyntaxFactory.SpreadElement(operand.WithoutTrivia()));

        var collectionExpression = SyntaxFactory.CollectionExpression(
            SyntaxFactory.SeparatedList(collectionElements));

        ExpressionSyntax newExpression = collectionExpression;

        // For 'var' declarations the collection expression needs an explicit cast because
        // the compiler cannot infer the target type from a spread-only expression.
        if (NeedsTypeCast(binaryExpression, semanticModel, out var castType, out bool typeIsGeneric))
        {
            if (!typeIsGeneric)
            {
                newExpression = SyntaxFactory.ParenthesizedExpression(collectionExpression);
            }

            newExpression = SyntaxFactory.CastExpression(castType, newExpression);
        }

        var newRoot = root.ReplaceNode(binaryExpression, newExpression.WithTriviaFrom(binaryExpression));
        return document.WithSyntaxRoot(newRoot);
    }

    private static IEnumerable<ExpressionSyntax> CollectChainOperands(BinaryExpressionSyntax binaryExpression, SemanticModel semanticModel)
    {
        // Recursively flatten the left side if it is also a GodotArray + chain.
        if (binaryExpression.Left is BinaryExpressionSyntax leftExpression
         && IsGodotArrayAddExpression(leftExpression, semanticModel))
        {
            foreach (var operand in CollectChainOperands(leftExpression, semanticModel))
            {
                yield return operand;
            }
        }
        else
        {
            yield return binaryExpression.Left;
        }

        yield return binaryExpression.Right;
    }

    private static bool NeedsTypeCast(BinaryExpressionSyntax binaryExpression, SemanticModel semanticModel, [NotNullWhen(true)] out TypeSyntax? castType, out bool typeIsGeneric)
    {
        castType = null;
        typeIsGeneric = false;

        // Only needed when the expression is directly inside a 'var' variable declaration.
        if (binaryExpression.Parent is not EqualsValueClauseSyntax
         || binaryExpression.Parent.Parent is not VariableDeclaratorSyntax
         || binaryExpression.Parent.Parent.Parent is not VariableDeclarationSyntax { Type: var declType }
         || !declType.IsVar)
        {
            return false;
        }

        // Determine the element type from the binary expression to build the 'GodotArray' or 'GodotArray<T>'.
        var typeInfo = semanticModel.GetTypeInfo(binaryExpression);
        var type = typeInfo.Type as INamedTypeSymbol;

        if (!IsGodotArrayType(type))
        {
            // The type of the expression is not a GodotArray, so we may not be able to determine
            // the correct type. But this should not be reachable because we only report diagnostics
            // on expressions that are GodotArray additions, which should always have a GodotArray type.
            return false;
        }

        IEnumerable<TypeSyntax>? typeArguments = null;
        if (type.IsGenericType)
        {
            typeIsGeneric = true;
            typeArguments = type.TypeArguments
                .Select(typeArgument => SyntaxUtils.CreateQualifiedName(typeArgument.Name));
        }

        castType = SyntaxUtils.CreateQualifiedName("GodotArray", typeArguments);
        return true;
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
