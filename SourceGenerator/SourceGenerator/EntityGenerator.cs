using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Parser;

namespace SourceGenerator;

[Generator]
public class EntityGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public MemberDeclarationSyntax[] CreateFields(List<ArgDeclaration> fields)
    {
        var msd = new MemberDeclarationSyntax[fields.Count];
        int i = 0;
        foreach (var field in fields)
        {
            msd[i] = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.IdentifierName(field.ArgType),
                    SyntaxFactory.Identifier(field.AgrName))
                .AddModifiers(
                        SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                            new AccessorDeclarationSyntax[]
                            {
                                SyntaxFactory.AccessorDeclaration(
                                        SyntaxKind.GetAccessorDeclaration)
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(SyntaxKind
                                            .SemicolonToken)),
                                SyntaxFactory.AccessorDeclaration(
                                        SyntaxKind.SetAccessorDeclaration)
                                    .WithSemicolonToken(
                                        SyntaxFactory.Token(SyntaxKind
                                            .SemicolonToken))
                            });
            i++;
        }
        return msd;
    }

    public MemberDeclarationSyntax[] CreateClasses(EntityDeclaration[] declarations, GeneratorExecutionContext context)
    {
        var mds = new MemberDeclarationSyntax[declarations.Length];
        var i = 0;
        foreach (var declaration in declarations)
        {
            mds[i] = SyntaxFactory.ClassDeclaration(declaration.Name)
                .AddModifiers(
                    SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(CreateFields(declaration.Fields));
            i++;
        }

        return mds;
    }
    public CompilationUnitSyntax CreateSource(EntityDeclaration[] declarations, GeneratorExecutionContext context)
    {
        var unitSyntax = SyntaxFactory.CompilationUnit()
            .AddMembers(
                    SyntaxFactory.NamespaceDeclaration(
                            SyntaxFactory.IdentifierName("GeneratorResult"))
                        .AddMembers(CreateClasses(declarations, context)))
            .NormalizeWhitespace();
        return unitSyntax;
    }
    public void Execute(GeneratorExecutionContext context)
    {
        var rabbitDeclaration = Parser.Parser.EntityParser(@"D:\ITMO\4 sem\ProgTech\lab-2\lab2\src\main\java\com\MyProject\lab2\Entities\Rabbit.java");
        var ownerDeclaration = Parser.Parser.EntityParser(@"D:\ITMO\4 sem\ProgTech\lab-2\lab2\src\main\java\com\MyProject\lab2\Entities\Owner.java");
        var declarations = new EntityDeclaration[]{rabbitDeclaration, ownerDeclaration};
        context.AddSource("GeneratorResult.cs", CreateSource(declarations, context).ToString());
    }
}