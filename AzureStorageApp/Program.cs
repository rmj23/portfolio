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

            string keyVaultUrl = config.GetConnectionString("KeyVault") ?? string.Empty;
            string keyVaultStorageName = config.GetValue<string>("StorageKey") ?? string.Empty;
            Console.WriteLine(keyVaultUrl);

            var creds = new DefaultAzureCredential(new DefaultAzureCredentialOptions 
            {
                ManagedIdentityClientId = "7494deba-68fe-48fe-a074-aef13a3446be"
            });
            var client = new SecretClient(new Uri(keyVaultUrl), creds);
            var storageKey = client.GetSecret(keyVaultStorageName);

            // Get app setting value for storage key
            Console.WriteLine("Hello, World!");

            Console.ReadLine();

        }
    }
}