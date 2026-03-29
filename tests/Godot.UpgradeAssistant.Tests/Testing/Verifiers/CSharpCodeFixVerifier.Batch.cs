using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Model;

namespace Godot.UpgradeAssistant.Tests;

partial class CSharpCodeFixVerifier<TCodeFix, TAnalyzer>
{
    public sealed class BatchTest : Test
    {
        private readonly List<DiagnosticAnalyzer> _additionalAnalyzersBefore = [];
        private readonly List<DiagnosticAnalyzer> _additionalAnalyzersAfter = [];
        private readonly List<CodeFixProvider> _additionalCodeFixProvidersBefore = [];
        private readonly List<CodeFixProvider> _additionalCodeFixProvidersAfter = [];

        public BatchTest(MetadataReference[] beforeReferences, MetadataReference[] afterReferences) : base(beforeReferences, afterReferences) { }

        public void AddAnalyzerBefore<TExtraAnalyzer>() where TExtraAnalyzer : DiagnosticAnalyzer, new() =>
            _additionalAnalyzersBefore.Add(new TExtraAnalyzer());

        public void AddAnalyzerAfter<TExtraAnalyzer>() where TExtraAnalyzer : DiagnosticAnalyzer, new() =>
            _additionalAnalyzersAfter.Add(new TExtraAnalyzer());

        public void AddCodeFixProviderBefore<TProvider>() where TProvider : CodeFixProvider, new() =>
            _additionalCodeFixProvidersBefore.Add(new TProvider());

        public void AddCodeFixProviderAfter<TProvider>() where TProvider : CodeFixProvider, new() =>
            _additionalCodeFixProvidersAfter.Add(new TProvider());

        protected override IEnumerable<DiagnosticAnalyzer> GetDiagnosticAnalyzers()
        {
            foreach (var analyzer in _additionalAnalyzersBefore)
            {
                yield return analyzer;
            }

            foreach (var analyzer in base.GetDiagnosticAnalyzers())
            {
                yield return analyzer;
            }

            foreach (var analyzer in _additionalAnalyzersAfter)
            {
                yield return analyzer;
            }
        }

        protected override IEnumerable<CodeFixProvider> GetCodeFixProviders()
        {
            foreach (var provider in _additionalCodeFixProvidersBefore)
            {
                yield return provider;
            }

            foreach (var provider in base.GetCodeFixProviders())
            {
                yield return provider;
            }

            foreach (var provider in _additionalCodeFixProvidersAfter)
            {
                yield return provider;
            }
        }

        protected override async Task RunImplAsync(CancellationToken cancellationToken)
        {
            Verify.NotEmpty($"{nameof(TestState)}.{nameof(SolutionState.Sources)}", TestState.Sources);

            var analyzers = GetDiagnosticAnalyzers().ToArray();

            var fixers = GetCodeFixProviders().ToImmutableArray();
            var fixableIds = fixers
                .SelectMany(f => f.FixableDiagnosticIds).ToImmutableArray();

            var defaultDiagnostic = GetDefaultDiagnostic(analyzers);
            var supportedDiagnostics = analyzers
                .SelectMany(a => a.SupportedDiagnostics).ToImmutableArray();

            var rawTest = TestState.WithInheritedValuesApplied(null, fixableIds);
            var rawFixed = FixedState.WithInheritedValuesApplied(rawTest, fixableIds);

            var testState = rawTest
                .WithProcessedMarkup(MarkupOptions, defaultDiagnostic, supportedDiagnostics, fixableIds, DefaultFilePath);
            var batchState = BatchFixedState
                .WithInheritedValuesApplied(rawFixed, fixableIds)
                .WithProcessedMarkup(MarkupOptions, defaultDiagnostic, supportedDiagnostics, fixableIds, DefaultFilePath);

            // Verify the initial diagnostics match the test state's expected diagnostics before applying any fixes.
            await VerifyDiagnosticsAsync(
                new EvaluatedProjectState(testState, ReferenceAssemblies),
                [],
                testState.ExpectedDiagnostics.ToArray(), Verify, cancellationToken)
                .ConfigureAwait(false);

            var project = await CreateProjectAsync(new EvaluatedProjectState(testState, ReferenceAssemblies), [], cancellationToken);

            // Apply Fix All in Solution for each fixer in registration order.
            foreach (var fixer in fixers)
            {
                var fixAllProvider = fixer.GetFixAllProvider();
                if (fixAllProvider is null)
                {
                    continue;
                }

                // Re-fetch diagnostics after each pass: the previous fixer may have changed the source.
                var allDiagnostics = await GetSortedDiagnosticsAsync(project.Solution, analyzers.ToImmutableArray(), [], CompilerDiagnostics, Verify, cancellationToken).ConfigureAwait(false);
                var filteredDiagnostics = allDiagnostics
                    .Where(d => fixer.FixableDiagnosticIds.Contains(d.diagnostic.Id))
                    .ToImmutableArray();
                if (filteredDiagnostics.IsEmpty)
                {
                    continue;
                }

                var (firstProject, firstDiagnostic) = filteredDiagnostics[0];
                var firstDocument = project.Solution.GetDocument(firstDiagnostic.Location.SourceTree);
                if (firstDocument is null)
                {
                    continue;
                }

                var actions = ImmutableArray.CreateBuilder<CodeAction>();
                var fixContext = CreateCodeFixContext(
                    document: firstDocument,
                    span: firstDiagnostic.Location.SourceSpan,
                    diagnostics: [firstDiagnostic],
                    registerCodeFix: (action, diagnostics) =>
                    {
                        actions.Add(action);
                    }, cancellationToken);

                await fixer.RegisterCodeFixesAsync(fixContext).ConfigureAwait(false);

                string? key = FilterCodeActions(actions.DrainToImmutable()).FirstOrDefault()?.EquivalenceKey;

                var fixAllContext = CreateFixAllContext(
                    firstDocument,
                    firstDiagnostic.Location.SourceSpan,
                    firstProject,
                    fixer,
                    FixAllScope.Solution,
                    key,
                    fixAllProvider.GetSupportedFixAllDiagnosticIds(fixer),
                    firstDiagnostic.Severity,
                    TestDiagnosticProvider.Create(allDiagnostics),
                    cancellationToken);
                var fixAllAction = await fixAllProvider.GetFixAsync(fixAllContext).ConfigureAwait(false);
                if (fixAllAction is null)
                {
                    continue;
                }

                (project, _) = await ApplyCodeActionAsync(project, fixAllAction, Verify, cancellationToken);
            }

            var documents = project.Documents.ToArray();
            Verify.Equal(batchState.Sources.Count, documents.Length, "document count after batch fix");
            for (int i = 0; i < documents.Length; i++)
            {
                var simplified = await Simplifier.ReduceAsync(documents[i], Simplifier.Annotation, cancellationToken: cancellationToken).ConfigureAwait(false);
                var formatted = await Formatter.FormatAsync(simplified, Formatter.Annotation, cancellationToken: cancellationToken).ConfigureAwait(false);
                Verify.EqualOrDiff(batchState.Sources[i].content.ToString(), (await formatted.GetTextAsync(cancellationToken).ConfigureAwait(false)).ToString(), $"content of '{batchState.Sources[i].filename}'");
            }

            // Verify the fixed code compiles against the post-fix references.
            await VerifyDiagnosticsAsync(
                new EvaluatedProjectState(batchState, ReferenceAssemblies),
                [],
                batchState.ExpectedDiagnostics.ToArray(),
                Verify.PushContext("Diagnostics of batch-fixed state"),
                cancellationToken)
                .ConfigureAwait(false);
        }

        private sealed class TestDiagnosticProvider : FixAllContext.DiagnosticProvider
        {
            private readonly ImmutableArray<(Project project, Diagnostic diagnostic)> _diagnostics;

            private TestDiagnosticProvider(ImmutableArray<(Project project, Diagnostic diagnostic)> diagnostics)
            {
                _diagnostics = diagnostics;
            }

            public override Task<IEnumerable<Diagnostic>> GetAllDiagnosticsAsync(Project project, CancellationToken cancellationToken)
            {
                return Task.FromResult(_diagnostics.Where(diagnostic => diagnostic.project.Id == project.Id).Select(diagnostic => diagnostic.diagnostic));
            }

            public override Task<IEnumerable<Diagnostic>> GetDocumentDiagnosticsAsync(Document document, CancellationToken cancellationToken)
            {
                return Task.FromResult(_diagnostics.Where(i => i.diagnostic.Location.GetLineSpan().Path == document.FilePath).Where(diagnostic => diagnostic.project.Id == document.Project.Id).Select(diagnostic => diagnostic.diagnostic));
            }

            public override Task<IEnumerable<Diagnostic>> GetProjectDiagnosticsAsync(Project project, CancellationToken cancellationToken)
            {
                return Task.FromResult(_diagnostics.Where(i => !i.diagnostic.Location.IsInSource).Where(diagnostic => diagnostic.project.Id == project.Id).Select(diagnostic => diagnostic.diagnostic));
            }

            internal static TestDiagnosticProvider Create(ImmutableArray<(Project project, Diagnostic diagnostic)> diagnostics)
            {
                return new(diagnostics);
            }
        }
    }
}
