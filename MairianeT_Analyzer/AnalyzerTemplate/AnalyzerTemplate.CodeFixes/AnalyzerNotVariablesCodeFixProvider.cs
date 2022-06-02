using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.CodeAnalysis.Rename;

namespace AnalyzerTemplate
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AnalyzerNotVariablesCodeFixProvider)), Shared]
    public class AnalyzerNotVariablesCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerNotVariables.DiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            // TODO: Replace the following code with your own analysis, generating a CodeAction for each fix to suggest
            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the type declaration identified by the diagnostic.
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => MakeNotVariable(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Document> MakeNotVariable(Document document, MethodDeclarationSyntax declarationSyntax, CancellationToken cancellationToken)
        {
            var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root = await tree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            var newDocument = document;
            var parameters = declarationSyntax.ParameterList.Parameters;
            var statements = declarationSyntax.Body.Statements;

            foreach (var statement in statements)
            {
                if (statement is LocalDeclarationStatementSyntax localDeclarationStatementSyntax)
                {
                    if (localDeclarationStatementSyntax.Declaration.Type.ToString() == "bool")
                    {
                        foreach (var variable in localDeclarationStatementSyntax.Declaration.Variables)
                        {
                            if (variable.Identifier.ToString().StartsWith("not"))
                            {
                                var newName = variable.Identifier.ToString().Replace("not", "").ToLower();
                                var compilation = await newDocument.Project.GetCompilationAsync();
                                var model = compilation.GetSemanticModel(root.SyntaxTree);
                                var original = variable;
                                var originalSymbol = model.GetDeclaredSymbol(original);
                                var newSolution = await Renamer.RenameSymbolAsync(newDocument.Project.Solution,
                                    originalSymbol, newName, newDocument.Project.Solution.Workspace.Options);
                                newDocument = newSolution.GetDocument(document.Id);
                            }
                        }
                    }
                }
            }
            var tree2 = await newDocument.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root2 = await tree2.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            foreach (var parameter in parameters)
            {
                if (parameter.Type.ToString() == "bool" && parameter.Identifier.ToString().StartsWith("not"))
                {
                    var newName = parameter.Identifier.ToString().Replace("not", "").ToLower();
                    
                    var compilation = await newDocument.Project.GetCompilationAsync();
                    var model = compilation.GetSemanticModel(root2.SyntaxTree);
                    
                    var original = root2.DescendantNodesAndSelf().OfType<ParameterSyntax>().First();
                    var originalSymbol = model.GetDeclaredSymbol(original);
                    var newSolution = await Renamer.RenameSymbolAsync(newDocument.Project.Solution, 
                        originalSymbol, newName, newDocument.Project.Solution.Workspace.Options);
                    newDocument = newSolution.GetDocument(document.Id);
                }
            }
            return newDocument;
        }
    }
}
