using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerBackingFields,
    AnalyzerTemplate.AnalyzerBackingFieldsCodeFixProvider>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerBackingFieldsUnitTest
    {
        [TestMethod]
        public async Task TestPropertyReturn()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        private string _url;

        public string Url
        {
            get { return _url; }
        }

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"namespace ConsoleApplication1
{
    class Test
    {
        public string Url
        {
            get;
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerBackingFieldsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        
        [TestMethod]
        public async Task TestPropertyExpression()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        private string _url;

        public string Url => _url;

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"namespace ConsoleApplication1
{
    class Test
    {
        public string Url
        {
            get;
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerBackingFieldsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task TestMethod()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        private string _url;
    
        public String getURL()
        {
            return _url;
        }

        static void Main()
        {
        }
    }
}";

            var expectedChangedCode = @"namespace ConsoleApplication1
{
    class Test
    {
        public string getURL
        {
            get;
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerBackingFieldsCodeFixProvider();

            CodeAction registeredCodeAction = null;

            var context = new CodeFixContext(document, diagnostic, (codeAction, _) =>
            {
                if (registeredCodeAction != null)
                    throw new Exception("Code action was registered more than once");

                registeredCodeAction = codeAction;

            }, CancellationToken.None);

            await codeFixProvider.RegisterCodeFixesAsync(context);

            if (registeredCodeAction == null)
                throw new Exception("Code action was not registered");

            var operations = await registeredCodeAction.GetOperationsAsync(CancellationToken.None);

            foreach (var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
    }
}