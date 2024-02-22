using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AzureStorageApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IKeyVaultService, KeyVaultService>();
                })
                .Build();

            var _keyVaultService = host.Services.GetRequiredService<IKeyVaultService>();
            var _config = host.Services.GetRequiredService<IConfiguration>();

            var storageKey = _keyVaultService.GetSecret(_config.GetValue<string>("StorageKey") ?? string.Empty);

            // Get app setting value for storage key
            Console.WriteLine("Hello, World!");

            Console.ReadLine();

        }
    }
}