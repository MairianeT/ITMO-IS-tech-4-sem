using System;
using System.Collections.Generic;
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

namespace AnalyzerTemplate
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(AnalyzerBackingFieldsCodeFixProvider)), Shared]
    public class AnalyzerBackingFieldsCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(AnalyzerBackingFields.DiagnosticId); }
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
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().First();

            // Register a code action that will invoke the fix.
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedDocument: c => MakeBackingFields(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }

        private async Task<Document> MakeBackingFields(Document document, ClassDeclarationSyntax declarationSyntax,
           CancellationToken cancellationToken)
        {
            var tree = await document.GetSyntaxTreeAsync(cancellationToken).ConfigureAwait(false);
            var root = await tree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
            
            var declarations = declarationSyntax.Members;
            var fields = new List<FieldDeclarationSyntax>();
            var fieldNames = new List<string>();

            foreach (var declaration in declarations)
            {
                if (declaration is FieldDeclarationSyntax field && field.Declaration.Variables.Count == 1)
                {
                    fieldNames.Add(field.Declaration.Variables.First().Identifier.ToString());
                    fields.Add(field);
                }
            }

            foreach (var declaration in declarations)
            {
                if (declaration is MethodDeclarationSyntax methodDeclarationSyntax)
                {
                    var statements = methodDeclarationSyntax.Body.Statements;

                    if (statements.Count == 1 &&
                        statements.First() is ReturnStatementSyntax returnStatement
                        && returnStatement.Expression is IdentifierNameSyntax nameSyntax)
                    {
                        foreach (var fieldName in fieldNames)
                        {
                            if (nameSyntax.Identifier.ToString() == fieldName)
                            {

                                var newDeclaration = SyntaxFactory.PropertyDeclaration(
                                        SyntaxFactory.PredefinedType(
                                            SyntaxFactory.Token(SyntaxKind.StringKeyword)
                                        ),
                                        SyntaxFactory.Identifier(methodDeclarationSyntax.Identifier.ToString())
                                    )
                                    .WithModifiers(
                                        SyntaxFactory.TokenList(
                                            SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                                        )
                                    )
                                    .WithAccessorList(
                                        SyntaxFactory.AccessorList(
                                            SyntaxFactory.SingletonList<AccessorDeclarationSyntax>(
                                                SyntaxFactory.AccessorDeclaration(
                                                        SyntaxKind.GetAccessorDeclaration
                                                    )
                                                    .WithSemicolonToken(
                                                        SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                                                    )
                                            )
                                        )
                                    );
                                root = root.RemoveNode(fields[fieldNames.IndexOf(fieldName)],
                                    SyntaxRemoveOptions.KeepNoTrivia).NormalizeWhitespace();
                                SyntaxTree newTree = CSharpSyntaxTree.ParseText(root.ToString());
                                CompilationUnitSyntax newRoot =
                                    await newTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
                                var oldDeclaration = newRoot.DescendantNodes()
                                    .OfType<MethodDeclarationSyntax>().First();
                                newRoot = newRoot.ReplaceNode(oldDeclaration, newDeclaration)
                                    .NormalizeWhitespace();
                                return document.WithSyntaxRoot(newRoot);
                            }
                        }
                    }
                }

                if (declaration is PropertyDeclarationSyntax propertyDeclarationSyntax)
                {
                    if (propertyDeclarationSyntax.AccessorList != null)
                    {
                        var accessors = propertyDeclarationSyntax.AccessorList.Accessors;
                        foreach (var accessor in accessors)
                        {
                            var statements = accessor.Body.Statements;
                            foreach (var statement in statements)
                            {
                                if (statement is ReturnStatementSyntax returnStatement
                                    && returnStatement.Expression is IdentifierNameSyntax nameSyntax)
                                {
                                    foreach (var fieldName in fieldNames)
                                    {
                                        if (fieldName == nameSyntax.Identifier.ToString())
                                        {
                                            var newAccessor = SyntaxFactory.AccessorDeclaration(
                                                    SyntaxKind.GetAccessorDeclaration
                                                )
                                                .WithSemicolonToken(
                                                    SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                                                ).NormalizeWhitespace();

                                            root = root.RemoveNode(fields[fieldNames.IndexOf(fieldName)],
                                                SyntaxRemoveOptions.KeepNoTrivia).NormalizeWhitespace();
                                            SyntaxTree newTree = CSharpSyntaxTree.ParseText(root.ToString());
                                            CompilationUnitSyntax newRoot =
                                                await newTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
                                            var oldAccessor = newRoot.DescendantNodes()
                                                .OfType<AccessorDeclarationSyntax>().First();
                                            newRoot = newRoot.ReplaceNode(oldAccessor, newAccessor)
                                                .NormalizeWhitespace();
                                            return document.WithSyntaxRoot(newRoot);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (propertyDeclarationSyntax.ExpressionBody is ArrowExpressionClauseSyntax arrowExpressionClauseSyntax
                        && arrowExpressionClauseSyntax.Expression is IdentifierNameSyntax expressionSyntax)
                    {
                        foreach (var fieldName in fieldNames)
                        {
                            if (expressionSyntax.Identifier.ToString() == fieldName)
                            {
                                var newProperty = SyntaxFactory.PropertyDeclaration(
                                        SyntaxFactory.PredefinedType(
                                            SyntaxFactory.Token(SyntaxKind.StringKeyword)
                                        ),
                                        SyntaxFactory.Identifier(propertyDeclarationSyntax.Identifier.ToString())
                                    )
                                    .WithModifiers(
                                        SyntaxFactory.TokenList(
                                            SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                                        )
                                    )
                                    .WithAccessorList(
                                        SyntaxFactory.AccessorList(
                                            SyntaxFactory.SingletonList<AccessorDeclarationSyntax>(
                                                SyntaxFactory.AccessorDeclaration(
                                                        SyntaxKind.GetAccessorDeclaration
                                                    )
                                                    .WithSemicolonToken(
                                                        SyntaxFactory.Token(SyntaxKind.SemicolonToken)
                                                    )
                                            )
                                        )
                                    );
                                root = root.RemoveNode(fields[fieldNames.IndexOf(fieldName)],
                                    SyntaxRemoveOptions.KeepNoTrivia).NormalizeWhitespace();
                                SyntaxTree newTree = CSharpSyntaxTree.ParseText(root.ToString());
                                CompilationUnitSyntax newRoot =
                                    await newTree.GetRootAsync(cancellationToken) as CompilationUnitSyntax;
                                var oldProperty = newRoot.DescendantNodes()
                                    .OfType<PropertyDeclarationSyntax>().First();
                                newRoot = newRoot.ReplaceNode(oldProperty, newProperty)
                                    .NormalizeWhitespace();
                                return document.WithSyntaxRoot(newRoot);
                            }
                        }
                    }
                }
            }
            return document.WithSyntaxRoot(root);
        }
    }
}
