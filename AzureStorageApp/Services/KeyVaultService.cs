using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Ardalis.GuardClauses;

/// <summary>
/// Implementation of the KeyVaultService
/// </summary>
public class KeyVaultService : IKeyVaultService
{
    /// <summary>
    /// The secret client
    /// </summary>
    private readonly SecretClient _client;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="config">Configuration</param>
    public KeyVaultService(IConfiguration config)
    {
        string keyVaultUrl = config.GetConnectionString("KeyVault");
        Guard.Against.NullOrEmpty(keyVaultUrl, nameof(keyVaultUrl));

        var creds = new DefaultAzureCredential(new DefaultAzureCredentialOptions 
        {
            ManagedIdentityClientId = "7494deba-68fe-48fe-a074-aef13a3446be"
        });
        
        _client = new SecretClient(new Uri(keyVaultUrl), creds);
    }

    /// <summary>
    /// Get the secret from the key vault
    /// </summary>
    /// <param name="secretName">The secret name</param>
    /// <returns>The secret's value. Empty string if noting found</returns>
    public string GetSecret(string secretName)
    {
        try
        {
            return _client.GetSecretAsync(secretName).Result.Value.Value ?? string.Empty;
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"An error occurred while getting the secret: {ex.Message}");
            return string.Empty;
        }
    }
}