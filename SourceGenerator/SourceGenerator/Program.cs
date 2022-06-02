using System.Net;

namespace SourceGenerator
{
    public static class Program
    {
        public static void POST(string jsonData)
        {
            var httpRequest = (HttpWebRequest)WebRequest.Create("http://localhost:8081/rabbits/create");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            using (var requestStream = httpRequest.GetRequestStream())
            using (var writer = new StreamWriter(requestStream))
            {
                writer.Write(jsonData);
            }
            using (var httpResponse = httpRequest.GetResponse())
            using (var responseStream = httpResponse.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                string response = reader.ReadToEnd();
            }
        }
        
        public static void Main()
        {
            string jsonData = @"{
        ""name"":""ThirdR"",
        ""color"" : ""Yellow"", 
        ""owner"": 
        {
          ""id"" : 1
        }
        }";
            POST(jsonData);
        }
    }
}