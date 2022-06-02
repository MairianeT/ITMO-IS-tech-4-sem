namespace Parser
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine("======== RabbitController =======");
            List<MethodDeclaration> RabbitController = Parser.ControllerParser(@"D:\ITMO\4 sem\ProgTech\lab-2\lab2\src\main\java\com\MyProject\lab2\Controllers\RabbitController.java");
            // Console.WriteLine("======== OwnerController =======");
            // List<MethodDeclaration> MethodDeclaration2 = Parser.ControllerParser(@"D:\ITMO\4 sem\ProgTech\lab-2\lab2\src\main\java\com\MyProject\lab2\Controllers\OwnerController.java");
            // Console.WriteLine("======== Rabbit =======");
            // EntityDeclaration EntityDeclaration1 = Parser.EntityParser(@"D:\ITMO\4 sem\ProgTech\lab-2\lab2\src\main\java\com\MyProject\lab2\Entities\Rabbit.java");
            // Console.WriteLine("======== Owner =======");
            // EntityDeclaration EntityDeclaration2 = Parser.EntityParser(@"D:\ITMO\4 sem\ProgTech\lab-2\lab2\src\main\java\com\MyProject\lab2\Entities\Owner.java");


            foreach (var rDeclaration in RabbitController)
            {
                Console.WriteLine(rDeclaration.MethodName);
                            Console.WriteLine(rDeclaration.HttpMethodName);
                            Console.WriteLine(rDeclaration.ReturnType);
                            Console.WriteLine(rDeclaration.Url);
                            foreach (var arg in rDeclaration.ArgList)
                            {
                                Console.Write(arg.ArgType);
                                Console.Write(" ");
                                Console.WriteLine(arg.AgrName);
                            }
            }

            
        }
    }
}