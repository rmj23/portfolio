using System;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace AzureStorageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Build app with app settings file
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true);
            IConfiguration config = builder.Build();

            string? keyVaultName = config.GetValue<string>("StorageKey");
            Console.WriteLine(keyVaultName);
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";

            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            // Get app setting value for storage key
            Console.WriteLine("Hello, World!");

            Console.ReadLine();

        }
    }
}