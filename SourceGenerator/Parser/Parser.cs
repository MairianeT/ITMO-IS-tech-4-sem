namespace Parser;

public class Parser
{
    public static EntityDeclaration EntityParser(string path)
    {
        var entityName = "";
        var fields = new List<ArgDeclaration>();
        var flag = false;
        foreach (var line in System.IO.File.ReadLines(path))
        {
            if (line.Contains("class"))
            {
                var words = line.Split(' ');
                entityName = words[2];
            }

            if (flag)
            {
                var words = line.Split(' ');
                
                var argType = words[5];
                if (argType == "LocalDate")
                    argType = "DateTime";
                
                var argName = words[6];
                if (argName.Contains(';'))
                    argName = words[6].Substring(0, words[6].Length - 1);
                
                var args = new ArgDeclaration(argType, argName);
                fields.Add(args);
            }

            flag = line.Contains("Column") || line.Contains("OneToMany");
        }
        var entityDeclaration = new EntityDeclaration(entityName, fields);
        return entityDeclaration;
    }
    
    public static List<MethodDeclaration> ControllerParser(string path)
        {
            var methods = new List<MethodDeclaration>();
            var methodName = "";
            var returnType = "";
            var argList = new List<ArgDeclaration>();
            var url = "";
            var url1 = "";
            var httpMethodName = "";
            var flag = false;
            var flag2 = false;
            
            foreach (var line in System.IO.File.ReadLines(path))
            {
                if (line.Contains("RequestMapping"))
                {
                    var words = line.Split(' ');
                    url1 = words[0].Substring(words[0].IndexOf('/'), words[0].LastIndexOf('"') - words[0].IndexOf('/'));
                }

                if (flag)
                {
                    var words = line.Split(' ');
                    returnType = words[5];
                    if (returnType.Contains("Optional"))
                    {
                        returnType = returnType.Substring(returnType.IndexOf('<') + 1, returnType.IndexOf('>') - returnType.IndexOf('<') - 1);
                    }
                    
                    methodName = words[6][..words[6].IndexOf('(')];
                    
                    for (var i = words.Length - 3; i > 5; i-=2)
                    {
                        if (words[i - 1].Contains("PathVariable") ) continue;
                        var argType = words[i].Substring(words[i].IndexOf('(') + 1, words[i].Length - words[i].IndexOf('(') - 1);
                        var argName = words[i+1].Trim( new Char[] { ')', ',' } );

                        var args = new ArgDeclaration(argType, argName);
                        argList.Add(args);
                    }

                    flag2 = true;
                }
                
                if (line.Contains("Mapping(") && !line.Contains("RequestMapping"))
                {
                    flag = true;
                    var words = line.Split(' ');
                    url = string.Concat(url1, "/", words[6].AsSpan(words[6].IndexOf('"') + 1, words[6].LastIndexOf('"') - words[6].IndexOf('"') - 1));
                    if (url.Contains("{id}"))
                        url = url.Replace("{id}", "");
                    
                    if (words[4][1] == 'G')
                    {
                        httpMethodName = "Get";
                    }
                    
                    if (words[4][1] == 'P' && words[4][2] == 'o')
                    {
                        httpMethodName = "Post";
                    }
                }
                else
                {
                    flag = false;
                }

                if (flag2)
                {
                    var methodDeclaration = new MethodDeclaration(methodName, returnType, argList, url, httpMethodName);
                    argList = new List<ArgDeclaration>();
                    methods.Add(methodDeclaration);
                    flag2 = false;
                }
            }
            return methods;
        }
}