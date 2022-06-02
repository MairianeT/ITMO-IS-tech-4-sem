using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace AnalyzerTemplate
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AnalyzerBackingFields : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "AnalyzerTemplate";
        
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNotVariable, SyntaxKind.ClassDeclaration);
        }

        private static void AnalyzeNotVariable(SyntaxNodeAnalysisContext context)
        {
            var classDeclaration = (ClassDeclarationSyntax)context.Node;

            var declarations = classDeclaration.Members;
            var fields = new List<string>();

            foreach (var declaration in declarations)
            {
                if (declaration is FieldDeclarationSyntax field && field.Declaration.Variables.Count == 1)
                {
                    fields.Add(field.Declaration.Variables.First().Identifier.ToString());
                }
            }

            foreach (var declaration in declarations)
            {
                if (declaration is MethodDeclarationSyntax methodDeclarationSyntax)
                {
                    var statements = methodDeclarationSyntax.Body.Statements;

                    foreach (var statement in statements)
                    {
                        if (statement is ReturnStatementSyntax returnStatement 
                            && returnStatement.Expression is IdentifierNameSyntax nameSyntax
                            && fields.Contains(nameSyntax.Identifier.ToString()))
                        {
                            var diagnostic = Diagnostic.Create(Rule, classDeclaration.GetLocation());
                            context.ReportDiagnostic(diagnostic);
                            return;
                        }
                    }
                }

                if (declaration is PropertyDeclarationSyntax propertyDeclarationSyntax)
                {
                    var accessors = propertyDeclarationSyntax.AccessorList.Accessors;
                    foreach (var accessor in accessors)
                    {
                        var statements = accessor.Body.Statements;
                        foreach (var statement in statements)
                        {
                            if (statement is ReturnStatementSyntax returnStatement 
                                && returnStatement.Expression is IdentifierNameSyntax nameSyntax
                                && fields.Contains(nameSyntax.Identifier.ToString()))
                            {
                                var diagnostic = Diagnostic.Create(Rule, classDeclaration.GetLocation());
                                context.ReportDiagnostic(diagnostic);
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}
