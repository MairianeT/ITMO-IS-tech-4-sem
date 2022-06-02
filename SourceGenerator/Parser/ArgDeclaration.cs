namespace Parser;

public class ArgDeclaration
{
    public string ArgType { get; set; }
    public string AgrName { get; set; }

    public ArgDeclaration(string argType, string agrName)
    {
        ArgType = argType;
        AgrName = agrName;
    }
}