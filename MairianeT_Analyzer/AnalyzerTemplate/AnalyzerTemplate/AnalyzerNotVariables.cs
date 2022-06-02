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
    public class AnalyzerNotVariables : DiagnosticAnalyzer
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
            context.RegisterSyntaxNodeAction(AnalyzeNotVariable, SyntaxKind.MethodDeclaration);
        }

        private static void AnalyzeNotVariable(SyntaxNodeAnalysisContext context)
        {
            var method = (MethodDeclarationSyntax)context.Node;
            var parameters = method.ParameterList.Parameters;
            
            if (parameters.Any(parameter => parameter.Type.ToString() == "bool"
                                            && parameter.Identifier.ToString().Contains("not")))
            {
                var diagnostic = Diagnostic.Create(Rule, method.GetLocation());
                context.ReportDiagnostic(diagnostic);
                return;
            }

            var statements = method.Body.Statements;
            foreach (var statement in statements)
            {
                if (statement is LocalDeclarationStatementSyntax st)
                {
                    if (st.Declaration.Type.ToString() == "bool"
                        && st.Declaration.Variables.Any(variable => variable.Identifier.ToString().Contains("not")))
                    {
                        var diagnostic = Diagnostic.Create(Rule, method.GetLocation());
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
