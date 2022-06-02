namespace Parser;

public class EntityDeclaration
{
    public string Name { get; set; }
    public List<ArgDeclaration> Fields { get; set; }

    public EntityDeclaration(string name, List<ArgDeclaration> fields)
    {
        Name = name;
        Fields = fields;
    }
}