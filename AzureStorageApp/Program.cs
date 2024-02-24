using AzureStorageApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureStorageApp
{
    class Program
    {
        static async void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IKeyVaultService, AzureKeyVaultService>();
                })
            .Build();

            // Get the configuration and the key vault service
            var _config = host.Services.GetRequiredService<IConfiguration>();
            var _keyVaultService = host.Services.GetRequiredService<IKeyVaultService>();

            // Get the storage key from the key vault
            var storageKey = await _keyVaultService.GetSecretAsync(_config.GetValue<string>("StorageKey") ?? string.Empty);

            // Create a new instance of the AzureStorageService
            var _storageService = new AzureStorageService(storageKey, "ContainerA");

            // Check if the file exists
            bool fileExists = await _storageService.FileExistsAsync(storageKey);

            // Get app setting value for storage key
            Console.WriteLine("Hello, World!");

            Console.ReadLine();

        }
    }
}