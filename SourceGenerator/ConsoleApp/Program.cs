using GeneratorResult;
namespace ConsoleApp;

public static class Program
{
    static void Main(string[] args)
    {
        var rabbit = new Rabbit();
        var owner = new Owner();
        var client = new Client();

        Console.WriteLine(client.readAllRabbits());
    }
}