using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = AnalyzerTemplate.Test.CSharpCodeFixVerifier<
    AnalyzerTemplate.AnalyzerNotVariables,
    AnalyzerTemplate.AnalyzerNotVariablesCodeFixProvider>;

namespace AnalyzerTemplate.Test
{
    [TestClass]
    public class AnalyzerNotVariablesUnitTest
    {
        [TestMethod]
        public async Task TestParameters()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        public int Test1(bool notGood, int notGood, bool Good)
        {
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
        public int Test1(bool good, int notGood, bool Good)
        {
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);       
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerNotVariablesCodeFixProvider();

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

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task TestIfCondition()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        public int Test1(bool notAvailable)
        {
            if (notAvailable)
            {
            }
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
        public int Test1(bool available)
        {
            if (available)
            {
            }
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);       
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerNotVariablesCodeFixProvider();

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

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task TestVarIdentifier()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        public int Test1(bool notFirst)
        {
            bool notAvailable = true;
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
        public int Test1(bool first)
        {
            bool available = true;
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);       
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerNotVariablesCodeFixProvider();

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

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
        
        [TestMethod]
        public async Task TestVarValue()
        {
            var code = @"namespace ConsoleApplication1
{
    class Test
    {
        public int Test1(bool notGood)
        {
            bool notAvailable = notGood;
            bool variable = notAvailable;
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
        public int Test1(bool good)
        {
            bool available = good;
            bool variable = available;
        }

        static void Main()
        {
        }
    }
}";

            var (diagnostics, document, workspace) = await Utilities.GetDiagnosticsAdvanced(code);       
            var diagnostic = diagnostics[0];

            var codeFixProvider = new AnalyzerNotVariablesCodeFixProvider();

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

            foreach(var operation in operations)
            {
                operation.Apply(workspace, CancellationToken.None);
            }

            var updatedDocument = workspace.CurrentSolution.GetDocument(document.Id);


            var newCode = (await updatedDocument.GetTextAsync()).ToString();

            Assert.AreEqual(expectedChangedCode, newCode);
        }
    }
}
