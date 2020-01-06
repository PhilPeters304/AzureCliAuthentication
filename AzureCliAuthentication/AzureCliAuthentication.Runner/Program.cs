using System;

namespace AzureCliAuthentication.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var example = new Example.Example();
            example.UsingAzureCredentials();
            example.UsingAzureFluentClient();
            Console.WriteLine("Press enter to continue");
            Console.ReadLine(); 
        }
    }
}
